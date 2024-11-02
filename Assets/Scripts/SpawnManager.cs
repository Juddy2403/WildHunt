using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonBase<SpawnManager>
{
    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!_spawnPoints.Contains(spawnPoint))
            _spawnPoints.Add(spawnPoint);
    }

    public void UnRegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoints.Remove(spawnPoint);
    }

    void Update()
    {
        _spawnPoints.RemoveAll(s => !s);
    }

    public void SpawnWave()
    {
        foreach (SpawnPoint point in _spawnPoints)
        {
            point.Spawn();
        }
    }
}