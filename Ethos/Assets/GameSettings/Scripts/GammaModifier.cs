//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GammaModifier : MonoBehaviour
{
    [SerializeField] private SOGameSettings _PostProcessingVolume;
    [SerializeField] private Slider _GammaSlider;
    [SerializeField] private Slider _BrightnessSlider;
    private LiftGammaGain gamma;
    private LiftGammaGain gamma2;
    private float _GammaValue;
    private float _BrightnessValue;


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    void Start()
    {
        _PostProcessingVolume.Volume.TryGet(out gamma);
        _PostProcessingVolume.Volume2.TryGet(out gamma2);
        _GammaSlider.value = gamma.gamma.value.w;
    }


    /// <summary>
    /// Changes the visual value of the slider specified
    /// </summary>
    /// <param name="sliderType">Name of the slider to be changed</param>
    public void SetVideo(string sliderType)
    {
        switch (sliderType)
        {
            case "Gamma":
                _GammaValue = _GammaSlider.value;
                var GammaVal = gamma.gamma.value;
                GammaVal.w = _GammaValue;
                gamma.gamma.value = GammaVal;
                gamma2.gamma.value = GammaVal;
                break;

            case "Brightness":
                // var BrightVal = gamma.gamma.value;
                // BrightVal.w = _GammaValue;
                // gamma.gamma.value = BrightVal;
                break;

            default:
                    Debug.LogWarning("No valid slider value");
                break;
        }
    }
}