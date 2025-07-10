using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public AudioClip shootingSound;

    public void Aim(float angleInDegrees)
    {
        firePoint.transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    public void Fire(float angleInDegrees, float speed)
    {
        if (shootingSound != null)
        {
            AudioSource.PlayClipAtPoint(shootingSound, firePoint.position);
        }

        var projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        var projectileScript = projectileInstance.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            var angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            var initialVelocity = new Vector2(
                speed * Mathf.Cos(angleInRadians),
                speed * Mathf.Sin(angleInRadians)
            );

            projectileScript.Launch(initialVelocity);
        }
    }

    public Vector2 GetFirePointPosition()
    {
        return firePoint.position;
    }
}