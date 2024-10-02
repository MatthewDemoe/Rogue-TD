using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent (typeof(SplineContainer))]
public class TrackCreator : MonoBehaviour
{
    [SerializeField]
    int numPoints;

    [SerializeField]
    float maxDistance = 2.0f;
    float adjustmentMaxDistance => maxDistance * 0.9f;

    [SerializeField]
    float minDistance = 0.25f;
    float clusterAdjustmentDistance => minDistance * 3f;

    [SerializeField]
    float minAngle = 30.0f;
    float adjustmentAngle => minAngle * 1.1f;

    const float MIN_RANGE = -7.5f;
    const float MAX_RANGE = 7.5f;
    const int MAX_LOOPS = 25;

    float3 startPoint = Vector3.zero;
    float3 endPoint = Vector3.zero;
    BoxCollider boundsCollider;

    SplineContainer splineContainer;
    Spline mapSpline;

    bool allPointsInPlace = false;

    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        boundsCollider = GetComponentInChildren<BoxCollider>();

        AddPoints();
    }

    private void AddPoints()
    {
        List<float3> points = new();

        startPoint = CreatePointInBounds();
        startPoint = ((Vector3)startPoint).normalized;
        startPoint *= MAX_RANGE;

        startPoint = CreatePointOnEdge();

        points.Add(startPoint);

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(CreatePointInBounds());
        }

        endPoint = CreatePointOnEdge();

        while ((startPoint.x == endPoint.x || startPoint.y == endPoint.y) || Vector3.Distance(startPoint, endPoint) < maxDistance)
        {
            endPoint = CreatePointOnEdge();
        }

        points.Add(endPoint);

        points = AdjustSplinePoints(points);

        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new float3(points[i].x, points[i].z, points[i].y);
        }

        mapSpline = SplineFactory.CreateCatmullRom(points);
        splineContainer.AddSpline(mapSpline);
    }

    private float3 CreatePointOnEdge()
    {
        RaycastHit hit;

        float3 point = CreatePointInBounds();
        point = ((Vector3)point).normalized;
        point *= MAX_RANGE;

        Physics.Raycast(point * 2, -point, out hit, math.INFINITY);

        return hit.point;
    }

    float3 CreatePointInBounds()
    {
        float positionX = UnityEngine.Random.Range(MIN_RANGE, MAX_RANGE);
        float positionY = UnityEngine.Random.Range(MIN_RANGE, MAX_RANGE);

        return new float3(positionX, positionY, 0.0f); 
    }

    private List<float3> AdjustSplinePoints(List<float3> pointList)
    {
        int loopCounter = 0;

        while (!allPointsInPlace)
        {            
            if (loopCounter > MAX_LOOPS)
            {
                Debug.Log("Loop Max reached");
                break;
            }

            allPointsInPlace = true;
            loopCounter++;

            pointList = BreakUpClusters(pointList);
            pointList = FABRIK(pointList);
            pointList = AdjustAngles(pointList);           
        }

        //startPoint = 

        return pointList;
    }

    private List<float3> BreakUpClusters(List<float3> pointList)
    {    
        List<float3> adjustedPoints = pointList;

        for (int i = 0; i < adjustedPoints.Count - 1; i++)
        {
            for (int j = 1; j < adjustedPoints.Count - 2; j++)
            {
                if (i == j)
                    continue;

                if (Vector3.Distance(adjustedPoints[i], adjustedPoints[j]) < minDistance)
                {
                    allPointsInPlace = false;

                    adjustedPoints[j] = CreatePointInBounds();
                }
            }
        }

        return adjustedPoints;
    }

    private List<float3> AdjustAngles(List<float3> pointList)
    {
        List<float3> adjustedPoints = pointList;        
        float angle = 0.0f;

        for (int i = 1; i < adjustedPoints.Count - 3; i++)
        {
            Vector3 sideA = ((Vector3)(adjustedPoints[i - 1] - adjustedPoints[i])).normalized;
            Vector3 sideB = ((Vector3)(adjustedPoints[i + 1] - adjustedPoints[i])).normalized;
            angle = Vector3.Angle(sideA, sideB);

            if (angle < minAngle)
            {
                allPointsInPlace = false;
                sideB = Quaternion.AngleAxis(adjustmentAngle, Vector3.forward) * sideB;
                adjustedPoints[i + 1] = boundsCollider.ClosestPoint(adjustedPoints[i] + ((float3)sideB * clusterAdjustmentDistance));
            }
        }

        for (int i = adjustedPoints.Count - 2; i > 2; i--)
        {
            Vector3 sideA = ((Vector3)(adjustedPoints[i - 1] - adjustedPoints[i])).normalized;
            Vector3 sideB = ((Vector3)(adjustedPoints[i + 1] - adjustedPoints[i])).normalized;
            angle = Vector3.Angle(sideA, sideB);

            if (angle < minAngle)
            {
                allPointsInPlace = false;
                sideA = Quaternion.AngleAxis(-adjustmentAngle, Vector3.forward) * sideA;
                adjustedPoints[i - 1] = boundsCollider.ClosestPoint(adjustedPoints[i] + ((float3)sideA * clusterAdjustmentDistance));
            }
        }

        return adjustedPoints;
    }

    private List<float3> FABRIK(List<float3> pointList)
    {
        List<float3> adjustedPoints = pointList;

        float distance = 0.0f;

        for (int i = 0; i < adjustedPoints.Count - 1; i++)
        {
            distance = Vector3.Distance(adjustedPoints[i], adjustedPoints[i + 1]);

            if (distance > maxDistance)
            {
                allPointsInPlace = false;

                float3 direction = ((Vector3)(adjustedPoints[i + 1] - adjustedPoints[i])).normalized;
                float3 adjustedPoint = adjustedPoints[i] + (direction * adjustmentMaxDistance);

                adjustedPoints[i + 1] = boundsCollider.ClosestPoint(adjustedPoint);
            }
        }

        RaycastHit hit;

        endPoint = adjustedPoints[adjustedPoints.Count - 1];
        endPoint = ((Vector3)endPoint).normalized;
        endPoint *= MAX_RANGE;

        Physics.Raycast(endPoint * 2, -endPoint, out hit, math.INFINITY);
        adjustedPoints[adjustedPoints.Count - 1] = hit.point;

        for (int i = adjustedPoints.Count - 1; i > 0; i--)
        {
            distance = Vector3.Distance(adjustedPoints[i], adjustedPoints[i - 1]);

            if (distance > maxDistance)
            {
                allPointsInPlace = false;

                float3 direction = ((Vector3)(adjustedPoints[i - 1] - adjustedPoints[i])).normalized;
                float3 adjustedPoint = adjustedPoints[i] + (direction * adjustmentMaxDistance);

                adjustedPoints[i - 1] = boundsCollider.ClosestPoint(adjustedPoint);
            }
        }

        startPoint = adjustedPoints[0];
        startPoint = ((Vector3)startPoint).normalized;
        startPoint *= MAX_RANGE;

        Physics.Raycast(startPoint * 2, -startPoint, out hit, math.INFINITY);
        adjustedPoints[0] = hit.point;

        return adjustedPoints;
    }
}
