using UnityEngine;

public class MachineGunReload : MonoBehaviour
{
    public GameObject magazine;            // Child of pistol magazine location 

    public GameObject magazinePrefab;       // Magazine Prefab

    public AudioClip reloadAudio;

    public static bool s_reloading = false;

    void Update()
    {   
        // Waiting for reload button
        if (Input.GetButtonDown("Reload") && !WeaponController.s_shooting)
        {
            if (WeaponController.machineGun_MagazineCurrent < WeaponController.machineGun_MagazineAll && Inventory.s_machineGun_MagazineInventoryCurrent > 0)
            {
                s_reloading = true;

                // Reload audio effeect
                this.GetComponent<AudioSource>().PlayOneShot(reloadAudio);

                gameObject.GetComponent<Animator>().SetTrigger("reload");
            }
        }
    }

    // Drop magazine. Calling from Animation
    void DropMagazine()
    {   
        Instantiate(magazinePrefab, magazine.transform.position, magazine.transform.rotation);
    }

    // Add new magazine. Calling from Animation
    void AddMagazine()
    {
        // Add bullets to counter
        WeaponController.machineGun_MagazineCurrent = WeaponController.machineGun_MagazineAll;

        // Minus ammo in inventory
        Inventory.s_machineGun_MagazineInventoryCurrent --;

        s_reloading = false;
    }
}
