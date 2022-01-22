using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int pistolMagazineInventoryAll = 3;                  // Max value of pistol magazines in inventory
    public static int s_pistolMagazineInventoryCurrent = 0;     // Current value of pistol magazines in inventory
    public TMP_Text pistolMagazineInventoryText;                // Text ammo status in inventory

    public TMP_Text popup;                                      // Pop up text to interact with object

    public AudioClip a_pistolMagazineTake;                      // Take magazine sound effect

    void Update()
    {
        // Clear popups
        popup.text = "";

        // Update ammo status text
        pistolMagazineInventoryText.text = s_pistolMagazineInventoryCurrent + " / " + pistolMagazineInventoryAll;

        // Check for target
        if (InspectTarget.targetInfo != null)
        {
            // Ask to take ammo
            if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo")
                popup.text = "To take press 'E'";

            // Take ammo
            if (Input.GetButtonDown("Submit"))
            {
                // Put pistol ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo")
                {
                    if (s_pistolMagazineInventoryCurrent < pistolMagazineInventoryAll) {
                        s_pistolMagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_pistolMagazineTake);
                    }
                }
            }
        }
    }
}