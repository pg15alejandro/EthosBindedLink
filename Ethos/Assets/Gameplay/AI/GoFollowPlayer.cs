//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFollowPlayer : MonoBehaviour
{
    [SerializeField] private SOTransform _Player;
    // Update is called once per frame
    void Update()
    {
        transform.position = _Player.value.position;
    }
}
