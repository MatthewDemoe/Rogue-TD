using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof(SplineContainer))]
public class TrackCreator : MonoBehaviour
{
    [SerializeField]
    int numPoints;

    const float POISSON_RANGE = 50.0f;
    const int POISSON_RETRIES = 30;
    const float POISSON_PERCENTAGE = 0.85f;

    [SerializeField]
    MeshCollider trackCollider;
    Bounds trackBounds;

    float3 startPoint = Vector3.zero;
    float3 endPoint = Vector3.zero;

    SplineContainer splineContainer;
    Spline mapSpline;

    [SerializeField]
    List<Vector2> sampling = new();

    [SerializeField]
    float poissonRadius = 7.0f;

    [SerializeField]
    GameObject trackPlane;

    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        trackBounds = trackCollider.bounds;

        AddPoints();

        if(TryGetComponent(out SplineInstantiate splineInstantiate))
            splineInstantiate.UpdateInstances();        
    }

    private void AddPoints()
    {
        List<float3> points = new();
        startPoint = CreatePointOnEdge();
        points.Add(startPoint);
        
        sampling = PoissonDiskSampling(poissonRadius, POISSON_RETRIES);
        List<Vector2> subset = new();
        int index = 0;

        for (int i = 0; i < numPoints; i++)
        {
            index = UnityEngine.Random.Range(0, sampling.Count);

            subset.Add(sampling[index]);
            sampling.RemoveAt(index);
        }

        endPoint = CreatePointOnEdge();

        while (Vector3.Distance(startPoint, endPoint) < trackBounds.extents.magnitude)
        {
            endPoint = CreatePointOnEdge();
        }

        List<float3> unorderedPoints = new();
        List<float3> pointsClosestToStart = new();
        List<float3> pointsClosestToEnd = new();

        subset.ForEach(point =>
        {
            point /= POISSON_RANGE;
            float mappedX = UtilMath.Lmap(point.x, 0.0f, 1.0f, trackBounds.min.x, trackBounds.max.x);
            float mappedY = UtilMath.Lmap(point.y, 0.0f, 1.0f, trackBounds.min.z, trackBounds.max.z);

            unorderedPoints.Add(new float3(mappedX, mappedY, 0.0f) * POISSON_PERCENTAGE);
        });

        pointsClosestToStart = unorderedPoints.OrderBy(point => Vector3.Distance(point, startPoint)).ToList();
        pointsClosestToEnd = unorderedPoints.OrderBy(point => Vector3.Distance(point, endPoint)).ToList();

        int pointCounter = 0;
        while (unorderedPoints.Any())
        {
            float3 point = pointsClosestToStart[0];
            points.Insert(pointCounter + 1, point);

            unorderedPoints.Remove(point);
            pointsClosestToStart.Remove(point);
            pointsClosestToEnd.Remove(point);

            if (!pointsClosestToEnd.Any())
                break;

            point = pointsClosestToEnd[0];
            points.Insert(points.Count - pointCounter, point);

            unorderedPoints.Remove(point);
            pointsClosestToStart.Remove(point);
            pointsClosestToEnd.Remove(point);

            pointCounter++;
        }

        points.Add(endPoint);

        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new float3(points[i].x, points[i].z, points[i].y);
        }

        mapSpline = SplineFactory.CreateCatmullRom(points);
        splineContainer.AddSpline(mapSpline);
    }

    private float3 CreatePointOnEdge()
    {
        float positionX = UnityEngine.Random.Range(trackBounds.min.x, trackBounds.max.x);
        float positionZ = UnityEngine.Random.Range(trackBounds.min.z, trackBounds.max.z);

        Vector3 point = new Vector3(positionX, 0.0f, positionZ);

        Vector3 direction = point / point.magnitude;

        Vector3 outOfBounds = trackBounds.center + (direction * trackBounds.size.magnitude);
        Vector3 closestPoint = trackBounds.ClosestPoint(outOfBounds);

        return new float3(closestPoint.x, closestPoint.z, 0.0f);
    }

    Vector2 CreateVectorInBounds()
    {
        float positionX = UnityEngine.Random.Range(0, POISSON_RANGE);
        float positionY = UnityEngine.Random.Range(0, POISSON_RANGE);

        return new Vector2(positionX, positionY);
    }

    private bool IsValidPoint(Vector2[,] grid, float cellSize, int gridWidth, int gridHeight, Vector2 point, float radius)
    {
        if (point.x < 0 || point.x >= POISSON_RANGE || point.y < 0 || point.y >= POISSON_RANGE)
            return false;

        int xIndex = Mathf.FloorToInt(point.x / cellSize);
        int yIndex = Mathf.FloorToInt(point.y / cellSize);

        int i0 = Mathf.Max(xIndex - 1, 0);
        int i1 = Mathf.Min(xIndex + 1, gridWidth - 1);

        int j0 = Mathf.Max(yIndex - 1, 0);
        int j1 = Mathf.Min(yIndex + 1, gridHeight - 1);

        for (int i = i0; i <= i1; i++)
        {
            for (int j = j0; j <= j1; j++)
            {
                if (grid[i,j] != null)
                {
                    if (Vector2.Distance(grid[i,j], point) < radius)
                        return false;
                }
            }
        }

        return true;
    }

    void InsertPoint(Vector2[,] grid, float cellSize, Vector2 point)
    {
        int xIndex = Mathf.FloorToInt(point.x / cellSize);
        int yIndex = Mathf.FloorToInt(point.y / cellSize);

        grid[xIndex, yIndex] = point;
    }

    List<Vector2> PoissonDiskSampling(float radius, int k)
    {
        int N = 2;

        List<Vector2> finalPoints = new();
        List<Vector2> tempPoints = new();

        Vector2 p0 = CreateVectorInBounds();

        float cellSize = Mathf.Floor(radius / Mathf.Sqrt(N));

        int numWidthCells = Mathf.CeilToInt(POISSON_RANGE / cellSize) + 1;
        int numHeightCells = Mathf.CeilToInt(POISSON_RANGE / cellSize) + 1;

        Vector2[,] grid = new Vector2[numWidthCells, numHeightCells];

        InsertPoint(grid, cellSize, p0);
        tempPoints.Add(p0);

        while (tempPoints.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempPoints.Count);
            Vector2 p = tempPoints[randomIndex];

            bool found = false;
            for (int tries = 0; tries < k; tries++)
            {
                float theta = UnityEngine.Random.Range(0, 360);
                float newRadius = UnityEngine.Random.Range(radius, 2 * radius);

                float newX = p.x + newRadius * Mathf.Cos(Mathf.Deg2Rad * theta);
                float newY = p.y + newRadius * Mathf.Sin(Mathf.Deg2Rad * theta);
                Vector2 newP = new Vector2(newX, newY);

                if (!IsValidPoint(grid, cellSize, numWidthCells, numHeightCells, newP, radius))
                    continue;

                finalPoints.Add(newP);
                InsertPoint(grid, cellSize, newP);
                tempPoints.Add(newP);
                found = true;
                break;
            }

            if(!found)
                tempPoints.RemoveAt(randomIndex);
        }

        return finalPoints;
    }
}
