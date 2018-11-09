using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	private Dictionary<string, ControlBinding> keys;
	public enum KeyState { Down, Up, Held };

	private PlayerInput currentInput;
	public PlayerInput CurrentInput { get { return currentInput; } }

	void Awake() {
		currentInput = new PlayerInput ();
		// After some resarch, joystick axes work differently. Needs more looking into if we want to bind those.
		// Probably needed for XBox360 Triggers, as they're analog.
		// This lets us programmatically have multiple key bindings per control.
		// And will let us change them at runtime in future.
		keys = new Dictionary<string, ControlBinding> ();
		keys.Add ("Fire1", new ControlBinding { KeyCode.LeftControl, KeyCode.Mouse0, KeyCode.Joystick1Button5 /* Right Bumper */, KeyCode.Joystick1Button0 /* A Button */ });
		keys.Add ("Menu", new ControlBinding { KeyCode.Escape, KeyCode.Joystick1Button7 /* Start */ });
		keys.Add ("Fire2", new ControlBinding { KeyCode.LeftAlt, KeyCode.Mouse1, KeyCode.Joystick1Button4 /* Left Bumper */, KeyCode.Joystick1Button1 /* B Button */ });
		//keys.Add ("Fire3", new ControlBinding { KeyCode.LeftShift, KeyCode.Mouse2, KeyCode.Joystick1Button2 /* X Button */ });
		//keys.Add ("Cancel", new ControlBinding { KeyCode.Escape, KeyCode.Joystick1Button1 /* B Button */ });
		//keys.Add ("Submit", new ControlBinding { KeyCode.Return, KeyCode.KeypadEnter, KeyCode.Space, KeyCode.Joystick1Button0 /* A Button */ });
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentInput.StickDir = Vector2.ClampMagnitude (new Vector2 (Input.GetAxisRaw ("Joy1RX"), Input.GetAxisRaw ("Joy1RY")), 1);
		currentInput.MousePos = Input.mousePosition;
		currentInput.MoveDir = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1);
	}

	public ControlBinding GetControlBinding(string bindingName)
	{
		return keys [bindingName];
	}

	public void ClearControlBinding(string bindingname)
	{
		keys [bindingname].Clear ();
	}

	public bool AddControlBinding(string bindingName, KeyCode key)
	{
		if (keys.ContainsKey (bindingName)) {
			if (!keys [bindingName].Contains (key)) {
				keys [bindingName].Add (key);
				return true;
			}
			// Do nothing if it's already bound.
		} else {
			// If there's no such control, make one, and add a binding for it.
			keys.Add (bindingName, new ControlBinding { key });
			return true;
		}
		return false;
	}

	public bool CheckControlBinding(string binding, KeyState keystate = KeyState.Down)
	{
		return CheckControlBinding (keys[binding], keystate);
	}

	public bool CheckControlBinding(ControlBinding binding, KeyState keystate = KeyState.Down)
	{
		switch (keystate) {
		case KeyState.Down:
			foreach (KeyCode key in binding) {
				if (Input.GetKeyDown (key)) {
					return true;
				}
			}
			break;
		case KeyState.Up:
			foreach (KeyCode key in binding) {
				if (Input.GetKeyUp (key)) {
					return true;
				}
			}
			break;
		case KeyState.Held:
			foreach (KeyCode key in binding) {
				if (Input.GetKey (key)) {
					return true;
				}
			}
			break;
		}


		return false;
	}
}
