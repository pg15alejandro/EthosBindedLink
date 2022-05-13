//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionerFightingSystem : MonoBehaviour
{
    private Animator _Anim;
    private ExecutionerMovement _Movement;
    private bool _AnimEnded = true;
    [Header("Attack Timer")]
    [SerializeField] private float _MaxTimeBetweenAttacks;
    [SerializeField] public float TimeBetweenAttacks;
    [Header("Needed")]
    [SerializeField] private SOAnimationHashes _AnimHashes;
    [Header("Collider and Fight")]
    [SerializeField] private LayerMask _PlayerLayer;
    [SerializeField] private LayerMask _PlayerSword;
    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private int _Damage;
    private bool _FirstAttack = true;
    private bool _Ready = false;
    private bool _hasntBeenDetected;
    public bool WasHit;

    public bool NEVERSWITCH;

    private void Start()
    {
        _Anim = gameObject.GetComponentInParent<Animator>();
        _Movement = gameObject.GetComponentInParent<ExecutionerMovement>();
    }

    private void Update()
    {
        if (TimeBetweenAttacks > 0)
        {
            TimeBetweenAttacks -= Time.deltaTime;  //decreasing the timerD
        }
        if (_Movement.CanCollide)
        {
            OverlapBoxCollisionChecker();
        }
        Attack();
    }

    private void Attack()
    {
        if (WasHit && !NEVERSWITCH)
        {
            _Movement.PlayerDetected();
            AttackChooser(_Anim);
            WasHit = false;
            _Ready = true;
            TimeBetweenAttacks = _MaxTimeBetweenAttacks;   //reset the timer
            _FirstAttack = false;
            NEVERSWITCH = true;
        }
        if (_Anim.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHashes.PlaceHolder)
        {
            _Ready = true;
            _hasntBeenDetected = true;
            _AnimEnded = true;
        }

        if (_Movement.CanAttack && _FirstAttack)
        {
            AttackChooser(_Anim);
            _Ready = true;
            TimeBetweenAttacks = _MaxTimeBetweenAttacks;   //reset the timer
            _FirstAttack = false;
        }

        if (_Movement.CanAttack && _AnimEnded && TimeBetweenAttacks <= 0)
        {
            TimeBetweenAttacks = _MaxTimeBetweenAttacks;   //reset the timer
            AttackChooser(_Anim);
            _Ready = true;
        }
    }

    private void AttackChooser(Animator anim)
    {
        int RandomAttack = UnityEngine.Random.Range(1, 6);

        switch (RandomAttack)
        {
            case 1:
                anim.CrossFade("HightAttack_L", .1f, 2);
                print("Random Attack HightAttack_L");
                break;

            case 2:
                anim.CrossFade("HightAttack_R", .1f, 2);
                print("Random Attack HightAttack_R");
                break;

            case 3:
                anim.CrossFade("SpinAttack_R", .1f, 2);
                print("Random Attack SpinAttack_R");
                break;

            case 4:
                anim.CrossFade("MidAttack_R", .1f, 2);
                print("Random Attack MidAttack_R");
                break;

            case 5:
                anim.CrossFade("StraightAttack", .1f, 2);
                print("Random Attack StraightAttack");
                break;

            default:
                break;
        }
    }

    public virtual void OverlapBoxCollisionChecker()
    {
        Collider[] hitColliders = new Collider[10];
        int _PlayerL = (int)Mathf.Log(_PlayerLayer.value, 2);
        int x = Physics.OverlapBoxNonAlloc(transform.position + _OverlapBoxOffset, _OverlapBoxHalfSize, hitColliders, transform.rotation, _PlayerLayer); //Should have distance
        if (_AnimEnded && _Ready)
        {
            foreach (var item in hitColliders)
            {
                if (item == null) continue;

                if (_hasntBeenDetected)
                {
                    _hasntBeenDetected = false;
                    _Ready = false;
                    _AnimEnded = false;
                    CollisionManagerOverlapBox(item.gameObject);
                    PlayerHitAnimationChooser(item.gameObject);
                }
            }
        }
        //#if UNITY_EDITOR
        DrawBoxAt(_OverlapBoxOffset, _OverlapBoxHalfSize, Color.red);
        //#endif

    }

    private void PlayerHitAnimationChooser(GameObject other)
    {
        if (other.GetComponent<AHealth>()._CurrentHealth <= 0)
            return;
        var animatorScr = other.GetComponentInParent<Animator>();
        var RandomNumber = UnityEngine.Random.Range(0, 2);
        switch (RandomNumber)
        {
            case 0:
                animatorScr.CrossFade("ChestHit", 0.1f, 2);
                break;
            case 1:
                animatorScr.CrossFade("LeftHit", 0.1f, 2);
                break;
        }

    }


    private void DrawBoxAt(Vector3 offset, Vector3 scale, Color lineColor)
    {
#if UNITY_EDITOR
        var from = transform.GetOffsetVector(offset, scale, new Vector3(-1, -1, -1));
        var to = transform.GetOffsetVector(offset, scale, new Vector3(-1, -1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(-1, 1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(-1, 1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(1, -1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(1, -1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(1, 1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(1, 1, 1));
        Debug.DrawLine(from, to, lineColor);
#endif
    }

    public void CollisionManagerOverlapBox(GameObject other)
    {
        if (other.transform.gameObject.layer == 10) //checks if the weapon collided with the player
        {
            if (other.transform.gameObject.GetComponentInChildren<FightingSystem>().Blocking)
            {
                AkSoundEngine.PostEvent("Play_Arcadia_Block", this.gameObject);
                other.transform.GetComponent<PlayerHealth>().Damage(_Damage / 2);
            }
            else
            {
                other.transform.GetComponent<PlayerHealth>().Damage(_Damage);
            }
        }
    }
}
