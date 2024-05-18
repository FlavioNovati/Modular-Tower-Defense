using System;
using UnityEditor;
using UnityEngine;

public class TotemSegment : MonoBehaviour, IPlaceableSpot
{
    public Action OnPickUp;

    public float ZOffset { get; set; }
    public float PlaceableRadious { get; set; }

    [SerializeField] private float m_ZOffset;
    [SerializeField] private float m_PlaceableRadious;

    private void Awake()
    {
        ZOffset = m_ZOffset;
        PlaceableRadious = m_PlaceableRadious;
    }

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }

    public void Tick()
    {

    }

    public void PickUp()
    {
        OnPickUp?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position + Vector3.up * m_ZOffset, Vector3.up, m_PlaceableRadious);
    }
#endif
}