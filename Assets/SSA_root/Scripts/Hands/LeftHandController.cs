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
        //leftGrabController = GetComponentInChildren<GrabController>();
    }

    public override void OnCurrentItemEquipped()
    {
        leftWeapon = GetComponentInChildren<BaseWeaponShooterProjectile>();
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
        if (Input.GetKeyDown(KeyCode.H))
        {
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
