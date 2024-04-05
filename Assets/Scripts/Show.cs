using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;
using UnityEngine.Video;
using Vrs.Internal;
public class Show : MonoBehaviour, IInteract
{
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private MediaPlayer video;
    //[SerializeField] private string path;
    //private AndroidJavaObject mediaPlayer;

    private void Start()
    {
        video = objectToShow.GetComponent<MediaPlayer>();
        /*try
        {
            mediaPlayer = new AndroidJavaObject("android.media.MediaPlayer");
            // Получение полного пути к видеофайлу в папке StreamingAssets
            //path = "file:///Tour/"+path;
            //video.url = path;
            //FPScounter.Print(path + "\n");
        }
        catch (Exception e)
        {
            
        }*/

    }
    public void Interact()
    {
        objectToShow.SetActive(true);
        if (video != null)
        {
            //video.Control.SeekFast(0);
            video.Play();
            
            /*try
            {
                video.source = VideoSource.Url;
                video.url = "file:///Tour/" + path;
                video.Play();
                PlayAudio(0);
            }
            catch (Exception e)
            {
                FPScounter.Print(e.ToString());
            }*/
        }
    }

    public void Close()
    {
        if (video != null)
        {
            video.Stop();
            video.Rewind(true);
            /*try
            {
                StopAudio();
            }
            catch (Exception e)
            {
                FPScounter.Print(e.ToString());
            }*/
        }
        objectToShow.SetActive(false);
    }

    /*private void PlayAudio(int startTime)
    {

#if !UNITY_EDITOR
    mediaPlayer.Call("setDataSource", "file:///" + path);
    mediaPlayer.Call("setAudioStreamType", 3);
    mediaPlayer.Call("prepare");
    mediaPlayer.Call("start");
#endif
    }
    public void StopAudio()
        {
#if !UNITY_EDITOR
mediaPlayer.Call("stop");
mediaPlayer.Call("reset");
#endif
    }*/
}