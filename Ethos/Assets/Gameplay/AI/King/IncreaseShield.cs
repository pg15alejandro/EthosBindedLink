//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseShield : MonoBehaviour
{
    private float _Scale = .1f;

    // Update is called once per frame
    void Update()
    {
        if(_Scale < 2)
            _Scale += .1f;

        transform.localScale = new Vector3(_Scale, _Scale, _Scale);
    }
}
