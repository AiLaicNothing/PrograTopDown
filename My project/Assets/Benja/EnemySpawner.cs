using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IObserver
{
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnDelay = 0.5f;

    private int enemiesAlive;

    private void Start()
    {
        GameManager.Instance.Attach(this);

        StartWave();
    }

    public void Execute(ISubject subject)
    {
        StartWave();
    }

    private void StartWave()
    {
        int enemiesToSpawn = Fibonacci(GameManager.Instance.progression);

        enemiesAlive = enemiesToSpawn;

        StartCoroutine(SpawnWave(enemiesToSpawn));
    }

    private IEnumerator SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();

        enemyScript.SetSpawner(this);
    }

    public void EnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            GameManager.Instance.IncreaseDifficulty();
        }
    }

    private int Fibonacci(int n)
    {
        if (n <= 1)
            return 1;

        int a = 1;
        int b = 1;

        for (int i = 2; i <= n; i++)
        {
            int temp = a + b;

            a = b;
            b = temp;
        }

        return b;
    }
}