using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeLevels : MonoBehaviour
{
    [SerializeField] private SOVolumeSettings _VolumeSettings;
    [SerializeField] private Slider _MasterSlider;
    [SerializeField] private Slider _MusicSlider;
    [SerializeField] private Slider _SFXSlider;
    [SerializeField] private Slider _DialogSlider;

    [SerializeField] private AK.Wwise.RTPC _MasterRTPC, _MusicRTPC, _SfxRTPC, _DialogRTPC;

    int type = 0;

    /// <summary>
    /// Changes the RTCP value of the slider specified
    /// </summary>
    /// <param name="sliderType">Name of the slider to be changed</param>
    public void SetVolume(string sliderType)
    {
        switch (sliderType)
        {
            case "Master":
                _VolumeSettings.MasterVolume = _MasterSlider.value * 100;
                AkSoundEngine.SetRTPCValue("options_master_vol", _VolumeSettings.MasterVolume);
                break;

            case "Music":
                _VolumeSettings.MusicVolume = _MusicSlider.value * 100;
                AkSoundEngine.SetRTPCValue("options_mus_vol", _VolumeSettings.MusicVolume);
                break;

            case "SFX":
                _VolumeSettings.SFXVolume = _SFXSlider.value * 100;
                AkSoundEngine.SetRTPCValue("options_sfx_vol", _VolumeSettings.SFXVolume);
                break;

            case "Dialog":
                _VolumeSettings.DialogVolume = _DialogSlider.value * 100;
                AkSoundEngine.SetRTPCValue("options_dial_vol", _VolumeSettings.DialogVolume);
                break;

            default:
                Debug.LogWarning("No valid slider value");
                break;
        }
    }

    private void OnEnable()
    {
        _VolumeSettings.MasterVolume = _MasterRTPC.GetValue(this.gameObject);
        _VolumeSettings.MusicVolume = _MusicRTPC.GetValue(this.gameObject);
        _VolumeSettings.SFXVolume = _SfxRTPC.GetValue(this.gameObject);
        _VolumeSettings.DialogVolume = _DialogRTPC.GetValue(this.gameObject);

        _MasterSlider.value = _VolumeSettings.MasterVolume / 100;
        _MusicSlider.value = _VolumeSettings.MusicVolume / 100;
        _SFXSlider.value = _VolumeSettings.SFXVolume / 100;
        _DialogSlider.value = _VolumeSettings.DialogVolume / 100;
    }
}
