using UnityEngine;
using StarterAssets;

public class StormController : MonoBehaviour
{
    public float liftSpeed = 3f; // Vertical lift speed
    public float horizontalForce = 2f; // Horizontal force to prevent player from being sucked to the center
    public float stormTopHeight = 15f; // Maximum height of the storm
    public float stopThreshold = 1f; // Threshold for stopping at the top
    public float exitSlowFallSpeed = 2f; // Smooth falling speed after exiting the storm

    private bool playerAtTop = false; // Tracks if the player has reached the top
    private bool playerInStorm = false; // Tracks if the player is inside the storm

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInStorm = true; // Mark player as inside the storm
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            StarterAssetsInputs input = other.GetComponent<StarterAssetsInputs>();

            if (controller != null && input != null)
            {
                Vector3 stormCenter = transform.position;
                Vector3 playerPos = other.transform.position;

                // Calculate the target height at the top of the storm
                float targetHeight = stormCenter.y + stormTopHeight;
                bool atTop = playerPos.y >= targetHeight - stopThreshold;

                if (!atTop)
                {
                    // Continue lifting the player but do not restrict movement
                    Vector3 moveDirection = (Vector3.up * liftSpeed);
                    controller.Move(moveDirection * Time.deltaTime);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                playerAtTop = false; // Reset state
                playerInStorm = false; // Mark player as outside the storm

                // Smooth falling after exiting the storm
                controller.Move(Vector3.down * exitSlowFallSpeed * Time.deltaTime);

                Debug.Log("Player left the storm, restoring normal state.");
            }
        }
    }

    public bool IsPlayerInStorm()
    {
        return playerInStorm;
    }
}
