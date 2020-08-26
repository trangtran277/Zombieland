using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        var vp = GetComponent<UnityEngine.Video.VideoPlayer>();
        vp.Play();
    }
}
