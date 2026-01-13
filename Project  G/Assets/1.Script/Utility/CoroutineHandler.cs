using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CoroutineHandler : Singleton<CoroutineHandler>
{
    protected override void Singleton_Awake()
    {
        
    }

    public Coroutine Run(IEnumerator routine) => StartCoroutine(routine);
    public void Stop(Coroutine coroutine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }


}
