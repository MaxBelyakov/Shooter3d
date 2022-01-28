using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float depth = 0.3f;

    private bool firstCollision = true;

    public GameObject impactStandartEffect;
    public GameObject impactStoneEffect;
    public GameObject impactMetalEffect;

    public AudioClip hitWoodAudio;
    public AudioClip impactAudioStandart;
    public AudioClip impactAudioMetal;

    void OnCollisionEnter(Collision collision)
    {
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null)
        {
            firstCollision = false;

            if (collision.transform.GetComponent<Renderer>().material.name == "laminate (Instance)"
                || collision.transform.GetComponent<Renderer>().material.name == "wooden box (Instance)"
                || collision.transform.GetComponent<Renderer>().material.name == "Military target (Instance)")
            {
                this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(this.GetComponent<BoxCollider>());
                Destroy(this.GetComponent<Rigidbody>());
                
                this.transform.Translate(depth * Vector2.up);

                if (collision.transform.GetComponent<Rigidbody>() != null)
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                
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
                {
                    impactEffect = impactStoneEffect;
                }

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
        }
    }
}