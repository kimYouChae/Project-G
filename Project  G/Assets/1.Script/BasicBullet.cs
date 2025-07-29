using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Vector3 directVector;  // 방향 벡터 

    public Vector3 DirectVector { get => directVector; set => directVector = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // ## 임시 속도 Nf
        rb.velocity = directVector.normalized * 3f;
    }
}
