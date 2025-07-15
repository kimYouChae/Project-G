
using UnityEngine;
using UnityEngine.UI;
// Dispatcher 사용 
using Battlehub.Dispatcher;
using TMPro;
using BackEnd;

public class LoginUI : MonoBehaviour
{

    public GameObject errorObject;
    // public GameObject loadingObject;

    public TextMeshProUGUI errorText;
    public TMP_InputField nickNameField;

    public Button guestLoginButton;
    public Button changeNickNameButton;

    private void Awake()
    {
        guestLoginButton.onClick.AddListener(GuestLoginEvent);
        changeNickNameButton.onClick.AddListener(UpdateNickNameEvent);
    }

    // 일단 게스트 로그인만
    public void GuestLoginEvent() 
    {
        // if (errorObject.activeSelf)
        //    return;

        // loadingObject.SetActive(true);
        
        // GuestLogin : 비동기
        // UI조작 필요(메인스레드에서 동작해야함) : Dispatcher 사용
        BackEndServerManager.GetInstance().GuestLogin((bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                // 로그인 못 했으면 
                if (!result) 
                {
                    // loadingObject.SetActive(false);
                    errorText.text = "로그인 에러\n" + error;
                    errorObject.SetActive(true);
                    return;
                }

                ChangeLobbyScene();

            });
        });
    }

    void ChangeLobbyScene() 
    {
        //##TODO : 씬 전환 
        // GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);
        GameManager.GetInstance().ChangeSceneToMatchLobby();
    }

    // 닉네임 바꾸기 (뒤끝 매치 사용하려면 꼭 있어야함 ! ) 
    public void UpdateNickNameEvent() 
    {
        string nickName = nickNameField.text;

        if (nickName.Equals(string.Empty)) 
        {
            errorText.text = "닉네임을 먼저 입력해 주세요";
            return;
        }

        BackEndServerManager.GetInstance().UpdateNickName(nickName, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if(!result) 
                {
                    errorText.text = "닉네임 생성 오류 " + error;
                    return;
                }

                ChangeLobbyScene();
            });
        });
    }
}
