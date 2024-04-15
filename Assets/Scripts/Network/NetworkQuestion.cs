using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Vrs.Internal;


public class NetworkQuestion : NetworkBehaviour
{
    private List<SpriteButtonGaze> _spriteButtonGazes;
    
    private void Awake()
    {
        _spriteButtonGazes = GetComponentsInChildren<SpriteButtonGaze>().ToList<SpriteButtonGaze>();
    }

    [Command]
    private void OnGazeEnter(int index)
    {
        _spriteButtonGazes[index].OnGazeEnter();
    }

    [Command]
    private void OnGazeExit(int index)
    {
        _spriteButtonGazes[index].OnGazeExit();
    }
}
