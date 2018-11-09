using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		Gameplay,
		PauseMenu,
		GameOver
	}

    private static bool isGamePaused;
    public static bool IsGamePaused { get { return isGamePaused; } }
    private InputController playerInput;

    private GameObject pauseMenu;
	private GameObject gameOverMenu;

    private EventSystem eventSystem;
    public GameObject defaultButton; // Refactor me to handle multiple menus.

	/// <summary>
	/// Do not access this directly unless you really mean to. Use the getter and setter.
	/// </summary>
	private GameState currentState;
	public GameState CurrentState{
		get { return currentState; }
		set { 
			if (UpdateGameState (value)) {
				currentState = value;
			}
		}
	}


    public static void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    public static void Continue()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }

    void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<InputController>();
    }

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		pauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
        pauseMenu.SetActive(false); // Start off.
		gameOverMenu = GameObject.FindGameObjectWithTag("GameOver Menu");
		gameOverMenu.SetActive(false); // Start off.

		currentState = GameState.Gameplay; // Initialize to gameplay, since we're currently started off right in it.
    }

    void Update()
    {
        if (CurrentState == GameState.PauseMenu &&
            playerInput.CheckControlBinding("Fire1", InputController.KeyState.Held) &&
            playerInput.CheckControlBinding("Fire2", InputController.KeyState.Held))
        {
            QuitGame();
        }

		if (playerInput.CheckControlBinding("Menu"))
        {
			if (CurrentState == GameState.Gameplay) {
				CurrentState = GameState.PauseMenu;
			} else if (CurrentState == GameState.PauseMenu) {
				CurrentState = GameState.Gameplay;
			}
        }

    }

	private bool UpdateGameState(GameState newState)
	{
		switch (newState) {
		case GameState.Gameplay: // If we want to switch to gameplay
			if (CurrentState == GameState.PauseMenu) { // And we're currently paused.
				pauseMenu.SetActive (false);
				GameManager.Continue ();
				return true;
			}
			break;
		case GameState.PauseMenu: // If we want to switch to the pause menu
			if (CurrentState == GameState.Gameplay) { // And we're currently in gameplay.
				pauseMenu.SetActive (true);
				eventSystem.SetSelectedGameObject (null);
				eventSystem.SetSelectedGameObject (defaultButton);
				GameManager.Pause ();
				return true;
			}
			break;
		case GameState.GameOver:
			gameOverMenu.SetActive (true);
			GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Restart"));
			return true;
		}

		return false;
	}

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}