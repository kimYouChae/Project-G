using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlayer : MonoBehaviour, IPlayerSkill
{
    [SerializeField] private int scoreAddAmount = 10;   // 점수 증가 
    [SerializeField] private float timeAmount = 10f;    // 증가 쿨타입

    public void IOnCollision(NetPlayer player, Collider2D c)
    {
        player.DiePlayer();           
    }

    public void IOnStart(NetPlayer player)
    {
        StartCoroutine(ScoreAdd(player));
    }

    IEnumerator ScoreAdd(NetPlayer player) 
    {
        // 준비 될 때 까지 대기
        while (true) 
        {
            if (player.IsReadToMove)
                break;
            yield return null;
        }

        while (true) 
        {
            yield return new WaitForSeconds(timeAmount);

            // 점수 증가
            ScoreManager.Instance.AddScore(scoreAddAmount);
        }

    }
}
