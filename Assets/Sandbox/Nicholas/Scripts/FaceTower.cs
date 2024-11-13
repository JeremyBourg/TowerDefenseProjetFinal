using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTower : MonoBehaviour
{
    
    [SerializeField] private GameObject _target;
    private GameObject _targetFirstChild;
    private float _distance;
    
    
    void Start()
    {
        _targetFirstChild = GameObject.Find("Tower(Clone)");
        Debug.Log(_targetFirstChild);
    }
    void Update()
    {
        Vector3 direction = _targetFirstChild.transform.position - transform.position;
        //direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        _distance = Vector3.Distance(_targetFirstChild.transform.position, gameObject.transform.position);
    }
}
