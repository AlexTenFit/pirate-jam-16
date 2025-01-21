using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3; // Maximum number of hearts
    private int currentHealth; // Current health of the player
    public int CurrentHealth => currentHealth;

    public static HealthSystem Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        // Ensure one instance exists and
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth); // Update UI at the start
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over!"); // Handle player death, in this case this will reload the level
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}