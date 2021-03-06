using UnityEngine;

public class PistolShoot : ShootEffects
{
    public Transform barrelLocation;
    public Transform casingExitLocation;

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1") && !WeaponController.s_reloading && !WeaponController.s_shooting)
        {
            WeaponController.s_shooting = true;
            if (Pistol.s_bulletsCurrent != 0)
            {
                //Calls animation on the gun that has the relevant animation events that will fire
                this.GetComponent<Animator>().SetTrigger("Fire");
            } else {
                // No bullets animation
                this.GetComponent<Animator>().SetTrigger("noBullets");
            }
        }
    }

    //This function creates the bullet behavior. Call by Animation
    void Shoot()
    {   
        // Minus bullet from counter
        Pistol.s_bulletsCurrent -= 1;

        ShowShootingEffects(barrelLocation, Pistol.s_flashDestroyTimer, Pistol.s_bulletRange, Pistol.s_shotPower);
    }

    // This function creates a casing at the ejection slot. Call by Animation
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, Pistol.s_ejectPower);

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