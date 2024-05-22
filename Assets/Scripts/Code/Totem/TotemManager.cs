using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public static TotemManager Instance;

    private List<Totem> m_TotemList;

    private void Awake()
    {
        //Set up Instance
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        //Setup Totem List
        m_TotemList = new List<Totem>();
        m_TotemList = FindObjectsOfType<Totem>().ToList();
    }

    private void FixedUpdate()
    {
        //Tick All Totems
        for(int i = 0; i < m_TotemList.Count; i++)
            m_TotemList[i].Tick();
    }

    /// <summary>
    /// Return closest reachable Totem, if null there are no Totem nearby
    /// </summary>
    /// <param name="position">Position where to check the distance from totem</param>
    /// <returns></returns>
    public Totem GetReachableToken(Vector3 position)
    {
        for (int i = 0; i < m_TotemList.Count; i++)
            if (Vector3.Distance(position, m_TotemList[i].transform.position) <= m_TotemList[i].PlaceableRadius)
                return m_TotemList[i];
        return null;
    }
}
