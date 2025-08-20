using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuideMissile : BaseBullet
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Transform ownerPosition;

    [SerializeField] float maxLifeTime = 3f;
    [SerializeField] float currTime = 0;

    public Transform OwnerPosition { get => ownerPosition; set => ownerPosition = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(Test());
    }

    void FixedUpdate() 
    {
        currTime += Time.deltaTime;
        if (currTime > maxLifeTime)
            Destroy(gameObject);
    }

    IEnumerator Test() 
    {
        while(true) 
        {
            if (ownerPosition != null)
                break;
            yield return null; 
        }

        while (true) 
        {
            // 방향 벡터
            Vector3 dir = ownerPosition.position - transform.position;
            rb.velocity = dir.normalized * 3f;
            yield return new WaitForSeconds(0.5f);
        }
    }


}
