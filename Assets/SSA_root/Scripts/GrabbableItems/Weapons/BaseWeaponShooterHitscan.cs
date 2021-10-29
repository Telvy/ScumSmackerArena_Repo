using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponShooterHitscan : MonoBehaviour
{
    public enum FireMode
    {
        SemiAuto,
        FullAuto
    }


    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    // public TextMeshProUGUI text;

    private static bool canShoot;

    [Header("Fire Mode")]
    [SerializeField] FireMode fireMode;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
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
        if (canShoot)
        {
            MyInput();
        }

        //SetText
        // text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void GrabController_OnItemDropped(object sender, System.EventArgs e)
    {
        Debug.Log("Item Dropped!");
        canShoot = false;
    }

    private void GrabController_OnItemGrabbed(object sender, System.EventArgs e)
    {
        Debug.Log("Item Grabbed!");
        canShoot = true;
    }

    private void MyInput()
    {
        switch (fireMode)
        {
            case FireMode.FullAuto:
                shooting = Input.GetKey(KeyCode.Mouse0);
                break;
            case FireMode.SemiAuto:
                shooting = Input.GetKeyDown(KeyCode.Mouse0);
                break;
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<TestDummy>().TakeDamage();
        }

        //ShakeCamera
        if(camShake != null)
            StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //Graphics
        if(bulletHoleGraphic != null)
            Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        if(muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}


