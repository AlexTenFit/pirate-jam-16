using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // The prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    [SerializeField] private int baseEnemiesPerWave = 1; // Total number of enemies to spawn
    [SerializeField] private float spawnDelay = 1f; // Time between each spawn
    [SerializeField] private float enemyMultiplierPerWave = 1.2f; // Enemy multiplier per wave
    [SerializeField] private float spawnVariety = .5f; // Variety for spawn location around spawner
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
            UIManager.Instance.UpdateWaveCounter(waveNumber); // Call the UIManager to handle wave count visualization

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
        float totalEnemiesThisWave = count * Mathf.Pow(enemyMultiplierPerWave, waveNumber - 1); // Multiply number of enemies per wave based on the multiplier set
        int totalEnemiesToSpawn = Mathf.CeilToInt(totalEnemiesThisWave); // Rounds the totalEnemiesThisWave float and returns the smalles int equal to float
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            Vector3 variety = new Vector3(Random.Range(-spawnVariety, spawnVariety), 0,
                Random.Range(-spawnVariety, spawnVariety)); // Create a random vector 3 in the variety range to add to the final spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Select a random spawn point
            

            if (spawnPoint != null)
            {
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position + variety, spawnPoint.rotation); // Spawn the enemy at the chosen spawn point
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
