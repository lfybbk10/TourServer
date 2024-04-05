using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IInteract
{
    [SerializeField] private int toZone;

    public void Interact()
    {
        print("To Zone " + toZone);
        Zones.Set(toZone-1);
    }
}
