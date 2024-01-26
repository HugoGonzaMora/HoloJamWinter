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
    
    public List<EnemyWaveType> enemyTypes;
    
    public GameObject[] spawnPoints;

    private float _timeBetweenWaves = 5f;

    private int _minIncreaseWeightCnt = 4;
    private int _maxIncreaseWeightCnt = 7;
    private int _minAdditionalWeight = 0;
    private int _maxAdditionalWeight = 0;
    private int weightForWave = 0;

    private int _waveIndex;

    private bool isWaveNow;

    public static int weightOnTheScene;

    private List<GameObject> enemyPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isWaveNow = false;
        _waveIndex = 0;
        weightOnTheScene = 0;
        
        InitializeEnemyPool();
    }

    private void Update()
    {
        _timeBetweenWaves -= Time.deltaTime;
        if (!isWaveNow)
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
                isWaveNow = false;
                _timeBetweenWaves = 10f;
            }
        }
    }

    private void InitializeEnemyPool()
    {
        foreach (var enemyType in enemyTypes)
        {
                GameObject enemy = Instantiate(enemyType.prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
                enemy.SetActive(false);
                enemyPool.Add(enemy);
        }
    }

    private void StartWave()
    {
        isWaveNow = true;
        _waveIndex++;
        
        if (_waveIndex % 3 == 0)
        {
            _minAdditionalWeight += _waveIndex / 3;
            _maxAdditionalWeight = _minAdditionalWeight + _waveIndex / 3;
        }
        
        weightForWave = Random.Range(_minIncreaseWeightCnt, _maxIncreaseWeightCnt + 1);
        weightOnTheScene = weightForWave;

        _minIncreaseWeightCnt = weightForWave + _minAdditionalWeight;
        _maxIncreaseWeightCnt = _minIncreaseWeightCnt + _maxAdditionalWeight;
        
        Debug.Log($"Weight in wave: {weightForWave}");

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (weightForWave > 0)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            GameObject enemy = ChooseRandomEnemy();
            Debug.Log(enemy);
            GameObject enemyInPool = enemyPool.Find(e => e != null && e.name.StartsWith(enemy.name));

            if (enemyPool.Contains(enemyInPool))
            {
                SpawnFromPool(enemyInPool);
            }
            else
            {
                GameObject enemyForPool = Instantiate(enemy.gameObject, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
                enemyForPool.SetActive(false);
                enemyPool.Add(enemyForPool);
                SpawnFromPool(enemyForPool);
            }
        }
    }

    private void SpawnFromPool(GameObject enemy)
    {
        enemyPool.Remove(enemy);
            
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

        enemy.transform.position = spawnPoint;
            
        enemy.SetActive(true);

        enemy.GetComponent<EnemyController>().OnEnemyDeath += HandleEnemyDeath;
    }

    private GameObject ChooseRandomEnemy()
    {
        int randomValue = Random.Range(0, 101);

        foreach (var enemyType in enemyTypes)
        {
            if (randomValue <= enemyType.chanceOfSpawn)
            {
                if (weightForWave - enemyType.weight >= 0)
                {
                    weightForWave -= enemyType.weight;
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

    public void AddEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        
        enemyPool.Add(enemy);
        enemy.GetComponent<EnemyController>().OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyController enemy)
    {
        weightOnTheScene -= enemy.weight;
        Debug.Log($"Enemies weightOnTheScene: {weightOnTheScene}");
    }
}
