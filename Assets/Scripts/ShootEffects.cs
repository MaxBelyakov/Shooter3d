/*
General class that defines shoot effects (flash, impact, holes).
Class is inherited by "MachineGunShoot" and "PistolShoot" classes
*/

using UnityEngine;

public class ShootEffects : MonoBehaviour
{
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

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

    public void ShowShootingEffects(Transform point, float destroyTimer, float range, float shotPower)
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(shotAudio);

        // Create the muzzle flash
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, point.position, point.rotation);
        // Destroy the muzzle flash effect
        Destroy(tempFlash, destroyTimer);

        // Inspect target element and create effects
        RaycastHit hit;
        Camera FPSCamera = FPCharacter.GetComponent<Camera>();
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range))
        {
            // Check the object for wood, stone, metal to choose effect style
            GameObject impactEffect = impactStandartEffect;
            GameObject bulletHoleEffect = null;
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
                if (hit.transform.GetComponent<Renderer>().material.name == "Military target (Instance)")
                {
                    impactEffect = impactWoodEffect;
                    bulletHoleEffect = bulletHoleWoodEffect;
                }
            }

            // Create an impact effect
            GameObject impact = Instantiate(impactEffect, hit.point + hit.normal * 0.02f, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);

            // Create bullet hole effect (rotate to player and move step from object)
            if (bulletHoleEffect != null)
            {
                GameObject bulletHole = Instantiate(bulletHoleEffect, hit.point + hit.normal * 0.0001f, Quaternion.LookRotation(-hit.normal));
                bulletHole.transform.SetParent(hit.transform);
            }

            // Add physics force to target
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * shotPower);
            }
        }
    }

    public void ShowCasingEffects(Transform point, float power)
    {
        //Create the casing
        GameObject casing = Instantiate(casingPrefab, point.position, point.rotation);
        
        //Add force on casing to push it out
        casing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(power * 0.7f, power), (point.position - point.right * 0.3f - point.up * 0.6f), 1f, 3f);
        //Add torque to make casing spin in random direction
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
    }
}