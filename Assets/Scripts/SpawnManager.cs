using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _firstWaveStart = 5.0f;
    [SerializeField] private float _waveFrequencyIncrement = 3.0f;
    [SerializeField] private GameObject _monsterTemplate = null;
    [SerializeField] private GameObject _creatureTemplate = null;

    private float _currentFrequency = 0.0f;
    void Awake()
    {
        _currentFrequency = GameMaster.Instance.WaveStartFrequency;
        SpawnWave(_creatureTemplate);
        Invoke(nameof(StartNewWave), _firstWaveStart);
    }
    void StartNewWave()
    {
        SpawnWave(_monsterTemplate, GameMaster.Instance.MonsterMinSpawnCount, GameMaster.Instance.MonsterMaxSpawnCount);

        _currentFrequency = Mathf.Clamp(_currentFrequency - _waveFrequencyIncrement,
            GameMaster.Instance.WaveEndFrequency, GameMaster.Instance.WaveStartFrequency);

        Invoke(nameof(StartNewWave), _currentFrequency);
    }
    
    private void SpawnWave(GameObject spawnTemplate,int minSpawnCount = 10, int maxSpawnCount = 20)
    {
        GameObject player = GameMaster.Player;
        if (player == null)
        {
            Debug.LogError("Player not found");
            return;
        }

        Vector3 playerPosition = player.transform.position;
        float minRadius = 40.0f; // Minimum radius for random position
        float maxRadius = 250.0f; // Maximum radius for random position

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        Debug.Log("spawned" + spawnCount + " " + spawnTemplate.name);
        for (int i = 0; i < spawnCount; i++)
        {
            float randomRadius = Random.Range(minRadius, maxRadius);
            Vector3 spawnPosition = Movement.NavMeshMovementBehaviour.RandomNavmeshLocation(playerPosition, randomRadius);
            Instantiate(spawnTemplate, spawnPosition, Quaternion.identity);
        }
    }
}