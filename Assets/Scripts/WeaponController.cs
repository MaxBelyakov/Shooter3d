using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public GameObject pistol;
    public GameObject machineGun;

    public TMP_Text bulletsText;

    public static bool s_shooting = false;
    public static bool s_reloading = false;

    private string weapon;

    void Start()
    {
        Pistol.bulletsCurrent = Pistol.bulletsAll;
        MachineGun.bulletsCurrent = MachineGun.bulletsAll;
        weapon = "Pistol";
        machineGun.SetActive(false);
        pistol.SetActive(true);
    }

    void Update()
    {
        if (weapon == "Machine Gun")
        {
            bulletsText.text = "Bullets: " + MachineGun.bulletsCurrent + " / " + MachineGun.bulletsAll;
        }
        if (weapon == "Pistol")
        {
            bulletsText.text = "Bullets: " + Pistol.bulletsCurrent + " / " + Pistol.bulletsAll;
        }

        if (Input.GetButtonDown("1"))
        {
            weapon = "Pistol";
            machineGun.SetActive(false);
            pistol.SetActive(true);
        }
        if (Input.GetButtonDown("2"))
        {
            weapon = "Machine Gun";
            pistol.SetActive(false);
            machineGun.SetActive(true);
        }
    }
}

public class Pistol : WeaponController
{
    public static int bulletsAll = 7;
    public static int bulletsCurrent;
    public static float ejectPower = 250f;
}

public class MachineGun : WeaponController
{
    public static int bulletsAll = 50;
    public static int bulletsCurrent;
    public static float ejectPower = 250f;
}