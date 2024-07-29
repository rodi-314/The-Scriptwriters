using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // The name of the scene to switch to
    public LayerMask playerLayer; // Layer mask to specify the player's layer
    public float delay = 2.0f; // Delay in seconds before changing the scene

    private bool isPlayerInTrigger = false;
    private float timer = 0.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is in the player layer
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInTrigger = true;
            timer = 0.0f; // Reset the timer
            Debug.Log("Player entered trigger, starting delay timer.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited the trigger is in the player layer
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInTrigger = false;
            Debug.Log("Player exited trigger, stopping delay timer.");
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                ChangeScene();
            }
        }
    }

    private void ChangeScene()
    {
        // Optionally, you can add any effects or animations here before the scene change
        Debug.Log("Changing scene to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
