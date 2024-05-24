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
    
    private List<Enemy> m_EnemyList = new List<Enemy>();
    private Horde m_Horde;

    private int m_HordeSize = 0;

    private void Awake()
    {
        m_HordeSize = m_StartingHordeSize;
        m_Horde = new Horde();
    }

    private void Start()
    {
        //Instanciate all enemies
        for(int i = 0; i < m_StartingPoolSize; i++)
        {
            m_EnemyList.Add(Instantiate<Enemy>(m_EnemyToSpawn, m_HordeSpawnTransform.position, m_HordeSpawnTransform.rotation));
            m_EnemyList[i].gameObject.SetActive(false);
        }

        //Start first horde
        StartCoroutine(NewHordeCoroutine());
    }

    //TODO: Fix an error when reloading scene all enemies are disabled and destroyed    
    private void StartNewHorde()
    {
        //Increment Horde Size
        m_HordeSize += m_HordeIncrement;
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
        //Create new Horde
        m_Horde = new Horde();
        //Add Enemies
        m_Horde.AddEnemy(GetFreeEnemies(m_HordeSize));
        //Connect Horde Defeated event to start a new one
        m_Horde.OnHordeDefeated += StartNewHorde;
        //Start new Horde
        m_Horde.StartHorde(m_HordeSpawnTransform.position, m_SpawnHordeRadius, m_TargetTransform);
    }

    private List<Enemy> GetFreeEnemies(int amount)
    {
        List<Enemy> freeEnemies = new List<Enemy>();

        //Find "amount" of enemies -> cycle throught the entirety of enemy list
        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            if (!m_EnemyList[i].gameObject.activeInHierarchy)
            {
                freeEnemies.Add(m_EnemyList[i]);
                //Set to true to avoid putting the same enmy multiple time inside the horde
                m_EnemyList[i].gameObject.SetActive(true);
                //stop for cycle
                if(freeEnemies.Count == amount)
                    return freeEnemies;
            }
        }

        //Enemies are missing -> Add new enemies to pool
        int missingEnemyAmount = amount - freeEnemies.Count;
        for(int i = 0; i < missingEnemyAmount; i++)
        {
            m_EnemyList.Add(Instantiate<Enemy>(m_EnemyToSpawn, m_HordeSpawnTransform.position, m_HordeSpawnTransform.rotation));
            m_EnemyList[i].gameObject.SetActive(true);
            freeEnemies.Add(m_EnemyList.Last());
        }

        return freeEnemies;
    }

    private void FixedUpdate()
    {
        if(m_Horde.IsHordeStarted())
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
