using UnityEngine;

public class DoorSwing : MonoBehaviour
{
    public float forceAmount = 50f; // Amount of force to apply
    private Rigidbody rb; // Rigidbody of the door

    private void Start()
    {
        // Ensure there is a Rigidbody component on the door
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
            return;
        }

        // Ensure the Rigidbody is not kinematic
        if (rb.isKinematic)
        {
            Debug.LogWarning("Rigidbody on " + gameObject.name + " is kinematic.");
        }

        // Create a HingeJoint at runtime and set up its parameters
        HingeJoint hinge = gameObject.AddComponent<HingeJoint>();
        hinge.anchor = new Vector3(0.5f, 0, 0); // Set the anchor point to one edge of the door
        hinge.axis = new Vector3(0, 1, 0); // Set the axis of rotation to be around the y-axis

        // Optional - limit the rotation of the hinge joint
        JointLimits limits = hinge.limits;
        limits.min = 0;
        limits.max = 90;
        hinge.limits = limits;
        hinge.useLimits = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")) // Check that the collision is with the player
        {
            // Apply a force to the door
            rb.AddForce(-transform.right * forceAmount, ForceMode.Impulse);
        }
    }
}

