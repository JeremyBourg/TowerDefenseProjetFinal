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
        public float speed = 5f; // Vitesse de déplacement
        public float velocityLerpCoef = 4f; // Coefficient de lissage de la vélocité
        public float jumpForce = 5f; // Force du saut
        public float gravity = -9.81f; // Force de la gravité
        public float jumpHeight = 1.5f; // Hauteur maximale du saut pour atteindre le cristal
        public Transform crystalTarget; // Position du cristal cible
        public NavMeshAgent agent; // L'agent de navigation (NavMeshAgent)

        private bool isJumping = false; // Est-ce que le Mimic est en train de sauter ?
        private bool isLevitating = false; // Est-ce que le Mimic est en train de léviter ?
        private bool isAttached = false; // Est-ce que le Mimic est attaché au cristal ?
        private float currentJumpHeight = 0f; // Hauteur actuelle du saut
        private Mimic myMimic; // Référence au Mimic pour contrôler les jambes et la vitesse

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            crystalTarget = GameObject.FindGameObjectWithTag("Crystal").transform; // Chercher le cristal par son tag
            agent = GetComponent<NavMeshAgent>(); // Récupérer le NavMeshAgent
            agent.stoppingDistance = 1f; // Définir la distance à laquelle l'agent s'arrête près du cristal
            agent.speed = speed; // Assigner la vitesse du NavMeshAgent
        }

        void Update()
        {
            // Calculer la direction horizontale vers le cristal
            Vector3 targetDirection = (crystalTarget.position - transform.position).normalized;
            float distanceToCrystal = Vector3.Distance(transform.position, crystalTarget.position);
            bool isAboveCrystal = transform.position.y >= crystalTarget.position.y;

            // Vérifier si un obstacle bloque le Mimic
            RaycastHit hit;
            bool hitObstacle = Physics.Raycast(transform.position, targetDirection, out hit, 2f);

            // Si un obstacle est détecté et que le Mimic est proche du cristal, il va commencer à léviter et s'attacher
            if (hitObstacle && !isAttached)
            {
                StartLevitating(hit.collider.transform);
            }

            // Si on est proche du cristal et qu'on n'est pas déjà attaché, commence le saut
            if (!isJumping && !hitObstacle && distanceToCrystal < 2f && !isAboveCrystal)
            {
                isJumping = true;
                currentJumpHeight = 0f;
                agent.isStopped = true; // Arrêter l'agent de se déplacer pendant le saut
            }

            // Si le Mimic est en train de sauter
            if (isJumping)
            {
                // Logique du saut : augmenter la hauteur jusqu'à atteindre la hauteur cible
                currentJumpHeight += jumpForce * Time.deltaTime;

                // Si la hauteur maximale est atteinte, arrêter le saut
                if (currentJumpHeight >= jumpHeight)
                {
                    isJumping = false;
                    currentJumpHeight = jumpHeight;
                    agent.isStopped = false; // Relancer l'agent une fois que le saut est terminé
                }

                // Appliquer le mouvement vertical (saut)
                agent.velocity = new Vector3(agent.velocity.x, jumpForce * Time.deltaTime, agent.velocity.z); // Changer seulement la composante Y (verticale)
            }
            else if (isLevitating)
            {
                // Si on est en lévitation, on applique un mouvement vertical
                agent.velocity = new Vector3(agent.velocity.x, gravity * Time.deltaTime, agent.velocity.z); // On laisse la gravité fonctionner normalement
            }
            else
            {
                // Sinon, déplacer l'agent horizontalement vers le cristal
                agent.SetDestination(crystalTarget.position);
            }

            // Vérifier si le Mimic a atteint la cible (le cristal) pour déclencher l'explosion
            if (distanceToCrystal < 1f && !isAttached)
            {
                AttachToCrystal();
            }

            // Réajuster la hauteur pour s'assurer que le Mimic reste à la bonne hauteur par rapport au sol
            RaycastHit groundHit;
            Vector3 destHeight = transform.position;

            // Utiliser un raycast pour vérifier la position du sol en dessous du Mimic
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out groundHit))
            {
                destHeight = new Vector3(transform.position.x, groundHit.point.y + height, transform.position.z);
            }

            // Lerp la hauteur pour la rendre plus fluide
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        void StartLevitating(Transform obstacle)
        {
            // Arrêter le mouvement horizontal
            agent.isStopped = true;

            // Démarrer la lévitation
            isLevitating = true;
        }

        void AttachToCrystal()
        {
            // Logique pour attacher les jambes au cristal et déclencher l'explosion
            Debug.Log("Mimic attaché au cristal et prêt à exploser!");

            isAttached = true;

            // Tu peux ici appeler une fonction pour lancer l'explosion
            Explode();
        }

        void Explode()
        {
            // Logique d'explosion lorsque le Mimic atteint le cristal
            Debug.Log("Explosion! Le cristal prend des dégâts!");

            // Tu peux ici ajouter des effets visuels, sonores, ou appliquer des dégâts au cristal
            // Exemple: ExplosionEffect.Play();

            // Détruire l'objet Mimic après l'explosion
            Destroy(gameObject);
        }
    }
}
