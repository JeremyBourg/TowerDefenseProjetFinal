using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Tower")){
            Destroy(gameObject);
        }
    }

}
