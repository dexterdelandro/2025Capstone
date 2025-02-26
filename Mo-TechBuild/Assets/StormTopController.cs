using UnityEngine;
using StarterAssets;

public class StormTopController : MonoBehaviour
{
    public float floatForce = 0.3f; // Small floating force to prevent falling
    public float movementDamping = 0.1f; // Damping for smooth movement at the storm top

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            StarterAssetsInputs input = other.GetComponent<StarterAssetsInputs>();

            if (controller != null && input != null)
            {
                // Apply a small upward force to maintain floating
                controller.Move(Vector3.up * floatForce * Time.deltaTime);

                // Allow the player to move freely at the top
                Vector3 freeMove = new Vector3(input.move.x, 0, input.move.y) * movementDamping;
                controller.Move(freeMove);
            }
        }
    }
}
