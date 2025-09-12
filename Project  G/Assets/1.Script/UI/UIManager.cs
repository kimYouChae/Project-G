using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<UIPopUP> popupList;
    [SerializeField] private Canvas canvas;

    const string PopupPath = "PopUp/";

    protected override void Singleton_Awake()
    {
        popupList = new List<UIPopUP>();

        FindCanvas();
    }

    private void FindCanvas() 
    {
        // canvas 이름의 오브젝트 찾기 
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public T GetPopUP<T>() where T : UIPopUP 
    {
        UIPopUP popup;
        // 1. 리스트에 있는지 검사 
        if (isInPopUpList<T>())
        {
            // 있으면 
            popup = ReturnPopupInList<T>();

            // 리스트에서 지우기 -> 맨 앞에 두기위한 
            popupList.Remove(popup);
        }
        else 
        {
            // 없으면 
            // 만들기 
            popup = InstancePopup<T>();
        }


        // 리스트의 맨 앞에 두기
        popupList.Insert(0, popup);

        return popup as T;
    }

    private bool isInPopUpList<T>() 
    {
        for (int i = 0; i < popupList.Count; i++) 
        {   
            // 상속관계여도 
            if (popupList[i].GetType() == typeof(T)) 
            {
                return true;
            }
        }

        return false;
    }

    private UIPopUP ReturnPopupInList<T>() 
    {
        for (int i = 0; i < popupList.Count; i++)
        {
            // 상속관계여도 
            if (popupList[i].GetType() == typeof(T))
            {
                return popupList[i];
            }
        }

        return null; ;
    }

    private UIPopUP InstancePopup<T>() 
    {
        // 0. Resource에서 가져오기 
        // 스크립트이름(T)랑 오브젝트랑 이름 같아야함 
        // (주의) nameof<T>라고 하면 T를 반환함
        GameObject popup = Resources.Load<GameObject>(PopupPath + typeof(T).Name);

        GameObject instancePop = Instantiate(popup);

        //  canvvas 하위로 
        instancePop.transform.SetParent(canvas.transform, false);

        return instancePop.GetComponent<UIPopUP>();
    }

}
