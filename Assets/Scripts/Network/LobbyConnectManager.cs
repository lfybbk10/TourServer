using System;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyConnectManager : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;
    [SerializeField] private NetworkDiscovery _networkDiscovery;

    private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    private void Awake()
    {
        _networkDiscovery = FindObjectOfType<NetworkDiscovery>();
    }

    private void Start()
    {
        _networkDiscovery.OnServerFound.AddListener(ConnectToFoundedServer);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void StartServer()
    {
        _buttons.SetActive(false);
        discoveredServers.Clear();
        NetworkManager.singleton.StartServer();
        _networkDiscovery.AdvertiseServer();
    }

    public void FindServer()
    {
        _buttons.SetActive(false);
        discoveredServers.Clear();
        _networkDiscovery.StartDiscovery();
    }

    private void ConnectToFoundedServer(ServerResponse response)
    {
        _networkDiscovery.StopDiscovery();
        ClientStudentManager.Instance.CreateStudent();
        NetworkManager.singleton.StartClient(response.uri);
    }
}
