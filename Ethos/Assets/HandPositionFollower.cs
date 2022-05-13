//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionFollower : MonoBehaviour
{
    [SerializeField] private SOTransform _HandTransform;

    // Update is called once per frame
    void Update()
    {
        _HandTransform.Hand = gameObject.transform;
    }
}
