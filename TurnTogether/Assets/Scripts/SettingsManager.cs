using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Toggles")]
    public Toggle toggleVignette;
    public Toggle toggleBloom;
    public Toggle toggleMotionBlur;
    public Toggle toggleSound;

    [Header("Post Processing")]
    public Volume volume;

    public Vignette vignette;
    public Bloom bloom;
    public MotionBlur motionBlur;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out motionBlur);

        // Load saved states
        toggleVignette.isOn = PlayerPrefs.GetInt("Vignette", 1) == 1;
        toggleBloom.isOn = PlayerPrefs.GetInt("Bloom", 1) == 1;
        toggleMotionBlur.isOn = PlayerPrefs.GetInt("MotionBlur", 1) == 1;
        toggleSound.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;

        ApplySettings();

        // Listen to toggles
        toggleVignette.onValueChanged.AddListener((val) => ToggleVignette(val));
        toggleBloom.onValueChanged.AddListener((val) => ToggleBloom(val));
        toggleMotionBlur.onValueChanged.AddListener((val) => ToggleMotionBlur(val));
        toggleSound.onValueChanged.AddListener((val) => ToggleSound(val));
    }

    void ApplySettings()
    {
        ToggleVignette(toggleVignette.isOn);
        ToggleBloom(toggleBloom.isOn);
        ToggleMotionBlur(toggleMotionBlur.isOn);
        ToggleSound(toggleSound.isOn);
    }

    public void ToggleVignette(bool isOn)
    {
        if (vignette != null)
        {
            // vignette.active = isOn;
            vignette.intensity.value = isOn ? .45f : 0f;  
            Debug.Log(vignette.active);
            PlayerPrefs.SetInt("Vignette", isOn ? 1 : 0);
        }
    }

    public void ToggleBloom(bool isOn)
    {
        if (bloom != null)
        {
            // bloom.active = isOn;
            bloom.intensity.value = isOn ? 25f : 0f; 
            PlayerPrefs.SetInt("Bloom", isOn ? 1 : 0);
        }
    }

    public void ToggleMotionBlur(bool isOn)
    {
        if (motionBlur != null)
        {
            // motionBlur.active = isOn;
            motionBlur.intensity.value = isOn ? 0.25f : 0f; 
            PlayerPrefs.SetInt("MotionBlur", isOn ? 1 : 0);
        }
    }

    public void ToggleSound(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
    }
}