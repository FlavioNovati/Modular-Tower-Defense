using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Totem : MonoBehaviour, IPlaceableSpot
{


    public float ZOffset { get; set; }
    [SerializeField] private float m_ZOffset;
    public float PlaceableRadious { get; set; }
    [SerializeField] private float m_PlaceableRadious;


    [SerializeField] private List<TotemSegment> Segments;
    float m_Height = 0f;

    private void Awake()
    {
        m_Height = m_ZOffset;
        PlaceableRadious = m_PlaceableRadious;
    }

    /// <summary>
    /// Calls Tick method for all segments on this Totem
    /// </summary>
    public void Tick()
    {
        //Tick all segment
        for(int i = 0; i < Segments.Count; i++)
            Segments[i].Tick();
    }

    /// <summary>
    /// Adds a Totem Segment on top
    /// </summary>
    /// <param name="NewSegment">Segment to add to the tome</param>
    public void AddSegment(TotemSegment NewSegment)
    {
        //Add Segment
        Segments.Add(NewSegment);
        //Move Segment
        Segments.Last().transform.position = new Vector3(transform.position.x, m_Height, transform.position.z);
        m_Height += Segments.Last().ZOffset;
        //Activate Segment
        Segments.Last().Activate();
    }

    /// <summary>
    /// Deactivate, Remove and return last totem segment, if null totem is empty
    /// </summary>
    /// <returns></returns>
    public TotemSegment GetLastSegment()
    {
        if(Segments.Count <= 0)
            return null;
        //Decrease height
        m_Height -= Segments.Last().ZOffset;
        //get last segment
        TotemSegment segment = Segments.Last();
        //remove it
        Segments.Last().Deactivate();
        Segments.RemoveLast();
        return segment;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position + Vector3.up * m_ZOffset, Vector3.up, m_PlaceableRadious);
    }
#endif
}
