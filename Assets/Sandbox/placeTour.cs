using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeTour : MonoBehaviour
{
    [SerializeField] private GameObject tour;
    [SerializeField] private GameObject tour2;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnTower", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTower()
    {
        GameObject tower = Random.value > 0.5f ? tour : tour2;
        Instantiate(tower, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}
