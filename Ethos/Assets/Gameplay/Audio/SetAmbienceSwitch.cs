//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAmbienceSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _AmbienceObject;
    [SerializeField] private string _AmbienceName;
    [SerializeField] private bool _SetAtStart;

    private void Start() {
        if(_SetAtStart)
        {
            AkSoundEngine.SetSwitch("ambience_zones", _AmbienceName, _AmbienceObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        AkSoundEngine.SetSwitch("ambience_zones", _AmbienceName, _AmbienceObject);
    }
}
