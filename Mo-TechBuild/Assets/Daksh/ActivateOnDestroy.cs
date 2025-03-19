using UnityEngine;

public class ActivateOnDestroy : MonoBehaviour
{
    [Tooltip("The GameObject to monitor for destruction.")]
    public GameObject targetObject;

    [Tooltip("The script to activate when the target is destroyed.")]
    public MonoBehaviour scriptToActivate;

    [Tooltip("The MeshCollider to modify when the target is destroyed.")]
    public MeshCollider meshColliderToModify;

    [Tooltip("The Particle System to enable when the target is destroyed.")]
    public ParticleSystem particleSystemToEnable;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("No target object assigned!");
        }
        if (scriptToActivate == null)
        {
            Debug.LogError("No script assigned to activate!");
        }
        else
        {
            scriptToActivate.enabled = false; // Ensure the script is disabled initially
        }
        if (meshColliderToModify == null)
        {
            Debug.LogError("No MeshCollider assigned!");
        }
        if (particleSystemToEnable == null)
        {
            Debug.LogError("No Particle System assigned!");
        }
        else
        {
            particleSystemToEnable.Stop(); // Ensure the particle system is initially off
        }
    }

    private void Update()
    {
        if (targetObject == null) // Object has been destroyed
        {
            Debug.Log("Target object destroyed. Activating script, modifying MeshCollider, and enabling Particle System.");
            if (scriptToActivate != null)
            {
                scriptToActivate.enabled = true;
            }
            if (meshColliderToModify != null)
            {
                meshColliderToModify.convex = true;
                meshColliderToModify.isTrigger = true;
            }
            if (particleSystemToEnable != null)
            {
                particleSystemToEnable.Play();
            }
            Destroy(this); // Optional: Destroy this script to prevent unnecessary updates
        }
    }
}
