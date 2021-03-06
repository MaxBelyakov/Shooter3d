/*
General class that defines shoot effects (flash, impact, holes).
Class is inherited by "MachineGunShoot" and "PistolShoot" classes
*/

using UnityEngine;
using System.Collections.Generic;

public class ShootEffects : MonoBehaviour
{
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    public static GameObject impactEffect;
    public static GameObject bulletHoleEffect;

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
            // Set default impact and hole effects
            impactEffect = impactStandartEffect;
            bulletHoleEffect = null;
            
            // Check the object for wood, stone, metal to choose effect style
            if (hit.transform.GetComponent<Renderer>() != null)
            {
                string materialName = MaterialCheck(hit.transform.GetComponent<Renderer>().material.name);
                if (materialName == "stone")
                {
                    impactEffect = impactStoneEffect;
                    bulletHoleEffect = bulletHoleStoneEffect;
                } else if (materialName == "wood") {
                    impactEffect = impactWoodEffect;
                    bulletHoleEffect = bulletHoleWoodEffect;
                } else if (materialName == "metal") {
                    impactEffect = impactMetalEffect;
                    bulletHoleEffect = bulletHoleMetalEffect;
                }
            }

            // Iron chain destroy on hit with no impact and bullet hole effects
            if (hit.transform.tag == "chain" && hit.transform.GetComponent<HingeJoint>() != null)
            {
                Destroy(hit.transform.GetComponent<HingeJoint>());
                impactEffect = impactStandartEffect;
                bulletHoleEffect = null;
            }

            // Hit dummy target, check for "dummy" tag also parent object because bullet can hit the bullet hole
            if ((hit.transform.tag == "dummy" || hit.transform.parent?.tag == "dummy") 
                && DummyGenerator.s_dummyWeapon == WeaponController.s_weapon)
            {
                // Dummy weapon compares with current player weapon and start drop the dummy
                hit.transform.gameObject.AddComponent<Rigidbody>();
                hit.transform.gameObject.transform.GetComponent<Rigidbody>().mass = DummyGenerator.s_dummyMass;
                hit.transform.tag = "Untagged";
                DummyGenerator.s_dummy = false;
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

    // Define what type of material hit the bullet
    public string MaterialCheck(string target)
    {
        // Stone materials
        List<string> stoneMaterialsList = new List<string>
        {"stone wall (Instance)", "stone_wall_2_d (Instance)", "stone_wall_3_d (Instance)", "concrete_1_mat (Instance)",
        "granite_1_d (Instance)"};

        // Wood materials
        List<string> woodMaterialsList = new List<string>
        {"laminate (Instance)", "wooden box (Instance)", "wood_1_d (Instance)", "Chairs_MAT (Instance)",
        "Military target (Instance)", "timber_1_fixed_d (Instance)", "wooden-boards-texture_d (Instance)",
        "BulletDecalWood (Instance)", "paper (Instance)", "_wood_barrel_mat (Instance)", "bag_mat 2 (Instance)",
        "wood_3_d (Instance)", "bark_2_d (Instance)", "trunk_1_d (Instance)", "Grass (Instance)", "Bathroom_Oven_MAT (Instance)",
        "platform_mat_b (Instance)", "Pistol dummy (Instance)", "Machine gun dummy (Instance)", "Shotgun dummy (Instance)",
        "Bow dummy (Instance)"};

        // Metal materials
        List<string> metalMaterialsList = new List<string>
        {"MetalSurface (Instance)", "cont_big_mat_2 (Instance)", "_locker_mat (Instance)", "_mat_lock (Instance)", 
        "cola_can (Instance)", "barrel_mat_1 (Instance)", "Barrel 1 (Instance)", "walls_mat (Instance)", "iron (Instance)",
        "dark_iron (Instance)"};

        if (stoneMaterialsList.Contains(target))
            return "stone";
        else if (woodMaterialsList.Contains(target))
            return "wood";
        else if (metalMaterialsList.Contains(target))
            return "metal";
        else
            return null;
    }
}