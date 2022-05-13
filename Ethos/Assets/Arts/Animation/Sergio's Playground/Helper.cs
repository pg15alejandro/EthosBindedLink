using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [Range(0, 1)] public float vertical;

    public bool playAnim;

    public string[] attacks;

    [SerializeField] public float animTransition = 0.2f;

    public bool enableRootMotion;
    public bool usePotion;
    public bool interacting;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        enableRootMotion = !anim.GetBool("canMove");
        anim.applyRootMotion = enableRootMotion;

        interacting = anim.GetBool("interacting");

        if(usePotion)
        {
            anim.Play("Drink Potion");
            usePotion = false;
        }

        if(interacting)
        {
            playAnim = false;
            vertical = Mathf.Clamp(vertical, 0, 0.5f);
        }


        if(playAnim)
        {
            string targetAnim;
            int r = Random.Range(0, attacks.Length);
            targetAnim = attacks[r];

            if(vertical > 0.5f)
            {
                targetAnim = "Arcadia_Attack03";
            }

            vertical = 0;
            anim.CrossFade(targetAnim, animTransition);
            //anim.SetBool("canMove", false);
            //enableRootMotion = true;
            playAnim = false;
        }

        anim.SetFloat("Vertical", vertical);

        usePotion = false;
    
    }
}
