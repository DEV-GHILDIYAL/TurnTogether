using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI starText;

    void Update()
    {
        coinText.text = "Coins: " + PlayerPrefs.GetInt("Coins", 0);
        starText.text = "Stars: " + PlayerPrefs.GetInt("Stars", 0);
    }
}
