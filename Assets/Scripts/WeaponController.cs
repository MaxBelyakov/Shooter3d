using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public TMP_Text bulletsText;
    public static int pistolMagazineAll = 7;

    public static int pistolMagazineCurrent;

    public static int machineGun_MagazineAll = 50;

    public static int machineGun_MagazineCurrent;

    public static bool s_shooting;

    void Start()
    {
        pistolMagazineCurrent = pistolMagazineAll;
        machineGun_MagazineCurrent = machineGun_MagazineAll;
    }

    void Update()
    {
        //bulletsText.text = "Bullets: " + pistolMagazineCurrent.ToString() + " / " + pistolMagazineAll;
        bulletsText.text = "Bullets: " + machineGun_MagazineCurrent.ToString() + " / " + machineGun_MagazineAll;
    }
}
