using System;
using UnityEngine;

public class Projection : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float timeStep = 0.02f; // simulation interval per point

    /// <summary>
    /// Simulates the trajectory analytically from start to landing
    /// </summary>
    public void SimulateTrajectory(Vector3 startPos, Vector3 launchForce, Vector3 currentVelocity)
    {
        if (_line == null) return;

        Vector3 initialVelocity = currentVelocity + launchForce;

        // Solve for total flight time using quadratic equation: y = y0 + vy*t + 0.5*g*t^2
        float a = 0.5f * Physics.gravity.y;
        float b = initialVelocity.y;
        float c = startPos.y;
        
        float discriminant = b * b - 4 * a * c;
        float totalTime = 0f;
        if (discriminant >= 0f)
        {
            float t1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
            float t2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);
            totalTime = Mathf.Max(t1, t2); // choose the positive, realistic time
        }

        int pointsCount = Mathf.CeilToInt(totalTime / timeStep) + 1;
        _line.positionCount = pointsCount;

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i * timeStep;
            Vector3 position = startPos + initialVelocity * t + 0.5f * Physics.gravity * t * t;
            _line.SetPosition(i, position);
        }
    }

    /// <summary>
    /// Returns a point along the arc as a percentage (0 = start, 1 = end)
    /// </summary>
    public Tuple<Vector3, float> GetPoint(float percentageOfArcCompleted)
    {
        Vector3[] points = new Vector3[_line.positionCount];
        _line.GetPositions(points);

        int index = Mathf.Clamp(Mathf.RoundToInt(points.Length * percentageOfArcCompleted), 0, points.Length - 1);
        float time = index * timeStep;

        return new Tuple<Vector3, float>(points[index], time);
    }
}
