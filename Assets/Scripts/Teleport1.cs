using UnityEngine;

public class MagicStone : MonoBehaviour
{
    public Vector2 teleportLocation; // The coordinates to which the MC will be teleported
    public float delay = 2.0f; // Delay in seconds before teleporting

    private bool isPlayerInTrigger = false;
    private float timer = 0.0f;
    private MC_Controller controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        controller = other.GetComponent<MC_Controller>();
        if (controller != null)
        {
            isPlayerInTrigger = true;
            timer = 0.0f; // Reset the timer
            Debug.Log("Player entered trigger, starting delay timer for teleportation.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited the trigger is the player
        MC_Controller exitingController = other.GetComponent<MC_Controller>();
        if (exitingController != null && exitingController == controller)
        {
            isPlayerInTrigger = false;
            Debug.Log("Player exited trigger, stopping delay timer for teleportation.");
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                TeleportPlayer();
                isPlayerInTrigger = false; // Ensure teleportation only happens once per entry
            }
        }
    }

    private void TeleportPlayer()
    {
        // Set the player's position to the teleport location
        controller.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, controller.transform.position.z);

        // Optionally, you can add a message or effect here to indicate teleportation
        Debug.Log("Player has been teleported to " + teleportLocation);
    }
}
