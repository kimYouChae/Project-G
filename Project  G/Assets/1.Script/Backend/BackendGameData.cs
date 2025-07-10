using BackEnd;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UserData 
{
    public int level = 1;

    // 데이터 디버깅용 함수
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");

        return result.ToString();
    }

}

public class BackendGameData
{
    private static BackendGameData _instance = null;

    public static BackendGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    public static UserData userData;

    private string gameDataRowInDate = string.Empty;

    public void GameDataInsert()
    {
        if (userData == null)
        {
            userData = new UserData();
        }

        Debug.Log("데이터를 초기화합니다.");
        userData.level = 1;

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        // ! 데이터테이블에 데이터를 넣으려면 Param에 뭔저 넣어야함!
        Param param = new Param();
        param.Add("level", userData.level);

        Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임 정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
        }


    }

    public void GameDataGet()
    {
        // Step 3. 게임 정보 불러오기 구현하기
        Debug.Log("게임 정보 조회 함수를 호출합니다");

        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공 했습니다 : " + bro);

            // json으로 리턴된 데이터를 받아옴
            LitJson.JsonData gameDataJson = bro.FlattenRows();

            // 받아온 데이터의 갯수가 0이라면 : 데이터 존재 x
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다");
            }
            else
            {
                // 불러온 게임 정보의 고유값 
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString();

                userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());

                Debug.Log(userData.ToString());
            }
        }
        else 
        {
            Debug.LogError("게임 정보 조회에 실패했습니다 :" + bro);
        }

    }

    public void LevelUp()
    {
        // Step 4. 게임 정보 수정 구현하기
        Debug.Log("레벨을 1 증가시킵니다");
        userData.level += 1;
    }

    public void GameDataUpdate()
    {
        // Step 4. 게임 정보 수정 구현하기

        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("level", userData.level);
        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}
