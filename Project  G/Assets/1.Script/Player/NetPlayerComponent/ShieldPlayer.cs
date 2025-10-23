using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour, IPlayerSkill
{
    public void IOnCollision(NetPlayer player, Collider2D collider)
    {
        // 플레이어가 바라보는 방향 (최근 입력 방향)
        Vector2 playerForward = player.Dir.normalized;

        // 상대 충돌체 중심과의 벡터
        Vector2 toCollider = ((Vector2)collider.transform.position - (Vector2)player.transform.position).normalized;

        // 내 시선방향과 충돌 방향의 내적값
        float dot = Vector2.Dot(playerForward, toCollider);

        // dot값 해석:
        // 1.0 → 완전 정면
        // 0   → 직각 방향
        // -1  → 완전 후면
        if (dot > 0.7f)
        {
            Debug.Log("정면 충돌!");
            return;
        }

        // 정면충돌이 아닌 경우 
        player.DiePlayer();
    }

    public void IOnStart(NetPlayer player)
    {

    }
}
