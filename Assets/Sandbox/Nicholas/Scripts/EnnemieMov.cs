using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieMov : MonoBehaviour
{
    
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _speed;
    private GameObject _targetFirstChild;
    private Vector3 _offset;
    private float _distance;
    
    
    void Start()
    {
        _offset = new Vector3(0,1,0);
        _targetFirstChild = GameObject.Find("Tower(Clone)");
        Debug.Log(_targetFirstChild);
    }
    void Update()
    {
        Vector3 direction = _targetFirstChild.transform.position+_offset - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
        _distance = Vector3.Distance(_targetFirstChild.transform.position, gameObject.transform.position);

        if(_distance<=.4)
        {
            Debug.Log("ALERT ALERT");
            _bullet.SetActive(true);
        }
        else if(_distance>.6)
        {
            transform.Translate(Vector3.forward*_speed*Time.deltaTime);
        }
    }
}
