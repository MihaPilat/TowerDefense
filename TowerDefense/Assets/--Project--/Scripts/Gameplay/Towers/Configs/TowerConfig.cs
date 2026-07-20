using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Configs/Tower Config")]
public class TowerConfig : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Cost { get; private set; } = 20;
    [field: SerializeField] public DamageType DamageType { get; private set; }
    [field: SerializeField] public List<TowerLevelData> Levels { get; private set; }
    [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
}
