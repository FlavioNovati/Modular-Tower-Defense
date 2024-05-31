using System.Collections.Generic;
using UnityEngine;
public class Horde
{
    public delegate void HordeDefeated();
    public event HordeDefeated OnHordeDefeated;

    public List<Enemy> m_HordeMembers;
    private int m_AliveEnemy;

    private bool m_IsStarted;
    
    public Horde()
    {
        m_HordeMembers = new List<Enemy>();
    }

    /// <summary>
    /// Add Enemy list to horde list
    /// </summary>
    /// <param name="enemies"></param>
    public void AddEnemy(List<Enemy> enemies)
    {
        m_HordeMembers.AddRange(enemies);
    }

    /// <summary>
    /// Enables and position enemies
    /// </summary>
    /// <param name="startHordePosition"></param>
    /// <param name="spawnRadius"></param>
    public void StartHorde(Vector3 startHordePosition, float spawnRadius, Transform hordeTarget)
    {
        m_IsStarted = true;
        Vector2 posV2;
        for(int i = 0;  i < m_HordeMembers.Count; i++)
        {
            //Update member position
            posV2 = UnityEngine.Random.insideUnitCircle * spawnRadius;
            m_HordeMembers[i].transform.position = startHordePosition + new Vector3(posV2.x, 0f, posV2.y);
            //Connect death event
            m_HordeMembers[i].OnDeath += ReduceAliveEnemy;
            //Enable member
            m_HordeMembers[i].gameObject.SetActive(true);
            //Set the target
            m_HordeMembers[i].SetTarget(hordeTarget);
        }
        m_AliveEnemy = m_HordeMembers.Count;
    }

    /// <summary>
    /// Cycles throught all enemies in the horde and tick them
    /// </summary>
    public void TickHorde()
    {
        for(int i = 0; i < m_HordeMembers.Count; i++)
        {
            //if is not dead -> tick
            if (m_HordeMembers[i].gameObject.activeInHierarchy)
                m_HordeMembers[i].Tick();
        }
    }

    /// <summary>
    /// Once enemy is killed reduce the horde enemy left counter
    /// </summary>
    private void ReduceAliveEnemy()
    {
        m_AliveEnemy--;
        if (m_AliveEnemy <= 0)
        {
            OnHordeDefeated?.Invoke();
            m_IsStarted = false;
        }
    }

    /// <summary>
    /// Return if horde is alive, if false the horde can be defeated or not started
    /// </summary>
    /// <returns></returns>
    public bool IsHordeStarted()
    {
        return m_IsStarted;
    }
}
