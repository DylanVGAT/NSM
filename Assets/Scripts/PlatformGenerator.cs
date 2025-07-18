using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 xSpawnRange = new Vector2(-0.7f, 0.7f);
    [SerializeField] private float spawnRate = 0.5f;
    [SerializeField] private float spawnY = 1f; // Position Y d’apparition au-dessus de l’écran

    [Header("Platform Chances")]
    [Range(0, 100)] public int chanceGreen = 50;
    [Range(0, 100)] public int chanceYellow = 20;
    [Range(0, 100)] public int chanceBlue = 20;
    [Range(0, 100)] public int chanceRed = 10;

    [Header("Platform Movement")]
    [SerializeField] public float baseSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float speedIncreaseRate = 0.05f; // vitesse augmente de 0.05f...
    [SerializeField] private float increaseInterval = 5f;     // ... toutes les 5 secondes

    public float platformSpeed;
    private float speedTimer;


    private float timer;

    private void Start()
    {
        platformSpeed = baseSpeed;
    }

    private void Update()
    {
        platformSpeed = Mathf.Min(platformSpeed, maxSpeed); // just in case
        timer += Time.deltaTime;
        speedTimer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnPlatform();
        }

        if (speedTimer >= increaseInterval)
        {
            speedTimer = 0f;
            platformSpeed += speedIncreaseRate;
        }
    }

    private void SpawnPlatform()
    {
        int platformsThisWave = Random.Range(1, 3); // 1 ou 2 plateformes

        List<float> usedX = new List<float>();

        for (int i = 0; i < platformsThisWave; i++)
        {
            float xPos;
            int safety = 0;
            do
            {
                xPos = Random.Range(xSpawnRange.x, xSpawnRange.y);
                safety++;
            } while (usedX.Exists(x => Mathf.Abs(x - xPos) < 0.8f) && safety < 10);

            usedX.Add(xPos);

            int index = GetRandomPlatformIndex();

            Vector3 spawnPos = new Vector3(xPos, spawnY, 0f);
            GameObject platform = Instantiate(platformPrefabs[index], spawnPos, Quaternion.identity);

            Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.down * platformSpeed;
            }
        }
    }


    private int GetRandomPlatformIndex()
    {
        int roll = Random.Range(0, 100);
        if (roll < chanceGreen) return 0;
        if (roll < chanceGreen + chanceYellow) return 1;
        if (roll < chanceGreen + chanceYellow + chanceBlue) return 2;
        return 3;
    }
}
