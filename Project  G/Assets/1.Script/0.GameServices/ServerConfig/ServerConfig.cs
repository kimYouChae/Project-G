
using UnityEngine;


[CreateAssetMenu(fileName = "ServerConfig", menuName = "Config/ServerConfig")]
public class ServerConfig : ScriptableObject
{
    [SerializeField]
    private string baseUrl; // azure 기본 url 
    [SerializeField]
    private string loginUrl;
    [SerializeField]
    private string userRankUrl;
    [SerializeField]
    private string rankerUrl;
    [SerializeField]
    private string gameDataUrl;
    [SerializeField]
    private string chartUrl;
    [SerializeField]
    private string achiveProgressUrl;

    public string LoginUrl { get => baseUrl + loginUrl; }
    public string UserRankUrl { get => baseUrl + userRankUrl; }
    public string RankerUrl { get => baseUrl + rankerUrl; }
    public string GameDataUrl { get => baseUrl + gameDataUrl; }
    public string ChartUrl { get => baseUrl + chartUrl; }
    public string AchiveProgressUrl { get => baseUrl + achiveProgressUrl; }
}
