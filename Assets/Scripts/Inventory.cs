using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int pistolMagazineInventoryAll = 3;                           // Max value of pistol magazines in inventory
    public static int s_pistol_MagazineInventoryCurrent = 0;             // Current value of pistol magazines in inventory
    public TMP_Text pistol_MagazineInventoryText;                        // Text ammo status in inventory

    public int machineGun_MagazineInventoryAll = 3;                     // Max value of machine gun magazines in inventory
    public static int s_machineGun_MagazineInventoryCurrent = 0;        // Current value of machine gun magazines in inventory
    public TMP_Text machineGun_MagazineInventoryText;                   // Text ammo status in inventory

    public TMP_Text popup;                                              // Pop up text to interact with object

    public AudioClip a_magazineTake;                                    // Take magazine sound effect

    void Update()
    {
        // Clear popups
        popup.text = "";

        // Update ammo status text
        pistol_MagazineInventoryText.text = s_pistol_MagazineInventoryCurrent + " / " + pistolMagazineInventoryAll;
        machineGun_MagazineInventoryText.text = s_machineGun_MagazineInventoryCurrent + " / " + machineGun_MagazineInventoryAll;

        // Check for target
        if (InspectTarget.targetInfo != null)
        {
            // Ask to take ammo
            if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo" || InspectTarget.targetInfo.TargetItem == "machine gun ammo")
                popup.text = "To take press 'E'";

            // Take ammo after check for shooting or reloading
            if (Input.GetButtonDown("Submit") && !WeaponController.s_shooting && !WeaponController.s_reloading)
            {
                // Put pistol ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo")
                {
                    if (s_pistol_MagazineInventoryCurrent < pistolMagazineInventoryAll) {
                        s_pistol_MagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put machine gun ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "machine gun ammo")
                {
                    if (s_machineGun_MagazineInventoryCurrent < machineGun_MagazineInventoryAll) {
                        s_machineGun_MagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
            }
        }
    }
}