using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _firstWaveStart = 5.0f;
    [SerializeField] private float _waveFrequencyIncrement = 3.0f;
    [SerializeField] private int _minCreatureSpawnCount = 10;
    [SerializeField] private int _maxCreatureSpawnCount = 20;
    [SerializeField] private GameObject _monsterTemplate = null;
    [SerializeField] private GameObject _creatureTemplate = null;

    private float _currentFrequency = 0.0f;
    void Awake()
    {
        _currentFrequency = GameMaster.Instance.MonsterManager.WaveStartFrequency;
        StartNewCreatureWave();
        Invoke(nameof(StartNewMonsterWave), _firstWaveStart);
    }
    void StartNewMonsterWave()
    {
        MonsterManager monsterManager = GameMaster.Instance.MonsterManager;
        SpawnWave(_monsterTemplate, monsterManager.MonsterMinSpawnCount, monsterManager.MonsterMaxSpawnCount);

        _currentFrequency = Mathf.Clamp(_currentFrequency - _waveFrequencyIncrement,
            monsterManager.WaveEndFrequency, monsterManager.WaveStartFrequency);

        Invoke(nameof(StartNewMonsterWave), _currentFrequency);
    }
    void StartNewCreatureWave()
    {
        SpawnWave(_creatureTemplate, _minCreatureSpawnCount, _maxCreatureSpawnCount);
        Invoke(nameof(StartNewCreatureWave), 60);
    }
    
    private void SpawnWave(GameObject spawnTemplate,int minSpawnCount, int maxSpawnCount)
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