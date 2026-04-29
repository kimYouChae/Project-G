using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootStrap : MonoBehaviour
{
    [SerializeField] private ServerEnvironment environment;

    [SerializeField] private ServerConfig localConfig;
    [SerializeField] private ServerConfig azureConfig;

    private void Awake()
    {
        ServerConfig selectedConfig = environment switch
        {
            ServerEnvironment.Local => localConfig,
            ServerEnvironment.Azure => azureConfig,
            _ => localConfig
        };

        GameServices.Instance.Init(selectedConfig);
    }
}
