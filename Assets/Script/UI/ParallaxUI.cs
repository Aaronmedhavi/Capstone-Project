using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxUI : MonoBehaviour
{
    public float speedMultiplier = 0.5f;
    private float spriteWidth;
    private Transform[] layerParts;

    void Start()
    {
        layerParts = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layerParts[i] = transform.GetChild(i);
        }

        SpriteRenderer spriteRenderer = layerParts[0].GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
    }

    void Update()
    {
        float movement = speedMultiplier * Time.deltaTime;
        transform.Translate(-movement, 0, 0);

        foreach (Transform part in layerParts)
        {
            if (part.position.x <= Camera.main.transform.position.x - spriteWidth)
            {
                float rightMostX = GetRightMost();
                Vector3 newPos = new Vector3(rightMostX + spriteWidth, part.position.y, part.position.z);
                part.position = newPos;
            }
        }
    }

    float GetRightMost()
    {
        float rightMostX = layerParts[0].position.x;
        foreach (Transform part in layerParts)
        {
            if (part.position.x > rightMostX)
            {
                rightMostX = part.position.x;
            }
        }
        return rightMostX;
    }
}
