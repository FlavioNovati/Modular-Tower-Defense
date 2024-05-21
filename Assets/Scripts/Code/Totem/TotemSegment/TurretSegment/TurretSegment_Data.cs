using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Settings", menuName = "Settings/Turret")]
public class TurretSegment_Data : ScriptableObject
{
    public float AttackRadiuos = 3f;
    public float AttackDelay = 1f;
    public float BaseDamage = 1f;
    public Projectile BaseBullet = null;
}
