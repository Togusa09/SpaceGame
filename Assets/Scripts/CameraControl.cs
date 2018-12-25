using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float panSpeed = 1.0f;
    public float rotateSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private Vector3 lastMousePosition;

    // Update is called once per frame
    void Update()
    {
        panMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            panMovement += Vector3.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            panMovement += Vector3.back * panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            panMovement += Vector3.left * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            panMovement += Vector3.right * panSpeed * Time.deltaTime;
        }

        var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        var angle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up);

        panMovement = Quaternion.AngleAxis(angle, Vector3.up) * panMovement;

        var verticalScroll = Input.mouseScrollDelta.y;
        panMovement += Vector3.up * verticalScroll;
        


        transform.Translate(panMovement, Space.World);

        if (Input.GetMouseButton(2)) // Right mouse button down
        {
            var rotation = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, rotation * rotateSpeed, Space.World);
        }
    }

    private Vector3 panMovement;

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, panMovement * 100);
    }
}
