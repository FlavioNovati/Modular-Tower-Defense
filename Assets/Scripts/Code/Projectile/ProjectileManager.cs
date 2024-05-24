using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;
    
    [SerializeField] private int m_PoolSize = 50;
    [SerializeField] private Projectile m_StartingProjectile = null;

    private List<Projectile> m_ProjectileList = new List<Projectile>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < m_PoolSize; i++)
        {
            m_ProjectileList.Add(Instantiate(m_StartingProjectile, transform.position, Quaternion.identity));
            m_ProjectileList[i].gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < m_PoolSize; i++)
            if (m_ProjectileList[i].gameObject.activeInHierarchy)
                m_ProjectileList[i].Tick();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startingPos"></param>
    /// <param name="enemyTarget"></param>
    /// <param name="damage"></param>
    public void RequestBullet(Vector3 startingPos, Enemy enemyTarget, float damage)
    {
        for(int i = 0; i < m_PoolSize; i++)
        {
            if(!m_ProjectileList[i].gameObject.activeInHierarchy)
            {
                m_ProjectileList[i].SetUp(startingPos, enemyTarget, damage);
                m_ProjectileList[i].gameObject.SetActive(true);
                return;
            }
        }
    }
}
