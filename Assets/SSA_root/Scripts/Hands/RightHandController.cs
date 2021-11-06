using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class RightHandController : BaseHandController
{
    protected BaseWeaponShooterProjectile rightWeapon;
    protected GrabController rightGrabController;


    public void Start()
    {
        weaponInventoryBase.OnRightHandEquipped += OnCurrentItemEquipped;
    }
    public override void OnCurrentItemEquipped()
    {
        rightWeapon = GetComponentInChildren<BaseWeaponShooterProjectile>();
        rightGrabController = GetComponentInChildren<GrabController>();
    }

    public override void UseCurrentItem()
    {
        rightWeapon.MyInput(KeyCode.Mouse0);
    }

    public override void DropCurrentItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && rightWeapon != null)
        {
            weaponInventoryBase.ResetRightHandGrab();
            rightGrabController.DropItem();
        }
    }

    public void Update()
    {
        DropCurrentItem();

        if (rightWeapon != null)
        {
            rightWeapon.MyInput(KeyCode.Mouse1);
        }


        //if (Input.GetMouseButton(1) && rightWeapon != null)
        //{
        //    UseCurrentItem();
        //    Debug.Log("Right Weapon Fired");
        //}
    }
}

//Note for later: Can I use Generics for any void function?

//private void UseItem<T>(T param)
//{


//}