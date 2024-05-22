using System;
using UnityEditor;
using UnityEngine;

public abstract class TotemSegment : MonoBehaviour, IPlaceableSpot
{
    public float ZOffset { get; set; }
    public float PlaceableRadious { get; set; }

    [Header("Totem Segment Settings")]
    [SerializeField] private float m_ZOffset;
    [SerializeField] private float m_PlaceableRadious;

    private void Awake()
    {
        ZOffset = m_ZOffset;
        PlaceableRadious = m_PlaceableRadious;
    }

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }

    public virtual void Tick()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position + Vector3.up * m_ZOffset, Vector3.up, m_PlaceableRadious);
    }
#endif
}