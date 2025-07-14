using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { Coin, Star }
    public Type collectibleType;

    public AudioClip coinSound;  // 🎵 Assign in Inspector
    public AudioClip starSound;  // 🌟 Optional
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.FindWithTag("Sound").GetComponent<AudioSource>();
        if (audioSource != null && coinSound != null)
        {
            Debug.Log("Manual test sound");
            // audioSource.PlayOneShot(coinSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (collectibleType == Type.Coin)
        {
            int gained = Random.Range(5, 11);
            int current = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("Coins", current + gained);

            FloatingPopupPool.Instance.Show(transform.position, "+" + gained, Color.yellow);

            if (coinSound != null && audioSource != null)
            {
                // Debug.Log("SOUND");
                audioSource.PlayOneShot(coinSound);
            }

            GameManager.Instance.AddCoin(gained);

            // Debug.Log("💰 Coin collected: +" + gained);
        }
        else if (collectibleType == Type.Star)
        {
            int current = PlayerPrefs.GetInt("Stars", 0);
            PlayerPrefs.SetInt("Stars", current + 1);

            FloatingPopupPool.Instance.Show(transform.position, "+1", Color.cyan);

            if (starSound != null && audioSource != null)
                audioSource.PlayOneShot(starSound);

                
            GameManager.Instance.AddStar();

            // Debug.Log("🌟 Star collected: +1");
        }

        PlayerPrefs.Save();
        Invoke("DestroyObject", 0.2f);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
