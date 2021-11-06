using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class LeftHandController : BaseHandController
{
    protected BaseWeaponShooterProjectile leftWeapon;
    protected GrabController leftGrabController;

    public void Start()
    {
        weaponInventoryBase.OnLeftHandEquipped += OnCurrentItemEquipped;
        
    }

    public override void OnCurrentItemEquipped()
    {
        leftWeapon = GetComponentInChildren<BaseWeaponShooterProjectile>();
        leftGrabController = GetComponentInChildren<GrabController>();
    }
    public override void OnCurrentItemDropped()
    {

    }

    public override void UseCurrentItem()
    {
        leftWeapon.MyInput(KeyCode.Mouse0);
    }

    public override void DropCurrentItem()
    {
        if (Input.GetKeyDown(KeyCode.H) && leftWeapon != null)
        {
            weaponInventoryBase.ResetLeftHandGrab();
            leftGrabController.DropItem();
        }
    }

    public void Update()
    {
        DropCurrentItem();

        if (leftWeapon != null)
        {
            leftWeapon.MyInput(KeyCode.Mouse0);
        }


        if (Input.GetMouseButton(1) && leftWeapon != null)
        {
            //UseItem();
            Debug.Log("Right Weapon Fired");
        }
    }

}
