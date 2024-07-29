using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;

public class HintInteraction : MonoBehaviour
{
    public string hintText;
    
    public int itemCount = 1;
    
    MC_Controller controller;
    

    void OnTriggerEnter2D(Collider2D collider)
    {
        controller = collider.GetComponent<MC_Controller>();
        if (controller != null)
        {
            MessageBar.Show(hintText);

            // Adding inventory item to the game model
            GameModel model = Schedule.GetModel<GameModel>();
            

            
        }
    }
}
