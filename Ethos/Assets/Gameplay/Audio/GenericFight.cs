//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;
using System.Linq;

public class GenericFight : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("Event")]
    [SerializeField] private string _EventNameFootstep = "Play_Arcadia_Normal_Swing";

    /// <summary>
    /// Plays an fight event
    /// </summary>
    public void PlayFight() // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    }

    /// <summary>
    /// Plays a fight event
    /// </summary>
    public void PlayGenFight(string eventName) // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
    }
}
