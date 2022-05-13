//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "VideoSettings", menuName = "VideoSettings")]
public class SOGameSettings : ScriptableObject
{
    public VolumeProfile Volume;
    public VolumeProfile Volume2;
}

