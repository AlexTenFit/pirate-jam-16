using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // The prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    [SerializeField] private int baseEnemiesPerWave = 1; // Total number of enemies to spawn
    [SerializeField] private float spawnDelay = 1f; // Time between each spawn
    [SerializeField] private float enemyMultiplierPerWave = 1.2f; // Enemy multiplier per wave
    private int currentEnemyCount = 0; // Current number of spawned enemies
    private int waveNumber = 1; // Initial wave that spawns one enemy

    void OnDrawGizmos() // Draws a cube to show where the spawn points are
    {
        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawCube(spawnPoint.position, new Vector3(0.5f, 0.5f, 0.5f)); // Draws a small cube at the spawn point location
            }
        }
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnEnemies(baseEnemiesPerWave, spawnDelay)); // Spawn enemies for the current wave
            while (currentEnemyCount > 0)   // Wait for all enemies to be destroyed before continuing to the next wave
            {
                yield return null; // Wait for the next frame, check if the count of enemies is still > 0
            }
            waveNumber++; // Increment wave number and adjust enemy count for the next wave
        }
    }
    IEnumerator SpawnEnemies(int count, float delay)
    {
        float totalEnemiesThisWave = count * Mathf.Pow(enemyMultiplierPerWave, waveNumber - 1); // Multiply number of enemies per wave by 1.2x
        int totalEnemiesToSpawn = Mathf.CeilToInt(totalEnemiesThisWave); // Rounds the totalEnemiesThisWave float and returns the smalles int equal to float
        for (int i = 0; i < totalEnemiesThisWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Select a random spawn point

            if (spawnPoint != null)
            {
                // Spawn the enemy at the chosen spawn point
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                currentEnemyCount++; // Increment the count of spawned enemies
            }
            else
            {
                Debug.LogWarning("No valid spawn point...Skipping spawn.");
            }
            yield return new WaitForSeconds(delay);
        }
    }
    public void OnEnemyDeath()
    {
        currentEnemyCount--; // Decreases the enemy count at the current wave to reflect the number of remaining enemies 
    }
}
