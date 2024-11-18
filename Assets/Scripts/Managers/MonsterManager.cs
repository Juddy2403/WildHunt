using System;
using UnityEngine;
[Serializable]
public class MonsterManager
{
    [SerializeField] private float _waveStartFrequency = 60.0f;
    [SerializeField] private float _waveEndFrequency = 40.0f;
    [SerializeField] private int _monsterMinSpawnCount = 10;
    [SerializeField] private int _monsterMaxSpawnCount = 20;
    public float WaveStartFrequency => _waveStartFrequency;
    public float WaveEndFrequency => _waveEndFrequency;
    public int MonsterMinSpawnCount => _monsterMinSpawnCount;
    public int MonsterMaxSpawnCount => _monsterMaxSpawnCount;

    public void UpdateSpawnCount()
    {
        //gradually increase the minimum and maximum spawn count based on how low the sanity is from 0 to 100
        _monsterMinSpawnCount = Mathf.FloorToInt(Mathf.Lerp(10, 20, (100 - GameMaster.Instance.SanityManager.Sanity) / 100.0f));
        _monsterMaxSpawnCount = Mathf.FloorToInt(Mathf.Lerp(20, 40, (100 - GameMaster.Instance.SanityManager.Sanity) / 100.0f));
    }
}