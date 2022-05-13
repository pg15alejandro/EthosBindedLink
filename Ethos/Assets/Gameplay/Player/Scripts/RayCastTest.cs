//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    private GameObject[] _RayPoints;
    private int _AmmountOfRays;
    private Vector3[] _ActualPositionA;
    private Vector3[] _PrevPositionA;
    private Vector3[] temp;
    private LayerMask layer = 1 << 10;

    private int Collisions = 0;

    void Start()
    {
        _AmmountOfRays = gameObject.transform.childCount;
        _RayPoints = new GameObject[_AmmountOfRays];
        _ActualPositionA = new Vector3[_AmmountOfRays];
        _PrevPositionA = new Vector3[_AmmountOfRays];
        temp = new Vector3[_AmmountOfRays];

        for(int i = 0; i < _AmmountOfRays; i++)
        {
            _RayPoints[i] = gameObject.transform.GetChild(i).gameObject;
            _ActualPositionA[i]= new Vector3(0,0,0);
            _PrevPositionA[i] = new Vector3(0,0,0);
            temp[i] = new Vector3(0,0,0);
        }
        StartCoroutine(TempFunc());
    }

    void RayCastCollisionChecker()
    {
        RaycastHit hit;
        int counter = 0;
        bool _AlreadyCollided = false;
        foreach (var RayCast in _RayPoints)
        {
            _PrevPositionA[counter] = temp[counter];
            _ActualPositionA[counter] = RayCast.transform.position;
            float _dist = Vector3.Distance(_PrevPositionA[counter], _ActualPositionA[counter]);
            bool ray = Physics.Raycast(_PrevPositionA[counter],_ActualPositionA[counter], out hit, _dist, layer);
            temp[counter] = _ActualPositionA[counter];

            if (ray && hit.transform.tag == "Check")
            {
                Debug.Log("Collision");
            }

            if (ray && !_AlreadyCollided)
            {
                Collisions++;
                _AlreadyCollided = true; 
            }

            Debug.DrawLine(_PrevPositionA[counter], _ActualPositionA[counter], Color.magenta);
            counter++;      
        }
    }

    private IEnumerator TempFunc()
    {
        yield return new WaitForSeconds(10);
        Debug.Log(Collisions);
        StartCoroutine(TempFunc());
    }
}
