using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PressAnyKeyMenu : MonoBehaviour {

	private KeyCode maxKeyCode, minKeyCode;
	private PauseMenu pauseMenuScript;

	void Awake() {
		minKeyCode = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Min(); // This is probably KeyCode.None, but a bit of future-proofing.
		maxKeyCode = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Max(); // This is currently Joystick8Button19, but a bit of future-proofing.
	}

	// Use this for initialization
	void Start () {
		pauseMenuScript = GameObject.FindGameObjectWithTag ("Pause Menu").GetComponent<PauseMenu> ();//.GetComponentInChildren<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf && pauseMenuScript.WaitingOnAnyKeypress && pauseMenuScript.CurrentControl != string.Empty) {
			// We're decrementing here, so we'll match Joystick#Button# before JoystickButton#.
			for (KeyCode i = maxKeyCode; i >= minKeyCode; i--) {
				if (Input.GetKeyDown (i)) {
					if (pauseMenuScript.AddBinding (pauseMenuScript.CurrentControl, i)) {
						gameObject.SetActive (false);
						return;
					}
				}
			}
		}
	}
}
