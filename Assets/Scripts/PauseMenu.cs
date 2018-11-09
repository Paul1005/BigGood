using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public Button SetFire1, SetFire2, SetMenu;
	public Button ClearFire1, ClearFire2, ClearMenu;
	public Text Fire1Bindings, Fire2Bindings, MenuBindings;

	private InputController playerInput;
	private GameObject pressAnyKeyMenu;
	private bool waitingOnAnyKeypress;
	public bool WaitingOnAnyKeypress { get { return waitingOnAnyKeypress; } }
	private string currentControl;
	public string CurrentControl { get { return currentControl; } }
	public KeyCode CurrentKey { get; set; } // Not implemented yet. To clear individual keys from a control binding.

	private Dictionary<string, Button> setButtons;
	private Dictionary<string, Button> clearButtons;
	private Dictionary<string, Text> bindingLists;

	void Awake() {
		waitingOnAnyKeypress = false;
		setButtons = new Dictionary<string, Button> {
			{ "Fire1", SetFire1 }, 
			{ "Fire2", SetFire2 }, 
			{ "Menu", SetMenu } 
			};
		clearButtons = new Dictionary<string, Button> {
			{ "Fire1", ClearFire1 }, 
			{ "Fire2", ClearFire2 }, 
			{ "Menu", ClearMenu } 
		};
		bindingLists = new Dictionary<string, Text> {
			{ "Fire1", Fire1Bindings }, 
			{ "Fire2", Fire2Bindings }, 
			{ "Menu", MenuBindings } 
		};
	}

	// Use this for initialization
	void Start () {
		currentControl = string.Empty;
		CurrentKey = KeyCode.None;

		playerInput = GameObject.FindGameObjectWithTag ("Player").GetComponent<InputController> ();

		pressAnyKeyMenu = GameObject.FindGameObjectWithTag ("Press Any Key");
		pressAnyKeyMenu.SetActive(false);

		foreach (KeyValuePair<string, Text> entry in bindingLists) {
			entry.Value.text = playerInput.GetControlBinding (entry.Key).AllKeysToString();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (pressAnyKeyMenu.activeSelf) {
			if (!waitingOnAnyKeypress) {
				waitingOnAnyKeypress = true;
			}
		} else {
			if (waitingOnAnyKeypress) {
				waitingOnAnyKeypress = false;
				currentControl = string.Empty;
				CurrentKey = KeyCode.None;
			}
		}
	}

	public void ClearBindings(string control)
	{
		playerInput.ClearControlBinding(control);
		bindingLists[control].text = playerInput.GetControlBinding (control).AllKeysToString();
	}

	public bool AddBinding(string control, KeyCode key)
	{
		if (!playerInput.AddControlBinding (control, key)) {
			// Did not succeed at setting a binding.
			return false;
		} else {
			// Successfully set a binding. Update the display.
			bindingLists[control].text = playerInput.GetControlBinding (control).AllKeysToString();
			return true;
		}
	}

	void OnEnable()
	{
		foreach (KeyValuePair<string, Button> entry in clearButtons) {
			entry.Value.onClick.AddListener (() => ClearBindings (entry.Key));
		}
		foreach (KeyValuePair<string, Button> entry in setButtons) {
			entry.Value.onClick.AddListener (() => GetAnyKey (entry.Key));
		}
	}

	public void GetAnyKey(string control)
	{
		currentControl = control;
		pressAnyKeyMenu.SetActive (true);
	}

	void OnDisable()
	{
		//Un-Register Button Events
		foreach (KeyValuePair<string, Button> entry in clearButtons) {
			entry.Value.onClick.RemoveAllListeners ();
		}
		foreach (KeyValuePair<string, Button> entry in setButtons) {
			entry.Value.onClick.RemoveAllListeners ();
		}

		waitingOnAnyKeypress = false;
        if (pressAnyKeyMenu != null)
        {
            pressAnyKeyMenu.SetActive(false);
        }
	}
}
