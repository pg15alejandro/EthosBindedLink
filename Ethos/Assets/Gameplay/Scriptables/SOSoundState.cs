using UnityEngine;

public enum GameIntensity {BATTLE, EXPLORE}

[CreateAssetMenu(fileName = "SoundState", menuName = "Sound")]
public class SOSoundState : ScriptableObject
{
    public GameIntensity _CurrentIntensity;
}
