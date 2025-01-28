using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float moveSpeed = 1f;

    private SpawnManager _spawnManager; // Declaring spawnManager field here


    // Start is called before the first frame update
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("chosen")
            ?.transform; // Stores transform only if gameobject is found (? indicates if not null)
        _spawnManager = FindObjectOfType<SpawnManager>(); // Call SpawnManager in the scene
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_target) return; // Skip this logic if target is missing

        // Move towards target at a consistent speed
        Vector3 position = Vector3.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("chosen"))
        {
            Destroy(gameObject);

            HealthSystem.Instance.TakeDamage(1);

            if (HealthSystem.Instance.CurrentHealth <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_spawnManager != null) // In case the enemy is destroyed without being caught in OnParticleTriggerEnter
        {
            _spawnManager.OnEnemyDeath(); // Ensure that the death is counted
        }
    }
}