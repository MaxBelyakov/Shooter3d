using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float depth = 0.3f;                         // Depth that arrow move in target

    private bool firstCollision = true;                 // Flag that signal about first arrow collision

    public GameObject impactStandartEffect;             // Impact standart effect prefab
    public GameObject impactStoneEffect;                // Impact stone effect prefab
    public GameObject impactMetalEffect;                // Impact metal effect prefab

    public AudioClip hitWoodAudio;                      // Sound of hit arrow in wood
    public AudioClip impactAudioStandart;               // Sound of standart arrow impact
    public AudioClip impactAudioMetal;                  // Sound of arrow impact in metal
    public AudioClip dropAudio;                         // Sound of arrow drop

    void OnCollisionEnter(Collision collision)
    {
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null)
        {
            // Arrow get stuck in wood just at first collision
            firstCollision = false;

            // Arrow behavior when hit in wood
            if (collision.transform.GetComponent<Renderer>().material.name == "laminate (Instance)"
                || collision.transform.GetComponent<Renderer>().material.name == "wooden box (Instance)"
                || collision.transform.GetComponent<Renderer>().material.name == "Military target (Instance)"
                || collision.transform.GetComponent<Renderer>().material.name == "BulletDecalWood (Instance)")
            {
                // Stop the arrow and remove physic body
                this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(this.GetComponent<Rigidbody>());

                // Remove box collider to ignore collision with player, but save triggered collider to take arrow in inventory
                foreach (BoxCollider col in this.GetComponents<BoxCollider>())
                    if (col.isTrigger == false)
                        Destroy(col);
                
                // Move arrow inside wood
                this.transform.Translate(depth * Vector2.up);

                // Stop target velocity
                if (collision.transform.GetComponent<Rigidbody>() != null)
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                
                // Fix: arrow become flat when hit the floor. It is because of deifference with the floor scale
                if (collision.transform.GetComponent<Renderer>().material.name == "laminate (Instance)")
                    this.transform.parent = collision.transform.parent;
                else
                    this.transform.parent = collision.transform;

                // Arrow hit sound
                this.transform.GetComponent<AudioSource>().PlayOneShot(hitWoodAudio);

            } else {

                // Check the object for stone and metal to choose effect style
                GameObject impactEffect = impactStandartEffect;
                AudioClip impactAudio = impactAudioStandart;
                if (collision.transform.GetComponent<Renderer>().material.name == "stone wall (Instance)")
                    impactEffect = impactStoneEffect;

                if (collision.transform.GetComponent<Renderer>().material.name == "MetalSurface (Instance)")
                {
                    impactEffect = impactMetalEffect;
                    impactAudio = impactAudioMetal;
                }

                // Impact sound
                this.transform.GetComponent<AudioSource>().PlayOneShot(impactAudio);

                // Create an impact effect
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                GameObject impact = Instantiate(impactEffect, pos, rot);
                Destroy(impact, 1f);
            }
        } else if (!firstCollision && collision.relativeVelocity.magnitude > 3f && collision.transform.name != "Player")
            // Check collision speed if fall fast play sound and ignore player collision
            this.GetComponent<AudioSource>().PlayOneShot(dropAudio);
    }
}