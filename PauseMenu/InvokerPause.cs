using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to your pause menu GameObject

    private void Start()
    {
        // Ensure the pause menu is initially inactive
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); //lol
        }
    }

    private void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the pause menu visibility
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        // Toggle the state of the pause menu
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            // Pause or resume the game based on the pause menu state
            Time.timeScale = (pauseMenu.activeSelf) ? 0f : 1f;
        }
    }
}
