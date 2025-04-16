using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Liquid : MonoBehaviour
{
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;

    [SerializeField] float MaxWobble = 0.03f;
    [SerializeField] float WobbleSpeedMove = 1f;
    [SerializeField] float Recovery = 1f;
    [SerializeField] float Thickness = 1f;
    [Range(0, 1)] public float CompensateShapeAmount;

    [SerializeField] Mesh mesh;
    [SerializeField] Renderer rend;

    public ItemCollector itemCollector;
    public int maxSpirits = 10;

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
        float deltaTime = updateMode == UpdateMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
        time += deltaTime;

        if (deltaTime != 0)
        {
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, deltaTime * Recovery);
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, deltaTime * Recovery);

            pulse = 2 * Mathf.PI * WobbleSpeedMove;
            sinewave = Mathf.Lerp(sinewave, Mathf.Sin(pulse * time), deltaTime * Mathf.Clamp(velocity.magnitude + angularVelocity.magnitude, Thickness, 10));

            wobbleAmountX = wobbleAmountToAddX * sinewave;
            wobbleAmountZ = wobbleAmountToAddZ * sinewave;

            velocity = (lastPos - transform.position) / deltaTime;
            angularVelocity = GetAngularVelocity(lastRot, transform.rotation);

            wobbleAmountToAddX += Mathf.Clamp((velocity.x + velocity.y * 0.2f + angularVelocity.z + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + velocity.y * 0.2f + angularVelocity.x + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
        }

        rend.sharedMaterial.SetFloat("_WobbleX", wobbleAmountX);
        rend.sharedMaterial.SetFloat("_WobbleZ", wobbleAmountZ);

        UpdatePos(deltaTime);

        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void UpdatePos(float deltaTime)
    {
        float fillAmount = 0f;

        if (itemCollector != null && maxSpirits > 0)
        {
            float spiritRatio = Mathf.Clamp01((float)itemCollector.currentSpiritsAvailable / maxSpirits);
            fillAmount = 1f - spiritRatio; // Inverted: more spirits = more liquid
        }

        Vector3 worldPos = transform.TransformPoint(mesh.bounds.center);

        if (CompensateShapeAmount > 0)
        {
            comp = deltaTime != 0
                ? Vector3.Lerp(comp, worldPos - new Vector3(0, GetLowestPoint(), 0), deltaTime * 10)
                : worldPos - new Vector3(0, GetLowestPoint(), 0);

            pos = worldPos - transform.position - new Vector3(0, fillAmount - comp.y * CompensateShapeAmount, 0);
        }
        else
        {
            pos = worldPos - transform.position - new Vector3(0, fillAmount, 0);
        }

        rend.sharedMaterial.SetVector("_FillAmount", pos);
    }

    Vector3 GetAngularVelocity(Quaternion previousRotation, Quaternion currentRotation)
    {
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);

        if (Mathf.Abs(deltaRotation.w) > 1023.5f / 1024.0f)
            return Vector3.zero;

        float angle = Mathf.Acos(deltaRotation.w < 0 ? -deltaRotation.w : deltaRotation.w);
        float gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);

        Vector3 angularVelocity = new Vector3(deltaRotation.x * gain, deltaRotation.y * gain, deltaRotation.z * gain);
        return float.IsNaN(angularVelocity.z) ? Vector3.zero : angularVelocity;
    }

    float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3[] vertices = mesh.vertices;

        foreach (var vertex in vertices)
        {
            Vector3 worldVertex = transform.TransformPoint(vertex);
            if (worldVertex.y < lowestY)
                lowestY = worldVertex.y;
        }

        return lowestY;
    }
}
