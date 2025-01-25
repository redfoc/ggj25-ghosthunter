using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;
    public TMPro.TextMeshProUGUI textDisplay;
    public UnityEngine.UI.Slider valueSlider;
    public UnityEngine.UI.Slider healthSlider;
    public GameObject txtLevelUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateSliderValue(int currentValue, int maxValue)
    {
        valueSlider.value = (float)currentValue / maxValue;
    }

    public void UpdateHealthSliderValue(int currentValue, int maxValue)
    {
        healthSlider.value = (float)currentValue / maxValue;
    }

    public void UpdateText(string text)
    {
        textDisplay.text = text;
    }

    public void ShowLevelUpText()
    {
        txtLevelUp.transform.position = transform.position; // Reset position
        txtLevelUp.SetActive(true);

        LeanTween.moveY(txtLevelUp, txtLevelUp.transform.position.y + 100f, 1f);
        LeanTween.alpha(txtLevelUp, 0f, 1f).setOnComplete(() =>
        {
            txtLevelUp.SetActive(false);
            txtLevelUp.GetComponent<CanvasGroup>().alpha = 1f; // Reset alpha
        });
    }
}
