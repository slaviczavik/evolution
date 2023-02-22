using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float MaximumHeight = 0;
    public float TimeOnGround = 0;

    LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Terrain");
    }

    void Update()
    {
        float y = transform.position.y;

        if (MaximumHeight < y)
        {
            MaximumHeight = y;
        }
    }

    void FixedUpdate()
    {
        if (IsGrounded())
        {
            TimeOnGround += Time.fixedDeltaTime;
        }
    }

    bool IsGrounded()
    {
        Vector3 scale = transform.localScale;
        Vector3 origin = transform.position;

        Ray ray = new Ray(origin, Vector3.down);

        float maxDistance = (scale.y / 2) + 0.05f;

        bool grounded = Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask);

        #if UNITY_EDITOR
        if (grounded)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        #endif

        return grounded;
    }
}
