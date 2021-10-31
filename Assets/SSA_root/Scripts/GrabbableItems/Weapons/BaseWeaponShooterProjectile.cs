using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this script handles the basic shooting logic for SSA. One class for projectiles and the other for hitscan

public class BaseWeaponShooterProjectile : MonoBehaviour
{
    public enum FireMode
    {
        SemiAuto,
        FullAuto
    }

    public enum HandSlot
    {
        RightHand,
        LeftHand
    }

    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //Recoil
    private Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    private GameObject playerRef;
    private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    private BoxCollider boxCollider;

    //Graphics
    public GameObject muzzleFlash;
    //public TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    public bool allowInvoke = true;

    //can shoot
    private bool canShoot;

    //which hand equipped to fire

    [Header("Fire Mode")]
    [SerializeField] FireMode fireMode;

    public HandSlot handSlot;




    private void Awake()
    {
        ReferenceSetup();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void ReferenceSetup()
    {
        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            if (this.transform.GetChild(i).transform.name == "AttackPoint")
            {
                attackPoint = this.transform.GetChild(i);
            }
        }

        playerRef = GameObject.FindGameObjectWithTag("Player");
        fpsCam = playerRef.GetComponentInChildren<Camera>();
        playerRb = playerRef.GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
      

        canShoot = false;
        GrabController grabController = GetComponent<GrabController>();
        grabController.OnItemGrabbed += GrabController_OnItemGrabbed;
        grabController.OnItemDropped += GrabController_OnItemDropped;

        WeaponInventoryBase inventory = playerRef.GetComponentInChildren<WeaponInventoryBase>();
        inventory.OnRightHandEquipped += WeaponInventoryBase_OnRightHandEquipped;
        inventory.OnLeftHandEquipped += WeaponInventoryBase_OnLeftHandEquipped;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists :D
        //if (ammunitionDisplay != null)
        //    ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void WeaponInventoryBase_OnRightHandEquipped(object sender, System.EventArgs e)
    {
        handSlot = HandSlot.RightHand;
        WeaponInventoryBase inventory = playerRef.GetComponentInChildren<WeaponInventoryBase>();
        inventory.OnRightHandEquipped -= WeaponInventoryBase_OnRightHandEquipped;
    }

    private void WeaponInventoryBase_OnLeftHandEquipped(object sender, System.EventArgs e)
    {
        handSlot = HandSlot.LeftHand;
        WeaponInventoryBase inventory = playerRef.GetComponentInChildren<WeaponInventoryBase>();
        inventory.OnLeftHandEquipped -= WeaponInventoryBase_OnLeftHandEquipped;
    }


    private void GrabController_OnItemDropped(object sender, System.EventArgs e)
    {
        Debug.Log("Item Dropped!");
        boxCollider.enabled = true;
        canShoot = false;
    }

    private void GrabController_OnItemGrabbed(object sender, System.EventArgs e)
    {
        Debug.Log("Item Grabbed!");
        boxCollider.enabled = false;
        canShoot = true;
    }

    private void MyInput()
    {

        //switch (fireMode)
        //{
        //    case FireMode.FullAuto:
        //        shooting = Input.GetKey(KeyCode.Mouse0);
        //        break;
        //    case FireMode.SemiAuto:
        //        shooting = Input.GetKeyDown(KeyCode.Mouse0);
        //        break;
        //}

        switch (handSlot)
        {
            case HandSlot.RightHand:
                shooting = Input.GetMouseButton(1);

                break;
            case HandSlot.LeftHand:
                shooting = Input.GetMouseButton(0);
                break;
        }

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }



    }

    private void Shoot()
    {
        if (canShoot)
        {
            readyToShoot = false;

            //Find the exact hit position using a raycast
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
            RaycastHit hit;

            //check if ray hits something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player

            //Calculate direction from attackPoint to targetPoint
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            //Calculate spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate new direction with spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

            //Instantiate bullet/projectile
            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
                                                                                                       //Rotate bullet to shoot direction
            currentBullet.transform.forward = directionWithSpread.normalized;

            //Add forces to bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

            //Instantiate muzzle flash, if you have one
            if (muzzleFlash != null)
                Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            bulletsLeft--;
            bulletsShot++;

            //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;

                //Add recoil to player (should only be called once)
                playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
            }

            //if more than one bulletsPerTap make sure to repeat shoot function
            if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }
}


