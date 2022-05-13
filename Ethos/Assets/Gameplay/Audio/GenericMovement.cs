//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;
using System.Linq;

public enum Speed { RUN, WALK }

public class GenericMovement : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("Event")]
    [SerializeField] private string _EventNameFootstep = "Play_Arcadia_Footsteps";

    [Header("Speed")]
    private Speed _CurrentSpeed;
    [SerializeField] private string _SwitchGroupNameSpeed = "speed";
    [SerializeField] private string[] _SwitchNamesSpeed = { "run" , "walk" };

    [Header("Surfaces")]
    [SerializeField] private string _SwitchGroupNameSurfaces = "surfaces";
    [SerializeField] private string[] _SwitchNamesSurfaces = { "dirt" , "stone" , "wood" };


    /// <summary>
    /// Plays a footstep event
    /// </summary>
    public void PlayFootstep() // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    }


    /// <summary>
    /// Changes the speed switch to walk
    /// </summary>
    public void SetSpeedToWalk()
    {
        if (_CurrentSpeed != Speed.WALK)
        {
            _CurrentSpeed = Speed.WALK;
            AkSoundEngine.SetSwitch(_SwitchGroupNameSpeed, _SwitchNamesSpeed[1], this.gameObject);
        }
    }


    /// <summary>
    /// Changes the speed switch to run
    /// </summary>
    public void SetSpeedToRun()
    {
        if (_CurrentSpeed != Speed.RUN)
        {
            _CurrentSpeed = Speed.WALK;
            AkSoundEngine.SetSwitch(_SwitchGroupNameSpeed, _SwitchNamesSpeed[0], this.gameObject);
        }
    }


    /// <summary>
    /// Changes the surface switch to the given one
    /// </summary>
    /// <param name="surfaceTag">The name of the surface you want the switch to be set to</param>
    public void SetSurface(string surfaceTag)
    {
        AkSoundEngine.SetSwitch(_SwitchGroupNameSurfaces, surfaceTag, this.gameObject);
    }
    

    /// <summary>
    /// Called when this collider/rigidbody has begun touching another collider/rigidbody
    /// </summary>
    /// <param name="other">The collision data associated with this collision</param>
    private void OnCollisionEnter(Collision other)
    {
        if (_SwitchNamesSurfaces.Any(surface => surface.Contains(other.gameObject.tag)))
        {
            SetSurface(other.gameObject.tag);
        }
    }

    /// <summary>
    /// Plays a movement event
    /// </summary>
    public void PlayGenMovement(string eventName) // => AkSoundEngine.PostEvent(_EventNameFootstep, this.gameObject);
    {
        AkSoundEngine.PostEvent(eventName, this.gameObject);
    }
}
