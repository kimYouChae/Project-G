using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchiveObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achiveTitleText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image completeImage;   // 성공 여부 이미지 

    public void SetAchiveTitle(string str) {  achiveTitleText.text = str; }

    public void SetProgressText(string str) {  progressText.text = str; }

    public void OnOffCompleteImage(bool flag) 
    {  
        completeImage.gameObject.SetActive(flag);
        progressText.gameObject.SetActive(!flag);
    }
}
