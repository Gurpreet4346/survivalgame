using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/Stats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int health;
    public float speed;
    public string weapon;
}
