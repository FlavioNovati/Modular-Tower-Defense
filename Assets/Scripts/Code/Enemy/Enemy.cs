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

    public void SetTarget(Transform target)
    {
        m_TargetTransform = target;
        m_Agent.SetDestination(m_TargetTransform.position);
    }
    
    public void Tick()
    {
        m_Agent.SetDestination(m_TargetTransform.position);

        //if distance from target is less than a value -> Damage player Base
        if ((transform.position - m_TargetTransform.position).magnitude < 1f)
            this.gameObject.SetActive(false);
    }

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
        OnDeath -= OnDeath;
    }

    private void OnEnable(){ m_Hp = m_Settings.HP; }
}
