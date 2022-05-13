//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class MoveTimeLerp : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    VisualEffect _Trail;

    float _LerpElapsed;
    private bool _StartMove;

    private void Start() {
        _Trail = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _LerpElapsed = 0;
            _StartMove = true;

            /*Vector3 test = _Trail.GetVector3("Position");

            Debug.Log("wuu");*/
        }

        if (_StartMove)
            _LerpElapsed += Time.deltaTime;

        if (_LerpElapsed >= 1){
            _StartMove = false;        
        }

        Vector3 lerpPos = Vector3.Lerp(pointA.position, pointB.position, _LerpElapsed);
        _Trail.SetVector3("Position", lerpPos);
    }
}
