using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refferences")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 1.5f;
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float difficultScalingFactor = 0.5f;
    [SerializeField] private float enemiesPerSecondCap = 500;
    [Header("Events")]
    public static UnityEvent onEnemyDestoyed = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private float eps; //inimigos por segundo

    private void Awake()
    {
        onEnemyDestoyed.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }


    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        Debug.Log("Ending Wave");
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        Debug.Log($"Starting Wave {currentWave}");
        eps = EnemiesPerSecond();
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void SpawnEnemy ()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        int enemies = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, 1.5f));
        Debug.Log($"EnemiesPerWave: {enemies}");
        return enemies;
    }

    private float EnemiesPerSecond()
    {
        float perSecond = Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, 1.5f), 0f, enemiesPerSecondCap);
        Debug.Log($"EnemiesPerSecond: {perSecond}");
        return perSecond;
    }
}
