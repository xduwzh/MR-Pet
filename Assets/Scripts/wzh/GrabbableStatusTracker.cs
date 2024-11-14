using UnityEngine;

public class GrabbableStatusTracker : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public bool IsGrabbed { get; private set; } = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the rigidbody is kinematic, which often indicates it's grabbed
        IsGrabbed = _rigidbody.isKinematic;
    }
}
