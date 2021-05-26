using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpdate : MonoBehaviour
{
	void Start()
	{
		if (Settings.DIALOGUE_SPEED == 0f)
			transform.GetChild(2).GetComponent<Button>().interactable = false;
		else if (Settings.DIALOGUE_SPEED == 0.03f)
			transform.GetChild(0).GetComponent<Button>().interactable = false;
		else
			transform.GetChild(1).GetComponent<Button>().interactable = false;
	}

    public void UpdateSpeed(float delay)
    {
        Settings.DIALOGUE_SPEED = delay;
    }
}
