using UnityEngine;

public class GyousterController : MonoBehaviour  
{
    [Header("Components")]
    public Rigidbody _mainRotor;
    public Rigidbody _body;

    private LinearForceApplicator _lift;
    private RotationalForceApplicator _mainRotorTorque;
    private RotationalForceApplicator _bodyTorque;
    private RotationalForceApplicator _bodyCounterTorque;
    private RotationalForceApplicator _pitchTorque;
    private RotationalForceApplicator _rollTorque;
    private InputController _controls;

    void Start()
    {
        _lift = new LinearForceApplicator(_body, 1000062, Vector3.up);
        _mainRotorTorque = new RotationalForceApplicator(_mainRotor, 3, 240, Vector3.down);
        _bodyTorque = new RotationalForceApplicator(_body, 25000, 120, Vector3.down);
        _bodyCounterTorque = new RotationalForceApplicator(_body, 50000, 120, Vector3.up);
        _pitchTorque = new RotationalForceApplicator(_body, 20000, 100, Vector3.right);
        _rollTorque = new RotationalForceApplicator(_body, 15000, 100, Vector3.back);
        _controls = new PlayerController();

        // This should account for any weird wobbling caused by misaligned pivots.
        _body.centerOfMass = Vector3.zero;
    }

    void Update()
    {
        _controls.HandleInput();
    }

    void FixedUpdate()
    {
        // Throttle
        _mainRotorTorque.ApplyForcePercentage(_controls.Throttle);
        _bodyTorque.ApplyForcePercentage(_mainRotorTorque.PercentMaxRPM);

        // Yaw
        _bodyCounterTorque.ApplyForcePercentage(_mainRotorTorque.PercentMaxRPM * _controls.Yaw);

        // Collective
        _lift.ApplyForcePercentage(_mainRotorTorque.PercentMaxRPM * _controls.Collective);

        // Cyclic
        _pitchTorque.ApplyForcePercentage(_mainRotorTorque.PercentMaxRPM * _controls.Pitch);
        _rollTorque.ApplyForcePercentage(_mainRotorTorque.PercentMaxRPM * _controls.Roll);
    }
}