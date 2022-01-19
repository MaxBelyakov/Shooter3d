using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public TMP_Text bulletsText;
    public static int pistolMagazineAll = 7;

    public static int pistolMagazineCurrent;

    void Start()
    {
        pistolMagazineCurrent = pistolMagazineAll;
    }

    void Update()
    {
        bulletsText.text = "Bullets: " + pistolMagazineCurrent.ToString() + " / " + pistolMagazineAll;
    }
}
