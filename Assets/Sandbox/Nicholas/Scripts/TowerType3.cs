using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerType3 : MonoBehaviour
{
    [SerializeField] private string targetObjectName; // Name of the target GameObject
    [SerializeField] private float moveSpeed = 5f; // Speed of the moving GameObject
    [SerializeField] private float proximityThreshold = 0.5f; // Distance to trigger the return
    [SerializeField] private float returnDelay = 5f; // Delay after returning to the initial position

    private Vector3 initialPosition; // Store the initial position of the parent GameObject
    private GameObject targetObject; // Reference to the target GameObject

    private bool isReturning = false; // Flag to indicate if the bullet is returning to its initial position
    private bool isWaiting = false; // Flag to check if the bullet is in the waiting state

    void Start()
    {
        // Save the initial position of the parent GameObject
        initialPosition = transform.position;

        // Find the target GameObject by name
        targetObject = GameObject.Find(targetObjectName);

        if (targetObject == null)
        {
            Debug.LogError($"Target GameObject with name '{targetObjectName}' not found.");
        }
    }

    void Update()
    {
        // Continuously update the targetObject reference in case it moves
        if (targetObject == null)
        {
            targetObject = GameObject.Find(targetObjectName);

            if (targetObject == null)
            {
                Debug.LogWarning($"Target GameObject with name '{targetObjectName}' not found during update.");
                return;
            }
        }

        // If the bullet is returning, teleport it directly to the initial position and start the wait timer
        if (isReturning && !isWaiting)
        {
            transform.position = initialPosition; // Teleport instantly
            isReturning = false; // Stop returning after teleport
            StartCoroutine(WaitAfterReturn()); // Start the wait timer
        }
        else if (!isWaiting)
        {
            // Move towards the target until within the proximity threshold
            if (targetObject != null)
            {
                // Calculate the step size based on the moveSpeed and frame time
                float step = moveSpeed * Time.deltaTime;

                // Move towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step);

                // Trigger return if within proximity of the target
                if (Vector3.Distance(transform.position, targetObject.transform.position) <= proximityThreshold)
                {
                    isReturning = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object matches the target object
        if (collision.gameObject == targetObject)
        {
            Debug.Log("Collision detected with target object!");
            // Start returning to the initial position
            isReturning = true;
        }
    }

    private IEnumerator WaitAfterReturn()
    {
        isWaiting = true; // Set the waiting flag to true

        // Wait for the specified delay time (5 seconds)
        yield return new WaitForSeconds(returnDelay);

        isWaiting = false; // Stop waiting after the delay
    }
}