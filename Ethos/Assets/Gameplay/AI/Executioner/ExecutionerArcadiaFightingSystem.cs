//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionerArcadiaFightingSystem : MonoBehaviour
{
    private bool _Attacking;

    private Animator _Animator;
    [SerializeField] private SOAnimationHashes _AnimHashes;
    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private LayerMask _EnemyLayer;
    [SerializeField] private int _BaseDamage;
    private bool _CollisionDetected;

    public bool CanCollide { get; private set; }

    private void Start()
    {
        _Animator = gameObject.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        InputReader();
        var currentAnim = _Animator.GetCurrentAnimatorStateInfo(2).shortNameHash;

        if (currentAnim != _AnimHashes.SpinAttack_R && currentAnim != _AnimHashes.HighAttack_R && currentAnim != _AnimHashes.HighAttack_L
           && currentAnim != _AnimHashes.StraightAttack && currentAnim != _AnimHashes.MidAttack_R)
        {
            CanCollide = false;
        }

        if (currentAnim == _AnimHashes.SpinAttack_R || currentAnim == _AnimHashes.HighAttack_R || currentAnim == _AnimHashes.HighAttack_L
           || currentAnim == _AnimHashes.StraightAttack || currentAnim == _AnimHashes.MidAttack_R)
        {
            CanCollide = true;
        }


        if (currentAnim == _AnimHashes.PlaceHolder)
        {
            CanCollide = false;
            _CollisionDetected = false;
        }

        if (_Attacking)
        {
            //checks if the animator is playing an attack
            if (currentAnim != _AnimHashes.SpinAttack_R && currentAnim != _AnimHashes.HighAttack_R && currentAnim != _AnimHashes.HighAttack_L && currentAnim != _AnimHashes.StraightAttack && currentAnim != _AnimHashes.MidAttack_R)
            {
                if (currentAnim != _AnimHashes.FrontHit && currentAnim != _AnimHashes.BackHit)
                {
                    if (_Attacking)
                    {
                        AttackChooser(_Animator);
                    }
                }
            }
        }

        OverlapBoxCollisionChecker();
    }

    private void InputReader()
    {
        _Attacking = Input.GetButtonDown("Attack");
    }

    private void AttackChooser(Animator anim)
    {
        int RandomAttack = UnityEngine.Random.Range(0, 5);

        switch (RandomAttack)
        {
            case 0:
                anim.CrossFade("HighAttack_L", .1f, 2);
                print("Random Attack HightAttack_L");
                break;

            case 1:
                anim.CrossFade("HighAttack_R", .1f, 2);
                print("Random Attack HightAttack_R");
                break;

            case 2:
                anim.CrossFade("SpinAttack_R", .1f, 2);
                print("Random Attack SpinAttack_R");
                break;

            case 3:
                anim.CrossFade("MidAttack_R", .1f, 2);
                print("Random Attack MidAttack_R");
                break;

            case 4:
                anim.CrossFade("StraightAttack", .1f, 2);
                print("Random Attack StraightAttack");
                break;

            default:
                break;
        }
    }

    private void OverlapBoxCollisionChecker()
    {
        Collider[] hitColliders = new Collider[10];
        int x = Physics.OverlapBoxNonAlloc(transform.position + _OverlapBoxOffset, _OverlapBoxHalfSize, hitColliders, transform.rotation, _EnemyLayer, QueryTriggerInteraction.Collide); //Should have distance

        if (CanCollide && !_CollisionDetected)
        {
            foreach (var item in hitColliders)
            {
                if (item == null) continue;

                //If I hit an enemy, or one of their weapons
                if (item.gameObject.layer == LayerMask.NameToLayer("Enemy") || item.gameObject.layer == LayerMask.NameToLayer("EliteEnemy") || item.gameObject.layer == LayerMask.NameToLayer("ExecutionerBoss"))
                {
                    _CollisionDetected = true;
                    print("@@@enemy hit");
                    CollisionManagerOverlapBox(item.gameObject);
                    EnemyHitAnimationChooser(item.gameObject);
                }
            }
        }
#if UNITY_EDITOR
        DrawBoxAt(_OverlapBoxOffset, _OverlapBoxHalfSize, Color.red);
#endif
    }

    private void CollisionManagerOverlapBox(GameObject other)
    {
        var healthScr = other.GetComponent<AHealth>();
        var fightAiScr = other.GetComponentInChildren<FightSystemAI>();

        if (other.layer == LayerMask.NameToLayer("Enemy") || other.layer == LayerMask.NameToLayer("EliteEnemy"))    //normal enemy
        {
            healthScr.Damage(_BaseDamage);

            if (fightAiScr != null) fightAiScr.Engaging = true;
        }
    }

    private void EnemyHitAnimationChooser(GameObject other)
    {
        var animatorScr = other.GetComponent<Animator>();
        var randomHit = UnityEngine.Random.Range(0, 2);

        if (randomHit == 0)
        {
            print("@@@Enemy chest collider");
            animatorScr.CrossFade("ChestHit", .1f, 2);
            return;
        }
        else if (randomHit == 1)
        {
            print("@@@Enemy left arm collider");
            animatorScr.CrossFade("LeftHit", .1f, 2);
            return;
        }
    }


    private void DrawBoxAt(Vector3 offset, Vector3 scale, Color lineColor)
    {
#if UNITY_EDITOR
        //Debug.Log();"Drawing box");
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

}
