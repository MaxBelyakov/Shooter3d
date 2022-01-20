using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    //public float submitDistance = 2f;                       // Distance to interact with object

    public int pistolMagazineInventoryAll = 3;              // Max value of pistol magazines in inventory
    public static int pistolMagazineInventoryCurrent = 0;   // Current value of pistol magazines in inventory
    public TMP_Text pistolMagazineInventoryText;            // Text ammo status in inventory

    public TMP_Text popup;                                  // Pop up text to interact with object

    void Update()
    {
        // Clear popups
        popup.text = "";

        // Update ammo status text
        pistolMagazineInventoryText.text = pistolMagazineInventoryCurrent + " / " + pistolMagazineInventoryAll;

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
                    if (pistolMagazineInventoryCurrent < pistolMagazineInventoryAll) {
                        pistolMagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);
                    }
                }
            }
        }
    }
}