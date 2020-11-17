using System.Collections.Generic;
using UnityEngine;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _lifePrefab;

    private List<GameObject> _lifeInstances = new List<GameObject>();
    public void Start()
    {
        GameManager.Instance.LivesUpdatedEvent += SetupLives;
        SetupLives(GameManager.Instance.Lives);
    }

    public void SetupLives(int newLives)
    {
        int currentCount = _lifeInstances.Count;
        
        if (currentCount < newLives)
        {
            for (int i = currentCount; i < GameManager.Instance.Lives; i++)
            {
                _lifeInstances.Add(Instantiate(_lifePrefab, transform));
            }
        }

        if (currentCount > newLives)
        {
            for (int i = currentCount - 1; i >= newLives; i--)
            {
                GameObject life = _lifeInstances[i];
                _lifeInstances.Remove(life);
                Destroy(life);
            }
        }
        
    }
    
    void OnDestroy()
    {
        GameManager.Instance.LivesUpdatedEvent -= SetupLives;
    }
}
