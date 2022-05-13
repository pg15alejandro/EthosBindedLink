//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPlacement : MonoBehaviour
{
    public List<Areas> Areas;
}

[System.Serializable]
public class Areas
{
    public List<GameObject> Enemies;
    public List<GameObject> CanvasPerZone;
}