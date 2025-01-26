using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public GameObject panelAbout;
    public GameObject panelTutorial;
    public GameObject btnPlay;
    public GameObject btnTutorial;
    public GameObject btnCredit;
    public GameObject btnExit;
    public GameObject gameTitle;

    private Vector3 originalScale;
    private float hoverScale = 1.2f;
    private float animationDuration = 0.3f;

    public Image fadePanel; // Assign a black UI Image that covers the screen
    private float fadeDuration = 1f;

    void Start()
    {
        fadePanel.enabled = true;
        panelAbout.SetActive(false);
        panelTutorial.SetActive(false);
        originalScale = btnPlay.transform.localScale;
        SetupButtonHoverEffects();
        AnimateGameTitle();
        StartCoroutine(FadeIn());
    }

    void SetupButtonHoverEffects()
    {
        SetupHoverEffect(btnPlay);
        SetupHoverEffect(btnCredit);
        SetupHoverEffect(btnTutorial);
        SetupHoverEffect(btnExit);
    }

    void SetupHoverEffect(GameObject button)
    {
        EventTrigger trigger = button.AddComponent<EventTrigger>();
        
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => {
            LeanTween.scale(button, originalScale * hoverScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        });
        
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => {
            LeanTween.scale(button, originalScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        });

        trigger.triggers.Add(enterEntry);
        trigger.triggers.Add(exitEntry);
    }

    void AnimateGameTitle()
    {
        LeanTween.scale(gameTitle, Vector3.one * 1.05f, 1f)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();
            
        LeanTween.rotateZ(gameTitle, 3f, 1.5f)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();
    }

    IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        float elapsedTime = 0;
        Color c = fadePanel.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1 - (elapsedTime / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        fadePanel.gameObject.SetActive(true);
        float elapsedTime = 0;
        Color c = fadePanel.color;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = elapsedTime / fadeDuration;
            fadePanel.color = c;
            yield return null;
        }
        
        SceneManager.LoadScene("Gagas - Level 1");
    }

    public void OpenPanelAbout() => panelAbout.SetActive(true);
    public void ClosePanelAbout() => panelAbout.SetActive(false);

    public void OpenPanelTutorial() => panelTutorial.SetActive(true);
    public void ClosePanelTutorial() => panelTutorial.SetActive(false);

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}