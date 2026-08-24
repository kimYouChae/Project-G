using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuideMissile : OutOfBounds
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Transform ownerPosition;

    [SerializeField] float currTime = 0;
    [SerializeField] float maxLifeTime; // 최대 생존시간
    [SerializeField] float speed;           // 속도
    [SerializeField] float bulletInterval;  // 유도 업데이트 간격 

    [SerializeField] bool isLocalOwner;

    public Transform OwnerPosition { get => ownerPosition; set => ownerPosition = value; }
    public bool IsLocalOwner { set => isLocalOwner = value; }

    public void SettingGuideMissile(float max, float speed, float intervavl ) 
    {
        rb = GetComponent<Rigidbody2D>();

        this.maxLifeTime = max;
        this.speed = speed;
        this.bulletInterval = intervavl;

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
            rb.velocity = dir.normalized * speed;

            // sfx 실행 
            // SFXManager.Instance.PlaySFX(SFXType.MissileFly);

            // 유도 업데이트 간격 동안 기다리기 
            yield return new WaitForSeconds(bulletInterval);
        }
    }


}
