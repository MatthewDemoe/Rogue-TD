using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineSampler : MonoBehaviour
{
    private SplineContainer m_splineContainer;

    private MeshFilter m_meshFilter;
    private MeshFilter m_directionMeshFilter;

    [SerializeField]
    float m_width = 0.25f;

    [SerializeField]
    float splineResolution = 100.0f;

    float3 position;
    float3 forward;
    float3 upVector;

    List<Vector3> m_vertsP1 = new();
    List<Vector3> m_vertsP2 = new();

    [SerializeField]
    GameObject directionIndicator;

    private void Awake()
    {
        m_splineContainer = GetComponent<SplineContainer>();
        m_meshFilter = GetComponent<MeshFilter>();

        m_directionMeshFilter = directionIndicator.GetComponent<MeshFilter>();
    }

    void Start()
    {
        BuildMesh();

        if (TryGetComponent(out MeshCollider meshCollider))
            meshCollider.sharedMesh = m_meshFilter.sharedMesh;
    }

    private void SampleSplineWidth(float t, out Vector3 p1, out Vector3 p2)
    {
        m_splineContainer.Evaluate(0, t, out position, out forward, out upVector);

        float3 right = Vector3.Cross(forward, upVector).normalized;
        p1 = position + (right * m_width);
        p2 = position + (-right * m_width);
    }

    private void GetVertices()
    {
        float step = 1.0f / splineResolution;

        for (int i = 0; i < splineResolution; i++)
        {
            float t = step * i;

            SampleSplineWidth(t, out Vector3 p1, out Vector3 p2);
            m_vertsP1.Add(p1);
            m_vertsP2.Add(p2);
        }
    }

    private void BuildMesh()
    {
        print($"Building Mesh");
        Mesh trackMesh = new();
        Mesh directionMesh = new();

        List<Vector3> vertices = new();
        List<Vector2> UVs = new();
        List<int> tris = new();
        int offset = 0;

        GetVertices();

        int length = m_vertsP1.Count;

        for (int i = 1; i < length; i++) 
        { 
            Vector3 p1 = m_vertsP1[i - 1];
            Vector3 p2 = m_vertsP2[i - 1];
            Vector3 p3 = m_vertsP1[i];
            Vector3 p4 = m_vertsP2[i];

            float uvY1 = (i - 1) / (float)(length - 1); // Normalize step range
            float uvY2 = i / (float)(length - 1);

            Vector2 uv1 = new Vector2(0.0f, uvY1);
            Vector2 uv2 = new Vector2(1.0f, uvY1);
            Vector2 uv3 = new Vector2(0.0f, uvY2);
            Vector2 uv4 = new Vector2(1.0f, uvY2);

            offset = 4 * (i - 1);

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;

            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            vertices.AddRange(new List<Vector3> { p1, p2, p3, p4});
            UVs.AddRange(new List<Vector2> {uv1, uv2, uv3, uv4 });
            tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
        }

        trackMesh.SetVertices(vertices);
        trackMesh.SetTriangles(tris, 0);
        trackMesh.SetUVs(0, UVs.ToArray());

        directionMesh.SetVertices(vertices);
        directionMesh.SetTriangles(tris, 0);
        directionMesh.SetUVs(0, UVs.ToArray());

        m_meshFilter.mesh = trackMesh;        
        m_directionMeshFilter.mesh = directionMesh;

        m_meshFilter.mesh.RecalculateNormals();
        m_directionMeshFilter.mesh.RecalculateNormals();
    }
}
