//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbientAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Battle_Sequence", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
