using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;

[RequireComponent (typeof(SplineContainer))]
public class TrackCreator : MonoBehaviour
{
    [SerializeField]
    int numPoints;

    [SerializeField]
    float maxDistance = 2.0f;
    float adjustmentMaxDistance => maxDistance * 0.75f;

    [SerializeField]
    float minDistance = 0.25f;
    float adjustmentMinDistance => minDistance * 1.25f;

    [SerializeField]
    float minAngle = 30.0f;
    float adjustmentAngle => minAngle * 1.25f;

    const float MIN_RANGE = -7.5f;
    const float MAX_RANGE = 7.5f;
    const int MAX_LOOPS = 25;

    float3 startPoint = Vector3.zero;
    float3 endPoint = Vector3.zero;
    BoxCollider boundsCollider;

    SplineContainer splineContainer;
    Spline mapSpline;
    SplineExtrude splineExtrude;

    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        splineExtrude = GetComponent<SplineExtrude>();
        boundsCollider = GetComponentInChildren<BoxCollider>();

        AddPoints();
        splineExtrude.enabled = true;
    }

    private void AddPoints()
    {
        List<float3> points = new();

        startPoint = CreatePointInBounds();
        startPoint = ((Vector3)startPoint).normalized;
        startPoint *= MAX_RANGE;

        RaycastHit hit;

        Physics.Raycast(startPoint * 2, -startPoint, out hit, math.INFINITY);
        startPoint = hit.point;

        points.Add(startPoint);

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(CreatePointInBounds());
        }

        endPoint = CreatePointInBounds();

        while (Vector3.Distance(startPoint, endPoint) < maxDistance)
        {
            endPoint = CreatePointInBounds();
        }

        endPoint = ((Vector3)endPoint).normalized;
        endPoint *= MAX_RANGE;

        Physics.Raycast(endPoint * 2, -endPoint, out hit, math.INFINITY);
        endPoint = hit.point;

        points.Add(endPoint);

        points = BreakUpClusters(points);
        points = FABRIK(points);
        points = AdjustAngles(points);

        mapSpline = SplineFactory.CreateCatmullRom(points);
        splineContainer.AddSpline(mapSpline);
    }

    float3 CreatePointInBounds()
    {
        float positionX = UnityEngine.Random.Range(MIN_RANGE, MAX_RANGE);
        float positionY = UnityEngine.Random.Range(MIN_RANGE, MAX_RANGE);

        return new float3(positionX, positionY, 0.0f); 
    }

    List<float3> BreakUpClusters(List<float3> pointList)
    {
        List<float3> adjustedPoints = pointList;
        bool allPointsInPlace = false;
        int loopCounter = 0;

        while (!allPointsInPlace)
        {
            if (loopCounter > MAX_LOOPS)
            {
                Debug.Log("Loop counter reached in Clusters");
                break;
            }

            loopCounter++;
            allPointsInPlace = true;
            for (int i = 0; i < adjustedPoints.Count - 1; i++)
            {

                for (int j = 1; j < adjustedPoints.Count - 2; j++)
                {
                    if (i == j)
                        continue;

                    if (Vector3.Distance(adjustedPoints[i], adjustedPoints[j]) < minDistance)
                    {
                        allPointsInPlace = false;

                        float3 direction = ((Vector3)(adjustedPoints[j] - adjustedPoints[i])).normalized;
                        adjustedPoints[j] = boundsCollider.ClosestPoint(adjustedPoints[i] + (direction * adjustmentMinDistance));
                    }
                }
            }
        }

        return adjustedPoints;
    }

    private List<float3> AdjustAngles(List<float3> pointList)
    {
        bool allPointsInPlace = false;
        List<float3> adjustedPoints = pointList;
        int loopCounter = 0;

        while (!allPointsInPlace)
        {
            if (loopCounter > MAX_LOOPS)
            {
                Debug.Log("Loop counter reached in Angle Adjustment");
                break;
            }

            loopCounter++;
            allPointsInPlace = true;
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
                }

                adjustedPoints[i + 1] = boundsCollider.ClosestPoint(adjustedPoints[i] + ((float3)sideB * adjustmentMinDistance));
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
                }

                adjustedPoints[i - 1] = boundsCollider.ClosestPoint(adjustedPoints[i] + ((float3)sideA * adjustmentMinDistance));
            }
        }

        return adjustedPoints;
    }

    private List<float3> FABRIK(List<float3> pointList)
    {
        bool allPointsInPlace = false;
        List<float3> adjustedPoints = pointList;

        int loopCounter = 0;

        while (!allPointsInPlace)
        {
            if (loopCounter > MAX_LOOPS)
            {
                Debug.Log("Loop counter reached in FABRIK");
                break;
            }

            loopCounter++;
            allPointsInPlace = true;
            float distance = 0.0f;

            for (int i = 0; i < adjustedPoints.Count - 2; i++)
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

            for (int i = adjustedPoints.Count - 1; i > 1; i--)
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
        }        

        return adjustedPoints;
    }
}
