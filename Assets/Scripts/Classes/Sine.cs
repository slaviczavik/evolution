using UnityEngine;

/*
 * ┌───────────┬────────────┬──────────┐
 * │  sin(x)   │  sin(2πx)  │   y      │
 * ├───────────┼────────────┼──────────┤
 * │  0        │  0.00      │   0      │
 * ├───────────┼────────────┼──────────┤
 * │  π / 2    │  0.25      │   1      │
 * ├───────────┼────────────┼──────────┤
 * │  π        │  0.50      │   0      │
 * ├───────────┼────────────┼──────────┤
 * │  3π / 2   │  0.75      │  -1      │
 * ├───────────┼────────────┼──────────┤
 * │  2π       │  1.00      │   0      │
 * └───────────┴────────────┴──────────┘
 */

public class Sine
{
    readonly float torque;
    readonly float frequency;

    public Sine(float torque, float speed)
    {
        // Narrow frequncy from 2PI to 1 for better timing.
        this.frequency = 2 * Mathf.PI * speed;
        this.torque = torque;
    }

    public float Value(float x)
    {
        // y = asin(bx - c) + d
        return Mathf.Sin(frequency * x);
    }

    public float Torque(float x)
    {
        return Value(x) * torque;
    }
}
