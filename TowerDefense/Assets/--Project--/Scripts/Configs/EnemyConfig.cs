using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public ResistanceConfig ResistanceConfig { get; private set; }
    [field: SerializeField] public float Speed { get; private set; } = 3f;
    [field: SerializeField] public int Damage { get; private set; } = 10;
    [field: SerializeField] public int RewardGold { get; private set; } = 2;
    [field: SerializeField] public float DeathDelay { get; private set; } = 0.4f;
}