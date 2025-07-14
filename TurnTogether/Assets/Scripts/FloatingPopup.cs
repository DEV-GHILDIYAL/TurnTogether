using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public CanvasGroup canvasGroup;

    private float floatSpeed = 0.5f; // Use smaller value for world space
    private float fadeDuration = 0.8f;
    private float lifetime = 5f;
    private Vector3 moveDirection = Vector3.up;

    void OnEnable()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeAndDisable());
    }

    public void Setup(string message, Color color)
    {
        popupText.text = message;
        popupText.color = color;
    }

    IEnumerator FadeAndDisable()
    {
        float elapsed = 0f;
        while (elapsed < lifetime)
        {
            transform.position += moveDirection * floatSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
