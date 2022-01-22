using UnityEngine;

public class CasingSounds : MonoBehaviour
{
    public AudioClip pistolCasingAudio;

    void OnCollisionEnter(Collision collision)
    {
        // Check collision speed if fall fast play sound and ignore player collision
        if (collision.relativeVelocity.magnitude > 3f && collision.transform.name != "Player")
            this.GetComponent<AudioSource>().PlayOneShot(pistolCasingAudio);
    }
}