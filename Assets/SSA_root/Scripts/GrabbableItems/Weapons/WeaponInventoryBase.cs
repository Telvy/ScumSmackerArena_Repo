using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryBase : MonoBehaviour
{
    public event EventHandler OnRightHandEquipped;
    public event EventHandler OnLeftHandEquipped;

    public event EventHandler OnRightHandDequipped;
    public event EventHandler OnLeftHandDequipped;

    //public event EventHandler On

    [SerializeField] private float i_radius;

    [SerializeField] private bool b_rightHandFull;
    [SerializeField] private bool b_leftHandFull;
    [SerializeField] private LayerMask l_weapons;
    [SerializeField] private Transform t_righthand, t_lefthand;
    


    private void Awake()
    {
        
    }

    private void Start()
    {
        this.OnRightHandEquipped += WeaponInventoryBase_OnRightHandEquipped;
        this.OnLeftHandEquipped += WeaponInventoryBase_OnLeftHandEquipped;
        this.OnRightHandDequipped += WeaponInventoryBase_OnRightHandDequipped;
        this.OnLeftHandDequipped += WeaponInventoryBase_OnLeftHandDequipped;


    }

    private void Update()
    {
        CheckForItems();
        //PreventPickup();
    }

  

    //checks for items within vicinity of the player
    private void CheckForItems()
    {
        Collider[] items = Physics.OverlapBox(transform.position, new Vector3(1, 5, 1), Quaternion.identity, l_weapons);
        for (int i = 0; i < items.Length; i++)
        {
            GrabController grabController = items[0].GetComponent<GrabController>();
            BaseWeaponShooterProjectile projectileShooter = items[0].GetComponent<BaseWeaponShooterProjectile>();
            grabController.OnItemDropped += GrabController_OnItemDropped;

            if (Input.GetKeyDown(KeyCode.E) && !b_rightHandFull)
            {
                grabController.GrabItem(t_righthand);
                projectileShooter.handSlot = BaseWeaponShooterProjectile.HandSlot.RightHand;
                OnRightHandEquipped?.Invoke(this, EventArgs.Empty);

            }
            else if (Input.GetKeyDown(KeyCode.Q) && !b_leftHandFull)
            {
                grabController.GrabItem(t_lefthand);
                projectileShooter.handSlot = BaseWeaponShooterProjectile.HandSlot.LeftHand;
                OnLeftHandEquipped?.Invoke(this, EventArgs.Empty);
            }

          
        }
    }

    private void WeaponInventoryBase_OnRightHandEquipped(object sender, System.EventArgs e)
    {
        OnRightHandDequipped?.Invoke(this, EventArgs.Empty);
        PreventRightHandGrab();
        //this.OnRightHandEquipped -= WeaponInventoryBase_OnRightHandEquipped;
    }

    private void WeaponInventoryBase_OnLeftHandEquipped(object sender, System.EventArgs e)
    {
        
        PreventLeftHandGrab();
        //this.OnLeftHandEquipped -= WeaponInventoryBase_OnLeftHandEquipped;
    }

    private void GrabController_OnItemDropped(object sender, System.EventArgs e)
    {
        ResetRightHandGrab();
        ResetLeftHandGrab();
    }

    private void WeaponInventoryBase_OnRightHandDequipped(object sender, System.EventArgs e)
    {
        ResetRightHandGrab();
    }

    private void WeaponInventoryBase_OnLeftHandDequipped(object sender, System.EventArgs e)
    {
        ResetLeftHandGrab();
    }


    //Note: How come the same logic behind PreventItemGrab() and PreventRightHand() the same, but the parameters for PreventItemGrab() doesn't make it work?

    //private static bool PreventItemGrab(Transform t_hand, bool b_slotFull)
    //{ 
    //    if (t_hand.transform.childCount > 0)
    //    {
    //        Debug.Log(t_hand.transform.childCount);
    //        b_slotFull = true;
    //    }
    //    return b_slotFull;
    //}

    private bool PreventRightHandGrab()
    {
        if (t_righthand.transform.childCount > 0)
        { 
            b_rightHandFull = true;
        }
        return b_rightHandFull;
    }

    private bool PreventLeftHandGrab()
    {
        if (t_lefthand.transform.childCount > 0)
        { 
            b_leftHandFull = true;
        }
        return b_leftHandFull;
    }

    private bool ResetRightHandGrab()
    {
        return b_rightHandFull = false;
    }

    private bool ResetLeftHandGrab()
    {
        return b_leftHandFull = false;
    }


    //private void PreventPickup()
    //{
    //    if (t_righthand.transform.childCount > 0)
    //    {
    //        b_rightHandFull = true;
    //    }
    //    else
    //    {
    //        b_rightHandFull = false;
    //    }

    //    if (t_lefthand.transform.childCount > 0)
    //    {
    //        b_leftHandFull = true;
    //    }
    //    else
    //    {
    //        b_leftHandFull = false;
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(3,5,3));
        
    }


}


