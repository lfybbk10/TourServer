using System;
using Mirror;
using UnityEngine;


public class NetworkGameMode : Singleton<NetworkGameMode>
{
    [HideInInspector] public int GameMode;
    
    private void Start()
    {
        //NetworkClient.RegisterHandler<GamemodeMessage>(SetGameMode);
    }


    public void SetGameMode(int mode)
    {
        GameMode = mode;
        foreach (var zone in FindObjectsOfType<Zones>())
        {
            zone.SelectMode(GameMode);
        }
    }
}
