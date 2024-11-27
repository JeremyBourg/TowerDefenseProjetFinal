using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.01f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        public float jumpForce = 5f; // Force of the jump
        public float gravity = -9.81f; // Gravity force
        public float jumpHeight = 1.5f; // Max jump height to reach the crystal
        public Transform crystalTarget; // Target crystal position

        private bool isJumping = false;
        private float currentJumpHeight = 0f;
        private Mimic myMimic;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
        }

        void Update()
        {
            Vector3 targetDirection = (crystalTarget.position - transform.position).normalized;
            float distanceToCrystal = Vector3.Distance(transform.position, crystalTarget.position);
            bool isAboveCrystal = transform.position.y >= crystalTarget.position.y;

            // Calculate horizontal velocity towards the crystal
            velocity = Vector3.Lerp(velocity, new Vector3(targetDirection.x, 0, targetDirection.z).normalized * speed, velocityLerpCoef * Time.deltaTime);

            // If we are close enough and below the crystal, start jumping
            if (!isJumping && distanceToCrystal < 2f && !isAboveCrystal)
            {
                isJumping = true;
                currentJumpHeight = 0f;
            }

            if (isJumping)
            {
                // Jump Logic: Increase the height until it reaches the target jump height
                currentJumpHeight += jumpForce * Time.deltaTime;
                if (currentJumpHeight >= jumpHeight)
                {
                    isJumping = false;
                    currentJumpHeight = jumpHeight;
                }

                velocity.y = jumpForce * Time.deltaTime; // Apply jump velocity
            }
            else
            {
                // Apply gravity if not jumping
                velocity.y = gravity * Time.deltaTime;
            }

            // Update Mimic's velocity for leg placement
            myMimic.velocity = velocity;

            // Update position with new velocity
            transform.position = transform.position + velocity * Time.deltaTime;

            // Keep the Mimic at the correct height from the ground
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);

            // Check for collision with the crystal (explosion trigger)
            if (Vector3.Distance(transform.position, crystalTarget.position) < 1f)
            {
                Explode();
            }
        }

        void Explode()
        {
            // Here, you can add your explosion logic to damage the crystal
            Debug.Log("Explosion! Crystal takes damage!");
            // You can also add visual effects or sound here

            // Destroy or disable the mimic after explosion
            Destroy(gameObject);
        }
    }
}
