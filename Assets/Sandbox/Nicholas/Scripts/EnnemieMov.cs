using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieMov : MonoBehaviour
{
    [SerializeField] private ScriptableTower towerData;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _bulletPrefab; // Prefab for the bullet    
    private GameObject _targetFirstChild;
    private Vector3 _offset;
    private float _distance;
    private bool _canShoot = true; // To control when a new instance can be created

    void Start()
    {
        _offset = new Vector3(0, .8f, 0);
        _targetFirstChild = GameObject.Find("Tower(Clone)");


    }

    void Update()
    {
        Vector3 direction = _targetFirstChild.transform.position + _offset - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        _distance = Vector3.Distance(_targetFirstChild.transform.position, gameObject.transform.position);

        if (_distance > .8f)
        {
            transform.Translate(Vector3.forward * towerData.vitesseDeplacement * Time.deltaTime);
        }
        else if (_canShoot)
        {
            StartCoroutine(ShootBullet());
        }
    }

    private IEnumerator ShootBullet()
    {
        _canShoot = false;

        // Create a new bullet instance as a child of this GameObject
        GameObject bulletInstance = Instantiate(_bulletPrefab, transform.position + transform.forward, Quaternion.identity, transform);

        // Copy the position of the non-instance bullet (_bulletPrefab) to the new instance
        bulletInstance.transform.position = _bulletPrefab.transform.position;

        // Set the scale of the bullet
        bulletInstance.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);

        // Activate the bullet instance
        bulletInstance.SetActive(true);

        // Add logic for when the bullet hits the target (via collision)
        Bullet bulletScript = bulletInstance.AddComponent<Bullet>();
        bulletScript.Initialize("Tower", () => StartCoroutine(RespawnBullet()));

        yield return null;
    }

    private IEnumerator RespawnBullet()
    {
        yield return new WaitForSeconds(towerData.tempsRecharge);
        _canShoot = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("touché");
            towerData.nbPointsVies=towerData.nbPointsVies-1;
            if(towerData.nbPointsVies<1)
            {
                Destroy(gameObject);
            }
        }
    }
}

public class Bullet : MonoBehaviour
{
    private System.Action _onHitCallback;

    public void Initialize(string targetTag, System.Action onHitCallback)
    {
        _onHitCallback = onHitCallback;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Destroy(gameObject); // Destroy this bullet
            _onHitCallback?.Invoke(); // Trigger the callback to respawn
        }

        else if (other.gameObject.CompareTag("Untagged"))
        {
            Destroy(gameObject); // Destroy this bullet
            _onHitCallback?.Invoke(); // Trigger the callback to respawn
        }
    }
}