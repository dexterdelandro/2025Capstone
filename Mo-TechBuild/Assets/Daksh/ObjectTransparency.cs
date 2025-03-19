using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class ObjectTransparency : MonoBehaviour
{
    public float fadeDuration = 2f; // Time taken to fully appear
    public float activationRange = 5f; // Range within which object appears
    private Material material;
    private Color originalColor;
    private float transparency = 0f;
    private bool isFading = false;
    public Transform player;
    private MeshCollider meshCollider; // Reference to MeshCollider

    public NavMeshObstacle blocker;

    void Start()
    {
        //blocker = GetComponentInChildren<NavMeshObstacle>();
        // Get the material of the object
        material = GetComponent<Renderer>().material;
        originalColor = material.color;

        // Get the MeshCollider component
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.enabled = false; // Initially disable the collider
        }

        // Set the object to be fully transparent initially
        SetTransparency(0f);

        // Find the player
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check if player is within range
        if (player != null && Vector3.Distance(transform.position, player.position) <= activationRange)
        {
            // Check for 'P' key press
            if (Input.GetKeyDown(KeyCode.P) && !isFading)
            {
                Debug.Log("WOrks");
                StartCoroutine(FadeIn());
            }
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            transparency = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetTransparency(transparency);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetTransparency(1f);

        // Enable MeshCollider once fully visible
        if (meshCollider != null)
        {
            meshCollider.enabled = true;
        }

        isFading = false;

        blocker.gameObject.SetActive(false);
    }

    private void SetTransparency(float alpha)
    {
        Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        material.color = newColor;

        // Ensure the material supports transparency
        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", newColor);
        }
        if (material.HasProperty("_BaseColor")) // For HDRP/URP materials
        {
            material.SetColor("_BaseColor", newColor);
        }
    }
}
