using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
	private Fade fader;

	void Start()
	{
		fader = transform.GetComponent<Fade>();
	}

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetMouseButtonUp(0) || Input.GetKeyDown("space") || Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
    		fader.FadeOut();
    }
}
