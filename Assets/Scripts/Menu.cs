using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using RPGM.Gameplay;

public class Menu : MonoBehaviour
{
    private SaveSystem saveSystem;
    private GameController gameController;

    void Start()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    public void OnPlayButton()
    {
        // Start a new game, which loads the first scene
        SceneManager.LoadScene(1);
    }

    public void OnResumeButton()
    {
        if (saveSystem.SaveFileExists())
        {
            SaveData data = saveSystem.LoadGame();
            StartCoroutine(LoadSceneAndData(data));
        }
        else
        {
            Debug.LogError("No save file found to resume.");
        }
    }

    private IEnumerator LoadSceneAndData(SaveData data)
    {
        // Load the saved scene
        SceneManager.LoadScene(data.currentScene);
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure the scene is fully loaded

        // Find the GameController in the loaded scene
        gameController = FindObjectOfType<GameController>();

        if (gameController != null)
        {
            gameController.LoadData();
        }
        else
        {
            Debug.LogError("GameController not found in the loaded scene.");
        }
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        Debug.Log("Quit button pressed. Application.Quit() does not work in the editor.");
#else
        Application.Quit();
#endif
    }
}
