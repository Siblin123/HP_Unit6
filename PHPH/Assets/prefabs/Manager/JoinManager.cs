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
            // ���� ���� �� Ŭ���̾�Ʈ ���� �̺�Ʈ ���
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Ŭ���̾�Ʈ {clientId} ����");
        MainMenuCanvas.SetActive(false);
    }




    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Ŭ���̾�Ʈ {clientId} ���� ����");
        PrintAllConnectedClients();
    }
    private void PrintAllConnectedClients()
    {
        Debug.Log("���� ���� ���� Ŭ���̾�Ʈ ���:");

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log($"Ŭ���̾�Ʈ ID: {client.ClientId}");
        }
    }


}
