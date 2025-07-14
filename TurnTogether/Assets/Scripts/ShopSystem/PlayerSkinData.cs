// 1. PlayerSkinData.cs - Scriptable Object for player skin data
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Skin", menuName = "Shop/Player Skin Data")]
public class PlayerSkinData : ScriptableObject
{
    public string skinName; // Scene mein GameObject ka name exactly same hona chahiye
    public int price;
    public bool isUnlockedByDefault = false;
    public Sprite iconSprite; // Shop mein dikhane ke liye
}
