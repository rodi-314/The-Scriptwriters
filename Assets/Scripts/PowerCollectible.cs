using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCollectible : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        MC_Controller controller = other.GetComponent<MC_Controller>();

        if (controller != null)
        {
            if (controller.Power < controller.maxPower)
            {
                controller.ChangePower(1);
                Destroy(gameObject);
            }

        }
    }
}
