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

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void Tick()
    {

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
    }

    private void OnEnable()
    {
        m_Hp = m_Settings.HP;
    }
}
