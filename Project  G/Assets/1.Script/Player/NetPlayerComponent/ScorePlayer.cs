using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlayer : MonoBehaviour, IPlayerSkill
{
    [SerializeField]
    private PlayerStatusData playerStatusData;

    private void Start()
    {
        playerStatusData = PlayerStatus.GetPlayerData(CharacterType.ScoreCharacter);
    }

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
            yield return new WaitForSeconds(playerStatusData.ScoreCooltime);

            // 점수 증가
            ScoreManager.Instance.AddScore(playerStatusData.ScoreAmount);

            // sfx 실행
            SFXManager.Instance.PlaySFX(SFXType.ScoreCoin);
        }

    }
}
