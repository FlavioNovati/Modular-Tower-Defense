using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSegment : TotemSegment, IBuffable
{
    [SerializeField] private Transform m_MuzzleTransform;
    [SerializeField] private TurretSegment_Data m_Settings;
}
