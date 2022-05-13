//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFollowsPlayer : MonoBehaviour
{
    [SerializeField] private SOTransform _PlayerTransform;
    [SerializeField] private ParticleSystem[] _ParticleToPlay;
    [SerializeField] private Vector3 _OffsetDestination;
    [SerializeField] private float _SpeedOrb;
    [SerializeField] private GameObject[] _Children;
    [System.NonSerialized] public ParticleSystem ExplodeParticle;
    [System.NonSerialized] public int AmountOfHealth;
    [System.NonSerialized] public int AmountOfEssence;
    [System.NonSerialized] public PlayerHealth PlayerHealth;
    [System.NonSerialized] public PlayerController PlayerController;



    private void Start()
    {
        AkSoundEngine.PostEvent("Play_Orbs", this.gameObject);
        // foreach (var item in _ParticleToPlay)
        // {
        //     item.Play();
        // }
    }

    private void Update()
    {
        float step = _SpeedOrb * Time.deltaTime;

        var dest = Vector3.MoveTowards(gameObject.transform.position, _PlayerTransform.value.position + _OffsetDestination, step);
        gameObject.transform.position = dest;
        ExplodeParticle.transform.position = _PlayerTransform.value.position;
        if (Vector3.Distance(gameObject.transform.position, _PlayerTransform.value.position + _OffsetDestination) < .3)
        {
            gameObject.transform.position = _PlayerTransform.value.position + _OffsetDestination;
            gameObject.transform.rotation = new Quaternion(90, 0, _PlayerTransform.value.rotation.z, 0);
            AddResource(AmountOfHealth, AmountOfEssence);
        }
    }

    public void AddResource(int health, int essence)
    {
        AkSoundEngine.SetSwitch("item_type", "orb", this.gameObject);
        AkSoundEngine.PostEvent("Play_Pick_up", this.gameObject);
        PlayerHealth.AddHealth(health);
        PlayerController.AddEssence(essence);
        var x =Instantiate(ExplodeParticle, _PlayerTransform.value.position + _OffsetDestination, _PlayerTransform.value.rotation);
        x.GetComponent<GettingOrbitParticle>().OffsetDestination = _OffsetDestination;
        Destroy(gameObject);
    }
}
