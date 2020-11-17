using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    public event Action<Meteor> MeteorDestroyedEvent;

    public int Points => _points;
    public GameObject DestroyParticles => _particles;

    [MinMaxRange(0,1000)]
    [SerializeField] private RangedFloat _speed;
    [SerializeField] private int _numberOfChildren;
    [SerializeField] private List<GameObject> _childPrefabs;
    [SerializeField] private int _points;
    [SerializeField] private GameObject _particles;
    
    private Rigidbody2D _rb2d;

    // Start is called before the first frame update
    void Awake()
    {
        _rb2d = GetComponentInChildren<Rigidbody2D>();
    }

    private void OnEnable()
    {
        float angle = Random.Range(0, 360);
        _rb2d.AddForce((Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right) * Random.Range(_speed.minValue,_speed.maxValue));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            other.GetComponent<Laser>().Kill();
            MeteorDestroyedEvent?.Invoke(this);
        }
    }

    public List<Meteor> SpawnChildren(ObjectPoolManager objectPoolManager)
    {
        if (_numberOfChildren == 0 || _childPrefabs == null || _childPrefabs.Count == 0)
        {
            return null;
        }

        List<Meteor> newMeteors = new List<Meteor>();
        
        for (int i = 0; i < _numberOfChildren; i++)
        {
            var newMeteor = objectPoolManager.GetPooledObject(_childPrefabs.RandomItem(),transform.position,Quaternion.identity).GetComponent<Meteor>();
            newMeteors.Add(newMeteor);
        }

        return newMeteors;
    }
}
