using System.Collections.Generic;
using UnityEngine;

public class TurretSegment : TotemSegment, IBuffable
{
    [SerializeField] private Transform m_MuzzleTransform;
    [SerializeField] private TurretSegment_Data m_Settings;

    protected float m_AttackRadiuos;
    protected float m_AttackDelay;
    protected float m_BaseDamage;
    protected Projectile m_BaseBullet;

    public List<Buff> buffs = new List<Buff>();

    public override void Activate()
    {
        base.Activate();
        //Reset parameters
        m_AttackRadiuos = m_Settings.AttackRadiuos;
        m_AttackDelay = m_Settings.AttackDelay;
        m_BaseDamage = m_Settings.BaseDamage;
        m_BaseBullet = m_Settings.BaseBullet;
    }

    public void AddBuff(Buff buff)
    {
        Debug.Log("Buff Added: \n"+buff.ToString());
        m_AttackRadiuos += buff.BuffIncrement.RadiuosBuff;
        m_AttackDelay += buff.BuffIncrement.AttackDelayBuff;
        m_BaseDamage += buff.BuffIncrement.DamageBuff;
        buffs.Add(buff);
    }

    public void RemoveBuff(Buff buff) //Only for logic continuity, could be removed since the data is resetted on activate
    {
        Debug.Log("Buff Removed: \n" + buff.ToString());
        //Reset parameters
        m_AttackRadiuos = m_Settings.AttackRadiuos;
        m_AttackDelay = m_Settings.AttackDelay;
        m_BaseDamage = m_Settings.BaseDamage;
        m_BaseBullet = m_Settings.BaseBullet;
        buffs.Remove(buff);
    }
}
