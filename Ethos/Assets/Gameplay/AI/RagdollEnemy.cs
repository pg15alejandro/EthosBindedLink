//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] _BodyPartsToFall;
    [SerializeField] private GameObject[] _BodyPartsToDisable;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
            ActivateRagdoll();

        if(Input.GetKeyDown(KeyCode.P))
            Time.timeScale = 1f;
    }

    public void ActivateRagdoll()
    {
        foreach (var item in _BodyPartsToFall)
        {
            item.AddComponent<Rigidbody>();
            item.GetComponent<BoxCollider>().isTrigger = false;
        }
        
        foreach (var item in _BodyPartsToDisable)
        {
            item.SetActive(false);
        }

        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
          
    }
}
