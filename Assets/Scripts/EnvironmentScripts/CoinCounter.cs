using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    private int currentCoins = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple CoinCounter instances detected in the scene!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void IncreaseCoins(int value)
    {
        currentCoins += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = $"Coins: {currentCoins}";
    }
}
