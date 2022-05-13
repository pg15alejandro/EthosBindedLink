//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KingMovement : MonoBehaviour
{
    [SerializeField] private SOTransform _PlayerTransform;

    private Animator _Anim;
    private NavMeshAgent _NavMesh;
    private KingFightingSystem _Fight;
    private KingController _Controller;


    private float _Distance;
    [SerializeField] private float _DistanceToStop;
    [SerializeField] private float _WalkBackDistance;
    [SerializeField] private Transform _Throne;
    [SerializeField] private float _RotSpeed = 1f;

    public bool DoMovement { get; private set; }
    public bool ReachedThrone { get; private set; }

    private bool _StartMove;
    private float _LerpPct;
    private Quaternion _FromRot;
    private Quaternion _ToRot;
    private bool _Rotate;
    private bool _MovingAnim;


    // Start is called before the first frame update
    void Start()
    {
        _Anim = GetComponent<Animator>();
        _NavMesh = GetComponent<NavMeshAgent>();
        _Fight = GetComponent<KingFightingSystem>();
        _Controller = GetComponent<KingController>();

        DoMovement = true;
        _StartMove = true;
    }

    public void SetDoMovement()
    {
        DoMovement = true;
        _StartMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetDest();
        UpdateRot();
    }


    public void SetLerpRotations()
    {
        _LerpPct = 0f;
        _FromRot = transform.rotation;

        transform.LookAt(_PlayerTransform.value);
        _ToRot = transform.rotation;

        _Rotate = true;
    }

    private void SetLerpRotations(Vector3 lookAtPos)
    {
        _LerpPct = 0f;
        _FromRot = transform.rotation;

        transform.LookAt(lookAtPos);
        _ToRot = transform.rotation;

        _Rotate = true;
    }

    private void UpdateRot()
    {
        if (!_Rotate) return;

        _LerpPct += Time.deltaTime * _RotSpeed;
        transform.rotation = Quaternion.Lerp(_FromRot, _ToRot, _LerpPct);

        if (_LerpPct >= 1){
            _Rotate = false;
            
            if(_Controller._KingStage == KingStage.SECOND)
                ReachedThrone = true;
        }
    }

    private void SetDest()
    {
        if (_Controller._KingStage == KingStage.SECOND)
        {
            if (!DoMovement) return;

            if (!_MovingAnim)
            {
                _MovingAnim = true;
                _Anim.SetBool("Walk", true);
                _Anim.SetFloat("Velocity", 1);
            }

            _NavMesh.SetDestination(_Throne.position);

            if (_NavMesh.remainingDistance <= 0.1f)
            {
                if (_StartMove)
                {
                    _StartMove = false;
                    return;
                }
                DoMovement = false;                                
                _MovingAnim = false;
                _Anim.SetBool("Walk", false);

                SetLerpRotations(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
            }
        }
        else
        {
            if (_Controller._KingState == KingState.DEFEND && !_Rotate)
                SetLerpRotations();

            if (!DoMovement) return;

            Vector3 distDifference = (transform.position - _PlayerTransform.value.position).normalized;
            Vector3 fightPos = _PlayerTransform.value.position + (distDifference * _DistanceToStop);

            if (_Controller._KingState == KingState.ATTACK)
            {
                _Anim.SetBool("Block", false);
                if (!_MovingAnim)
                {
                    _MovingAnim = true;
                    _Anim.SetBool("Walk", true);
                    _Anim.SetFloat("Velocity", 1);
                }

                _NavMesh.SetDestination(fightPos);

                if (_NavMesh.remainingDistance <= 0.1f)
                {
                    if (_StartMove)
                    {
                        _StartMove = false;
                        return;
                    }
                    DoMovement = false;
                    _MovingAnim = false;
                    _Anim.SetBool("Walk", false);
                }
            }

            if (_Controller._KingState == KingState.DEFEND)
            {
                if (!_MovingAnim)
                {
                    _MovingAnim = true;
                    _Anim.SetBool("Walk", true);
                    _Anim.SetFloat("Velocity", -1);
                    _Anim.SetBool("Block", true);
                }

                //If the player is close to the king, go back
                if (Vector3.Distance(fightPos, _PlayerTransform.value.position) <= _DistanceToStop + 1)
                {
                    var defendPos = _PlayerTransform.value.position + (distDifference * _WalkBackDistance);
                    _NavMesh.SetDestination(defendPos);

                    if (_NavMesh.remainingDistance <= 0.1f)
                    {
                        if (_StartMove)
                        {
                            _StartMove = false;
                            return;
                        }
                        DoMovement = false;
                        _MovingAnim = false;
                        _Anim.SetBool("Walk", false);
                        _Anim.SetBool("Block", false);
                    }
                }
                else
                {
                    DoMovement = false;
                    _MovingAnim = false;
                    _Anim.SetBool("Walk", false);
                    _Anim.SetBool("Block", false);
                }


            }
        }
    }
}
