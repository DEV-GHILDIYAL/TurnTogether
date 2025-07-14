using UnityEngine;
using System.Collections.Generic;

public class FloatingPopupPool : MonoBehaviour
{
    public static FloatingPopupPool Instance;

    public GameObject popupPrefab;
    public int poolSize = 10;

    private Queue<GameObject> popupPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(popupPrefab, transform);
            obj.SetActive(false);
            popupPool.Enqueue(obj);
        }
    }

    public void Show(Vector3 worldPos, string msg, Color color)
    {
        GameObject popup = popupPool.Count > 0 ? popupPool.Dequeue() : Instantiate(popupPrefab, transform);
        popup.SetActive(true);

        popup.transform.position = worldPos + Vector3.up * 1.5f; // 👈 Use world position directly

        popup.GetComponent<FloatingPopup>().Setup(msg, color);

        popupPool.Enqueue(popup);
    }

}
