using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.01f, 5f)]
        public float height = 0.8f; // Hauteur du corps par rapport au sol
        public float speed = 5f; // Vitesse de d�placement
        public float velocityLerpCoef = 4f; // Coefficient de lissage de la v�locit�
        public float jumpForce = 5f; // Force du saut
        public float gravity = -9.81f; // Force de la gravit�
        public float jumpHeight = 1.5f; // Hauteur maximale du saut pour atteindre le cristal
        public Transform crystalTarget; // Position du cristal cible
        public NavMeshAgent agent; // L'agent de navigation (NavMeshAgent)

        private bool isJumping = false; // Est-ce que le Mimic est en train de sauter ?
        private bool isLevitating = false; // Est-ce que le Mimic est en train de l�viter ?
        private bool isAttached = false; // Est-ce que le Mimic est attach� au cristal ?
        private float currentJumpHeight = 0f; // Hauteur actuelle du saut
        private Mimic myMimic; // R�f�rence au Mimic pour contr�ler les jambes et la vitesse

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            crystalTarget = GameObject.FindGameObjectWithTag("Crystal").transform; // Chercher le cristal par son tag
            agent = GetComponent<NavMeshAgent>(); // R�cup�rer le NavMeshAgent
            agent.stoppingDistance = 1f; // D�finir la distance � laquelle l'agent s'arr�te pr�s du cristal
            agent.speed = speed; // Assigner la vitesse du NavMeshAgent
        }

        void Update()
        {
            // Calculer la direction horizontale vers le cristal
            Vector3 targetDirection = (crystalTarget.position - transform.position).normalized;
            float distanceToCrystal = Vector3.Distance(transform.position, crystalTarget.position);
            bool isAboveCrystal = transform.position.y >= crystalTarget.position.y;

            // V�rifier si un obstacle bloque le Mimic
            RaycastHit hit;
            bool hitObstacle = Physics.Raycast(transform.position, targetDirection, out hit, 2f);

            // Si un obstacle est d�tect� et que le Mimic est proche du cristal, il va commencer � l�viter et s'attacher
            if (hitObstacle && !isAttached)
            {
                StartLevitating(hit.collider.transform);
            }

            // Si on est proche du cristal et qu'on n'est pas d�j� attach�, commence le saut
            if (!isJumping && !hitObstacle && distanceToCrystal < 2f && !isAboveCrystal)
            {
                isJumping = true;
                currentJumpHeight = 0f;
                agent.isStopped = true; // Arr�ter l'agent de se d�placer pendant le saut
            }

            // Si le Mimic est en train de sauter
            if (isJumping)
            {
                // Logique du saut : augmenter la hauteur jusqu'� atteindre la hauteur cible
                currentJumpHeight += jumpForce * Time.deltaTime;

                // Si la hauteur maximale est atteinte, arr�ter le saut
                if (currentJumpHeight >= jumpHeight)
                {
                    isJumping = false;
                    currentJumpHeight = jumpHeight;
                    agent.isStopped = false; // Relancer l'agent une fois que le saut est termin�
                }

                // Appliquer le mouvement vertical (saut)
                agent.velocity = new Vector3(agent.velocity.x, jumpForce * Time.deltaTime, agent.velocity.z); // Changer seulement la composante Y (verticale)
            }
            else if (isLevitating)
            {
                // Si on est en l�vitation, on applique un mouvement vertical
                agent.velocity = new Vector3(agent.velocity.x, gravity * Time.deltaTime, agent.velocity.z); // On laisse la gravit� fonctionner normalement
            }
            else
            {
                // Sinon, d�placer l'agent horizontalement vers le cristal
                agent.SetDestination(crystalTarget.position);
            }

            // V�rifier si le Mimic a atteint la cible (le cristal) pour d�clencher l'explosion
            if (distanceToCrystal < 1f && !isAttached)
            {
                AttachToCrystal();
            }

            // R�ajuster la hauteur pour s'assurer que le Mimic reste � la bonne hauteur par rapport au sol
            RaycastHit groundHit;
            Vector3 destHeight = transform.position;

            // Utiliser un raycast pour v�rifier la position du sol en dessous du Mimic
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out groundHit))
            {
                destHeight = new Vector3(transform.position.x, groundHit.point.y + height, transform.position.z);
            }

            // Lerp la hauteur pour la rendre plus fluide
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        void StartLevitating(Transform obstacle)
        {
            // Arr�ter le mouvement horizontal
            agent.isStopped = true;

            // D�marrer la l�vitation
            isLevitating = true;
        }

        void AttachToCrystal()
        {
            // Logique pour attacher les jambes au cristal et d�clencher l'explosion
            Debug.Log("Mimic attach� au cristal et pr�t � exploser!");

            isAttached = true;

            // Tu peux ici appeler une fonction pour lancer l'explosion
            Explode();
        }

        void Explode()
        {
            // Logique d'explosion lorsque le Mimic atteint le cristal
            Debug.Log("Explosion! Le cristal prend des d�g�ts!");

            // Tu peux ici ajouter des effets visuels, sonores, ou appliquer des d�g�ts au cristal
            // Exemple: ExplosionEffect.Play();

            // D�truire l'objet Mimic apr�s l'explosion
            Destroy(gameObject);
        }
    }
}
