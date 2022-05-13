//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDialog : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("Event")]
    [SerializeField] private string _EventNameDialog = "Dungeon_Arcadia_001";

    /// <summary>
    /// Plays an fight event
    /// </summary>
    public void PlayFight() // => AkSoundEngine.PostEvent(_EventNameDialog, this.gameObject);
    {
        AkSoundEngine.PostEvent(_EventNameDialog, this.gameObject);
    }

    /// <summary>
    /// Plays a fight event
    /// </summary>
    public void PlayGenFight(string eventName) // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
    }
}
