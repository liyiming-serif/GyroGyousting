using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputController  
{
    void Update()
    {
        // Pitch = Input.GetAxis("R Stick V");
        // Roll = Input.GetAxis("R Stick H");
        // Yaw = Mathf.InverseLerp(-1F, 1F, Input.GetAxis("L Stick H"));
        // Collective = Mathf.InverseLerp(-1F, 1F, Input.GetAxis("L Stick V"));
        
        Throttle = 1.0F;
    }

    public void OnPitchAndRoll(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        Roll = inputVec.x;
        Pitch = inputVec.y;
    }

    public void OnYawAndCollective(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        Yaw = inputVec.x;
        Collective = inputVec.y;
    }
}