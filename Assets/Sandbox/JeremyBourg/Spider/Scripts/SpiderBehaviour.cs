using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    public Transform crystal;
    public NavMeshAgent agent;  // NavMeshAgent for pathfinding
    public float distanceToTriggerJump = 1f;
    public float jumpHeight = 5f;
    public float jumpDuration = 2f;
    public float explosionRadius = 2f;
    public float damage = 50f;

    private bool isJumping = false;
    private Vector3 jumpStartPosition;
    private Vector3 jumpEndPosition;
    private float jumpStartTime;

    void Start()
    {
        // Set the NavMeshAgent's destination to the crystal position
        agent.SetDestination(crystal.position);
    }

    void Update()
    {
        if (isJumping)
        {
            PerformJump();
        }

        // Check if we're close enough to the crystal to start the jump
        if (!isJumping && Vector3.Distance(transform.position, crystal.position) <= distanceToTriggerJump)
        {
            StartJump();
        }
    }

    void StartJump()
    {
        isJumping = true;
        agent.isStopped = true;  // Stop the NavMeshAgent when jumping
        jumpStartPosition = transform.position;
        jumpEndPosition = crystal.position;
        jumpStartTime = Time.time;
    }

    void PerformJump()
    {
        float timeElapsed = (Time.time - jumpStartTime) / jumpDuration;
        float height = Mathf.Sin(timeElapsed * Mathf.PI) * jumpHeight;
        Vector3 newPosition = Vector3.Lerp(jumpStartPosition, jumpEndPosition, timeElapsed);
        newPosition.y += height;

        transform.position = newPosition;

        if (timeElapsed >= 1f)
        {
            isJumping = false;
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        // Apply explosion effect to the crystal
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Crystal"))
            {
                Debug.Log("Damage dealt!");
            }
        }

        Destroy(gameObject);  // Destroy the spider after explosion
    }
}
