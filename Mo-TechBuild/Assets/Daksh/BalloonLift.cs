using System.Collections;
using UnityEngine;

public class BalloonLift : MonoBehaviour
{
    public float liftSpeed = 2f; // Speed at which the balloon rises
    public float delay = 2f; // Delay before the balloon starts rising
    private bool isActivated = false; // Whether the balloon should start lifting
    private Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is the one interacting with the trigger
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            playerTransform = other.transform;

            // Make the player a child of the balloon to move with it
            playerTransform.SetParent(transform);

            // Optionally, reset the player's position to stay on the balloon
            playerTransform.localPosition = Vector3.zero; // Lock to the balloon's position

            // Start lifting the balloon and its child after the delay
            StartCoroutine(StartLift());
        }
    }

    private IEnumerator StartLift()
    {
        yield return new WaitForSeconds(delay); // Wait for the delay before lifting

        while (isActivated)
        {
            // Move the balloon and its child upwards
            transform.position += Vector3.up * liftSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the player as a child of the balloon when they exit the trigger
            playerTransform.SetParent(null);
        }
    }
}