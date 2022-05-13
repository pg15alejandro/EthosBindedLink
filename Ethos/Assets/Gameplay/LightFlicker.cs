using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float _MaxReduction;
    [SerializeField] private float _MaxIncrease;
    [SerializeField] private float _MinRateDamping;
    [SerializeField] private float _MaxRateDamping;
    [SerializeField] private float _Strength;
    [SerializeField] private float _MaxTimeToRestart;
    [Header("Debug")]
    [SerializeField] private float _TimeToRestart;

    private Light _lightSource;
    private float _baseIntensity;
    private float _baseIntensityTweakable;

    public void Start()
    {
        _lightSource = GetComponent<Light>();
        _baseIntensity = _lightSource.intensity;
        _baseIntensityTweakable = _baseIntensity;
        StartCoroutine(DoFlicker());
    }

    void Update()
    {
        if (_TimeToRestart <= 0)
        {
            _lightSource.intensity = _baseIntensity;
            _TimeToRestart = _MaxTimeToRestart;
        }
        else
        {
            _TimeToRestart -= Time.deltaTime;
        }
    }

    private IEnumerator DoFlicker()
    {
        float _RateDamping = Random.Range(_MinRateDamping, _MaxRateDamping);
        _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - _MaxReduction, _baseIntensity + _MaxIncrease), _Strength * _RateDamping);
        yield return new WaitForSeconds(_RateDamping);
        StartCoroutine(DoFlicker());
    }
}