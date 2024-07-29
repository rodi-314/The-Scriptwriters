using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButtonController : MonoBehaviour
{
    public Vector2 teleportLocation; // The coordinates to which the MC will be teleported
    private GameModel model;
    private MC_Controller controller;

    void Start()
    {
        // Initialize GameModel and MC_Controller
        model = Schedule.GetModel<GameModel>();
        if (model != null)
        {
            controller = model.MC;
            if (controller == null)
            {
                Debug.LogError("MC_Controller is not found in the model.");
            }
        }
        else
        {
            Debug.LogError("GameModel is not found.");
        }

        // Add the OnClick event listener to the button
        GetComponent<Button>().onClick.AddListener(OnTeleportButtonClicked);
    }

    public void OnTeleportButtonClicked()
    {
        if (controller != null)
        {
            // Teleport to the specified location
            controller.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, controller.transform.position.z);
            Debug.Log("Teleported to: " + teleportLocation);
        }
        else
        {
            Debug.LogError("MC_Controller is not assigned. Cannot teleport.");
        }
    }
}
