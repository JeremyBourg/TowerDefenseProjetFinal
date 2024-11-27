using System.Collections;
using UnityEngine;

public class InstanceMover : MonoBehaviour
{
    [SerializeField] private GameObject objectToInstantiate;  // The GameObject to instantiate
    [SerializeField] private GameObject target;              // The target to move toward
    [SerializeField] private float speed = 5f;  
    [SerializeField] private float reloadTime = 42f;         // Time interval to create a new instance (42 seconds)

    private GameObject instantiatedObject;                    // The created instance

    private void Start()
    {
        target = GameObject.Find("Tower(Clone)"); // Find the target GameObject by name
        
        // Start the process after the initial reload time
        Invoke("CreateInstance", reloadTime);

        // Repeatedly call CreateInstance every 'reloadTime' seconds
        InvokeRepeating("CreateInstance", reloadTime, reloadTime);
    }

    private void Update()
    {
        // Rotate the GameObject this script is attached to, to look at the target
        if (target != null)
        {
            // Rotate towards the target
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y; // Optionally keep the rotation on the same plane (X and Z only)
            transform.LookAt(targetPosition);
        }
    }

    // Create an instance of the object and start moving it
    private void CreateInstance()
    {
        if (objectToInstantiate != null && target != null)
        {
            // Instantiate the object at the position of the current object but set it inactive initially
            instantiatedObject = Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
            
            // Set the position of the instantiated object to match the current object
            instantiatedObject.transform.position = transform.position;
            
            // Set the object to inactive initially
            instantiatedObject.SetActive(false);

            // Activate the instance
            instantiatedObject.SetActive(true);

            // Start moving the instance
            StartCoroutine(MoveInstance());
        }
    }

    // Move the instantiated object towards the target
    private IEnumerator MoveInstance()
    {
        while (instantiatedObject != null)
        {
            if (target != null)
            {
                // Move towards the target's position
                Vector3 direction = (target.transform.position - instantiatedObject.transform.position).normalized;
                instantiatedObject.transform.position += direction * speed * Time.deltaTime;
            }

            yield return null;  // Wait until the next frame
        }
    }

    // Detect collision with an object tagged "Tower"
    private void OnCollisionEnter(Collision collision)
    {
        if (instantiatedObject != null && collision.gameObject.CompareTag("Tower"))
        {
            Destroy(instantiatedObject); // Destroy the instantiated object
        }

        else if (instantiatedObject != null && collision.gameObject.CompareTag("Untagged"))
        {
            Destroy(instantiatedObject); // Destroy the instantiated object
        }
    }
}