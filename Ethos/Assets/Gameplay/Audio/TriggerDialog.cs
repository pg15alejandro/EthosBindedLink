//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField] private string _SoundToPlay;
    [SerializeField] private string _SecondSoundToPlay;
    [SerializeField] private float _TimeBetweenSounds;
    [SerializeField] private GameObject _ObjectToPlay;
    [SerializeField] private bool _TwoAudios;

    bool _HasPlayed;


    private void OnTriggerEnter(Collider other) {
        int playerL = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer != playerL) return;
        
        if(_HasPlayed) return;

        AkSoundEngine.PostEvent(_SoundToPlay, this.gameObject);
        _HasPlayed = true;

        if(_TwoAudios)
            StartCoroutine(SecondAudio());
    }

    IEnumerator SecondAudio()
    {
        yield return new WaitForSeconds(_TimeBetweenSounds);

        AkSoundEngine.PostEvent(_SecondSoundToPlay, this.gameObject);

        StopAllCoroutines();
    }
}
