using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TurretSegment : TotemSegment, IBuffable
{
    //Turret Variables
    [Header("Turret Variables")]
    [SerializeField] private TurretSegment_Data m_Settings;
    private Transform m_Target = null;
    [Space]

    //Turrets Stats
    protected float m_AttackRadiuos;
    protected float m_AttackDelay;
    protected float m_BaseDamage;
    protected Projectile m_BaseBullet;

    //Animation
    [Header("Animation")]
    [SerializeField] private Transform m_BarrelTransform;
    protected float m_StartingRotation = 0f;
    protected float m_StartingRotationTime = 0f;
    [Space]

    //TODO: Remove DEBUG FEATURE
    public List<Buff> buffs = new List<Buff>();

    public override void Activate()
    {
        base.Activate();
        //Reset parameters
        m_AttackRadiuos = m_Settings.AttackRadiuos;
        m_AttackDelay = m_Settings.AttackDelay;
        m_BaseDamage = m_Settings.BaseDamage;
        m_BaseBullet = m_Settings.BaseBullet;
        //Set up animation values
        m_StartingRotation = transform.eulerAngles.y;
        m_StartingRotationTime = Time.time;
    }

    public override void Tick()
    {
        base.Tick();
        
        if(m_Target == null)    //Idle state
            IdleTick();
        else                    //Attack state
            AttackTick();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        m_Target = null;
    }

    #region Idle Region
    /// <summary>
    /// Rotate with a simple sine wave rotation
    /// </summary>
    private void IdleTick()
    {
        AnimateIdle();
        m_Target = TryGetTarget();
    }

    /// <summary>
    /// return an object transform within overlappin distance with layermask 7, if null no target is found
    /// </summary>
    /// <returns></returns>
    private Transform TryGetTarget()
    {
        Collider[] Enemy = Physics.OverlapSphere(new Vector3(transform.position.x, 0f, transform.position.z), m_AttackRadiuos, (1<<7));
        if (Enemy.Length > 0)
        {
            if(InReach(Enemy[0].transform.position))
                return Enemy[0].transform;
            else 
                return null;
        }
        else
            return null;
    }

    private void AnimateIdle()
    {
        //Sine angle
        float angle = Mathf.Sin(Time.time * m_Settings.RotationBaseSpeed + m_StartingRotationTime) * m_Settings.RotationMaxAngles;
        angle += m_StartingRotation;
        transform.eulerAngles = Vector3.up * angle;
    }
    #endregion

    #region Attack Region
    private void AttackTick()
    {
        //Rotate towards target
        AnimateAttack();
        
        if (!InReach(m_Target.position))//Lose target
        {
            m_StartingRotation = transform.eulerAngles.y;
            m_Target = null;
        }
        else //Shoot target
        {
            //TODO: Shoot
        }
    }

    private bool InReach(Vector3 targetPos)
    {
        targetPos.y = 0f;
        Vector3 currentPos = transform.position;
        currentPos.y = 0f;

        return Vector3.Distance(currentPos, targetPos) <= m_AttackRadiuos;
    }

    private void AnimateAttack()
    {
        //Body Rotation
        Vector3 targetPos = m_Target.position;
        targetPos.y = 0f;
        Vector3 currentPos = transform.position;
        currentPos.y = 0f;
        //Apply Body Rotation
        transform.rotation = Quaternion.LookRotation(targetPos - currentPos);
        //Barrel Rotation
        m_BarrelTransform.rotation = Quaternion.LookRotation((m_Target.position - transform.position));
    }

    #endregion

    #region IBuffable Region
    /// <summary>
    /// Sum current stats according to buff values
    /// </summary>
    /// <param name="buff">Buff to add</param>
    public void AddBuff(Buff buff)
    {
        Debug.Log("Buff Added: \n"+buff.ToString());
        m_AttackRadiuos += buff.BuffIncrement.RadiuosBuff;
        m_AttackDelay += buff.BuffIncrement.AttackDelayBuff;
        m_BaseDamage += buff.BuffIncrement.DamageBuff;
        buffs.Add(buff);
    }

    /// <summary>
    /// Subtract current stats according to buff values
    /// </summary>
    /// <param name="buff">Buff to remove</param>
    public void RemoveBuff(Buff buff) //Only for logic continuity, could be removed since the data is resetted on activate
    {
        Debug.Log("Buff Removed: \n" + buff.ToString());
        //Reset parameters
        m_AttackRadiuos -= buff.BuffIncrement.RadiuosBuff;
        m_AttackDelay = buff.BuffIncrement.DamageBuff;
        m_BaseDamage = buff.BuffIncrement.DamageBuff;
        buffs.Remove(buff);
    }
    #endregion
}
