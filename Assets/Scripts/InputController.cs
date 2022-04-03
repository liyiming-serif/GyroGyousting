public abstract class InputController  
{
    public abstract void HandleInput();

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