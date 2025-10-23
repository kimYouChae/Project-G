using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour , IPlayerSkill
{
    public void IOnCollision(NetPlayer player, Collider2D collider)
    {
        player.DiePlayer();
    }

    public void IOnStart(NetPlayer player)
    {

    }
}
