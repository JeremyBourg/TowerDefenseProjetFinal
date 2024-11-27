using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        public Transform crystalTarget; // Cible du cristal
        public NavMeshAgent agent; // L'agent NavMesh
        public float speed = 5f; // Vitesse de d�placement
        public float jumpForce = 5f; // Force du saut (l�vitation)
        public float levitationHeight = 2f; // Hauteur � atteindre pour l�viter au-dessus de l'obstacle
        public float stuckThreshold = 1f; // Seuil pour d�tecter si l'agent est coinc�

        private bool isLevitating = false; // Est-ce que l'araign�e est en train de l�viter ?
        private bool isAttachedToObstacle = false; // Est-ce que l'araign�e est attach�e � un obstacle ?
        private float lastDistanceToDestination = 0f; // Distance entre l'agent et sa destination lors de la derni�re mise � jour
        private float stuckTime = 0f; // Temps �coul� depuis que l'agent est coinc�
        private Mimic myMimic;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            crystalTarget = GameObject.FindGameObjectWithTag("Crystal").transform; // R�cup�rer le cristal par son tag
            agent = GetComponent<NavMeshAgent>(); // R�cup�rer le NavMeshAgent
            agent.speed = speed; // Assigner la vitesse � l'agent
            agent.stoppingDistance = 1f; // Distance de stop � atteindre avant de consid�rer que l'agent est arriv�
            agent.SetDestination(crystalTarget.position); // D�finir la destination vers le cristal
        }

        void Update()
        {
            // V�rifier si l'agent est bloqu�
            CheckIfStuck();

            // Si l'agent est coinc� et n'arrive pas � avancer, on le fait l�viter
            if (isLevitating)
            {
                HandleLevitatingMovement();
            }
            else
            {
                // Si l'agent n'est pas coinc�, continue de se diriger vers le cristal
                agent.SetDestination(crystalTarget.position);
            }

            // V�rifier si l'agent est suffisamment proche du cristal pour s'attacher
            if (Vector3.Distance(transform.position, crystalTarget.position) < 1f && !isAttachedToObstacle)
            {
                AttachToCrystal();
            }
        }

        // V�rifier si l'agent est coinc� par un obstacle
        void CheckIfStuck()
        {
            float currentDistanceToDestination = Vector3.Distance(transform.position, crystalTarget.position);

            // Si la distance n'a pas chang� depuis un certain temps, l'agent est probablement coinc�
            if (Mathf.Abs(lastDistanceToDestination - currentDistanceToDestination) < stuckThreshold)
            {
                stuckTime += Time.deltaTime;
            }
            else
            {
                stuckTime = 0f;
            }

            lastDistanceToDestination = currentDistanceToDestination;

            // Si l'agent est coinc� pendant plus d'une seconde, on le fait l�viter
            if (stuckTime > 1f && !isLevitating)
            {
                isLevitating = true;
                agent.isStopped = true; // Stopper l'agent
            }
        }

        // Gestion du mouvement en l�vitation
        void HandleLevitatingMovement()
        {
            // D�placement vers la position au-dessus de l'obstacle (dans le cas d'une table, par exemple)
            Vector3 levitationTarget = new Vector3(transform.position.x, levitationHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, levitationTarget, jumpForce * Time.deltaTime);

            // Si la position de l�vitation est atteinte, on commence � marcher dessus
            if (Vector3.Distance(transform.position, levitationTarget) < 0.1f)
            {
                isAttachedToObstacle = true; // L'araign�e est attach�e � l'obstacle
                isLevitating = false; // Arr�ter la l�vitation
                agent.isStopped = false; // Relancer l'agent pour qu'il marche sur l'obstacle
                agent.SetDestination(crystalTarget.position); // Reprendre la destination vers le cristal
            }
        }

        // Lorsque le Mimic atteint le cristal, il se fixe dessus
        void AttachToCrystal()
        {
            Debug.Log("Mimic attach� au cristal !");
            isAttachedToObstacle = true;

            // Logique d'explosion ou autre ici
            Explode();
        }

        // Fonction d'explosion (exemple)
        void Explode()
        {
            Debug.Log("Explosion! Le cristal prend des d�g�ts!");

            // Tu peux ici ajouter des effets visuels, sonores ou appliquer des d�g�ts au cristal
            // Exemple: ExplosionEffect.Play();

            // D�truire l'objet Mimic apr�s l'explosion
            Destroy(gameObject);
        }
    }
}
