using UnityEngine;

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
            HealthSystem.Instance.TakeDamage(1);

            if (HealthSystem.Instance.CurrentHealth <= 0)
            {
                Destroy(other.gameObject);
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