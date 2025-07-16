using Fusion;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    // 모든 클라이언트에서 Ball이 동일하게 동작하도록 하는것이 목표

    [Networked] private TickTimer life { get; set; }

    public void Init() 
    {
        // 생명주기 , N초 동안 생존 
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += 1 * transform.right * -1 * Runner.DeltaTime;
    }
}
