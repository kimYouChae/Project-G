using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPopUP : MonoBehaviour
{
    [SerializeField] private CharacterView characterView;
    [SerializeField] private CharacterController characterController;

    private void Start()
    {
        characterView = GetComponent<CharacterView>();


    }
}
