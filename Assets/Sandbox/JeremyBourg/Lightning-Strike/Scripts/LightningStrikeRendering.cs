using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningStrikeRendering : MonoBehaviour
{
    public Transform firePoint;
    public Transform target;
    [SerializeField] private int segments = 10;
    [SerializeField] private float jaggedness = 0.5f;
    [SerializeField] private float duration = 0.2f;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = segments + 1;
    }

    public void CreateLightning()
    {
        Vector3 startPoint = firePoint.position;
        Vector3 endPoint = target.position;

        Vector3[] points = new Vector3[segments + 1];
        points[0] = startPoint;
        points[segments] = endPoint;

        for (int i = 1; i < segments; i++)
        {
            float t = i / (float)segments;
            Vector3 interpolatedPoint = Vector3.Lerp(startPoint, endPoint, t);

            interpolatedPoint += new Vector3(
                Random.Range(-jaggedness, jaggedness),
                Random.Range(-jaggedness, jaggedness),
                Random.Range(-jaggedness, jaggedness)
            );

            points[i] = interpolatedPoint;
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }

        Invoke(nameof(DestroyLightning), duration);
    }

    void DestroyLightning()
    {
        Destroy(gameObject);
    }
        
}
