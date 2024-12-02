using UnityEngine;

public class Trajector : MonoBehaviour
{
    public Vector3 offset_position; 
    public float speed = 5f;
    public float curveValue = 5f;
    public AnimationCurve curve;
    public Color gizmoColor = Color.green;
    public int resolution = 30;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float totalTravelTime;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = transform.position + offset_position;

        float distance = Vector3.Distance(startPosition, targetPosition);
        totalTravelTime = distance / speed;
    }

    void Update()
    {
        float timeElapsed = Mathf.Min(Time.timeSinceLevelLoad, totalTravelTime);

        float t = timeElapsed / totalTravelTime;
        if (t <= 1f)
        {
            Vector3 linearPosition = Vector3.Lerp(startPosition, targetPosition, t);

            float curveOffset = curve.Evaluate(t) * curveValue;

            Vector3 curvedPosition = new(linearPosition.x, linearPosition.y + curveOffset, linearPosition.z);

            transform.position = curvedPosition;
        }
    }

    // Draw trajectory gizmos in the editor
    private void OnDrawGizmos()
    {
        if (curve == null)
            return;

        Gizmos.color = gizmoColor;

        // Calculate trajectory
        Vector3 previousPoint = transform.position; // Start at the current position
        Vector3 endPoint = transform.position + offset_position;

        float distance = Vector3.Distance(previousPoint, endPoint);
        float travelTime = distance / speed;

        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution; // Normalized time (0 to 1)
            float time = t * travelTime;

            Vector3 linearPoint = Vector3.Lerp(previousPoint, endPoint, t);

            // Apply the curve offset
            float curveOffset = curve.Evaluate(t);
            Vector3 curvedPoint = new Vector3(linearPoint.x, linearPoint.y + curveOffset, linearPoint.z);

            // Draw line segment
            Gizmos.DrawLine(previousPoint, curvedPoint);

            // Update previous point
            previousPoint = curvedPoint;
        }
    }
}