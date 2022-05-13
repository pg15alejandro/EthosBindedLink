using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "UI")]
public class SOResources : ScriptableObject
{
    public int CurrentHealth = 100;                         //Current health
    public int MaxHealth = 100;                             //Max health
    public float healthPercentage => (float)CurrentHealth / MaxHealth;    //Health percentage

    public int CurrentEssence = 100;                        //Current essence
    public int MaxEssence = 100;                            //Max essence
    public float essencePercentage => (float)CurrentEssence / MaxEssence;    //Essence percentage

    public int CurrentStamina = 100;                        //Current stamina
    public int MaxStamina = 100;                            //Max stamina
    public float staminaPercentage => (float)CurrentStamina / MaxStamina;    //Stamina percentage

    public void RestartResources()
    {
        CurrentHealth = MaxHealth;
        CurrentEssence = MaxEssence;
        CurrentStamina = MaxStamina;
    }
}
