using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class HandController : BaseHandController
{
    protected BaseWeaponShooterProjectile handWeapon;
    protected GrabController handGrabController;


    public void Start()
    {
        handGrabController.OnItemGrabbed += OnCurrentItemEquipped;
    }
    public override void OnCurrentItemEquipped()
    {
        handWeapon = GetComponentInChildren<BaseWeaponShooterProjectile>();
        handGrabController = GetComponentInChildren<GrabController>();
    }

    public override void UseCurrentItem()
    {
        handWeapon.MyInput(KeyCode.Mouse0);
    }

    public override void DropCurrentItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && handWeapon != null)
        {
            //weaponInventoryBase.ResetRightHandGrab();
            handGrabController.DropItem();
        }
    }

    public void Update()
    {
        DropCurrentItem();

        if (handWeapon != null)
        {
            handWeapon.MyInput(KeyCode.Mouse0);
        }
    }
}
