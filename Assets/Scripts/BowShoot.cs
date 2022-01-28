using UnityEngine;

public class BowShoot : MonoBehaviour
{
    private bool bowFire = false;
    private bool stringReturn = false;
    private float stringTime = 0f;
    private float stringTimeCorrection = 0f;

    private bool noArrow = true;

    private Animator gunAnimator;

    public Transform stringPos;
    private Vector3 stringStartPos;
    public Transform stringEndPos;
    private Vector3 bowStartPos;

    public GameObject arrowPrefab;
    public AudioClip shootAudio;

    private float stringSpeed = 130f;
    private float shootSpeed = 850f;

    void Start()
    {
        gunAnimator = GetComponent<Animator>();
        stringStartPos = stringPos.localPosition;
        bowStartPos = this.transform.localPosition;
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            stringReturn = false;
            bowFire = false;

            // Calculating string time
            stringTime += Time.deltaTime;

            // Create new arrow with position correction
            if (noArrow)
            {
                var newArrow = Instantiate(arrowPrefab, stringPos.position + stringPos.right / 150, stringPos.rotation);
                newArrow.transform.SetParent(stringPos);
                noArrow = false;
            }

            // String on maximum size and get up the bow
            if (stringPos.localPosition.y > stringEndPos.localPosition.y)
            {
                stringPos.localPosition += new Vector3(0, -0.001f, 0) * stringSpeed * Time.deltaTime;
                
                // Get up the bow and limit the height
                if (this.transform.localPosition.y < -0.3f)
                    this.transform.localPosition += new Vector3(0, 0.01f, 0) * stringSpeed * Time.deltaTime;
            }
        } else {
            // Return bow to start position
            if (noArrow && this.transform.localPosition.y > bowStartPos.y)
                this.transform.localPosition += new Vector3(0, -0.01f, 0) * stringSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            bowFire = true;
            stringTimeCorrection = stringTime;
            stringTime = 0;

            if (stringTimeCorrection > 1f)
                stringTimeCorrection = 1f;

            // Shoot sound
            this.transform.GetComponent<AudioSource>().PlayOneShot(shootAudio);

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
        arrow.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.up * shootSpeed / 15 * stringTimeCorrection;

        noArrow = true;
    }
}