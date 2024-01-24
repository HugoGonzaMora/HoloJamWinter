using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    #region Singleton

    public static WaveManager Instance;
    

    #endregion
    
    [System.Serializable]
    public class EnemyWaveType
    {
        public GameObject prefab;
        public int weight;
    }
    
    public List<EnemyWaveType> enemyTypes;
    
    public GameObject[] spawnPoints;

    private float _timeBetweenWaves = 5f;

    private int _minIncreaseEnemyCnt = 1;
    private int _maxIncreaseEnemyCnt = 3;

    private int _waveIndex;

    private bool isWaveNow;

    public static int enemiesAlive;

    private List<GameObject> enemyPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isWaveNow = false;
        _waveIndex = 0;
        enemiesAlive = 0;
        
        InitializeEnemyPool();
        Debug.Log(enemyPool.Count);
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
            if (enemiesAlive <= 0)
            {
                isWaveNow = false;
                _timeBetweenWaves = 20f;
            }
        }
    }

    private void InitializeEnemyPool()
    {
        foreach (var enemyType in enemyTypes)
        {
            for (int i = 0; i < enemyType.weight; i++)
            {
                GameObject enemy = Instantiate(enemyType.prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
                enemy.SetActive(false);
                enemyPool.Add(enemy);
            }
        }
    }

    private void StartWave()
    {
        isWaveNow = true;
        _waveIndex++;
        
        if (_waveIndex % 5 == 0)
        {
            _maxIncreaseEnemyCnt++;
            _minIncreaseEnemyCnt++;
        }
        
        int enemiesToSpawn = Random.Range(_minIncreaseEnemyCnt, _maxIncreaseEnemyCnt + 1);
        enemiesAlive = enemiesToSpawn;

        Debug.Log($"Всего противников в волне: {enemiesToSpawn}");
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            StartCoroutine(SpawnEnemy());
        }

        _minIncreaseEnemyCnt += Random.Range(1, 2);
        _maxIncreaseEnemyCnt += Random.Range(1, 2);
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));

        if (enemyPool.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyPool.Count);
            GameObject enemy = enemyPool[randomIndex];
            enemyPool.RemoveAt(randomIndex);

            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

            enemy.transform.position = spawnPoint;
            
            enemy.SetActive(true);

            enemy.GetComponent<EnemyController>().OnEnemyDeath += HandleEnemyDeath;
        }
        else
        {
            foreach (var enemyType in enemyTypes)
            {
                for (int i = 0; i < enemyType.weight; i++)
                {
                    GameObject enemy = Instantiate(enemyType.prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
                    enemy.SetActive(false);
                    enemyPool.Add(enemy);
                    StartCoroutine(SpawnEnemy());
                }
            }
        }
    }

    public void AddEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        
        enemyPool.Add(enemy);
        enemy.GetComponent<EnemyController>().OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        Debug.Log($"Кол-во противников: {enemiesAlive}");
    }
}
