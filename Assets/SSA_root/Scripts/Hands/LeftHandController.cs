using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class LeftHandController : BaseHandController
{
    protected GameObject lWeaponObj;
    protected BaseWeaponShooterProjectile lWeaponBaseShooterProj;
    protected GrabController leftGrabController;

    public void Start()
    {
        weaponInventoryBase.OnLeftHandEquipped += OnCurrentItemEquipped;
        
    }

    public override void OnCurrentItemEquipped()
    {
        lWeaponObj = gameObject.transform.GetChild(0).gameObject;
        lWeaponBaseShooterProj = lWeaponObj.GetComponent<BaseWeaponShooterProjectile>();
        leftGrabController = lWeaponObj.GetComponent<GrabController>();
    }
    public override void OnCurrentItemDropped()
    {

    }

    public override void UseCurrentItem()
    {
        lWeaponBaseShooterProj.MyInput(KeyCode.Mouse0);
    }

    public override void DropCurrentItem()
    {
        if (lWeaponObj == null)
        {
            return;
        }
        else if (lWeaponObj != null)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                weaponInventoryBase.ResetLeftHandGrab();
                leftGrabController.DropItem();
                lWeaponObj = null;
            }
        }
    }

    public void Update()
    {
        DropCurrentItem();

        if (lWeaponObj != null)
        {
            UseCurrentItem();
        }
    }

}
