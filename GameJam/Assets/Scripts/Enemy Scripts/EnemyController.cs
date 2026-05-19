using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    public Enemy enemyPrefab;
    public PlayerMovement playerScript;
    public LayerMask enemyMask;

    public float x = Random.Range(-5f, 5f);
    public float y = Random.Range(-5f, 5f);

    public int waveNum;
    public int enemyNum = Random.Range(3, 6);

    public float movespeed;
    public float distanceToPlayer = Mathf.Infinity;
    public Vector3 nearestEnemy;
    public Enemy closestEnemy = null;
    
    private List<(Enemy enemy, float x, float y)> enemies = new List<(Enemy, float, float)>();
    private int numEnemies;

    void Start()
    {
        StartCoroutine(SpawnCount());
    }

    void Update()
    {
        foreach (var e in enemies)
        {
            float distance = Vector3.Distance(playerScript.playerPos, e.enemy.transform.position);
            if (distance < distanceToPlayer)
            {
                distanceToPlayer = distance;
                closestEnemy = e.enemy;
            }
        }

        nearestEnemy = closestEnemy.transform.position;
        numEnemies = enemies.Count;
    }

    public void SpawnEnemies(int waveNumber, int enemyNumber)
    {
        for (int i = 0; i < waveNumber + 1; i++)
        {
            for (int j = 0; j < enemyNumber + 1; j++)
            {
                Enemy enemy = Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
                enemies.Add((enemy, x, y));
            }
        }
    }

    private IEnumerator SpawnCount()
    {
        yield return new WaitForSeconds(60);
        SpawnEnemies(waveNum, enemyNum);
    }
}
