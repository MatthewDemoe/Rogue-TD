using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndication : MonoBehaviour
{
    Renderer renderer;

    float timeCounter = 0.0f;
    float timeMultiplier = 1.0f;

    Vector2 offset = new();

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        timeCounter -= Time.fixedDeltaTime * timeMultiplier;
        timeCounter %= 1.0f;

        offset.y = timeCounter;
            
        renderer.material.mainTextureOffset = offset;
    }
}
