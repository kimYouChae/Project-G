using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class InGameUI : MonoBehaviour
{
    [Space]
    [Header("===CharacterSeletUI")]
    [SerializeField] GameObject charaterSelectPanel;
    [SerializeField] int selectIndex = -1;

    public void OnOffCharaterSelectPanel() 
    {
    
    }

    public void SetCharaterIndex(int idx) 
    {
        this.selectIndex = idx;
    }
}
