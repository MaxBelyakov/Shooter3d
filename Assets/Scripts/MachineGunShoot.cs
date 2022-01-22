using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunShoot : MonoBehaviour
{
    public GameObject machineGun_casingPrefab;
    /*public GameObject muzzleFlashPrefab;

    public GameObject impactStandartEffect;

    public GameObject impactStoneEffect;
    public GameObject bulletHoleStoneEffect;

    public GameObject impactWoodEffect;
    public GameObject bulletHoleWoodEffect;

    public GameObject impactMetalEffect;
    public GameObject bulletHoleMetalEffect;

    public GameObject FPCharacter;

    public AudioClip shotAudio;
    public AudioClip noBulletsAudio;

    */
    private Animator machineGun_Animator;

    public GameObject machineGun_ParticleSystem;

    /*[SerializeField] private Transform barrelLocation;*/
    public GameObject machineGun_casingExitLocation;
/*
    [Header("Settings")]
    [Tooltip("Specify time to destory flash object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 30f;
    */
    private float ejectPower = 250f;
    /*
    public float range = 100f;  // bullet working distance*/

    void Start()
    {
        if (machineGun_Animator == null)
            machineGun_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButton("Fire1")) //&& !WeaponReload.s_reloading)
        {
            /*if (WeaponController.pistolMagazineCurrent != 0)
            {*/
                //Calls animation on the gun that has the relevant animation events that will fire
                machineGun_Animator.SetTrigger("Shoot");
                if (!machineGun_ParticleSystem.GetComponent<ParticleSystem>().isEmitting)
                    machineGun_ParticleSystem.GetComponent<ParticleSystem>().Play();
            /*} else {
                // No bullets animation
                gunAnimator.SetTrigger("noBullets");
            }*/
        } else {
            if (machineGun_ParticleSystem.GetComponent<ParticleSystem>().isEmitting)
                machineGun_ParticleSystem.GetComponent<ParticleSystem>().Stop();
        }
    }


    //This function creates the bullet behavior
    /*void Shoot()
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
                if (hit.transform.GetComponent<Renderer>().material.name == "MetalSurface (Instance)")
                {
                    impactEffect = impactMetalEffect;
                    bulletHoleEffect = bulletHoleMetalEffect;
                }
            }

            // Create an impact effect
            GameObject impact = Instantiate(impactEffect, hit.point + hit.normal * 0.02f, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);

            // Create bullet hole effect (rotate to player and move step from object)
            GameObject bulletHole = Instantiate(bulletHoleEffect, hit.point + hit.normal * 0.0001f, Quaternion.LookRotation(-hit.normal));
            bulletHole.transform.SetParent(hit.transform);

            // Add physics force to target
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * shotPower);
            }
        }
    }
*/
    // This function creates a casing at the ejection slot
    void MachineGunCasingRelease()
    {
        //Create the casing
        Transform casingTransform = machineGun_casingExitLocation.transform;
        GameObject tempCasing = Instantiate(machineGun_casingPrefab, casingTransform.position, casingTransform.rotation);
        
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingTransform.position - casingTransform.right * 0.3f - casingTransform.up * 0.6f), 1f, 3f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
    }
/*
    void NoBulletsSounds()
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);
    }*/

}
