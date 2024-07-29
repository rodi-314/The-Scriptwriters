using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        MC_Controller controller = other.GetComponent<MC_Controller>();

        if (controller != null)
        {
            controller.ChangePower(-1);
        }
    }

}
