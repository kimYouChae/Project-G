using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    /// <summary>
    /// 하위에서 Update필요하면 
    /// </summary>

    void Update()
    {
        UpdateLogic();

        if (transform.position.x < Define.mapMinX
            || transform.position.y < Define.mapMinY
            || transform.position.x > Define.mapMaxX
            || transform.position.y > Define.mapMaxY)
        { 
            Destroy(gameObject);
        }
    }

    protected virtual void UpdateLogic() { }

}
