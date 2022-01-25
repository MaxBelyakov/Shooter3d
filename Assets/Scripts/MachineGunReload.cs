using UnityEngine;

public class MachineGunReload : MonoBehaviour
{
    public GameObject magazine;             // Machine gun magazine location 
    public GameObject magazinePrefab;       // New magazine Prefab

    public AudioClip reloadAudio;           // Reload sound

    void Update()
    {   
        // Waiting for reload button
        if (Input.GetButtonDown("Reload") && !WeaponController.s_shooting)
        {
            if (MachineGun.bulletsCurrent < MachineGun.bulletsAll && Inventory.s_machineGun_MagazineInventoryCurrent > 0)
            {
                WeaponController.s_reloading = true;

                // Reload audio effeect
                this.GetComponent<AudioSource>().PlayOneShot(reloadAudio);

                this.GetComponent<Animator>().SetTrigger("reload");
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
        MachineGun.bulletsCurrent = MachineGun.bulletsAll;

        // Minus ammo in inventory
        Inventory.s_machineGun_MagazineInventoryCurrent --;

        WeaponController.s_reloading = false;
    }
}