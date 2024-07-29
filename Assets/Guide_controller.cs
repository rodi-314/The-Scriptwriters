using System.Collections;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.UI
{
    public class Guide_controller : MonoBehaviour
    {
        public Canvas guideCanvas;  // Reference to the Canvas component
        bool visible = false;

        void Start()
        {
            // Ensure guideCanvas is set
            if (guideCanvas == null)
            {
                Debug.LogError("guideCanvas is not assigned.");
                enabled = false;
                return;
            }

            // Set the initial state to be hidden
            guideCanvas.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                visible = !visible;
                guideCanvas.gameObject.SetActive(visible);
            }
        }
    }
}
