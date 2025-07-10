using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Tham chiếu")] public GunController gun;
    public TargetController target;
    public Transform groundTransform;

    [Header("Cấu hình màn chơi")] public float launchSpeed = 15f;

    void Start()
    {
        SetupNewChallenge();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FireAtTarget();
        }
    }

    void SetupNewChallenge()
    {
        if (groundTransform == null)
        {
            return;
        }

        var groundRenderer = groundTransform.GetComponent<Renderer>();
        var targetRenderer = target.GetComponent<Renderer>();

        var groundTopY = groundTransform.position.y + groundRenderer.bounds.extents.y;
        var targetHalfHeight = targetRenderer.bounds.extents.y;
        var spawnY = groundTopY + targetHalfHeight;

        var groundLeftX = groundTransform.position.x - groundRenderer.bounds.extents.x;
        var groundRightX = groundTransform.position.x + groundRenderer.bounds.extents.x;

        var minTargetDistance = gun.GetFirePointPosition().x + 3f;
        var spawnMinX = Mathf.Max(groundLeftX, minTargetDistance);

        var spawnX = Random.Range(spawnMinX, groundRightX);
        target.transform.position = new Vector2(spawnX, spawnY);
    }

    void FireAtTarget()
    {
        var gunPosition = gun.GetFirePointPosition();
        var targetPosition = target.transform.position;

        float? launchAngleRad =
            TrajectorySolver.CalculateLaunchAngle(gunPosition, targetPosition, launchSpeed, -Physics2D.gravity.y);

        if (launchAngleRad.HasValue)
        {
            var launchAngleDeg = launchAngleRad.Value * Mathf.Rad2Deg;

            gun.Aim(launchAngleDeg);
            gun.Fire(launchAngleDeg, launchSpeed);
        }
    }

    public void OnTargetHit()
    {
        StartCoroutine(ResetChallengeCoroutine());
    }

    private IEnumerator ResetChallengeCoroutine()
    {
        var oldProjectile = FindObjectOfType<Projectile>();
        if (oldProjectile != null)
        {
            Destroy(oldProjectile.gameObject);
        }

        SetupNewChallenge();
        yield return null;
    }
}