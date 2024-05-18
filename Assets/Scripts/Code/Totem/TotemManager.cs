using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    private List<Totem> m_TotemList;

    private void Awake()
    {
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
    /// <param name="position">Position where to check the closest totem</param>
    /// <returns></returns>
    public Totem GetReachableToken(Vector3 position)
    {
        for (int i = 0; i < m_TotemList.Count - 1; i++)
            if ((position - m_TotemList[i].transform.position).magnitude < m_TotemList[i].PlaceableRadious)
                return m_TotemList[i];
        return null;
    }
}
