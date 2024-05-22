using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Settings", menuName = "Settings/Enemy")]
public class Enemy_Data : ScriptableObject
{
    [SerializeField] public float Speed = 1f;
    [SerializeField] public float HP = 1f;
    [SerializeField] public float Damage = 1f;
}
