using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;
    public TMPro.TextMeshProUGUI textDisplay;
    public UnityEngine.UI.Slider valueSlider;
    public UnityEngine.UI.Slider healthSlider;
    public GameObject txtLevelUp;
    public TMPro.TextMeshProUGUI textCountBiasa;
    public TMPro.TextMeshProUGUI textCountBuff;
    public TMPro.TextMeshProUGUI textCountKebut;
    public GameObject popupGameover;
    public GameObject popupGameCompleted;

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

    public void UpdateTextCounterGhost(string biasa, string buff, string kebut)
    {
        textCountBiasa.text = (string)biasa;
        textCountBuff.text = (string)buff;
        textCountKebut.text = (string)kebut;
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

    public void ShowPopupGameover(){
        GameObject[] collection = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in collection)
        {
            item.GetComponent<Enemy>().speed = 0;
        }
        GameManager.instance.isGameover = true;
        popupGameover.SetActive(true);
    }

    public void ShowPopupGameCompleted(){
        GameManager.instance.isGameComplete = true;
        popupGameCompleted.SetActive(true);
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHome(){
        SceneManager.LoadScene("MainMenu");
    }
}
