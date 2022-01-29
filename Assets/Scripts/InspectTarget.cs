using UnityEngine;

// Contains information about current target
public class TargetInfo
{
    public string Name { get; set; }                // Target name
    public float Distance { get; set; }             // Distance to target
    public string TargetItem { get; set; }          // Target inventory identificator
    public GameObject TargetObject { get; set; }    // Target object
}

// Collect information about current target
public class InspectTarget : MonoBehaviour
{
    public float submitDistance = 2f;               // Distance to interact with object

    public static TargetInfo targetInfo;            // Save inforamation about current target here

    void Update()
    {
        // Inspect target element
        RaycastHit target;
        Camera FPSCamera = this.GetComponent<Camera>();
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out target, submitDistance))
        {
            // Save information
            targetInfo = new TargetInfo 
            { 
                Name = target.transform.name, 
                Distance = target.distance, 
                TargetItem = target.transform.tag, 
                TargetObject = target.transform.gameObject 
            };
        } else
            targetInfo = null;
        
    }
}