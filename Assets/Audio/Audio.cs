using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class Audio
{
	// Converts volume (0 to 1) to decibels
	public static float VolumeToDecibels(float volume)
	{
		return Mathf.Log10(Mathf.Clamp(volume, 0.00001f, 1)) * 20;
	}

	// Converts decibels to volume (0 to 1)
	private static float DecibelsToVolume(float decibels)
	{
		return Mathf.Pow(10, decibels / 20);
	}

	// Fade the exposedParam Audio Mixer Group of the audioMixer AudioMixer from its current volume to the targetVolume, over duration seconds.
    public static IEnumerator Fade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = DecibelsToVolume(currentVol);

        // Lerp between starting volume and target volume.
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / duration);
            audioMixer.SetFloat(exposedParam, VolumeToDecibels(newVol));
            yield return null;
        }
        yield break;
    }
}
