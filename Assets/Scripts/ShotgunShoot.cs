using UnityEngine;

public class ShotgunShoot : ShootEffects
{
    private Animator gunAnimator;
    public Transform barrelLocation;
    public Transform casingExitLocation;

    void Start()
    {
        gunAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1") && !WeaponController.s_reloading)
        {
            WeaponController.s_shooting = true;
            if (Shotgun.s_bulletsCurrent != 0)
            {
                //Calls animation on the gun that has the relevant animation events that will fire
                gunAnimator.SetTrigger("Fire");
            } else {
                // No bullets animation
                gunAnimator.SetTrigger("noBullets");
            }
        }
    }

    // This function creates the bullet behavior. Different of ShootEffect class. Call by Animation
    void Shoot()
    {   
        // Minus bullet from counter
        Shotgun.s_bulletsCurrent --;

        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(shotAudio);

        // Create the muzzle flash
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        // Destroy the muzzle flash effect
        Destroy(tempFlash, Shotgun.s_flashDestroyTimer);

        // Create buckshot
        Buckshot();
    }

    // This function creates a casing at the ejection slot. Call by Animation
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, Shotgun.s_ejectPower);

        // Finish shooting
        WeaponController.s_shooting = false;
    }

    void NoBulletsSounds()
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);

        // Finish shooting
        WeaponController.s_shooting = false;
    }

    // Generate random shotgun bullet points and creates impacts and holes
    void Buckshot()
    {
        // Do it for each random point
        for (int i = 0; i < Shotgun.s_buckshotBullets; i++)
        {
            // Random buckshot correction
            Vector3 correction = new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f));

            // Inspect target element and create effects
            RaycastHit hit;
            Camera FPSCamera = FPCharacter.GetComponent<Camera>();
            if (Physics.Raycast(FPSCamera.transform.position + correction, FPSCamera.transform.forward, out hit, Shotgun.s_bulletRange))
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
                    hit.rigidbody.AddForce(-hit.normal * Shotgun.s_shotPower);
                }
            }
        }
    }
}