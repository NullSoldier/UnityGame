using System;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject EnemyPrefab = null;
    public int Health = 10;
    public float SpawnInterval = 4f;
    public bool IsDead = false;

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;

        if (Health <= 0)
            die();
    }

    private void Start ()
    {
        if(EnemyPrefab == null)
            throw new Exception("EnemyGenerator must have EnemyPrefab set");

        InvokeRepeating("spawnEnemy", SpawnInterval, SpawnInterval);
	}

    private void die()
    {
        IsDead = true;
        CancelInvoke("spawnEnemy");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void spawnEnemy()
    {
        GameObject enemy = Instantiate(EnemyPrefab);
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
    }
}
