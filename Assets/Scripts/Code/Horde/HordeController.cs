using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    [Header("Hordes Settings")]
    [SerializeField] private Enemy m_EnemyToSpawn;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private int m_StartingHordeSize = 5;
    [SerializeField] private int m_HordeIncrement = 2;
    [SerializeField] private int m_StartingPoolSize = 20;
    [SerializeField] private Transform m_HordeSpawnTransform;
    [SerializeField] private float m_SpawnHordeRadius = 5f;
    [SerializeField] private float m_DelayBetweenHordes = 5f;
    
    private List<Enemy> m_EnemyList = new List<Enemy> ();
    private Horde m_Horde;

    private float m_HordeSize = 0f;

    private void Awake()
    {
        m_Horde = new Horde();
        m_Horde.OnHordeDefeated += StartNewHorde;
        m_HordeSize = m_StartingHordeSize;
    }

    private void Start()
    {
        //Instanciate all enemies
        for(int i = 0; i < m_StartingPoolSize; i++)
        {
            m_EnemyList.Add(Instantiate<Enemy>(m_EnemyToSpawn, m_HordeSpawnTransform.position, m_HordeSpawnTransform.rotation));
            m_EnemyList.Last().gameObject.SetActive(false);
        }
        //Set up first Horde
        for (int i = 0; i < m_StartingHordeSize; i++)
            m_Horde.AddEnemy(GetFreeEnemy());

        //Start first horde
        StartCoroutine(NewHordeCoroutine());
    }

    private void StartNewHorde()
    {
        m_HordeSize += m_HordeIncrement;
        m_Horde = new Horde();
        //Increment Horde Size
        for (int i = 0; i < m_HordeSize; i++)
            m_Horde.AddEnemy(GetFreeEnemy());
        //Start coroutine
        StartCoroutine(NewHordeCoroutine());
    }

    /// <summary>
    /// Spawn new Horde x seconds after horde defeat
    /// </summary>
    /// <returns></returns>
    private IEnumerator NewHordeCoroutine()
    {
        //Wait untill time enlaps
        yield return new WaitForSeconds(m_DelayBetweenHordes);
        //Start new Horde
        m_Horde.StartHorde(m_HordeSpawnTransform.position, m_SpawnHordeRadius, m_TargetTransform);
    }

    /// <summary>
    /// Return free enemy found in list, if no free enemy is found a new enemy is added
    /// </summary>
    /// <returns></returns>
    private Enemy GetFreeEnemy()
    {
        //TODO: Fix an error when reloading scene all enemies are disabled and destroyed    

        //Find free enemy
        for(int i = 0; i < m_EnemyList.Count; i++)
            if (m_EnemyList[i].gameObject.activeInHierarchy)
                return m_EnemyList[i];

        m_EnemyList.Add(Instantiate<Enemy>(m_EnemyToSpawn, m_HordeSpawnTransform.position, m_HordeSpawnTransform.rotation));
        m_EnemyList.Last().gameObject.SetActive(false);
        return m_EnemyList.Last();
    }

    private void FixedUpdate()
    {
        if(m_Horde.IsHordeAlive())
            m_Horde.TickHorde();
    }

    private void OnDisable()
    {
        m_Horde.OnHordeDefeated -= StartNewHorde;
    }

#if UNITY_EDITOR
    //Debug
    private void OnDrawGizmos()
    {
        if (m_HordeSpawnTransform == null)
            return;

        Handles.color = Color.red;
        Handles.DrawWireDisc(m_HordeSpawnTransform.position, Vector3.up, m_SpawnHordeRadius);
    }
#endif
}
