using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Parallax : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] float parallaxEffect;
    [SerializeField] Transform cam;
    float StartPosition, length;
    private void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        transform.position = new Vector2(Random.Range(-length / 4, length / 4), transform.position.y);
        StartPosition = transform.position.x;
        cam = Camera.main.transform;
    }
    void FixedUpdate()
    {
        float temp = cam.position.x * (1 - parallaxEffect);
        float position = cam.position.x * parallaxEffect;

        transform.position = new Vector3(StartPosition + position, transform.position.y, transform.position.z);
        if (temp > length + StartPosition) StartPosition += length;
        else if (temp < StartPosition - length) StartPosition -= length;
    }
}
