using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Wave/Wave Data")]
public class WaveData : ScriptableObject
{
    public int totalEnemies;             // Total number of enemies to spawn
    public float spawnDelay;             // Delay between each enemy spawn
}
