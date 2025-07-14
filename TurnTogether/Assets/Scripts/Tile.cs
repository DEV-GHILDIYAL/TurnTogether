using UnityEngine;

public class Tile : MonoBehaviour
{
    public EndlessMapGenerator generator;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            GameManager.Instance.AddScore(1);
            generator.RecycleOldTile();
        }
    }

}
