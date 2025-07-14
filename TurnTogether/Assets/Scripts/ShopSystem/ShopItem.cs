// 3. ShopItem.cs - Individual shop item UI
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button actionButton;
    public TextMeshProUGUI buttonText;
    public GameObject lockIcon;
    
    private TextureData textureData;
    private TextureShopManager shopManager;
    
    public void Initialize(TextureData data, TextureShopManager manager)
    {
        textureData = data;
        shopManager = manager;
        
        // UI setup
        iconImage.sprite = textureData.iconSprite;
        nameText.text = textureData.textureName;
        
        // Button click event
        actionButton.onClick.AddListener(OnButtonClick);
        
        // Default unlock check
        if (textureData.isUnlockedByDefault)
        {
            PlayerPrefs.SetString("UnlockedTexture_" + textureData.textureName, "true");
        }
        
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        bool isUnlocked = shopManager.IsTextureUnlocked(textureData.textureName);
        bool isSelected = shopManager.IsTextureSelected(textureData.textureName);
        
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
            priceText.text = textureData.price + " Stars";
            
            buttonText.text = "PURCHASE";
            actionButton.interactable = PlayerPrefs.GetInt("Stars", 0) >= textureData.price;
            actionButton.GetComponent<Image>().color = actionButton.interactable ? Color.yellow : Color.gray;
        }
    }
    
    private void OnButtonClick()
    {
        bool isUnlocked = shopManager.IsTextureUnlocked(textureData.textureName);
        
        if (isUnlocked)
        {
            // Select texture
            shopManager.SelectTexture(textureData.textureName);
        }
        else
        {
            // Purchase texture
            shopManager.PurchaseTexture(textureData);
        }
    }
}