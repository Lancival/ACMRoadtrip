using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Settings is a static class which stores global values needed in the game.
 * Some values are saved to PlayerPrefs so that they are preserved when the game is opened or closed.
 * These values are accessible as properties (ie you can access FONT_SCALE by using Settings.FONT_SCALE)
 * instead of through public functions.
 */

public static class Settings
{
	// Gameplay State
    public static bool PAUSED = false;

    // Text Display Delay
	public static float DIALOGUE_SPEED {
		get {return PlayerPrefs.GetFloat("Speed", 0.03f);}	// Returns 0.03 second delay between characters as default
		set {PlayerPrefs.SetFloat("Speed", value);}
	}

	// Font Size
	public static float FONT_SCALE {
		get {return PlayerPrefs.GetFloat("Scale", 1.0f);} // Returns 1.0 as default font scale
		set {PlayerPrefs.SetFloat("Scale", value);}
	}

	// Master Volume, affect all sounds and music
	public static float MASTER_VOLUME {
		get {return PlayerPrefs.GetFloat("Master", 1.0f);} // Default of full volume
		set {PlayerPrefs.SetFloat("Master", value);}
	}

	// Music Volume
	public static float MUSIC_VOLUME {
		get {return PlayerPrefs.GetFloat("Music", 1.0f);} // Default of full volume
		set {PlayerPrefs.SetFloat("Music", value);}
	}
}
