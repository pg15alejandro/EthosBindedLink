//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGenericSwitch : MonoBehaviour
{
    [SerializeField] private string _SwitchName;
    [SerializeField] private string _ValueName;
    [SerializeField] private bool _SetAtStart;

    // Start is called before the first frame update
    void Start()
    {
        if(_SetAtStart)
            AkSoundEngine.SetSwitch(_SwitchName, _ValueName, this.gameObject);
    }

    public void SetSwitch()
    {
        AkSoundEngine.SetSwitch(_SwitchName, _ValueName, this.gameObject);
    }
    
}
