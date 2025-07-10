using UnityEngine;

public static class TrajectorySolver
{
    public static float? CalculateLaunchAngle(Vector2 startPosition, Vector2 targetPosition, float launchSpeed,
        float gravity)
    {
        Vector2 delta = targetPosition - startPosition;
        float dx = delta.x;
        float dy = delta.y;

        float speedSquared = launchSpeed * launchSpeed;
        float underSqrt = speedSquared * speedSquared - gravity * (gravity * dx * dx + 2 * dy * speedSquared);

        if (underSqrt < 0f)
            return null; // No solution

        float sqrt = Mathf.Sqrt(underSqrt);
        float angle1 = Mathf.Atan((speedSquared + sqrt) / (gravity * dx));
        float angle2 = Mathf.Atan((speedSquared - sqrt) / (gravity * dx));

        // Choose the lower angle (for a flatter trajectory)
        float angle = Mathf.Min(angle1, angle2);

        if (float.IsNaN(angle))
            return null;

        return angle;
    }
}