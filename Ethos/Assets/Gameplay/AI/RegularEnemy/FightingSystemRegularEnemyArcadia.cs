//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingSystemRegularEnemyArcadia : MonoBehaviour
{
    private Animator _Anim;
    [SerializeField] private SOAnimationHashes _AnimHash;
    private PlayerController _PlayerCont;
    // Start is called before the first frame update

    //INPUT
    private bool _Attack;
    private float _Block;
    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private LayerMask _EnemyLayer;
    [SerializeField] private int _BaseDamage;
    private InputSwitchCombo _InputCombo;
    private bool _CollisionDetected;

    private bool _DetectingCollisions;
    void Start()
    {
        _Anim = gameObject.GetComponentInParent<Animator>();
        _PlayerCont = gameObject.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputReader();
        Block();
        _Anim = gameObject.GetComponentInParent<Animator>();
        if (_Anim.runtimeAnimatorController.name != "Anim_ArcadiaAsGuard_AS") return;
        var actAnim = _Anim.GetCurrentAnimatorStateInfo(2).shortNameHash;
        if (_Attack && actAnim == _AnimHash.PlaceHolder) Attack();

        if (actAnim == _AnimHash.Attack_01 || actAnim == _AnimHash.Attack_02)
        {
            _DetectingCollisions = true;
        }
        else if (actAnim == _AnimHash.PlaceHolder)
        {
            _DetectingCollisions = false;
            _CollisionDetected = false;
        }

        OverlapBoxCollisionChecker();
    }

    private void Block()
    {
        if (_Anim.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHash.PlaceHolder || _Anim.GetCurrentAnimatorStateInfo(1).shortNameHash == _AnimHash.Block)
        {
            if (_Block >= .1f)
            {
                _Anim.SetBool("Block", true);
            }
            else
            {
                _Anim.SetBool("Block", false);
            }
        }
    }

    private void Attack()
    {
        int randomNumber = UnityEngine.Random.Range(1, 3);
        switch (randomNumber)
        {
            case 1:
                _Anim.CrossFade("Attack_01", .1f, 2); //naming may change
                break;

            case 2:
                _Anim.CrossFade("Attack_02", .1f, 2); //naming may change
                break;
        }

    }

    private void InputReader()
    {
        _Attack = Input.GetButtonDown("Attack");
        _Block = Input.GetAxisRaw("Block");
    }


    private void OverlapBoxCollisionChecker()
    {
        Collider[] hitColliders = new Collider[10];
        int x = Physics.OverlapBoxNonAlloc(transform.position + _OverlapBoxOffset, _OverlapBoxHalfSize, hitColliders, transform.rotation, _EnemyLayer, QueryTriggerInteraction.Collide); //Should have distance

        if (_DetectingCollisions && !_CollisionDetected)
        {
            foreach (var item in hitColliders)
            {
                if (item == null) continue;

                //If I hit an enemy, or one of their weapons
                if (item.gameObject.layer == LayerMask.NameToLayer("Enemy") || item.gameObject.layer == LayerMask.NameToLayer("EliteEnemy"))
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

    private void EnemyHitAnimationChooser(GameObject other)
    {
        var animatorScr = other.GetComponentInParent<Animator>();
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

    private void CollisionManagerOverlapBox(GameObject other)
    {
        var healthScr = other.GetComponent<AHealth>();
        var fightAiScr = other.GetComponentInChildren<FightSystemAI>();
        var movaiSrc = other.GetComponent<MovementAI>();

        if (movaiSrc != null)
        {
            movaiSrc.PlayerDetected();
        }

        if (other.layer == LayerMask.NameToLayer("Enemy") || other.layer == LayerMask.NameToLayer("EliteEnemy"))    //normal enemy
        {
            healthScr.Damage(_BaseDamage);
            if (fightAiScr != null) fightAiScr.Engaging = true;
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
