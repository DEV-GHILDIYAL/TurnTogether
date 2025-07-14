// 2. TextureShopManager.cs - Main shop logic
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TextureShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    public TextureData[] availableTextures;
    public GameObject shopItemPrefab;
    public Transform shopContent;
    public Material playerMaterial; // Player material reference

    public GameObject shopPanel;
    public Button resetButton;
    
    private List<ShopItem> shopItems = new List<ShopItem>();
    private int currentStars;
    private string currentSelectedTexture;
    
    private void Start()
    {
        currentStars = PlayerPrefs.GetInt("Stars", 0);
        currentSelectedTexture = PlayerPrefs.GetString("SelectedTexture", "");
        
        // Reset button setup
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetShop);
        }
        
        // Agar koi texture selected nahi hai toh default unlock kar do
        if (string.IsNullOrEmpty(currentSelectedTexture) && availableTextures.Length > 0)
        {
            for (int i = 0; i < availableTextures.Length; i++)
            {
                if (availableTextures[i].isUnlockedByDefault)
                {
                    currentSelectedTexture = availableTextures[i].textureName;
                    PlayerPrefs.SetString("SelectedTexture", currentSelectedTexture);
                    PlayerPrefs.SetString("UnlockedTexture_" + currentSelectedTexture, "true");
                    break;
                }
            }
        }
        
        CreateShopItems();
        UpdateStarsUI();
        ApplyCurrentTexture();
    }
    
    private void CreateShopItems()
    {
        foreach (TextureData textureData in availableTextures)
        {
            GameObject itemObj = Instantiate(shopItemPrefab, shopContent);
            ShopItem shopItem = itemObj.GetComponent<ShopItem>();
            shopItem.Initialize(textureData, this);
            shopItems.Add(shopItem);
        }
    }
    
    public void UpdateStarsUI()
    {
        currentStars = PlayerPrefs.GetInt("Stars", 0);
    }
    
    public bool IsTextureUnlocked(string textureName)
    {
        return PlayerPrefs.GetString("UnlockedTexture_" + textureName, "false") == "true";
    }
    
    public bool IsTextureSelected(string textureName)
    {
        return currentSelectedTexture == textureName;
    }
    
    public void PurchaseTexture(TextureData textureData)
    {
        if (currentStars >= textureData.price)
        {
            // Stars cut karo
            currentStars -= textureData.price;
            PlayerPrefs.SetInt("Stars", currentStars);
            
            // Texture unlock karo
            PlayerPrefs.SetString("UnlockedTexture_" + textureData.textureName, "true");
            
            // Automatically select kar do
            SelectTexture(textureData.textureName);
            
            PlayerPrefs.Save();
            
            // UI update karo
            UpdateStarsUI();
            RefreshShopItems();
            
            Debug.Log("Texture purchased: " + textureData.textureName);
        }
        else
        {
            Debug.Log("Not enough stars!");
        }
    }
    
    public void SelectTexture(string textureName)
    {
        currentSelectedTexture = textureName;
        PlayerPrefs.SetString("SelectedTexture", currentSelectedTexture);
        PlayerPrefs.Save();
        
        ApplyCurrentTexture();
        RefreshShopItems();
        
        Debug.Log("Texture selected: " + textureName);
    }
    
    private void ApplyCurrentTexture()
    {
        if (playerMaterial != null && !string.IsNullOrEmpty(currentSelectedTexture))
        {
            // Current texture data find karo
            TextureData selectedTextureData = null;
            foreach (TextureData data in availableTextures)
            {
                if (data.textureName == currentSelectedTexture)
                {
                    selectedTextureData = data;
                    break;
                }
            }
            
            if (selectedTextureData != null)
            {
                // Player material mein texture apply karo
                playerMaterial.mainTexture = selectedTextureData.texture;
                
                Debug.Log("Texture applied to material: " + selectedTextureData.textureName);
            }
        }
    }
    
    private void RefreshShopItems()
    {
        foreach (ShopItem item in shopItems)
        {
            item.UpdateUI();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshShopItems();
        UpdateStarsUI();
    }
    
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
    
    public void ResetShop()
    {
        // Confirmation dialog (optional)
        if (UnityEngine.Application.isEditor || 
            UnityEngine.Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Shop Reset!");
        }
        
        // Sab unlocked textures clear karo (except default ones)
        foreach (TextureData data in availableTextures)
        {
            if (!data.isUnlockedByDefault)
            {
                PlayerPrefs.DeleteKey("UnlockedTexture_" + data.textureName);
            }
        }
        
        // Selected texture reset karo to default
        string defaultTexture = "";
        foreach (TextureData data in availableTextures)
        {
            if (data.isUnlockedByDefault)
            {
                defaultTexture = data.textureName;
                break;
            }
        }
        
        if (!string.IsNullOrEmpty(defaultTexture))
        {
            currentSelectedTexture = defaultTexture;
            PlayerPrefs.SetString("SelectedTexture", currentSelectedTexture);
            PlayerPrefs.SetString("UnlockedTexture_" + currentSelectedTexture, "true");
        }
        
        PlayerPrefs.Save();
        
        // UI refresh karo
        UpdateStarsUI();
        ApplyCurrentTexture();
        RefreshShopItems();
        
        Debug.Log("Shop has been reset to default state!");
    }
}
