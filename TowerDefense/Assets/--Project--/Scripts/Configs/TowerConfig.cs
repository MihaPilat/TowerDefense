using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Configs/Tower Config")]
public class TowerConfig : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Cost { get; private set; } = 20;
    [field: SerializeField] public int Damage { get; private set; } = 10;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 3f;
    [field: SerializeField] public float AttackRange { get; private set; } = 4f;
    [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
}
