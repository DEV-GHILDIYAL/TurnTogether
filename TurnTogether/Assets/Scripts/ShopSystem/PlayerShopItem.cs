// 3. PlayerShopItem.cs - Individual player shop item UI
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShopItem : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button actionButton;
    public TextMeshProUGUI buttonText;
    public GameObject lockIcon;
    
    private PlayerSkinData skinData;
    private PlayerShopManager shopManager;
    
    public void Initialize(PlayerSkinData data, PlayerShopManager manager)
    {
        skinData = data;
        shopManager = manager;
        
        // UI setup
        iconImage.sprite = skinData.iconSprite;
        nameText.text = skinData.skinName;
        
        // Button click event
        actionButton.onClick.AddListener(OnButtonClick);
        
        // Default unlock check
        if (skinData.isUnlockedByDefault)
        {
            PlayerPrefs.SetString("UnlockedPlayerSkin_" + skinData.skinName, "true");
        }
        
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        bool isUnlocked = shopManager.IsSkinUnlocked(skinData.skinName);
        bool isSelected = shopManager.IsSkinSelected(skinData.skinName);
        
        if (isUnlocked)
        {
            lockIcon.SetActive(false);
            priceText.gameObject.SetActive(false);
            
            if (isSelected)
            {
                buttonText.text = "SELECTED";
                actionButton.interactable = false;
                actionButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                buttonText.text = "SELECT";
                actionButton.interactable = true;
                actionButton.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            lockIcon.SetActive(true);
            priceText.gameObject.SetActive(true);
            priceText.text = skinData.price + "";
            
            buttonText.text = "PURCHASE";
            actionButton.interactable = PlayerPrefs.GetInt("Stars", 0) >= skinData.price;
            actionButton.GetComponent<Image>().color = actionButton.interactable ? Color.yellow : Color.gray;
        }
    }
    
    private void OnButtonClick()
    {
        bool isUnlocked = shopManager.IsSkinUnlocked(skinData.skinName);
        
        if (isUnlocked)
        {
            // Select skin
            shopManager.SelectSkin(skinData.skinName);
        }
        else
        {
            // Purchase skin
            shopManager.PurchaseSkin(skinData);
        }
    }
}