using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit.SceneDecorator;
using UnityEngine;

public class CrystalBehaviour : MonoBehaviour
{
    [SerializeField] float cooldown = 5f;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] Transform firePoint;

    private Transform target;
    private float cooldownTimer = 0;



    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0){
            cooldownTimer -= Time.deltaTime;
        }

        if (target != null && cooldownTimer <= 0f){
            cooldownTimer = cooldown;
            LightningStrike();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            // Vérifier s'il n'y a aucune cible, ou s'il existe une autre cible plus proche.
            if(target == null || Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, target.transform.position)){
                target = other.transform;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform == target)
        {
            target = null;
        }
    }

    private void LightningStrike(){
        if (lightningPrefab != null && target != null)
        {
            GameObject lightning = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
            LightningStrikeRendering lightningScript = lightning.GetComponent<LightningStrikeRendering>();

            if (lightningScript != null)
            {
                lightningScript.firePoint = firePoint;
                lightningScript.target = target;
                lightningScript.CreateLightning();

            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(999);
            }
        }
    }
}
