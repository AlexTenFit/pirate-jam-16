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
        Gizmos.color = Color.red; // Sets the color to red
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
        StartCoroutine(SpawnEnemies(baseEnemiesPerWave, spawnDelay));
    }

    IEnumerator SpawnEnemies(int count, float delay)
    {
        float totalEnemiesThisWave = count * Mathf.Pow(enemyMultiplierPerWave, waveNumber - 1); // Multiply number of enemies per wave by 1.2x
        int totalEnemiesToSpawn = Mathf.CeilToInt(totalEnemiesThisWave); // Rounds the totalEnemies float and returns the smalles int equal to float
        for (int i = 0; i < totalEnemiesThisWave; i++)
        {
            // Select a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

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
}
