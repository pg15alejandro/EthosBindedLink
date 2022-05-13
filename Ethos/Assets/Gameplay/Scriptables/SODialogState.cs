//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingState{DUNGEON = 1, CASTLE_HALLWAY, FIRST_BUILDING, PATIO, FINAL_BATTLE}

public class SODialogState : MonoBehaviour
{
    public bool PlayingAudio;
    public BuildingState BuildingState;
}
