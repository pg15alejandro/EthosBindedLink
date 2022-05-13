//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;
using System.Linq;

public class GenericInteractables : MonoBehaviour
{
    /// <summary>
    /// Plays an interactable event
    /// </summary>
    public void PlayAbility(string eventName) // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
    }
}
