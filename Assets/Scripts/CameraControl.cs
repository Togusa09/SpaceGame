using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float panSpeed = 10.0f;
    public float rotateSpeed = 1.0f;
    public float verticalSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveNorth()
    {
        panMovement += Vector3.forward * panSpeed * Time.deltaTime;
    }

    public void MoveSouth()
    {
        panMovement += Vector3.back * panSpeed * Time.deltaTime;
    }

    public void MoveWest()
    {
        panMovement += Vector3.left * panSpeed * Time.deltaTime;
    }

    public void MoveEast()
    {
        panMovement += Vector3.right * panSpeed * Time.deltaTime;
    }

    public void MoveVertical(float distance)
    {
        //var verticalScroll = Input.mouseScrollDelta.y * verticalSpeed;
        panMovement += Vector3.up * (distance * verticalSpeed);
    }

    public void Rotate(float rotation)
    {
        rotationMovement = rotation;
    }

    private Vector3 _lastMousePosition;

    // Update is called once per frame
    void Update()
    {
        var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        var angle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up);

        panMovement = Quaternion.AngleAxis(angle, Vector3.up) * panMovement;

        transform.Translate(panMovement, Space.World);
        transform.Rotate(Vector3.up, rotationMovement * rotateSpeed, Space.World);

        rotationMovement = 0;
        panMovement = Vector3.zero;
    }

    private Vector3 panMovement;
    private float rotationMovement;

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, panMovement * 100);
    }
}
