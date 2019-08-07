using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnTime = 5f;        // Количество времени между каждым спауном
    public float spawnDelay = 3f;       // Задержка перед началом спауна
    public GameObject[] enemies;        // Массив противников


    void Start()
    {
        // Вызываем вункцию спаун несколько раз после задержки
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }


    void Spawn()
    {
        // Создаём случайного врага
        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
    }
}