//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExecutionerMovement : MonoBehaviour
{
    [SerializeField] private SOTransform _Player;
    [SerializeField] private float _RotateSpeed;
    [SerializeField] private SOEnemyRegister _EnemyList;
    [SerializeField] private Transform _ProtectKingPos;
    [SerializeField] private GameplayLogic _GmLogic;
    [SerializeField] private SOAnimationHashes _AnimHash;

    [System.NonSerialized] public bool CanAttack;
    [System.NonSerialized] public bool ProtectKing;

    private float _Distance;
    private NavMeshAgent _NavMesh;
    private FieldOfView _Fov;
    private Animator _Anim;
    private bool _ISawPlayer = false;
    private Quaternion _LookRotation;
    private Vector3 _Direction;


    [System.NonSerialized] public bool CanCollide;
    private bool _CanRotate;
    private bool _StartMove;


    private float _PrevTo;
    private bool _GetValue;
    private float _TLerp;
    private float _AnimVal;
    [SerializeField] private float _LerpT = .01f;

    private void Start()
    {
        _NavMesh = gameObject.GetComponent<NavMeshAgent>();
        _Fov = gameObject.GetComponent<FieldOfView>();
        _Anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        var currentAnimHash = _Anim.GetCurrentAnimatorStateInfo(2).shortNameHash;
        if (currentAnimHash != _AnimHash.SpinAttack_R && currentAnimHash != _AnimHash.HighAttack_R && currentAnimHash != _AnimHash.HighAttack_L
         || currentAnimHash != _AnimHash.StraightAttack && currentAnimHash != _AnimHash.MidAttack_R)
        {
            isPlayerInView();
            FollowPlayer();
        }

        LookAtPlayer();
    }

    private void isPlayerInView()
    {
        if (_GmLogic.ExecStartAttack)
        {
            _ISawPlayer = true;
            _NavMesh.SetDestination(_Player.value.position);
            _EnemyList.ExecutionerInside(gameObject);
        }
    }

    public void PlayerDetected()
    {
        _GmLogic.ExecStartAttack = true;
        _ISawPlayer = true;
        AkSoundEngine.PostEvent("Patio_Arcadia_019", this.gameObject);
        _EnemyList.ExecutionerInside(gameObject);
    }

    private void FollowPlayer()
    {
        _Distance = Vector3.Distance(transform.position, _Player.value.position);

        if (_ISawPlayer)
        {
            if (_Distance > 3)
            {
                if (!gameObject.GetComponent<ExecutionerHealth>().IsAlive) return;
                if (!_EnemyList.Executioners.Contains(gameObject)) _EnemyList.Executioners.Add(gameObject);
                _Anim.SetFloat("Velocity", LerpAnimator(1));
                _NavMesh.isStopped = false;
                _NavMesh.SetDestination(_Player.value.position);
                CanAttack = false;
            }
            else
            {
                CanAttack = true;
                _Anim.SetFloat("Velocity", LerpAnimator(0));
                _NavMesh.isStopped = true;
            }
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
