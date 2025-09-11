using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnTitledUI : MonoBehaviour
{
    [Space]
    [Header("===TitleUI===")]
    [SerializeField]
    private Button reStartButton;

    private void InitUnTitledUI()
    {
        // 시작 버튼 
        if (reStartButton != null)
            reStartButton.onClick.AddListener(ReStartButton);
    }

    private void ReStartButton() 
    {
        // 로컬에서 재시작해야함 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
