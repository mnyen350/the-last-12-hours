using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class Rat : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private float GetAngle(Vector2 v1, Vector2 v2)
    {
        var angle = (float)Math.Atan2(v1.y - v2.y, v1.x - v2.x);
        if (angle < 0)
            angle += 2 * Mathf.PI;
        angle *= Mathf.Rad2Deg;
        return angle;
    }

    private bool IsInAngleRange(float f, float t, float w)
    {
        // https://stackoverflow.com/questions/71881043/how-to-check-angle-in-range

        f %= 360;
        t %= 360;
        w %= 360;

        while (f < 0) f += 360;
        while (t < f) t += 360;
        while (w < f) w += 360;


        return f <= w && w <= t;
    }

    void Update()
    {
        var light2d = player.GetComponentInChildren<Light2D>();
        if (light2d.enabled)
        {
            var halfAngle = (light2d.pointLightInnerAngle / 2);

            var p2m = GetAngle(player.position, player.Controls.GetMouseWorldPosition());
            var p2s = GetAngle(player.position, this.position);
            var hit = IsInAngleRange(p2m - halfAngle, p2m + halfAngle, p2s);

            Debug.Log(string.Format("{0} {1} {2}", p2m, p2s, hit));

            // stop moving if the player shines the flashlight on the rat
            canMove = !hit;
        }
        else
        {
            canMove = true;
        }
    }

    protected override void DropReward()
    {
        // example of dropping an item or something on death...
        Instantiate(manager.Prefabs.BandagePrefab, this.transform.position, Quaternion.identity);
    }

    protected override void Attack()
    {
        ani?.SetTrigger("triggerBite");
        base.Attack();
    }
}
