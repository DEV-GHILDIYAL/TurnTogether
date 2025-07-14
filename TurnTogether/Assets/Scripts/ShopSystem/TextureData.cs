// 1. TextureData.cs - Scriptable Object for texture data
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture", menuName = "Shop/Texture Data")]
public class TextureData : ScriptableObject
{
    public string textureName;
    public Texture2D texture;
    public int price;
    public bool isUnlockedByDefault = false;
    public Sprite iconSprite; // Shop mein dikhane ke liye
}