using UnityEngine;

public class Projectile : MonoBehaviour
{
    const float PROJECTILE_SPEED = 10f;

    private Vector3 m_StartingPosition;
    private Enemy m_EnemyTarget;
    private float m_Damage;
    private float m_Progress = 0f;

    public void SetUp(Vector3 startingPos, Enemy target, float damage)
    {
        m_StartingPosition = startingPos;
        transform.position = startingPos;
        m_EnemyTarget = target;
        m_Damage = damage;
        m_Progress = 0f;
    }
    
    public void Tick()
    {
        if(!m_EnemyTarget.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.Lerp(m_StartingPosition, m_EnemyTarget.transform.position, m_Progress);
        m_Progress += Time.deltaTime * PROJECTILE_SPEED;

        if (m_Progress > 1f)
        {
            m_EnemyTarget.TakeDamage(m_Damage);
            this.gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        m_EnemyTarget = null;
    }
}
