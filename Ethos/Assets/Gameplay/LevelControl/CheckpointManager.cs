//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    private Transform lastCheckpoint;       //Position of the last checkpoint reached
    [SerializeField] private SOTransform _PlayerTransform;

    /// <summary>
    /// Sets the last checkpoint where the player has gone through
    /// </summary>
    /// <param name="stepCheckpoint">The transform of the checkpoint to be stored</param>
    public void SetLastCheckpoint(Transform stepCheckpoint)
    {
        lastCheckpoint = stepCheckpoint;
        _PlayerTransform.checkpoint = stepCheckpoint.position;
        
        print("ENTRO A UN CHECKPOINT!!! ----------------------------------");
    }


    /// <summary>
    /// Returns last checkpoint stored
    /// </summary>
    public Transform GetCheckpoint(){
        return lastCheckpoint;
    }
}
