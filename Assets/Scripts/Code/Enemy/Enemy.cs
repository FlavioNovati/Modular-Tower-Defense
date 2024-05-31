using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    [SerializeField] private Enemy_Data m_Settings;

    [HideInInspector] public float Hp { get; set; }
    private float m_Hp;

    private NavMeshAgent m_Agent;
    private Transform m_TargetTransform;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = m_Settings.Speed;
    }

    /// <summary>
    /// Set target position, at the end it will try get IDamageable to damage it
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        m_TargetTransform = target;
        m_Agent.SetDestination(m_TargetTransform.position);
    }
    
    /// <summary>
    /// Update this Enemy
    /// </summary>
    public void Tick()
    {
        m_Agent.SetDestination(m_TargetTransform.position);

        //if distance from target is less than a value -> Damage player Base
        if ((transform.position - m_TargetTransform.position).magnitude < 1f)
        {
            if (m_TargetTransform.TryGetComponent<IDamageable>(out IDamageable damageable))
                damageable.TakeDamage(m_Settings.Damage);
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reduce health points, if <= 0 then disable game object
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        m_Hp -= damage;
        if (m_Hp <= 0f)
            this.gameObject.SetActive(false);
    }

    private void OnDisable()
    { 
        OnDeath?.Invoke();
        m_TargetTransform = null;
        m_Agent.enabled = false;
        OnDeath -= OnDeath;
    }

    private void OnEnable()
    {
        m_Agent.enabled = true;
        m_Hp = m_Settings.HP;
    }
}
