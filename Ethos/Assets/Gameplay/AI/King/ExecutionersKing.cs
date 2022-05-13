//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExecutionersKing : MonoBehaviour
{
    [SerializeField] private SOTransform _Player;
    [SerializeField] private float _RotateSpeed;
    [SerializeField] private Transform _ProtectKingPos;
    [SerializeField] private KingExecutionerList _ExecList;
    [SerializeField] private float _RotSpeed = 1f;

    [System.NonSerialized] public bool CanAttack;
    [System.NonSerialized] public bool ProtectKing;

    private float _Distance;
    private NavMeshAgent _NavMesh;
    private Animator _Anim;
    private bool _ISawPlayer = true;
    private Quaternion _LookRotation;
    private Vector3 _Direction;


    [System.NonSerialized] public bool CanCollide;
    private bool _CanRotate;
    private bool _StartMove = true;

    private float _LerpPct;
    private Quaternion _FromRot;
    private Quaternion _ToRot;
    private bool _Rotate;

    private float _PrevTo;
    private bool _GetValue;
    private float _TLerp;
    private float _AnimVal;
    [SerializeField] private float _LerpT = .01f;

    private void Start()
    {
        _NavMesh = gameObject.GetComponent<NavMeshAgent>();
        _Anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (_ExecList.ExecutionerDefending.Contains(gameObject))
        {
            DefendKing();
            return;
        }

        FollowPlayer();
        LookAtPlayer();
        UpdateRot();
    }

    private void DefendKing()
    {
        _NavMesh.SetDestination(_ProtectKingPos.position);

        if (_NavMesh.remainingDistance <= 0.1f)
        {
            if (_StartMove)
            {
                _StartMove = false;
                return;
            }
            _NavMesh.isStopped = true;
            SetLerpRotations(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
            _Anim.SetFloat("Velocity", LerpAnimator(0));
        }
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

        if (_LerpPct >= 1)
            _Rotate = false;
    }

    private void FollowPlayer()
    {
        _Distance = Vector3.Distance(transform.position, _Player.value.position);

        gameObject.GetComponent<ExecutionerHealthKing>().enabled = true;

        gameObject.GetComponentInChildren<KingExecutionerFightingSystem>().enabled = true;

        if (_Distance > 3)
        {
            _Anim.SetFloat("Velocity", LerpAnimator(1));
            _NavMesh.isStopped = false;
            _NavMesh.SetDestination(_Player.value.position);
            CanAttack = false;
        }
        else if (_Distance <= 3)
        {
            CanAttack = true;
            _Anim.SetFloat("Velocity", LerpAnimator(0));
            _NavMesh.isStopped = true;
        }
    }

    private void LookAtPlayer()
    {
        if (_ISawPlayer && _CanRotate)
        {

            //find the vector pointing from our position to the target
            _Direction = (_Player.value.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _LookRotation = Quaternion.LookRotation(_Direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _LookRotation, Time.deltaTime * _RotateSpeed);
        }
    }

    private float LerpAnimator(float to)
    {
        if (to != _PrevTo)
        {
            _GetValue = true;
            _TLerp = 0;
        }
        _PrevTo = to;

        if (_GetValue)
        {
            _AnimVal = _Anim.GetFloat("Velocity");
            _GetValue = false;
        }

        var newAnimVal = Mathf.Lerp(_AnimVal, to, _TLerp);
        _TLerp += _LerpT * Time.deltaTime;

        return newAnimVal;
    }

    public void EnableRotation()
    {
        _CanRotate = true;
    }

    public void DisableRotation()
    {
        _CanRotate = false;
    }

    public void EnableCollision()
    {
        CanCollide = true;
    }

    public void DisableCollision()
    {
        CanCollide = false;
    }
}
