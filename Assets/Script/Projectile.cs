using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Action<Projectile> onRelease;
    public float speed;
    public float time;
    public Vector3 direction;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private IEnumerator Start()
    {
        rb.velocity = direction * speed;
        yield return new WaitForSeconds(time);
        onRelease?.Invoke(this);
    }
}
