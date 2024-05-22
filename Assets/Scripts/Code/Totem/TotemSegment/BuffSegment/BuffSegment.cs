using UnityEngine;

public class BuffSegment : TotemSegment
{
    [SerializeField] private Buff m_Buff;
    public Buff GetBuff() { return m_Buff; }
}
