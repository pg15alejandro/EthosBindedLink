//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class DungeonDisabler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _GOList;
    [SerializeField] private Volume _Pp;
    // [SerializeField] private VolumeProfile _DungeonProf;
    // [SerializeField] private VolumeProfile _OutsideProf;
    [SerializeField] private bool _Load;
    [SerializeField] private CinemachineVirtualCamera _CmVc;

    [SerializeField] private int _RenderDistance;

    private void OnTriggerEnter(Collider other)
    {
        int _PlayerL = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == _PlayerL)
        {
            foreach (var item in _GOList)
            {
                item.transform.gameObject.SetActive(_Load);
            }
                _CmVc.m_Lens.FarClipPlane = _RenderDistance;
        }
    }

    public void DoDisableEnable()
    {
        foreach (var item in _GOList)
            {
                item.transform.gameObject.SetActive(_Load);
            }
             _CmVc.m_Lens.FarClipPlane = 5000;
    }
}
