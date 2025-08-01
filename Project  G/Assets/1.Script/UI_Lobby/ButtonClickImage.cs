
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickImage : MonoBehaviour
{
    [Header("===이미지 세팅===")]
    [SerializeField] Sprite buttonClickSprite;

    [Space]
    [SerializeField] Image myImage;
    [SerializeField] Sprite oriSprite;
    [SerializeField] EventTrigger trigger;

    private bool isClicked = false;

    private void Start()
    {
        myImage = GetComponent<Image>();
        oriSprite = GetComponent<Image>().sprite;
        trigger = GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnHover(); }); // delegate 또는 람다로 처리

        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { OutHover(); });

        trigger.triggers.Add(entry2);
    }

    private void OnHover()
    {
        myImage.sprite = buttonClickSprite;
    }

    private void OutHover() 
    {
        myImage.sprite = oriSprite;
    }

}
