using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeUpdate : MonoBehaviour
{

	[SerializeField] private AudioMixer am;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Slider>().value = Settings.MASTER_VOLUME;
    }

    public void UpdateVolume(float volume)
    {
    	am.SetFloat("Master Volume", Audio.VolumeToDecibels(volume));
    }
}
