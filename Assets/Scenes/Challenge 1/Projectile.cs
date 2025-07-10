using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public AudioClip explosionSound;

    private Rigidbody2D rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    public void Launch(Vector2 initialVelocity)
    {
        rb.linearVelocity = initialVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        if (collision.gameObject.CompareTag("Target"))
        {
            hasHit = true;

            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            FindObjectOfType<Manager>().OnTargetHit();

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}