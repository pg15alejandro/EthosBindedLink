//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;
using System.Linq;

public class GenericAbility : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("Event")]
    [SerializeField] private string _EventNameFootstep = "Play_Arcadia_Knockback";

    /// <summary>
    /// Plays an ability event
    /// </summary>
    public void PlayAbility() // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    }

    /// <summary>
    /// Plays an interactable event
    /// </summary>
    public void PlayGenAbility(string eventName) // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
    }
}
