using UnityEngine;

public class PlayerController : InputController  
{
    public override void HandleInput()
    {
        Pitch = Input.GetAxis("R Stick V");
        Roll = Input.GetAxis("R Stick H");
        Yaw = Mathf.InverseLerp(-1F, 1F, Input.GetAxis("L Stick H"));
        Collective = Mathf.InverseLerp(-1F, 1F, Input.GetAxis("L Stick V"));
        Throttle = 1.0F;
    }
}