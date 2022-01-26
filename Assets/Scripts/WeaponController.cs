using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public GameObject pistol;
    public GameObject machineGun;
    public GameObject shotgun;

    public TMP_Text bulletsText;

    public static bool s_shooting = false;
    public static bool s_reloading = false;

    private string weapon;

    void Start()
    {
        Pistol.s_bulletsCurrent = Pistol.s_bulletsAll;
        MachineGun.s_bulletsCurrent = MachineGun.s_bulletsAll;
        Shotgun.s_bulletsCurrent = Shotgun.s_bulletsAll;
        weapon = "Pistol";
        machineGun.SetActive(false);
        pistol.SetActive(true);
        shotgun.SetActive(false);
    }

    void Update()
    {
        if (weapon == "Machine Gun")
        {
            bulletsText.text = "Bullets: " + MachineGun.s_bulletsCurrent + " / " + MachineGun.s_bulletsAll;
        }
        if (weapon == "Pistol")
        {
            bulletsText.text = "Bullets: " + Pistol.s_bulletsCurrent + " / " + Pistol.s_bulletsAll;
        }
        if (weapon == "Shotgun")
        {
            bulletsText.text = "Bullets: " + Shotgun.s_bulletsCurrent + " / " + Shotgun.s_bulletsAll;
        }

        if (Input.GetButtonDown("1"))
        {
            weapon = "Pistol";
            machineGun.SetActive(false);
            pistol.SetActive(true);
            shotgun.SetActive(false);
        }
        if (Input.GetButtonDown("2"))
        {
            weapon = "Machine Gun";
            pistol.SetActive(false);
            machineGun.SetActive(true);
            shotgun.SetActive(false);
        }
        if (Input.GetButtonDown("3"))
        {
            weapon = "Shotgun";
            pistol.SetActive(false);
            machineGun.SetActive(false);
            shotgun.SetActive(true);
        }
    }
}

public class Pistol : WeaponController
{
    public static int s_bulletsAll = 7;             // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 250f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 30f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class MachineGun : WeaponController
{
    public static int s_bulletsAll = 50;            // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 250f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 30f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class Shotgun : WeaponController
{
    public static int s_bulletsAll = 2;             // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 250f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 30f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
    public static int s_buckshotBullets = 6;        // Buckshot bullets amount
}