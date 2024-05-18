using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Totem : MonoBehaviour, IPlaceableSpot
{
    private List<TotemSegment> Segments;

    public float ZOffset { get; set; }
    public float PlaceableRadious { get; set; }

    [SerializeField] private float m_ZOffset;
    [SerializeField] private float m_PlaceableRadious;

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
        //Activate Segment
        Segments[Segments.Count - 1].Activate();
    }

    /// <summary>
    /// Deactivate, Remove and return last totem segment
    /// </summary>
    /// <returns></returns>
    public TotemSegment GetSegment()
    {
        TotemSegment segment = Segments.Last();
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
