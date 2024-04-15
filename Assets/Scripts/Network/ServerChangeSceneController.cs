
using System;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerChangeSceneController : Singleton<ServerChangeSceneController>
{
    public enum State
    {
        InLobby,
        InGame
    }

    private State _currState = State.InLobby;
    private NetworkDiscovery _discovery;
    
    [SerializeField] private Dropdown _dropdownGamemode;

    private void Start()
    {
        _discovery = FindObjectOfType<NetworkDiscovery>();
    }

    private void OnEnable()
    {
        ConnectedStudentsManager.Instance.OnAllStudentsDisconnected += ReturnToLobby;
        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene currScene, Scene nextScene)
    {
        
        if (_currState == State.InGame && nextScene.buildIndex == 0)
        {
            _currState = State.InLobby;
            _discovery.enabled = true;
        }
        
    }

    private void OnDisable()
    {
        ConnectedStudentsManager.Instance.OnAllStudentsDisconnected -= ReturnToLobby;
        SceneManager.activeSceneChanged -= SceneChanged;
    }

    public void ReturnToLobby()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            NetworkManager.singleton.StopServer();
            Destroy(ServerUICanvas.Instance.gameObject);
            Destroy(CameraManager.Instance.gameObject);
            Destroy(ConnectedStudentsManager.Instance.gameObject);
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        Destroy(ServerUICanvas.Instance.gameObject);
        Destroy(CameraManager.Instance.gameObject);
        StartGame();
    }

    public void StartGame()
    {
        _currState = State.InGame;
        _discovery.enabled = false;
        NetworkGameMode.Instance.GameMode = _dropdownGamemode.value;
        NetworkServer.SendToAll(new GamemodeMessage()
        {
            gameMode = _dropdownGamemode.value
        });
        NetworkManager.singleton.ServerChangeScene("NetworkTiger");
    }
}


