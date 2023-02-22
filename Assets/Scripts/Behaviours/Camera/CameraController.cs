using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;

    Transform Target;

    readonly int zoomSpeed = 1;
    readonly int dragSpeed = 10;

    Vector3 pevMousePosition = Vector3.zero;

    public void Follow(Transform player)
    {
        Target = player;
    }

    public void Unfollow()
    {
        Target = null;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    void Start()
    {
        StartCoroutine(ObjectRotate());
    }

    void FixedUpdate()
    {
        if (Target == null)
        {
            return;
        }

        // Follow the creature.
        UpdateHolderPosition();
        UpdateCameraRotation();
    }

    void UpdateCameraRotation()
    {
        // Camera has to look directly on the target!
        // This solution is smooth unlike the LookAt function.
        Vector3 relativePos = Target.position - Camera.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);

        Quaternion smoothedRotation = Quaternion.Lerp(
            Camera.transform.rotation, toRotation, 0.125f);

        Camera.transform.rotation = smoothedRotation;
    }

    void UpdateHolderPosition()
    {
        // Holder position change
        Vector3 nextPosition = Vector3.Lerp(
            transform.position, Target.position, 0.5f);

        transform.position = nextPosition;
    }

    void HandleZooming()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        Vector3 cameraPosition = Camera.transform.position;
        Vector3 targetPosition = Target.position;

        float distance = Vector3.Distance(cameraPosition, targetPosition);

        // Zoom-in
        if (wheel > 0)
        {
            if (distance < 2) return;
        }
        else
        {
            if (distance > 10) return;
        }

        Vector3 offset = targetPosition - cameraPosition;
        Vector3 translation = offset * wheel * zoomSpeed;
        Camera.transform.Translate(translation, Space.World);
    }

    void HandleDragging()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 mouseDelta = Input.mousePosition - pevMousePosition;
        pevMousePosition = Input.mousePosition;

        float angle = transform.eulerAngles.x;
        angle = angle > 180 ? angle - 360 : angle;

        bool isVerticallyMovable = (mouseDelta.y > 0 && angle < 40)
            || (mouseDelta.y < 0 && angle > 0);

        if (isVerticallyMovable)
        {
            float yAngle = mouseY * dragSpeed;
            transform.Rotate(new Vector3(1, 0, 1), yAngle);
        }

        float xAngle = mouseX * dragSpeed;
        transform.Rotate(new Vector3(0, 1, 0), xAngle, Space.World);
    }

    IEnumerator ObjectRotate()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                HandleDragging();
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                HandleZooming();
            }

            yield return null;
        }
    }
}