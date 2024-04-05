using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Vrs.Internal;


public class Zones : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private List<GameObject> fields = new List<GameObject>();
    [SerializeField] private List<Quaternion> looks = new List<Quaternion>();
    [SerializeField] private Material material;
    [SerializeField] private Transform sphere;


    private static Zones self;

    private void Start()
    {
        if(self == null)
        {
            self = this;
        }
        Set(0);
    }

    public static void Set(int num)
    {
        self.material.mainTexture = self.sprites[num].texture;
        self.sphere.rotation = self.looks[num];
        self.ActivateField(num);
    }

    private void ActivateField(int num)
    {
        for(var i=0;i<fields.Count;i++)
        {
            if(i==num)
            {
                fields[i].gameObject.SetActive(true);
            }
            else
            {
                fields[i].gameObject.SetActive(false);
                var shows = fields[i].GetComponentsInChildren<Show>();
                foreach(var show in shows)
                {
                    show.Close();
                }
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    


}
