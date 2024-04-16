using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    //Singleton
    public static WaveManager Instance;
    
    [System.Serializable]
    public class EnemyWaveType
    {
        public GameObject prefab;
        public int weight;
        public int chanceOfSpawn;
    }

    [System.Serializable]
    public class SpawnPointsInWave
    {
        public GameObject spawnPoint;
        public int chanceForPoint;
    }

    public List<EnemyWaveType> enemyTypes;
    public List<SpawnPointsInWave> spawnPoints;

    private float _timeBetweenWaves = 5f;

    [HideInInspector] public int waveIndex { get; private set; } 
    private int _weightForWave = 0;
    private int _previousWaveWeight = 0;

    private int _enemiesKilledInWave = 0;
    private const float _increaseIndex = 2;

    private bool _isWaveNow;

    public static int weightOnTheScene;

    private List<GameObject> _enemyPool = new List<GameObject>();

    private GameObject _randomSpawnPoint;
    private GameObject _decreasedSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isWaveNow = false;
        waveIndex = 0;
        weightOnTheScene = 0;
        
        InitializeEnemyPool();
    }

    private void Update()
    {
        _timeBetweenWaves -= Time.deltaTime;
        if (!_isWaveNow)
        {
            if (_timeBetweenWaves <= 0)
            {
                StartWave();
            }
        }
        else
        {
            if (weightOnTheScene <= 0)
            {
                _isWaveNow = false;
                _timeBetweenWaves = 10f;
            }
        }
    }

    private void InitializeEnemyPool()
    {
        foreach (var enemyType in enemyTypes)
        {
                _randomSpawnPoint = ChooseRandomSpawnPoint();
                GameObject enemy = Instantiate(enemyType.prefab, _randomSpawnPoint.transform.position, Quaternion.identity);
                enemy.SetActive(false);
                _enemyPool.Add(enemy);
        }
    }

    private void StartWave()
    {
        _isWaveNow = true;
        waveIndex++;
        _enemiesKilledInWave = 0;

        WeightForWaveCalculations(waveIndex);
        weightOnTheScene = _weightForWave;
        
        Debug.Log($"Weight in wave: {_weightForWave}");

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (_weightForWave > 0)
        {

            yield return new WaitForSeconds(Random.Range(3f, 4f));
            
            _randomSpawnPoint = ChooseRandomSpawnPoint();
            GameObject enemy = ChooseRandomEnemy();
            //Debug.Log(enemy);
            GameObject enemyInPool = _enemyPool.Find(e => e != null && e.name.StartsWith(enemy.name));

            if (_enemyPool.Contains(enemyInPool))
            {
                SpawnFromPool(enemyInPool);
            }
            else
            {
                GameObject enemyForPool = Instantiate(enemy.gameObject, _randomSpawnPoint.transform.position, Quaternion.identity);
                enemyForPool.SetActive(false);
                _enemyPool.Add(enemyForPool);
                SpawnFromPool(enemyForPool);
            }
        }
    }

    private void SpawnFromPool(GameObject enemy)
    {
        _enemyPool.Remove(enemy);
            
        Vector3 spawnPoint = _randomSpawnPoint.transform.position;

        enemy.transform.position = spawnPoint;
            
        enemy.SetActive(true);

        enemy.GetComponent<EnemyController>().OnEnemyDeath += HandleEnemyDeath;
    }

    #region ChooseRandomEnemy

        private GameObject ChooseRandomEnemy()
        {
            int randomValue = Random.Range(0, 101);

            foreach (var enemyType in enemyTypes)
            {
                if (randomValue <= enemyType.chanceOfSpawn)
                {
                    if (_weightForWave - enemyType.weight >= 0)
                    {
                        _weightForWave -= enemyType.weight;
                        return enemyType.prefab;
                    }
                    else
                    {
                        return ChooseRandomEnemy();
                    }
                }
                else
                {
                    randomValue -= enemyType.chanceOfSpawn;
                }
            }
            
            return null;
        }

    #endregion
    
    #region SpawnPointSystem

        private GameObject ChooseRandomSpawnPoint()
        {
            int randomValue = Random.Range(0, 101);

            foreach (var spawnPoint in spawnPoints)
            {
                if (randomValue <= spawnPoint.chanceForPoint)
                {
                    spawnPoint.chanceForPoint -= 4;
                    _decreasedSpawnPoint = spawnPoint.spawnPoint;
                    IncreaseChanceForSpawnPoints();
                    return spawnPoint.spawnPoint;
                }
                else
                {
                    randomValue -= spawnPoint.chanceForPoint;
                }
            }
            
            return null;
        }

        private void IncreaseChanceForSpawnPoints()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                if (spawnPoint.spawnPoint != _decreasedSpawnPoint)
                {
                    spawnPoint.chanceForPoint += 1;
                }
            }
        }

    #endregion

    private int WeightForWaveCalculations(int waveIndex)
    {
        _weightForWave = (int)(_previousWaveWeight * 0.8f + _enemiesKilledInWave * _increaseIndex)+waveIndex;

        _previousWaveWeight = _weightForWave;
        return _weightForWave;
    }
    
    public void AddEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        
        _enemyPool.Add(enemy);
        enemy.GetComponent<EnemyController>().OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyController enemy)
    {
        weightOnTheScene -= enemy.weight;
        _enemiesKilledInWave += 1;
        GameManager.Instance.enemiesKilledCnt += 1;
        Debug.Log($"Enemies weightOnTheScene: {weightOnTheScene}");
    }
}
