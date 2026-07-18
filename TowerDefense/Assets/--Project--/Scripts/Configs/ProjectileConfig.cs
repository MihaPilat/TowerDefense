using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile")]
public class ProjectileConfig : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; } = 15f;
    [field: SerializeField] public float _damageRadius { get; private set; } = 0f;
}
