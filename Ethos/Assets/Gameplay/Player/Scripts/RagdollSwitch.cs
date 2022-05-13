//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSwitch : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private new List<Rigidbody> _DragDollList;
    [SerializeField] private KeyCode _KeyCode;
    private void Awake()
    {
        foreach (var item in _DragDollList)
        {
            Destroy(item);
        }
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(_KeyCode))
        {
            foreach (var item in _DragDollList)
            {
                if (item.IsSleeping() == false)
                {
                    item.Sleep();
                }
                else
                {
                    item.WakeUp();
                }
            }
        }
    }


}
