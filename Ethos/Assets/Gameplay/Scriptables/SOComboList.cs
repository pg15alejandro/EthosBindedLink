//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ComboList")]
public class SOComboList : ScriptableObject
{
    public List<Attack> Attacks = new List<Attack>();
    public enum Attack
    {
        Attack_01 = 0,
        Attack_02,
        Attack_03,
        Attack_04
    };
}
