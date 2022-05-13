//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;
using System.Linq;

public class GenericAIMovement : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("Event")]
    [SerializeField] private string _EventNameFootstep = "Enemy_Footsteps";

    /// <summary>
    /// Plays an AI footstep event
    /// </summary>
    public void PlayFootstep() // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    }
}
