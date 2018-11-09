using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUpBehavior : PowerUpBehaviour
{
    public int speed = 10;
    public int time = 4;
    
    public IEnumerator Pickup(Collider2D Player)
    {
        PlayerController speedadd = Player.GetComponent<PlayerController>();
        speedadd.thrust *= speed;
        speedadd.maxControlledVelocity *= speed;
        yield return new WaitForSeconds(time);
        speedadd.thrust /= speed;
        speedadd.maxControlledVelocity /= speed;
    }
}