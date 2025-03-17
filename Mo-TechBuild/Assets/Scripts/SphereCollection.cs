using UnityEngine;

public class SphereCollection : MonoBehaviour
{
    private bool playerNearby = false;
    public Liquid liquid; // Reference to the Liquid script

    private void Start()
    {
        // Find the Liquid object automatically if not set
        if (liquid == null)
        {
            liquid = FindAnyObjectByType<Liquid>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            CollectSphere();
        }
    }

    void CollectSphere()
    {
        if (liquid != null)
        {
            liquid.UpdateFillAmount();
        }

        Destroy(gameObject); // Destroy the sphere when collected
    }
}
