using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSkill 
{
    public void IOnStart(NetPlayer player);
    public void IOnCollision(NetPlayer player, Collider2D collider );
}
