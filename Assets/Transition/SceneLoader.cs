using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/*
 *  This script facilitates transitioning between Unity scenes asynchronously.
 *  It will activate the visual and audio transitions.
 *  This script should be attached to a game object with a Fade script component, a CanvasGroup component, and the transitional UI Image.
 *  This script also needs to be provided the a String of the name of the next scene, and the AudioMixer used for all sounds.
 */

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string nextScene;              // Name of the next scene
    [SerializeField] private AudioMixer mixer;              // AudioMixer controlling volume of sounds.
    [SerializeField] private float TRANSITION_DURATION;     // Length of scene transition, in seconds

    private Fade fader;							// Fade script component attached to game object
    private bool loading = false;               // Whether this script is already loading a scene

    // Fades out transitional image and fades in audio at the start of a scene.
    void Start()
    {
    	// Fade out transitional image
        fader = gameObject.GetComponent<Fade>();
        if (fader == null)
        {
        	Debug.Log("Error: Fade script component not attached to SceneManager.");
        	Destroy(this);
        }
        else
            fader.FadeOut();

        // Fade in sound to default volume
        if (mixer == null)
        {
            Debug.Log("Error: AudioMixer group not provided to SceneLoader script.");
            Destroy(this);
        }
        else
            StartCoroutine(Audio.Fade(mixer, "Master Volume", TRANSITION_DURATION, Settings.MASTER_VOLUME));
    }

    // Loads the nextScene scene asynchronously, waiting until at least duration seconds have passed to finish.
    private static IEnumerator LoadSceneAsync(string nextScene, float duration)
    {
        if (nextScene == null)
        {
            Debug.Log("Error: No name provided for next scene.");
            yield return null;
        }

        float time = 0;

        // Start loading the next scene asynchronously.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        asyncOperation.allowSceneActivation = false;

        // Wait until the next scene has been loaded in the background.
        while (asyncOperation != null && !asyncOperation.isDone)
        {
            time += Time.deltaTime;
            // Don't allow the next scene to activate until the transition duration has finished.
            if (time > duration)
                asyncOperation.allowSceneActivation = true;

            yield return null;
        }
    }

    // Load the next scene, fading in the transitional image and fading out the audio.
    public void LoadNextScene() {
        if (!loading)
        {
            loading = true;
            fader.FadeIn();
            StartCoroutine(Audio.Fade(mixer, "Master Volume", TRANSITION_DURATION, 0));
            StartCoroutine(LoadSceneAsync(nextScene, TRANSITION_DURATION));
        }
    }
}
