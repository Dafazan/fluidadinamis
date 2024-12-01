using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bernouli : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField area1Input;  // Input field for A1
    public TMP_InputField area2Input;  // Input field for A2
    public TMP_InputField height1Input;  // Input field for H1
    public TMP_InputField height2Input;  // Input field for H2
    public TMP_InputField initialVelocityInput;  // Input field for initial velocity (v1)

    [Header("Output Fields")]
    public TMP_Text velocity1Output;  // Display for velocity1
    public TMP_Text velocity2Output;  // Display for velocity2

    [Header("Constants")]
    public float fluidDensity = 1000f;  // Fluid density (water = 1000 kg/m^3)
    public float gravity = 9.81f;  // Gravitational acceleration

    [Header("Objects to Scale")]
    public Transform object1;  // Object affected by area1Input
    public Transform object2;  // Object affected by area2Input


    public LineRenderer line; // Reference to the LineRenderer component

    void Start()
    {
        if (line == null)
        {
            Debug.LogError("LineRenderer reference is missing! Please assign it in the Inspector.");
            return;
        }

        line.positionCount = 2;
    }

    void Update()
    {
        if (line != null && object1 != null && object2 != null)
        {
            line.SetPosition(0, object1.position);
            line.SetPosition(1, object2.position);
        }
    }

    void UpdateLineWidth()
    {
        // Parse input fields for scale values
        float scale1Value = 1f;
        float scale2Value = 1f;

        if (float.TryParse(area1Input.text, out float parsedScale1))
        {
            scale1Value = parsedScale1;
        }
        if (float.TryParse(area2Input.text, out float parsedScale2))
        {
            scale2Value = parsedScale2;
        }

        // Update the LineRenderer width using an AnimationCurve
        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0f, scale1Value); // Start width
        widthCurve.AddKey(1f, scale2Value); // End width

        line.widthCurve = widthCurve;
    }

    private void UpdateScales()
    {
        // Update the scale of object1 based on area1Input
        if (float.TryParse(area1Input.text, out float scale1Value))
        {
            if (object1 != null)
            {
                Vector3 scale1 = object1.localScale;
                scale1.x = scale1Value;  // Set the Y scale
                scale1.z = scale1Value;  // Set the Z scale
                object1.localScale = scale1;  // Apply the new scale to object1
            }
        }

        // Update the scale of object2 based on area2Input
        if (float.TryParse(area2Input.text, out float scale2Value))
        {
            if (object2 != null)
            {
                Vector3 scale2 = object2.localScale;
                scale2.x = scale2Value;  // Set the Y scale
                scale2.z = scale2Value;  // Set the Z scale
                object2.localScale = scale2;  // Apply the new scale to object2
            }
        }
    }

    void UpdatePositions()
    {
        // Update the Y position of object1 based on height1Input
        if (float.TryParse(height1Input.text, out float height1Value))
        {
            if (object1 != null)
            {
                Vector3 position1 = object1.position;
                position1.y = height1Value;  // Set the Y position
                object1.position = position1;  // Apply the new position to object1
            }
        }

        // Update the Y position of object2 based on height2Input
        if (float.TryParse(height2Input.text, out float height2Value))
        {
            if (object2 != null)
            {
                Vector3 position2 = object2.position;
                position2.y = height2Value;  // Set the Y position
                object2.position = position2;  // Apply the new position to object2
            }
        }
    }


    public void CalculateVelocities()
    {
        // Parse inputs
        if (!float.TryParse(area1Input.text, out float area1) || area1 <= 0)
        {
            velocity1Output.text = "Invalid A1";
            velocity2Output.text = "Invalid A1";
            return;
        }
        if (!float.TryParse(area2Input.text, out float area2) || area2 <= 0)
        {
            velocity1Output.text = "Invalid A2";
            velocity2Output.text = "Invalid A2";
            return;
        }
        if (!float.TryParse(height1Input.text, out float height1))
        {
            velocity1Output.text = "Invalid H1";
            velocity2Output.text = "Invalid H1";
            return;
        }
        if (!float.TryParse(height2Input.text, out float height2))
        {
            velocity1Output.text = "Invalid H2";
            velocity2Output.text = "Invalid H2";
            return;
        }
        if (!float.TryParse(initialVelocityInput.text, out float initialVelocity))
        {
            initialVelocity = 0;  // Default to 0 if not provided
        }

        // Calculate velocities
        float heightDifference = height1 - height2;
        float v1 = initialVelocity;

        // Bernoulli: v2^2 = v1^2 + 2 * g * (h1 - h2)
        float v2Squared = v1 * v1 + 2 * gravity * heightDifference;

        if (v2Squared < 0)
        {
            velocity1Output.text = "Error: Unrealistic inputs.";
            velocity2Output.text = "Error: Unrealistic inputs.";
            return;
        }

        float velocity1 = v1;  // Initial velocity
        float velocity2 = Mathf.Sqrt(v2Squared);  // Calculated velocity

        // Update UI
        velocity1Output.text = $"v1: {velocity1:F2} m/s";
        velocity2Output.text = $"v2: {velocity2:F2} m/s";

        // Update scales
        UpdateScales();
        UpdateLineWidth();
        UpdatePositions();
    }
}
