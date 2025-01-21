using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton pattern for global access
    
    [SerializeField] private int heartSize = 75; // Set standard size of hearts for UI

    [SerializeField] private TextMeshProUGUI waveCounterText;
    [SerializeField] private Transform healthContainer;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    
    private GameObject[] heartObjects;
    private Image[] heartFills;

    private void Awake()
    {
        // Ensure only one instance of UIManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void UpdateWaveCounter(int waveNumber)
    {
        waveCounterText.text = $"Wave: {waveNumber}";
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Initialize arrays if they haven't been created yet
        if (heartObjects == null || heartObjects.Length != maxHealth)
        {
            InitializeHearts(maxHealth);
        }

        // Update the fill amount of each heart
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < currentHealth)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }
    }

    private void InitializeHearts(int maxHealth)
    {
        // Clear existing hearts
        foreach (Transform child in healthContainer)
        {
            Destroy(child.gameObject);
        }
        
        healthContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heartSize); // Set heart UI height
        healthContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartSize * maxHealth * 1.2f); // Set heart UI Width 

        // Initialize arrays
        heartObjects = new GameObject[maxHealth];
        heartFills = new Image[maxHealth];

        // Create heart objects
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(healthPrefab, healthContainer);
            heartObjects[i] = heart;

            // Get the fill image
            heartFills[i] = heart.transform.Find("HeartFill").GetComponent<Image>();

            if (heartFills[i] == null)
            {
                Debug.LogError($"Heart {i} is missing HeartFill image component");
            }
        }
    }
}
