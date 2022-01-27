using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShoot : MonoBehaviour
{
    private bool bowFire = false;
    private bool stringReturn = false;
    private float stringTime = 0f;
    private float stringTimeCorrection = 0f;

    private Animator gunAnimator;

    public Transform stringPos;
    private Vector3 stringStartPos;
    public Transform stringEndPos;

    public GameObject arrowPrefab;

    private float stringSpeed = 130f;
    private float shootSpeed = 650f;

    void Start()
    {
        gunAnimator = GetComponent<Animator>();
        stringStartPos = stringPos.localPosition;
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0)) //&& !WeaponController.s_reloading)
        {
            stringReturn = false;

            // Calculating string time
            stringTime += Time.deltaTime;

            // String on maximum size
            if (stringPos.localPosition.y > stringEndPos.localPosition.y)
            {
                stringPos.localPosition += new Vector3(0, -0.001f, 0) * stringSpeed * Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            bowFire = true;
            stringTimeCorrection = stringTime;
            stringTime = 0;

            if (stringTimeCorrection > 1f)
                stringTimeCorrection = 1f;

        }

        // Return string to start position with physics correction
        if (bowFire && stringPos.localPosition.y <= stringStartPos.y + 0.01f * stringTimeCorrection)
        {
            stringPos.localPosition += new Vector3(0, 0.001f, 0) * shootSpeed * Time.deltaTime * stringTimeCorrection;
        }
        else if (bowFire)
        {
            stringReturn = true;
            ArrowShoot();
        }

        // Return string to start position after physics correction
        if (stringReturn && stringPos.localPosition.y > stringStartPos.y)
        {
            bowFire = false;
            stringPos.localPosition += new Vector3(0, -0.001f, 0) * shootSpeed * Time.deltaTime * stringTimeCorrection;
        }
        
    }

    void ArrowShoot()
    {
        var arrow = stringPos.gameObject.transform.GetChild(0);
        arrow.parent = null;

        arrow.gameObject.AddComponent<Rigidbody>();
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.up * 20 * stringTimeCorrection;

        var newArrow = Instantiate(arrowPrefab, stringPos.position, stringPos.rotation);
        newArrow.transform.SetParent(stringPos);

    }
}