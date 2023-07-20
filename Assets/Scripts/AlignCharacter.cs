using UnityEngine;

public class AlignCharacter : MonoBehaviour
{
    public LayerMask ground;
    public float time;
    public AnimationCurve curve;
    public float playerHeight;
    public bool isOnGround;
    
    // Update is called once per frame
    void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        alignGround();
    }

    private void alignGround()
    {
        var ray = new Ray(transform.position, -transform.up);
        if (!Physics.Raycast(ray, out var rayHit, ground) || !isOnGround) return;
        var reference = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, rayHit.normal),
            curve.Evaluate(time));
        transform.rotation = Quaternion.Euler(reference.eulerAngles.x, transform.eulerAngles.y, reference.eulerAngles.z);
    }
}