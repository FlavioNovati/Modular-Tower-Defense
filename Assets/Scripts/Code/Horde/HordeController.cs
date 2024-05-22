using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    [SerializeField] private Enemy EnemyToSpawn;
    [SerializeField] private int StartingHordeSize = 5;
    [SerializeField] private int HordeIncrement = 2;
    [SerializeField] private int StartingPoolSize = 20;
}
