using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float destroyDelay = 1f;

    private SpawnManager spawnManager; // Declaring spawnManager field here

    // Start is called before the first frame update
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("chosen").transform;
        spawnManager = FindObjectOfType<SpawnManager>(); // Call SpawnManager in the scene
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_target) return;

        Vector3 position = Vector3.Lerp(transform.position, _target.position, moveSpeed * Time.deltaTime);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("chosen"))
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene("Game");
        }
    }

    public void TriggerOnParticleEnter(GameObject other) // Alex; I renamed it from 'OnParticleTriggerEnter' to avoid syntax issues
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (spawnManager != null) // Notify the SpawnManager that this enemy has died
            {
                spawnManager.OnEnemyDeath(); // Call OnEnemyDeath to update the enemy count
            }
        }
    }
    private void OnDestroy()
    {
        if (spawnManager != null) // In case the enemy is destroyed without being caught in OnParticleTriggerEnter
        {
            spawnManager.OnEnemyDeath(); // Ensure that the death is counted
        }
    }
}