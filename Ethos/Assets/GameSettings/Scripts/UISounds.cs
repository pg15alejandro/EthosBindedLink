//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] private string _Moved;
    private float _TempTimer;

    bool _CanPlayHover = true;

    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    void Update()
    {
        /*_TempTimer -= Time.deltaTime;
        
        if (_TempTimer >= 0) return;
        
        _TempTimer = 0.2f;
        if (Input.GetAxis("Vertical") != 0)
        {
            AkSoundEngine.PostEvent(_Moved, gameObject);
        }*/
    }
    public void PlayBack()
    {
        _CanPlayHover = false;
        AkSoundEngine.PostEvent("Play_Menu_UI_Back", gameObject);
    }

    public void PlaySelect()
    {
        _CanPlayHover = false;
        AkSoundEngine.PostEvent("Play_Menu_UI_Select", gameObject);
    }

    public void PlayHover()
    {
        if(_CanPlayHover){
            AkSoundEngine.PostEvent("Play_Menu_UI_Hover", gameObject);
        }
        _CanPlayHover = true;
    }
    
    public void PlayUISound(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

}
