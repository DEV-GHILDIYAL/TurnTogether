using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarExchangeUI : MonoBehaviour
{
    public TMP_InputField coinInputField;
    public TextMeshProUGUI resultText;       // ⭐ Only star count
    public TextMeshProUGUI warningText;      // ⚠️ Warnings / errors
    public Button convertButton;
    public Button plusButton;
    public Button minusButton;

    private int currentInputCoins = 0;
    private int exchangeRate = 100;

    void Start()
    {
        coinInputField.readOnly = true;

        convertButton.onClick.AddListener(ExchangeCoinsForStars);
        plusButton.onClick.AddListener(AddCoins);
        minusButton.onClick.AddListener(SubtractCoins);

        UpdateUI();
    }

    void AddCoins()
    {
        currentInputCoins += exchangeRate;
        UpdateUI();
    }

    void SubtractCoins()
    {
        if (currentInputCoins >= exchangeRate)
        {
            currentInputCoins -= exchangeRate;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        coinInputField.text = currentInputCoins.ToString();

        if (currentInputCoins >= exchangeRate && currentInputCoins % exchangeRate == 0)
        {
            int stars = currentInputCoins / exchangeRate;
            resultText.text = stars.ToString();         // ✅ Just number
            warningText.text = "";                      // ❌ Clear warning
            convertButton.interactable = true;
        }
        else
        {
            resultText.text = "";                       // No number
            warningText.text = "Enter a multiple of 100";  // ⚠️ Show warning
            convertButton.interactable = false;
        }
    }

    void ExchangeCoinsForStars()
    {
        int playerCoins = PlayerPrefs.GetInt("Coins", 0);
        int playerStars = PlayerPrefs.GetInt("Stars", 0);

        if (currentInputCoins > playerCoins)
        {
            warningText.text = "Not enough coins! You have only " + playerCoins;
            return;
        }

        int starsToAdd = currentInputCoins / exchangeRate;

        playerCoins -= currentInputCoins;
        playerStars += starsToAdd;

        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.SetInt("Stars", playerStars);
        PlayerPrefs.Save();

        resultText.text = starsToAdd.ToString();
        warningText.text = "✅ Exchange successful!";

        currentInputCoins = 0;
        UpdateUI();
    }
}
