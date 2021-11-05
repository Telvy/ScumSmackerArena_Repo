using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryBase : MonoBehaviour
{
    public event Action OnRightHandEquipped = delegate { };
    public event Action OnLeftHandEquipped = delegate { };


    [SerializeField] private float i_radius;
    [SerializeField] public bool b_rightHandFull;
    [SerializeField] public bool b_leftHandFull;
    [SerializeField] private LayerMask l_weapons;
    [SerializeField] public Transform t_righthand, t_lefthand;



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
            

            if (Input.GetKeyDown(KeyCode.E) && !b_rightHandFull)
            {
                grabController.GrabItem(t_righthand);
                OnRightHandEquipped?.Invoke();
                grabController.OnItemGrabbed += OnItemGrabbed_OnRightHandGrabbed;
                grabController.OnItemDropped += OnItemDropped_OnRightHandDropped;
            }

            if (Input.GetKeyDown(KeyCode.Q) && !b_leftHandFull)
            {
                grabController.GrabItem(t_lefthand);
                OnLeftHandEquipped?.Invoke();
                grabController.OnItemGrabbed += OnItemGrabbed_OnLeftHandGrabbed;
            }
        }
    }

    private void OnItemGrabbed_OnRightHandGrabbed()
    {
        PreventRightHandGrab();
    }

    private void OnItemDropped_OnRightHandDropped()
    {
        ResetRightHandGrab();
    }

    private void OnItemGrabbed_OnLeftHandGrabbed()
    {
        PreventLeftHandGrab();
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

    public bool ResetRightHandGrab()
    {
        return b_rightHandFull = false;
        //b_rightHandFull = false;
    }

    public bool ResetLeftHandGrab()
    {
        return b_leftHandFull = false;
        //b_leftHandFull = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(3,5,3));
        
    }


}


