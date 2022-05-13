//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

[CreateAssetMenu(fileName = "Volume", menuName = "Settings")]
public class SOVolumeSettings : ScriptableObject
{
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
    public float DialogVolume;
}
