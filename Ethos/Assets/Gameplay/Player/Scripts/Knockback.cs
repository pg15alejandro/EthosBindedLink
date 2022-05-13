using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private float _Radius = 5f;        //Radius of detection of the enemies
    [SerializeField] private int _Force = 10;           //Force of the pushback
    [SerializeField] private float _PushedTime;
    [SerializeField] private float _AnimationDelay = 1.3f;
    private PlayerController _PlayController;
    private Transform _ModelPivotTransform;             //Position from where the knockback radius is going to start
    private Animator _Anim;                             //The character's animator
    private bool _KnockbackInput;                       //Input that says if the knockback was pressed
    private List<GameObject> _Enemies;
    private bool _PushPlayers;

    /// <summary>
    /// Sets the possesion input to given value
    /// </summary>
    /// <param name="keyPressed">Value of the input</param>    
    public void SetInput(bool keyPressed)
    {
        _KnockbackInput = keyPressed;
    }


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        _Anim = GetComponentInChildren<Animator>();
        _PlayController = GetComponent<PlayerController>();

        Transform model = transform.GetChild(0);
        _ModelPivotTransform = model.GetChild(2);
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        if (!_KnockbackInput)
            return;

        //Set the input back to false
        _KnockbackInput = false;

        //Set the animation of the player
        _Anim.SetTrigger("Knockback");

        //Create a list of enemies that are going to be pushed
        _Enemies = new List<GameObject>();

        //Get the enemy layer to knock which objects around me are enemies
        int enemyL = LayerMask.NameToLayer("Enemy");

        Collider[] hitColliders = Physics.OverlapSphere(_ModelPivotTransform.position, _Radius);
        foreach (var col in hitColliders)
        {
            if (col.gameObject.layer != enemyL) continue;

            _Enemies.Add(col.gameObject);
        }

        StartCoroutine(PushEnemies());
    }


    /// <summary>
    /// Pushes all the enemies in the given list
    /// </summary>
    private IEnumerator PushEnemies()
    {
        //Wait for .4 seconds while the animation begins
        yield return new WaitForSeconds(_AnimationDelay);

        _PushPlayers = true;

        //Tell the player controller that the knockback has finished
        //_PlayController.RestoreKnockback();

        StopAllCoroutines();
        StartCoroutine(RestoreNavMesh());
    }

    private void FixedUpdate()
    {
        if(!_PushPlayers) return;

        //Push each enemy back
        foreach (var enemy in _Enemies)
        {
            //Get the vector direction where the enemy is going to be pushed, deactivate its AI movement and push them back
            var direction = enemy.transform.position - transform.position;
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            enemy.GetComponent<Rigidbody>().isKinematic = false;

            enemy.GetComponent<Rigidbody>().velocity = direction.normalized * _Force;

            //enemy.GetComponentInChildren<Animator>().SetTrigger("PushedBack");
        }
        
    }


    /// <summary>
    /// Restores the NavMesh behaviour of all the pushed enemies
    /// </summary>
    private IEnumerator RestoreNavMesh()
    {

        //Wait for 2 seconds while the enemies are stunned
        yield return new WaitForSeconds(_PushedTime);

        _PushPlayers = false;

        //Restore all the enemys AI movement
        foreach (var enemy in _Enemies)
        {
            enemy.GetComponent<Rigidbody>().isKinematic = true;
            enemy.GetComponent<NavMeshAgent>().enabled = true;
        }

        StopAllCoroutines();
    }
}
