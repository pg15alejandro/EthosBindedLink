//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIcon : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] SOTransform _PlayerTransform;


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    void Update()
    {
        transform.LookAt(_PlayerTransform.value);
    }
}
