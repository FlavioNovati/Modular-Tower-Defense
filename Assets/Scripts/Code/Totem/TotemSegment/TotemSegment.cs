using System;
using UnityEditor;
using UnityEngine;

public abstract class TotemSegment : MonoBehaviour, IPlaceableSpot
{
    public float ZOffset { get; set; }
    public float PlaceableRadius { get; set; }

    [Header("Totem Segment Settings")]
    [SerializeField] private float m_ZOffset;
    [SerializeField] private float m_PlaceableRadious;
    [SerializeField] private Collider m_Collider;

    private void Awake()
    {
        ZOffset = m_ZOffset;
        PlaceableRadius = m_PlaceableRadious;
        m_Collider = GetComponent<Collider>();
    }

    public virtual void Activate()
    {
        m_Collider.enabled = false;
    }

    public virtual void Deactivate()
    {
        m_Collider.enabled = true;
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