using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script handles the basic shooting logic for SSA. One class for projectiles and the other for hitscan
namespace WeaponShooter
{
    public class BaseWeaponShooterProjectile : MonoBehaviour
    {
        public enum FireMode
        {
            SemiAuto,
            FullAuto
        }

        [Header("Object References")]
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject playerRef;
        [SerializeField] private Camera fpsCam;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private BoxCollider boxCollider;

        [Header("Bullet Force")]
        [SerializeField] private float shootForce, upwardForce;

        [Header("Gun Stats")]
        [SerializeField] private float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
        [SerializeField] private int magazineSize, bulletsPerTap;
        [SerializeField] private bool allowButtonHold;
        [SerializeField] private int bulletsLeft, bulletsShot;

        [Header("Recoil")]
        [SerializeField] private Rigidbody playerRb;
        [SerializeField] private float recoilForce;

        [Header("Bool Values")]
        private bool shooting, readyToShoot, reloading;
        private bool canShoot;

        [Header("Visuals")]
        [SerializeField] private GameObject muzzleFlash;
        //public TextMeshProUGUI ammunitionDisplay;

        [Header("Fire Mode")]
        [SerializeField] FireMode fireMode;

        //bug fixing 
        public bool allowInvoke = true;

       

        private void Awake()
        {
            ReferenceSetup();
            bulletsLeft = magazineSize;
            readyToShoot = true;
        }

        private void ReferenceSetup()
        {
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
        }

        private void Update()
        {
            //MyInput();


            //Set ammo display, if it exists :D
            //if (ammunitionDisplay != null)
            //    ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }

        private void GrabController_OnItemDropped()
        {
            Debug.Log("Item Dropped!");
            canShoot = false;
        }

        private void GrabController_OnItemGrabbed()
        {
            Debug.Log("Item Grabbed!");
            canShoot = true;
        }

        public void MyInput(KeyCode mouse)
        {

            switch (fireMode)
            {
                case FireMode.FullAuto:
                    shooting = Input.GetKey(mouse);
                    break;
                case FireMode.SemiAuto:
                    shooting = Input.GetKeyDown(mouse);
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

        public void Shoot()
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

        public void ResetShot()
        {
            //Allow shooting and invoking again
            readyToShoot = true;
            allowInvoke = true;
        }

        public void Reload()
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }

        public void ReloadFinished()
        {
            //Fill magazine
            bulletsLeft = magazineSize;
            reloading = false;
        }
    }
}



