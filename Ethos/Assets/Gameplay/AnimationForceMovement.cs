//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

public class AnimationForceMovement : MonoBehaviour
{
    private Rigidbody _Rb;
    private Animator _Anim;
    private Transform _ModelTransf;
    private PlayerController _Pc;

    private void Start()
    {
        _Rb = GetComponentInParent<Rigidbody>();
        _Anim = GetComponent<Animator>();
        _ModelTransf = GetComponentInParent<Transform>();
        _Pc = GetComponentInParent<PlayerController>();
    }

    private void ApplyMovement(float _ForceAmmount)
    {
        _Anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
        _Rb.AddForce(_ModelTransf.forward * _ForceAmmount, ForceMode.VelocityChange);
        print("Applying movement in animation");
    }
}
