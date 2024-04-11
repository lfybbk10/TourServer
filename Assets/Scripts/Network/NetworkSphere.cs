using Mirror;
using RenderHeads.Media.AVProVideo;
using UnityEngine;


public class NetworkSphere : NetworkBehaviour
{
    [SerializeField] private GameObject _viarusCamera;

    private MediaPlayer _mediaPlayer;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _viarusCamera.SetActive(true);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _mediaPlayer = GetComponent<MediaPlayer>();
        MutePlayer();
    }

    public void MutePlayer()
    {
        _mediaPlayer.AudioMuted = true;
    }

    public void UnmutePlayer()
    {
        _mediaPlayer.AudioMuted = false;
    }
}
