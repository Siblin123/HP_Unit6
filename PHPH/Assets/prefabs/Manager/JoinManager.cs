using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;

public class JoinManager : NetworkBehaviour
{
    public GameObject MainMenuCanvas;
    void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            // 서버 시작 및 클라이언트 접속 이벤트 등록
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"클라이언트 {clientId} 접속");
        MainMenuCanvas.SetActive(false);
    }




    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"클라이언트 {clientId} 연결 해제");
        PrintAllConnectedClients();
    }
    private void PrintAllConnectedClients()
    {
        Debug.Log("현재 접속 중인 클라이언트 목록:");

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log($"클라이언트 ID: {client.ClientId}");
        }
    }


}
