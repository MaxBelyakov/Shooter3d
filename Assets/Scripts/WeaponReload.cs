using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    private GameObject magazine;            // Child of pistol magazine location 

    public GameObject magazinePrefab;       // Magazine Prefab
    public Transform magazineLocation;      // Magazine location

    public AudioClip reloadAudio;

    public static bool s_reloading = false;

    void Start()
    {
        // Find magazine in magazin location
        magazine = magazineLocation.GetChild(0).gameObject;
    }

    void Update()
    {   
        // Waiting for reload button
        if (Input.GetButtonDown("Reload") && !WeaponController.s_shooting)
        {
            if (WeaponController.pistolMagazineCurrent < WeaponController.pistolMagazineAll && Inventory.s_pistolMagazineInventoryCurrent > 0)
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
        // Unpin magazin from pistol
        magazine.transform.parent = null;

        // Add rigidbody to drop magazin on the floor
        magazine.AddComponent<Rigidbody>();

    }

    // Add new magazine. Calling from Animation
    void AddMagazine()
    {   
        // Create new magazine
        GameObject newMagazine = Instantiate(magazinePrefab, magazineLocation.position, magazineLocation.rotation);

        // Pin new magazine to the pistol
        newMagazine.transform.parent = magazineLocation;
        magazine = newMagazine;

        // Add bullets to counter
        WeaponController.pistolMagazineCurrent = WeaponController.pistolMagazineAll;

        // Minus ammo in inventory
        Inventory.s_pistolMagazineInventoryCurrent --;

        s_reloading = false;
    }
}
