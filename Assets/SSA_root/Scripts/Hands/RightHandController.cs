using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponShooter;

public class RightHandController : BaseHandController
{
    protected GameObject rWeaponObj;
    protected BaseWeaponShooterProjectile rWeaponBaseShooterProj;
    protected GrabController rightGrabController;


    public void Start()
    {
        weaponInventoryBase.OnRightHandEquipped += OnCurrentItemEquipped;
       
    }
    public override void OnCurrentItemEquipped()
    {
        rWeaponObj = gameObject.transform.GetChild(0).gameObject;
        rWeaponBaseShooterProj = rWeaponObj.GetComponent<BaseWeaponShooterProjectile>();
        rightGrabController = rWeaponObj.GetComponent<GrabController>();
    }

    public override void UseCurrentItem()
    {
        rWeaponBaseShooterProj.MyInput(KeyCode.Mouse1);
    }

    public override void DropCurrentItem()
    {
        if (rWeaponObj == null)
        {
            return;
        }
        else if(rWeaponObj != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                weaponInventoryBase.ResetRightHandGrab();
                rightGrabController.DropItem();
                rWeaponObj = null;
            }
        }
    }

    public void Update()
    {
        DropCurrentItem();

        if (rWeaponObj != null)
        {
            UseCurrentItem();
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