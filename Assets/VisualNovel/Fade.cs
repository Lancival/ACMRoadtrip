using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	private CanvasGroup uiElement; // Canvas group on the GameObject this script is attached to
	private static float FADE_DURATION = 1f; // Duration of fade effects, in seconds
    private IEnumerator coroutine = null;

	// Intialize variable even when inactive at start
	private void Awake()
	{
		uiElement = gameObject.GetComponent<CanvasGroup>();
	}

	// Changes the alpha value of the canvas group over time, along with activating/deactivating the group
    private static IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration, bool active) {
    	float startTime = Time.time;
    	float percent = 0;
    	while (true) {
    		percent = (Time.time - startTime) / duration;
    		if (percent < 1)
    			cg.alpha = Mathf.Lerp(start, end, percent);
    		else
    		{
    			cg.alpha = 1;
    			break;
    		}
    		yield return new WaitForEndOfFrame();
    	}
    	cg.transform.gameObject.SetActive(active);
    }

    // Reactivates the uiElement and fades it to full visibilty
    public void FadeIn()
    {
    	gameObject.SetActive(true);
        Stop();
    	coroutine = FadeCanvasGroup(uiElement, 0, 1, FADE_DURATION, true);
        StartCoroutine(coroutine);
    }

    // Fades the uiElement to full invisibility and deactivates it
    public void FadeOut()
    {
        Stop();
    	coroutine = FadeCanvasGroup(uiElement, 1, 0, FADE_DURATION, false);
        StartCoroutine(coroutine);
    }

    public void Appear()
    {
        Stop();
        gameObject.SetActive(true);
        uiElement.alpha = 1;
    }

    public void Disappear()
    {
        Stop();
        uiElement.alpha = 0;
        gameObject.SetActive(false);
    }

    private void Stop()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
