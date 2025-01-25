using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour
{
   public Image sponsor1;
   public Image sponsor2;
   public float displayDuration = 2f;
   public float fadeDuration = 1f;

   public Image fadePanel; // Assign a black UI Image that covers the screen

   void Start()
   {
       StartCoroutine(PlaySplashSequence());
       StartCoroutine(FadeIn());
   }

   IEnumerator PlaySplashSequence()
   {
       // Setup initial state
       sponsor1.gameObject.SetActive(true);
       sponsor2.gameObject.SetActive(false);
       sponsor1.color = new Color(1, 1, 1, 0);

       // Fade in sponsor1
       LeanTween.alpha(sponsor1.rectTransform, 1, fadeDuration);
       yield return new WaitForSeconds(fadeDuration + displayDuration);

       // Fade out sponsor1
       LeanTween.alpha(sponsor1.rectTransform, 0, fadeDuration);
       yield return new WaitForSeconds(fadeDuration);

       // Switch to sponsor2
       sponsor1.gameObject.SetActive(false);
       sponsor2.gameObject.SetActive(true);
       sponsor2.color = new Color(1, 1, 1, 0);

       // Fade in sponsor2
       LeanTween.alpha(sponsor2.rectTransform, 1, fadeDuration);
       yield return new WaitForSeconds(fadeDuration + displayDuration);

       // Fade out sponsor2 and load next scene
       LeanTween.alpha(sponsor2.rectTransform, 0, fadeDuration);
       yield return new WaitForSeconds(fadeDuration);

       StartCoroutine(FadeAndLoadScene());
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
        
        SceneManager.LoadScene("MainMenu");
    }
}