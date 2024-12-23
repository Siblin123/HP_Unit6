using UnityEngine;
using Unity.Services.Multiplayer;
using TMPro;
public class test99 : MonoBehaviour
{
     public TextMeshProUGUI SessionCodeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void StartSessionAsHost()
    {
        var options = new SessionOptions
        {
            IsPrivate = true,
            MaxPlayers = 4
        }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay
        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        SessionCodeText.text = session.Code;
        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
    }
}
