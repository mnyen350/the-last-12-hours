using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class ClickWeapon : MonoBehaviour, IPointerDownHandler
{
    public Player player => Player.Instance;

    private Outline _outline;

    private SpriteRenderer _handRenderer;
    private Light2D _flashlight;

    [field: SerializeField]
    public ItemType weaponType { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _outline = this.GetComponent<Outline>();
        _flashlight = player.GetComponentInChildren<Light2D>();
        _handRenderer = player.hand.GetComponentInChildren<SpriteRenderer>();

        player.OnChangeWeapon += player_OnChangeWepaon;
        player.OnUseWeapon += Player_OnUseWeapon;

        player_OnChangeWepaon(); // forcefully trigger so outlining is done.

    }

    private void Player_OnUseWeapon()
    {
        if (player.activeWeapon != this.weaponType)
            return;

        //Debug.Log("OnUseWeapon " + this.weaponType);

        switch (this.weaponType)
        {
            case ItemType.Flashlight:
                {
                    // toggle the flashlight
                    _flashlight.enabled = !_flashlight.enabled;
                    break;
                }
            case ItemType.Knife:
                {
                    var enemy = player.GetNearby<Enemy>(2).FirstOrDefault();
                    if (enemy != null)
                    {
                        Debug.Log("Enemy found");
                        Debug.Log(Vector2.Distance(player.position, enemy.position));

                        enemy.ReceiveAttack(player, player.attack);
                    }
                    else
                    {
                        Debug.Log("No enemy found");
                    }


                    break;
                }
        }
    }

    void OnDestroy()
    {
        player.OnChangeWeapon -= player_OnChangeWepaon;
        player.OnUseWeapon += Player_OnUseWeapon;
    }

    private void player_OnChangeWepaon()
    {
        // when weapon is selected, highlight it
        if (this.weaponType == player.activeWeapon)
        {
            _outline.enabled = true;
            _handRenderer.sprite = player.inventory.Get(this.weaponType).sprite;

            // disable flashlight if weapon isn't flashlight
            if (this.weaponType != ItemType.Flashlight)
                _flashlight.enabled = false;
        }
        else
        {
            _outline.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"OnPointerDown()");
        player.ChangeWeapon(this.weaponType);
    }
}
