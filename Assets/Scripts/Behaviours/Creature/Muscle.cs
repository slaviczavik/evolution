using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscle : MonoBehaviour
{
    Rigidbody rb;
    ConfigurableJoint joint;
    float time = 0;

    readonly Dictionary<Axis, Sine> movements = new Dictionary<Axis, Sine>();

    public void SetJoint(GameObject parent, Vector3 anchor, Movement movement)
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = parent.GetComponent<Rigidbody>();
        joint.anchor = anchor;

        joint.axis = new Vector3(1, 0, 0);
        joint.secondaryAxis = new Vector3(0, 1, 0);

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.enableCollision = true;
        joint.projectionMode = JointProjectionMode.PositionAndRotation;

        AxisLimit limitX = movement.X;
        AxisLimit limitY = movement.Y;
        AxisLimit limitZ = movement.Z;

        SetXAxis(limitX);
        SetYAxis(limitY);
        SetZAxis(limitZ);

        SetMovement(Axis.x, limitX);
        SetMovement(Axis.y, limitY);
        SetMovement(Axis.z, limitZ);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float x = GetTorque(Axis.x, time);
        float y = GetTorque(Axis.y, time);
        float z = GetTorque(Axis.z, time);

        rb.AddRelativeTorque(new Vector3(x, y, z));

        time += Time.fixedDeltaTime;
    }

    float GetTorque(Axis axis, float time)
    {
        if (!movements.ContainsKey(axis))
        {
            return transform.rotation[(int)axis];
        }

        Sine sine = movements[axis];

        return sine.Torque(time);
    }

    void SetMovement(Axis axis, AxisLimit axisLimit)
    {
        if (axisLimit.enabled)
        {
            float length = transform.localScale[(int)axis];
            float limit = (length / 2) * axisLimit.force;
            movements[axis] = new Sine(limit, 1.0f);
        }
    }

    // Movement freedom.

    void SetXAxis(AxisLimit axisLimit)
    {
        if (!axisLimit.enabled)
        {
            joint.angularXMotion = ConfigurableJointMotion.Locked;

            return;
        }

        SoftJointLimit lowLimit = joint.lowAngularXLimit;
        SoftJointLimit highLimit = joint.highAngularXLimit;

        lowLimit.limit = axisLimit.angle * -1;
        highLimit.limit = axisLimit.angle;

        joint.lowAngularXLimit = lowLimit;
        joint.highAngularXLimit = highLimit;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
    }

    void SetYAxis(AxisLimit axisLimit)
    {
        if (!axisLimit.enabled)
        {
            joint.angularYMotion = ConfigurableJointMotion.Locked;

            return;
        }

        SoftJointLimit jointLimit = joint.angularYLimit;
        jointLimit.limit = axisLimit.angle;

        joint.angularYLimit = jointLimit;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
    }

    void SetZAxis(AxisLimit axisLimit)
    {
        if (!axisLimit.enabled)
        {
            joint.angularZMotion = ConfigurableJointMotion.Locked;

            return;
        }

        SoftJointLimit jointLimit = joint.angularZLimit;
        jointLimit.limit = axisLimit.angle;

        joint.angularZLimit = jointLimit;
        joint.angularZMotion = ConfigurableJointMotion.Limited;
    }
}
