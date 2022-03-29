using UnityEngine;

public class ForceApplicator  
{
    protected Rigidbody _rigidbody;
    protected Vector3 _forceAxis;
    public float MaxForce  
    {
        get;
        protected set;
    }

    public ForceApplicator(Rigidbody _newRigidbody, float _newMaxForce, Vector3 _newForceAxis)  
    {
        _rigidbody = _newRigidbody;
        _forceAxis = _newForceAxis;
        MaxForce = _newMaxForce;
    }

    public virtual void ApplyForcePercentage(float _percent)  
    {
        // Apply forces from inherited classes.
    }
    
    protected Vector3 CalculateForce(float _percentMaxForce)  
    {
        return _forceAxis * (MaxForce * _percentMaxForce);
    }

}