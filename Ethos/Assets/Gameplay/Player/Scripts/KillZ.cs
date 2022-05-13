//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZ : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private WinLose _WinLose;
    [SerializeField] private float _Height = -30;

    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        if (transform.position.y < _Height)
        {
            _WinLose.Death();
        }
    }
}
