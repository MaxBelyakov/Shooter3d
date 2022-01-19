using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject impactStandartEffect;
    public GameObject impactStoneEffect;
    public GameObject impactWoodEffect;
    public GameObject bulletHoleStoneEffect;
    public GameObject bulletHoleWoodEffect;
    public GameObject FPCharacter;

    public AudioClip shotAudio;
    public AudioClip noBulletsAudio;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory flash object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    public float range = 100f;  // bullet working distance


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1"))
        {
            if (WeaponController.pistolMagazineCurrent != 0)
            {
                //Calls animation on the gun that has the relevant animation events that will fire
                gunAnimator.SetTrigger("Fire");
            } else {
                // No bullets animation
                gunAnimator.SetTrigger("noBullets");
            }
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {   
        // Minus bullet from counter
        WeaponController.pistolMagazineCurrent -= 1;

        // Shot sound effect
        this.GetComponent<AudioSource>().Play();

        if (muzzleFlashPrefab)
        {
            // Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            // Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        // Inspect target element and create effects
        RaycastHit hit;
        Camera FPSCamera = FPCharacter.GetComponent<Camera>();
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range))
        {
            // Check the object for wood, stone, metal to choose effect style
            GameObject impactEffect = impactStandartEffect;
            GameObject bulletHoleEffect = bulletHoleStoneEffect;
            if (hit.transform.GetComponent<Renderer>() != null)
            {
                if (hit.transform.GetComponent<Renderer>().material.name == "stone wall (Instance)")
                {
                    impactEffect = impactStoneEffect;
                    bulletHoleEffect = bulletHoleStoneEffect;
                }
                if (hit.transform.GetComponent<Renderer>().material.name == "laminate (Instance)")
                {
                    impactEffect = impactWoodEffect;
                    bulletHoleEffect = bulletHoleWoodEffect;
                }
                if (hit.transform.GetComponent<Renderer>().material.name == "wooden box (Instance)")
                {
                    impactEffect = impactWoodEffect;
                    bulletHoleEffect = bulletHoleWoodEffect;
                }
            }

            // Create an impact effect
            GameObject impact = Instantiate(impactEffect, hit.point + hit.normal * 0.02f, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);

            // Create bullet hole effect (rotate to player and move step from object)
            GameObject bulletHole = Instantiate(bulletHoleEffect, hit.point + hit.normal * 0.02f, Quaternion.LookRotation(-hit.normal));
            bulletHole.transform.SetParent(hit.transform);
        }
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
    }

    void NoBulletsSounds()
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);
    }

}
