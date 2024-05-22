using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Totem : MonoBehaviour, IPlaceableSpot
{
    public float ZOffset { get; set; }
    [SerializeField] private float m_ZOffset;
    public float PlaceableRadious { get; set; }
    [SerializeField] private float m_PlaceableRadious;
    
    private List<TotemSegment> Segments = new List<TotemSegment>();
    private List<Buff> Buffs = new List<Buff>();

    private float m_Height = 0f;

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
        for (int i = 0; i < Segments.Count; i++)
            Segments[i].Tick();
    }

    /// <summary>
    /// Add a segment to totem
    /// - Add
    /// - Move
    /// - Enable
    /// - Try get Buff
    /// - Apply Buff
    /// </summary>
    /// <param name="newSegment">Segment to add to the totem</param>
    public void AddSegment(TotemSegment newSegment)
    {
        //Add Segment to segment list
        Segments.Add(newSegment);

        //Move Segment
        Segments.Last().transform.position = new Vector3(transform.position.x, m_Height, transform.position.z);
        m_Height += Segments.Last().ZOffset;

        //Activate Segment
        Segments.Last().Activate();

        //Try Get buff
        Buff buff = TryGetBuff(Segments.Last());
        if (buff != null) //new buff -> apply to all turret
        {
            Buffs.Add(buff);
            for(int turretIndex = 0; turretIndex < Segments.Count; turretIndex++)
                TryApplyBuff(buff, Segments[turretIndex]);
        }
        else //not a buff, is a turret -> apply old buffs to new turret
        {
            for (int buffIndex = 0; buffIndex < Buffs.Count; buffIndex++)
                TryApplyBuff(Buffs[buffIndex], Segments.Last());
        }
    }

    /// <summary>
    /// Try to apply a buff to a totem segment
    /// </summary>
    /// <param name="buff">Buff to apply</param>
    /// <param name="segment">Segment to apply buff</param>
    private void TryApplyBuff(Buff buff, TotemSegment segment)
    {
        segment.TryGetComponent<IBuffable>(out IBuffable buffable);
        if(buffable != null)
            buffable.AddBuff(buff);
    }

    /// <summary>
    ///Remove all buffs from a Segment
    /// </summary>
    /// <param name="segment"></param>
    private void ClearBuffs(TotemSegment segment)
    {
        segment.TryGetComponent<IBuffable>(out IBuffable buffable);
        if (buffable != null)
            for (int i = 0; i < Buffs.Count; i++)
                buffable.RemoveBuff(Buffs[i]);
    }

    /// <summary>
    /// Try to remove a buff to a totem segment
    /// </summary>
    /// <param name="buff">Buff to apply</param>
    /// <param name="segment">Segment to apply buff</param>
    private void TryRemoveBuff(Buff buff, TotemSegment segment)
    {
        segment.TryGetComponent<IBuffable>(out IBuffable buffable);
        if (buffable != null)
            buffable.RemoveBuff(buff);
    }
    
    /// <summary>
    /// Returns a segment buff, if is null there is no buff
    /// </summary>
    /// <param name="segment">Segment to get the buff from</param>
    /// <returns></returns>
    private Buff TryGetBuff(TotemSegment segment)
    {
        segment.TryGetComponent<BuffSegment>(out BuffSegment buff);
        if(buff != null)
            return buff.GetBuff();
        else
            return null;
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
        
        //Remove effect if the last one is a buff segment
        Buff buff = TryGetBuff(Segments.Last());
        if(buff != null)
        {
            for(int i =  0; i < Segments.Count; i++)
                TryRemoveBuff(buff, Segments[i]);
            //Remove last buff from Buffs list
            Buffs.RemoveLast();
        }
        else
        ClearBuffs(Segments.Last());

        //Deactivate segment
        Segments.Last().Deactivate();
        //Remove last segment
        TotemSegment segment = Segments.Last();
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
