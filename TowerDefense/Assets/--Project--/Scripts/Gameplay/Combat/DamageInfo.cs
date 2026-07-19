public struct DamageInfo
{
    public int Damage { get; set; }
    public DamageType Type { get; set; }

    public DamageInfo(int damage, DamageType type)
    {
        Damage = damage;
        Type = type;
    }
}