using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour, IPlayerSkill
{
    private const float ShieldAngle = 45f; // 방어 각도
    private float ShieldDotThreshold => Mathf.Cos(ShieldAngle * Mathf.Deg2Rad);

    public void IOnCollision(NetPlayer player, Collider2D collider)
    {
        Vector2 playerForward = player.LastMoveDir.normalized;
        Vector2 bulletToPlayerDir = (collider.transform.position - player.transform.position ).normalized;

        float dot = Vector2.Dot(playerForward, bulletToPlayerDir);

        Debug.Log($"dot: {dot}, threshold: {ShieldDotThreshold}");

        if (dot >= ShieldDotThreshold)
        {
            Debug.Log("정면(±45도) 방패로 막음");

            // sfx 실행
            SFXManager.Instance.PlaySFX(SFXType.ShieldBlock);

            return; // 막기 성공
        }
        else
        {
            Debug.Log(" 측/후면에서 맞음");
            player.DiePlayer(); // 사망 처리
        }
    }

    public void IOnStart(NetPlayer player)
    {

    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;

        NetPlayer player = GetComponent<NetPlayer>();
        if (player == null) return;

        Vector3 forward = player.LastMoveDir.normalized;
        Vector3 pos = player.transform.position;

        // 정면선
        Gizmos.color = Color.green;
        Gizmos.DrawRay(pos, forward * 2f);

        // 좌우 경계
        float halfAngle = 45f;
        Quaternion leftRot = Quaternion.Euler(0, 0, halfAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, -halfAngle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(pos, leftRot * forward * 2f);
        Gizmos.DrawRay(pos, rightRot * forward * 2f);
    }
}
