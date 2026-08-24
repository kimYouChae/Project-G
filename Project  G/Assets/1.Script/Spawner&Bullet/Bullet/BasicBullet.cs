using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BasicBullet : OutOfBounds
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Vector3 directVector;  // 방향 벡터 
    [SerializeField] float speed;

    public void SettingBasicBullet(Vector3 dir, float sp) 
    {
        rb = GetComponent<Rigidbody2D>();

        this.directVector = dir;
        this.speed = sp;
    }

    private void FixedUpdate()
    {
        rb.velocity = directVector.normalized * speed;
    }
}
