//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using UnityEngine;

public class KingFightingSystem : MonoBehaviour
{
    [SerializeField] private bool _DetectingCollisions;
    [SerializeField] private bool _CollisionDetected;

    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private SOAnimationHashes _AnimHash;
    KingController _KingController;
    private Animator _Anim;
    [SerializeField] private int _BlockDamage;
    [SerializeField] private int _BaseDamage;

    private void Start()
    {
        _KingController = GetComponentInParent<KingController>();
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = _Anim.GetCurrentAnimatorStateInfo(0).shortNameHash;

        if (currentState == _AnimHash.PlaceHolder)
        {
            _CollisionDetected = false;
        }

        if (_DetectingCollisions) OverlapBoxCollisionChecker();
    }

    private void OverlapBoxCollisionChecker()
    {
        LayerMask _PlayerMask = LayerMask.GetMask("Player");
        int _PlayerL = LayerMask.NameToLayer("Player");
        RaycastHit hit;

        //The world position of where the box is going to start
        Vector3 newPos = transform.position + transform.TransformVector(_OverlapBoxOffset);

        //Create a box with almost no Z
        Vector3 _NewOverlap = new Vector3(_OverlapBoxHalfSize.x, _OverlapBoxHalfSize.y, .01f);

        //Do the boxcast from the newPos, drawing our box cast until the halfsize * 2
        bool hitDetected = Physics.BoxCast(newPos, _NewOverlap, transform.forward, out hit, transform.rotation, _OverlapBoxHalfSize.z * 2, _PlayerMask, QueryTriggerInteraction.Ignore);
        if (!_CollisionDetected && hitDetected)
        {
            if (hit.transform.gameObject.layer == _PlayerL) //checks if the weapon collided with the player
            {
                if (hit.transform.gameObject.GetComponentInChildren<FightingSystem>().Blocking)
                {
                    //AkSoundEngine.PostEvent("Play_Arcadia_Block", this.gameObject);
                    var phealth = hit.transform.gameObject.GetComponent<PlayerHealth>();
                    phealth.Damage(_BlockDamage);
                }
                else
                {
                    var phealth = hit.transform.gameObject.GetComponent<PlayerHealth>();
                    phealth.Damage(_BaseDamage);
                }
                print("King hit arcadia");
                _CollisionDetected = true;
                HitAnimationChooser(hit.transform.gameObject);
            }
        }


#if UNITY_EDITOR
        //Get the middle of the box (where the box starts to be raycasted + half the size of z)
        var newOffset = new Vector3(_OverlapBoxOffset.x, _OverlapBoxOffset.y, _OverlapBoxOffset.z + _OverlapBoxHalfSize.z);
        DrawBoxAt(newOffset, _OverlapBoxHalfSize, Color.green);
#endif
    }

    private void HitAnimationChooser(GameObject other)
    {
        if (other.GetComponent<AHealth>()._CurrentHealth <= 0)
            return;

        var RanAnim = UnityEngine.Random.Range(0, 2);
        var PlayerAnim = other.GetComponent<Animator>();
        if (RanAnim == 0)
        {
            PlayerAnim.CrossFade("LeftHit", .1f, 2);
        }
        else if (RanAnim == 1)
        {
            PlayerAnim.CrossFade("ChestHit", .1f, 2);
        }
    }

    public void AnimStart()
    {
        _DetectingCollisions = true;
    }

    public void AnimStop()
    {
        _DetectingCollisions = false;
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
}
