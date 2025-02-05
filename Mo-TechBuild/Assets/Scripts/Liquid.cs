using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Liquid : MonoBehaviour
{
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;

    [SerializeField]
    float MaxWobble = 0.03f;
    [SerializeField]
    float WobbleSpeedMove = 1f;
    [SerializeField]
    float Recovery = 1f;
    [SerializeField]
    float Thickness = 1f;
    [Range(0, 1)]
    public float CompensateShapeAmount;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Renderer rend;

    private Vector3 pos;
    private Vector3 lastPos;
    private Vector3 velocity;
    private Quaternion lastRot;
    private Vector3 angularVelocity;
    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float pulse;
    private float sinewave;
    private float time = 0.5f;
    private Vector3 comp;

    private int collectedSpheres = 0; // Count of spheres collected
    private float fillAmount = 1.0f; // Start with a full jar

    // Start is called before the first frame update
    void Start()
    {
        GetMeshAndRend();
    }

    private void OnValidate()
    {
        GetMeshAndRend();
    }

    void GetMeshAndRend()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
    }

    void Update()
    {
        float deltaTime = 0;
        switch (updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;
            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        time += deltaTime;

        if (deltaTime != 0)
        {
            // Decrease wobble over time
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, (deltaTime * Recovery));
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, (deltaTime * Recovery));

            // Make a sine wave of the decreasing wobble
            pulse = 2 * Mathf.PI * WobbleSpeedMove;
            sinewave = Mathf.Lerp(sinewave, Mathf.Sin(pulse * time), deltaTime * Mathf.Clamp(velocity.magnitude + angularVelocity.magnitude, Thickness, 10));

            wobbleAmountX = wobbleAmountToAddX * sinewave;
            wobbleAmountZ = wobbleAmountToAddZ * sinewave;

            // Calculate velocity
            velocity = (lastPos - transform.position) / deltaTime;
            angularVelocity = GetAngularVelocity(lastRot, transform.rotation);

            // Add clamped velocity to wobble
            wobbleAmountToAddX += Mathf.Clamp((velocity.x + (velocity.y * 0.2f) + angularVelocity.z + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (velocity.y * 0.2f) + angularVelocity.x + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
        }

        // Send wobble values to shader
        rend.sharedMaterial.SetFloat("_WobbleX", wobbleAmountX);
        rend.sharedMaterial.SetFloat("_WobbleZ", wobbleAmountZ);

        // Update fill amount position
        UpdatePos(deltaTime);

        // Keep last position for next frame
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void UpdatePos(float deltaTime)
    {
        Vector3 worldPos = transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));
        if (CompensateShapeAmount > 0)
        {
            // Only lerp if not paused/normal update
            if (deltaTime != 0)
            {
                comp = Vector3.Lerp(comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            }
            else
            {
                comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));
            }

            pos = worldPos - transform.position - new Vector3(0, fillAmount - (comp.y * CompensateShapeAmount), 0);
        }
        else
        {
            pos = worldPos - transform.position - new Vector3(0, fillAmount, 0);
        }

        // Update shader with new fill amount
        rend.sharedMaterial.SetVector("_FillAmount", pos);
    }

    // This function gets called when a sphere is collected
    public void UpdateFillAmount()
    {
        collectedSpheres++;
        fillAmount = Mathf.Clamp(1.0f - (collectedSpheres * 0.1f), 0f, 1.0f); // Decrease by 10% per sphere collected

        // Update shader
        rend.sharedMaterial.SetVector("_FillAmount", new Vector3(0, fillAmount, 0));
    }

    // Calculates angular velocity (used for wobble)
    Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);

        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;

        float gain;
        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }

        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);
        return float.IsNaN(angularVelocity.z) ? Vector3.zero : angularVelocity;
    }

    // Find the lowest point in the mesh
    float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = transform.TransformPoint(vertices[i]);
            if (position.y < lowestY)
            {
                lowestY = position.y;
            }
        }
        return lowestY;
    }
}
