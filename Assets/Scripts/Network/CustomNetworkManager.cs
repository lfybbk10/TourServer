using Mirror;
using UnityEngine;


public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _networkObjects;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        print("add student");
        ConnectedStudentsManager.Instance.AddConnectedStudent(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ConnectedStudentsManager.Instance.gameObject.SetActive(true);
        print("init server ui");
        FindObjectOfType<LobbyUIManager>().InitServerUI();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientStudentManager.Instance.SetNetworkConnection(NetworkClient.connection);
        FindObjectOfType<LobbyUIManager>().InitClientUI();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        ConnectedStudentsManager.Instance.RemoveStudent(conn);
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        Destroy(ServerUICanvas.Instance.gameObject);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Instantiate(_networkObjects);
    }
}
