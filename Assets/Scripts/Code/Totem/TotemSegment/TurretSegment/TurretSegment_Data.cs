using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Settings", menuName = "Settings/Turret")]
public class TurretSegment_Data : ScriptableObject
{
    [Header("Turrets Stats")]
    public float AttackRadiuos = 3f;
    public float AttackDelay = 1f;
    public float BaseDamage = 1f;
    public Projectile BaseBullet = null;
    [Header("Animation Parameters")]
    public float RotationBaseSpeed = 1f;
    public float RotationMaxAngles = 25f;
}
