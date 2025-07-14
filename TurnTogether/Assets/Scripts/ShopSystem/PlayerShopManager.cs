// 2. PlayerShopManager.cs - Main player shop logic
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    public PlayerSkinData[] availableSkins;
    public GameObject shopItemPrefab;
    public Transform shopContent;
    public Transform playerGFXParent; // Player object ke andar GFX parent
    
    [Header("UI Elements")]
    // public TextMeshProUGUI starsText;
    public GameObject shopPanel;
    public Button resetButton;
    
    private List<PlayerShopItem> shopItems = new List<PlayerShopItem>();
    private int currentStars;
    private string currentSelectedSkin;
    private Dictionary<string, GameObject> sceneSkins = new Dictionary<string, GameObject>();
    
    private void Start()
    {
        currentStars = PlayerPrefs.GetInt("Stars", 0);
        currentSelectedSkin = PlayerPrefs.GetString("SelectedPlayerSkin", "");
        
        // Reset button setup
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetShop);
        }
        
        // Agar koi skin selected nahi hai toh default unlock kar do
        if (string.IsNullOrEmpty(currentSelectedSkin) && availableSkins.Length > 0)
        {
            for (int i = 0; i < availableSkins.Length; i++)
            {
                if (availableSkins[i].isUnlockedByDefault)
                {
                    currentSelectedSkin = availableSkins[i].skinName;
                    PlayerPrefs.SetString("SelectedPlayerSkin", currentSelectedSkin);
                    PlayerPrefs.SetString("UnlockedPlayerSkin_" + currentSelectedSkin, "true");
                    break;
                }
            }
        }
        
        FindSceneSkins();
        CreateShopItems();
        UpdateStarsUI();
        ApplyCurrentSkin();
    }
    
    private void FindSceneSkins()
    {
        // Scene mein already placed skins ko find karo
        foreach (PlayerSkinData skinData in availableSkins)
        {
            Transform skinTransform = playerGFXParent.Find(skinData.skinName);
            if (skinTransform != null)
            {
                sceneSkins[skinData.skinName] = skinTransform.gameObject;
                skinTransform.gameObject.SetActive(false); // Initially sab inactive
            }
            else
            {
                Debug.LogWarning("Skin not found in scene: " + skinData.skinName + ". Make sure GameObject name matches skinName in ScriptableObject.");
            }
        }
    }
    
    private void CreateShopItems()
    {
        foreach (PlayerSkinData skinData in availableSkins)
        {
            GameObject itemObj = Instantiate(shopItemPrefab, shopContent);
            PlayerShopItem shopItem = itemObj.GetComponent<PlayerShopItem>();
            shopItem.Initialize(skinData, this);
            shopItems.Add(shopItem);
        }
    }
    
    public void UpdateStarsUI()
    {
        currentStars = PlayerPrefs.GetInt("Stars", 0);
        // starsText.text = "Stars: " + currentStars;
    }
    
    public bool IsSkinUnlocked(string skinName)
    {
        return PlayerPrefs.GetString("UnlockedPlayerSkin_" + skinName, "false") == "true";
    }
    
    public bool IsSkinSelected(string skinName)
    {
        return currentSelectedSkin == skinName;
    }
    
    public void PurchaseSkin(PlayerSkinData skinData)
    {
        if (currentStars >= skinData.price)
        {
            // Stars cut karo
            currentStars -= skinData.price;
            PlayerPrefs.SetInt("Stars", currentStars);
            
            // Skin unlock karo
            PlayerPrefs.SetString("UnlockedPlayerSkin_" + skinData.skinName, "true");
            
            // Automatically select kar do
            SelectSkin(skinData.skinName);
            
            PlayerPrefs.Save();
            
            // UI update karo
            UpdateStarsUI();
            RefreshShopItems();
            
            Debug.Log("Player skin purchased: " + skinData.skinName);
        }
        else
        {
            Debug.Log("Not enough stars!");
        }
    }
    
    public void SelectSkin(string skinName)
    {
        currentSelectedSkin = skinName;
        PlayerPrefs.SetString("SelectedPlayerSkin", currentSelectedSkin);
        PlayerPrefs.Save();
        
        ApplyCurrentSkin();
        RefreshShopItems();
        
        Debug.Log("Player skin selected: " + skinName);
    }
    
    private void ApplyCurrentSkin()
    {
        if (!string.IsNullOrEmpty(currentSelectedSkin))
        {
            // Pehle sab skins inactive kar do
            foreach (var skinPair in sceneSkins)
            {
                skinPair.Value.SetActive(false);
            }
            
            // Selected skin active kar do
            if (sceneSkins.ContainsKey(currentSelectedSkin))
            {
                sceneSkins[currentSelectedSkin].SetActive(true);
                Debug.Log("Player skin applied: " + currentSelectedSkin);
            }
            else
            {
                Debug.LogWarning("Selected skin not found in scene: " + currentSelectedSkin);
            }
        }
    }
    
    private void RefreshShopItems()
    {
        foreach (PlayerShopItem item in shopItems)
        {
            item.UpdateUI();
        }
    }
    
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateStarsUI();
        RefreshShopItems();
    }
    
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
    
    public void ResetShop()
    {
        // Sab unlocked skins clear karo (except default ones)
        foreach (PlayerSkinData data in availableSkins)
        {
            if (!data.isUnlockedByDefault)
            {
                PlayerPrefs.DeleteKey("UnlockedPlayerSkin_" + data.skinName);
            }
        }
        
        // Selected skin reset karo to default
        string defaultSkin = "";
        foreach (PlayerSkinData data in availableSkins)
        {
            if (data.isUnlockedByDefault)
            {
                defaultSkin = data.skinName;
                break;
            }
        }
        
        if (!string.IsNullOrEmpty(defaultSkin))
        {
            currentSelectedSkin = defaultSkin;
            PlayerPrefs.SetString("SelectedPlayerSkin", currentSelectedSkin);
            PlayerPrefs.SetString("UnlockedPlayerSkin_" + currentSelectedSkin, "true");
        }
        
        PlayerPrefs.Save();
        
        // UI refresh karo
        UpdateStarsUI();
        ApplyCurrentSkin();
        RefreshShopItems();
        
        Debug.Log("Player shop has been reset to default state!");
    }
}