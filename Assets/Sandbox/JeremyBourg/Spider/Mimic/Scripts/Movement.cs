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
        public float speed = 5f; // Vitesse de déplacement
        public float jumpForce = 5f; // Force du saut (lévitation)
        public float levitationHeight = 2f; // Hauteur à atteindre pour léviter au-dessus de l'obstacle
        public float stuckThreshold = 1f; // Seuil pour détecter si l'agent est coincé

        private bool isLevitating = false; // Est-ce que l'araignée est en train de léviter ?
        private bool isAttachedToObstacle = false; // Est-ce que l'araignée est attachée à un obstacle ?
        private float lastDistanceToDestination = 0f; // Distance entre l'agent et sa destination lors de la dernière mise à jour
        private float stuckTime = 0f; // Temps écoulé depuis que l'agent est coincé
        private Mimic myMimic;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            crystalTarget = GameObject.FindGameObjectWithTag("Crystal").transform; // Récupérer le cristal par son tag
            agent = GetComponent<NavMeshAgent>(); // Récupérer le NavMeshAgent
            agent.speed = speed; // Assigner la vitesse à l'agent
            agent.stoppingDistance = 1f; // Distance de stop à atteindre avant de considérer que l'agent est arrivé
            agent.SetDestination(crystalTarget.position); // Définir la destination vers le cristal
        }

        void Update()
        {
            // Vérifier si l'agent est bloqué
            CheckIfStuck();

            // Si l'agent est coincé et n'arrive pas à avancer, on le fait léviter
            if (isLevitating)
            {
                HandleLevitatingMovement();
            }
            else
            {
                // Si l'agent n'est pas coincé, continue de se diriger vers le cristal
                agent.SetDestination(crystalTarget.position);
            }

            // Vérifier si l'agent est suffisamment proche du cristal pour s'attacher
            if (Vector3.Distance(transform.position, crystalTarget.position) < 1f && !isAttachedToObstacle)
            {
                AttachToCrystal();
            }
        }

        // Vérifier si l'agent est coincé par un obstacle
        void CheckIfStuck()
        {
            float currentDistanceToDestination = Vector3.Distance(transform.position, crystalTarget.position);

            // Si la distance n'a pas changé depuis un certain temps, l'agent est probablement coincé
            if (Mathf.Abs(lastDistanceToDestination - currentDistanceToDestination) < stuckThreshold)
            {
                stuckTime += Time.deltaTime;
            }
            else
            {
                stuckTime = 0f;
            }

            lastDistanceToDestination = currentDistanceToDestination;

            // Si l'agent est coincé pendant plus d'une seconde, on le fait léviter
            if (stuckTime > 1f && !isLevitating)
            {
                isLevitating = true;
                agent.isStopped = true; // Stopper l'agent
            }
        }

        // Gestion du mouvement en lévitation
        void HandleLevitatingMovement()
        {
            // Déplacement vers la position au-dessus de l'obstacle (dans le cas d'une table, par exemple)
            Vector3 levitationTarget = new Vector3(transform.position.x, levitationHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, levitationTarget, jumpForce * Time.deltaTime);

            // Si la position de lévitation est atteinte, on commence à marcher dessus
            if (Vector3.Distance(transform.position, levitationTarget) < 0.1f)
            {
                isAttachedToObstacle = true; // L'araignée est attachée à l'obstacle
                isLevitating = false; // Arrêter la lévitation
                agent.isStopped = false; // Relancer l'agent pour qu'il marche sur l'obstacle
                agent.SetDestination(crystalTarget.position); // Reprendre la destination vers le cristal
            }
        }

        // Lorsque le Mimic atteint le cristal, il se fixe dessus
        void AttachToCrystal()
        {
            Debug.Log("Mimic attaché au cristal !");
            isAttachedToObstacle = true;

            // Logique d'explosion ou autre ici
            Explode();
        }

        // Fonction d'explosion (exemple)
        void Explode()
        {
            Debug.Log("Explosion! Le cristal prend des dégâts!");

            // Tu peux ici ajouter des effets visuels, sonores ou appliquer des dégâts au cristal
            // Exemple: ExplosionEffect.Play();

            // Détruire l'objet Mimic après l'explosion
            Destroy(gameObject);
        }
    }
}
