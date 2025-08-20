using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x <= Define.mapMinX
            || transform.position.y <= Define.mapMinY
            || transform.position.x >= Define.mapMaxX
            || transform.position.y >= Define.mapMaxY)
        { 
            Destroy(gameObject);
        }
    }
}
