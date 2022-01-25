using UnityEngine;

public class PistolShoot : ShootEffects
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
            if (Pistol.bulletsCurrent != 0)
            {
                //Calls animation on the gun that has the relevant animation events that will fire
                gunAnimator.SetTrigger("Fire");
            } else {
                // No bullets animation
                gunAnimator.SetTrigger("noBullets");
            }
        }
    }

    //This function creates the bullet behavior. Call by Animation
    void Shoot()
    {   
        // Minus bullet from counter
        Pistol.bulletsCurrent -= 1;

        ShowShootingEffects(barrelLocation);
    }

    // This function creates a casing at the ejection slot. Call by Animation
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, Pistol.ejectPower);

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
}