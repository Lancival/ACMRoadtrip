using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
    	Settings.PAUSED = true;
    }

    public void UnPause()
    {
    	StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
    	yield return null;
    	Settings.PAUSED = false;
    }
}
