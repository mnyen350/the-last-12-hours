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


    void Update()
    {
        var light2d = player.GetComponentInChildren<Light2D>();
        if (light2d.enabled)
        {
            var halfAngle = (light2d.pointLightInnerAngle / 2);

            var p2m = GetAngle(player.position, player.Controls.GetMouseWorldPosition());
            var p2s = GetAngle(player.position, this.position);
            var hit = IsInAngleRange(p2m - halfAngle, p2m + halfAngle, p2s);

            //Debug.Log(string.Format("{0} {1} {2}", p2m, p2s, hit));

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
        DropItem(ItemType.Bandage);
    }
}
