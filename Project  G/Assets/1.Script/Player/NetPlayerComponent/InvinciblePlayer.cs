using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePlayer : MonoBehaviour, IPlayerSkill
{
    [SerializeField] private bool isHitted = false;
    [SerializeField] private float colorChangeTime = 0.1f;
    [SerializeField] private List<Color> invincibleColors;
    [SerializeField] private Color originalColor;

    Coroutine changeColorCoru;

    public void IOnCollision(NetPlayer player, Collider2D collision)
    {
        // 충돌한 적이 없으면 
        if (!isHitted)
        {
            isHitted = true;

            // 색 변환 코루틴 중지 
            StopCoroutine(changeColorCoru);
            // 원래색으로
            player.ChangeColor(originalColor);
            return;
        }

        // 충돌한적이 있으면 
        player.DiePlayer();
    }

    public void IOnStart(NetPlayer player)
    {
        originalColor = player.OriginalColor;

        changeColorCoru = StartCoroutine(ChangeColor(player));
    }

    IEnumerator ChangeColor(NetPlayer player) 
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
            for(int i = 0; i < invincibleColors.Count; i++) 
            {
                player.ChangeColor(invincibleColors[i]);

                yield return new WaitForSeconds(colorChangeTime);
            }
        }
    }
}
