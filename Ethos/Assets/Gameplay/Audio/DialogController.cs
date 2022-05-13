//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private SODialogState _DialogState;
    private float _TempTimer;

    private void Update()
    {
        _TempTimer -= Time.deltaTime;

        if (_TempTimer <= 0){
            _TempTimer = 5f;
            SayLine();
        }
    }

    private void SayLine()
    {
    //    if(!_DialogState.PlayingAudio){}
        
        //AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    }
}
