using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROFPowerUpBehavior : PowerUpBehaviour
{
    public void Effect(GameObject obj)
    {
        obj.gameObject.GetComponent<PlayerController>().fireWait /= 2;
    }
}
