using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    public float Pitch
    {
        get;
        protected set;
    }

    public float Roll
    {
        get;
        protected set;
    }    

    public float Yaw
    {
        get;
        protected set;
    }

    public float Collective
    {
        get;
        protected set;
    }

    public float Throttle
    {
        get;
        protected set;
    }
}