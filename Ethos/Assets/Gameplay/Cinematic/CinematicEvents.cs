//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicEvents : MonoBehaviour
{
    [SerializeField] private GameObject _GameMusicGameObject;

    public void StartCinematic()
    {
        AkSoundEngine.PostEvent("Stop_Ethos_Music", _GameMusicGameObject);
        AkSoundEngine.PostEvent("Play_Cinematic_Music", _GameMusicGameObject);
    }
}
