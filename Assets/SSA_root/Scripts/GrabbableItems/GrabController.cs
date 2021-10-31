using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour, IGrabbableItem
{
    //Events for when an item is grabbed
    public event EventHandler OnItemGrabbed;
    public event EventHandler OnItemDropped;


    //[SerializeField] protected float f_pickupRange;
    [SerializeField] protected float f_dropForwardForce;
    [SerializeField] protected float f_dropUpwardForce;

    private bool b_isEquipped;
    

    private Rigidbody rb_itemRigidbody;
    private BoxCollider c_itemCollider;
    private GameObject t_player;

    [SerializeField] protected Transform t_righthand, t_lefthand;
    [SerializeField] protected Transform t_camera;
       
    private void Awake()
    {
        t_player = GameObject.FindGameObjectWithTag("Player");
        rb_itemRigidbody = GetComponent<Rigidbody>();
        c_itemCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (!b_isEquipped)
        {
            rb_itemRigidbody.isKinematic = false;
            c_itemCollider.isTrigger = false;
        }

        if (b_isEquipped)
        {
            rb_itemRigidbody.isKinematic = true;
            c_itemCollider.isTrigger = true;
            //b_slotFull = true;
        }

        
    }

    private void Update()
    {
        SetWeaponPosition();
    }

    private void SetWeaponPosition()
    {
        //Drop if equipped and "G" is pressed
        if (b_isEquipped && Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    public void GrabItem(Transform t_hand)
    {
        OnItemGrabbed?.Invoke(this, EventArgs.Empty);

        b_isEquipped = true;
        //b_slotFull = true;

        //Make weapon a child of the camera and move it to default position

        transform.SetParent(t_hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb_itemRigidbody.isKinematic = true;
        c_itemCollider.isTrigger = true;

        //Enable script
        //gunScript.enabled = true;
    }

    public void UseItem()
    {

    }

    public void DropItem()
    {
        OnItemDropped?.Invoke(this, EventArgs.Empty);
        

        b_isEquipped = false;
        //b_slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb_itemRigidbody.isKinematic = false;
        c_itemCollider.isTrigger = false;

        //Gun carries momentum of player
        //rb_itemRigidbody.velocity = t_player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb_itemRigidbody.AddForce(t_camera.forward * f_dropForwardForce, ForceMode.Impulse);
        rb_itemRigidbody.AddForce(t_camera.up * f_dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = UnityEngine.Random.Range(-1f, 1f);
        rb_itemRigidbody.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        //gunScript.enabled = false;
    }

    public void ThrowItem()
    {

    }

}
