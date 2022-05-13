//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private SOSoundState _SoundState;
    CheckpointManager _CheckManager;                        //Checkpoint manager which has the latest checkpoint reached
    bool _Check;                                            //Boolean that says if this checkpoint has been reached before


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        _CheckManager = GetComponentInParent<CheckpointManager>();

        if(_SoundState._CurrentIntensity == GameIntensity.BATTLE){
            AkSoundEngine.SetState("game_intensity", "explore");
            _SoundState._CurrentIntensity = GameIntensity.EXPLORE;
        }
    }


    /// <summary>
    /// Called when the collider other enters the trigger
    /// </summary>
    /// <param name="other">The collision data associated with this collision</param>    
    private void OnTriggerEnter(Collider other)
    {
        int playerL = LayerMask.NameToLayer("Player");

        if(other.gameObject.layer != playerL) return;

        if(_SoundState._CurrentIntensity == GameIntensity.BATTLE){
            AkSoundEngine.SetState("game_intensity", "explore");
            _SoundState._CurrentIntensity = GameIntensity.EXPLORE;
        }

        //If this is the first time the player touches this trigger
        if (!_Check)
        {
            _Check = true;                                  //Make the check true so that it's not stored again
            _CheckManager.SetLastCheckpoint(transform);     //Set the last checkpoint to this object's transform
        }
    }
}
