//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceSound : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private string _Hover = null;
    [SerializeField] private string _Select = null;
    [SerializeField] private string _Back = null;
    [SerializeField] private string _Cancel = null;
    [SerializeField] private string _Start = null;
    [SerializeField] private string _Quit = null;


    /// <summary>
    /// Plays a hover event
    /// </summary>
    public void PlayHoverSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }


    /// <summary>
    /// Plays a selected event
    /// </summary>
    public void PlaySelectSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }


    /// <summary>
    /// Plays a back event
    /// </summary>
    public void PlayBackSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }


    /// <summary>
    /// Plays a cancel event
    /// </summary>
    public void PlayCancelSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }


    /// <summary>
    /// Plays a play event
    /// </summary>
    public void PlayStartSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }


    /// <summary>
    /// Plays a quit event
    /// </summary>
    public void PlayQuitSound()
    {
        AkSoundEngine.PostEvent(_Hover, gameObject);
    }
}
