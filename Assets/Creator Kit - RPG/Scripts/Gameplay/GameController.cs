using UnityEngine;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPGM.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public GameModel model;
        private SaveSystem saveSystem;

        protected virtual void OnEnable()
        {
            Schedule.SetModel<GameModel>(model);
            saveSystem = FindObjectOfType<SaveSystem>();
        }

        protected virtual void Update()
        {
            Schedule.Tick();

            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                LoadScene();
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadData();
            }
        }

        public void SaveGame()
        {
            SaveData data = model.CreateSaveData();
            saveSystem.SaveGame(data);
            Debug.Log("Game saved.");
        }

        public void LoadScene()
        {
            if (saveSystem.SaveFileExists())
            {
                SaveData data = saveSystem.LoadGame();
                StartCoroutine(LoadSceneCoroutine(data));
            }
            else
            {
                Debug.LogError("No save file found.");
            }
        }

        public void LoadData()
        {
            if (saveSystem.SaveFileExists())
            {
                SaveData data = saveSystem.LoadGame();
                model.LoadFromSaveData(data);
            }
            else
            {
                Debug.LogError("No save file found.");
            }
        }

        private IEnumerator LoadSceneCoroutine(SaveData data)
        {
            Debug.Log("Loading scene: " + data.currentScene);
            SceneManager.LoadScene(data.currentScene);
            yield return null; // Wait for the scene to load

            // Additional wait to ensure everything is initialized
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Scene loaded. Press F9 to apply save data.");
        }
    }
}
