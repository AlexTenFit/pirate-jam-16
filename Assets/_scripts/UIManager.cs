using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton pattern for global access

    [SerializeField] private TextMeshProUGUI waveCounterText;

    private void Awake()
    {
        // Ensure only one instance of UIManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void UpdateWaveCounter(int waveNumber)
    {
        waveCounterText.text = $"Wave: {waveNumber}";
    }
}
