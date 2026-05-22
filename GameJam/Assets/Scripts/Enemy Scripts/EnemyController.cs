using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Enemy enemyPrefab;
    public PlayerMovement playerScript;
    public LayerMask enemyMask;

    public float x;
    public float y;

    public int waveNum;
    public int enemyNum;

    public float movespeed;
    public float distanceToPlayer = Mathf.Infinity;
    public Vector3 nearestEnemy;
    public Enemy closestEnemy = null;

    private List<Enemy> enemies = new List<Enemy>();
    private int numEnemies;

    void Start()
    {
        x = Random.Range(-5f, 5f);
        y = Random.Range(-5f, 5f);
        enemyNum = Random.Range(3, 6);
        StartCoroutine(SpawnCount());
    }

    void Update()
    {
        // Remove any destroyed enemies before iterating
        enemies.RemoveAll(e => e == null);

        // Reset each frame so the closest is recalculated fresh
        distanceToPlayer = Mathf.Infinity;
        closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(playerScript.playerPos, enemy.transform.position);
            if (distance < distanceToPlayer)
            {
                distanceToPlayer = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            nearestEnemy = closestEnemy.transform.position;
        }

        numEnemies = enemies.Count;
    }

    public void SpawnEnemies(int waveNumber, int enemyNumber)
    {
        // Randomise spawn position each wave
        x = Random.Range(-5f, 5f);
        y = Random.Range(-5f, 5f);

        for (int i = 0; i < waveNumber + 1; i++)
        {
            for (int j = 0; j < enemyNumber + 1; j++)
            {
                Enemy enemy = Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
                enemies.Add(enemy);
            }
        }
    }

    private IEnumerator SpawnCount()
    {
        yield return new WaitForSeconds(60);
        SpawnEnemies(waveNum, enemyNum);
    }
}