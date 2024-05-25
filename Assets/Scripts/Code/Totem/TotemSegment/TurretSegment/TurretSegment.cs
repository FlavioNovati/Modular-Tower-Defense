using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSegment : TotemSegment, IBuffable
{
    //Turret Variables
    [Header("Turret Variables")]
    [SerializeField] private TurretSegment_Data m_Settings;
    private Enemy m_Target = null;
    private float m_TurretShootDelay = 0f;
    [Space]

    //Turrets Stats
    protected float m_AttackRadius;
    protected float m_AttackDelay;
    protected float m_BaseDamage;

    //Animation
    [Header("Animation")]
    [SerializeField] private Transform m_BarrelTransform;
    protected float m_StartingRotation = 0f;
    protected float m_StartingRotationTime = 0f;

    public override void Activate()
    {
        base.Activate();
        //Reset parameters
        m_AttackRadius = m_Settings.AttackRadiuos;
        m_AttackDelay = m_Settings.AttackDelay;
        m_BaseDamage = m_Settings.BaseDamage;
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

        //Reset cool down
        if( m_Target != null )
            m_TurretShootDelay = Time.time;
    }

    /// <summary>
    /// return an object transform within overlappin distance with layermask 7, if null no target is found
    /// </summary>
    /// <returns></returns>
    private Enemy TryGetTarget()
    {
        Collider[] Enemy = Physics.OverlapSphere(new Vector3(transform.position.x, 0f, transform.position.z), m_AttackRadius, (1<<7));
        if (Enemy.Length > 0)
        {
            int randomEnemy = UnityEngine.Random.Range(0, Enemy.Length);
            if(InReach(Enemy[randomEnemy].transform.position))
                return Enemy[randomEnemy].GetComponent<Enemy>();
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
        //Lose Target if gameobject is disabled (Dead)
        if (!m_Target.gameObject.activeInHierarchy)
        {
            m_Target = null;
            return;
        }
        //Lose target if not in reach
        if (!InReach(m_Target.transform.position))
        {
            m_StartingRotation = transform.eulerAngles.y;
            m_StartingRotationTime = Time.time;
            m_Target = null;
            return;
        }

        //Shoot the target
        if (Time.time > m_TurretShootDelay + m_AttackDelay)
        {
            m_TurretShootDelay = Time.time;
            ProjectileManager.Instance.RequestBullet(transform.position, m_Target, m_BaseDamage);
        }
        //Rotate towards target
        AnimateAttack();
    }

    /// <summary>
    /// Overlap sphere can get target even if is further that actual attack range, this function return true if distance between target and turret is less equals than attack radius
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private bool InReach(Vector3 targetPos)
    {
        targetPos.y = 0f;
        Vector3 currentPos = transform.position;
        currentPos.y = 0f;

        return Vector3.Distance(currentPos, targetPos) <= m_AttackRadius;
    }

    /// <summary>
    /// Rotate towards target
    /// </summary>
    private void AnimateAttack()
    {
        if (m_Target == null)
            return;

        //Body Rotation
        Vector3 targetPos = m_Target.transform.position;
        targetPos.y = 0f;
        Vector3 currentPos = transform.position;
        currentPos.y = 0f;
        //Apply Body Rotation
        transform.rotation = Quaternion.LookRotation(targetPos - currentPos);
        //Barrel Rotation
        m_BarrelTransform.rotation = Quaternion.LookRotation((m_Target.transform.position - transform.position));
    }

    #endregion

    #region IBuffable Region
    /// <summary>
    /// Sum current stats according to buff values
    /// </summary>
    /// <param name="buff">Buff to add</param>
    public void AddBuff(Buff buff)
    {
        m_AttackRadius += buff.BuffIncrement.RadiuosBuff;
        m_AttackDelay += buff.BuffIncrement.AttackDelayBuff;
        m_BaseDamage += buff.BuffIncrement.DamageBuff;
    }

    /// <summary>
    /// Subtract current stats according to buff values
    /// </summary>
    /// <param name="buff">Buff to remove</param>
    public void RemoveBuff(Buff buff)
    {
        m_AttackRadius -= buff.BuffIncrement.RadiuosBuff;
        m_AttackDelay -= buff.BuffIncrement.AttackDelayBuff;
        m_BaseDamage -= buff.BuffIncrement.DamageBuff;
    }
    #endregion
}
