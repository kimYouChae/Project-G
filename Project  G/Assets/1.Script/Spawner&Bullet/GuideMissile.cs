using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuideMissile : BaseBullet
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Transform ownerPosition;

    [SerializeField] float maxLifeTime = 3f;
    [SerializeField] float currTime = 0;

    [SerializeField] bool isLocalOwner;

    public Transform OwnerPosition { get => ownerPosition; set => ownerPosition = value; }
    public bool IsLocalOwner { set => isLocalOwner = value; }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(ShootGuideBulletCycle());
    }

    void FixedUpdate() 
    {
        currTime += Time.deltaTime;
        if (currTime > maxLifeTime)
        {
            if(isLocalOwner) 
            {
                // sfx 실행 
                SFXManager.Instance.PlaySFX(SFXType.MissileExplosion);    
            }
            // 파괴
            Destroy(gameObject);
        }
    }

    IEnumerator ShootGuideBulletCycle() 
    {
        while(true) 
        {
            if (ownerPosition != null)
                break;
            yield return null; 
        }

        while (true) 
        {
            // 방향 벡터계산 후 속도주기
            Vector3 dir = ownerPosition.position - transform.position;
            rb.velocity = dir.normalized * 3f;

            // sfx 실행 
            // SFXManager.Instance.PlaySFX(SFXType.MissileFly);

            yield return new WaitForSeconds(0.5f);
        }
    }


}
