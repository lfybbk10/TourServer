
using System;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerChangeSceneController : Singleton<ServerChangeSceneController>
{
    public enum State
    {
        InLobby,
        InGame
    }

    private State _currState = State.InLobby;
    private NetworkDiscovery _discovery;

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
        if (nextScene.buildIndex != 0)
        {
            if (_currState == State.InGame)
            {
                _currState = State.InLobby;
                _discovery.enabled = true;
            }
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
            Destroy(CameraManager.Instance);
            Destroy(ConnectedStudentsManager.Instance.gameObject);
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        _currState = State.InGame;
        _discovery.enabled = false;
        NetworkManager.singleton.ServerChangeScene("Scene1");
    }
}


