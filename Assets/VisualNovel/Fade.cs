using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	private CanvasGroup uiElement; // Canvas group on the GameObject this script is attached to
	private static float FADE_DURATION = 0.5f; // Duration of fade effects, in seconds

	// Intialize variable even when inactive at start
	private void Awake()
	{
		uiElement = gameObject.GetComponent<CanvasGroup>();
	}

	// Changes the alpha value of the canvas group over time, along with activating/deactivating the group
    private static IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration, bool active) {
    	float startTime = Time.time;
    	float elapsedTime = 0;
    	while (true) {
    		elapsedTime = Time.time - startTime;
    		if (elapsedTime/duration < 1)
    			cg.alpha = Mathf.Lerp(start, end, elapsedTime/duration);
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
    	StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, FADE_DURATION, true));
    }

    // Fades the uiElement to full invisibility and deactivates it
    public void FadeOut()
    {
    	StartCoroutine(FadeCanvasGroup(uiElement, 1, 0, FADE_DURATION, false));
    }
}
