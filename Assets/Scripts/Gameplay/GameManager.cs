using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
        private static GameManager _instance;

        public static GameManager Instance => _instance;
        
        public event Action<int> ScoreUpdatedEvent;
        public event Action<int> LivesUpdatedEvent;
        public event Action<int> LevelBeginEvent;
        public event Action GameOverEvent;

        public int Score => _score;
        public int Lives => _lives;
        
        [Header("Required Objects")]
        [SerializeField] private List<GameObject> _meteorPrefabs;
        [SerializeField] private ObjectPoolManager _objectPoolManager;
        [SerializeField] private Player _player;
        [SerializeField] private Transform _playerSpawnPos;

        [Header("Balance Values")]
        [SerializeField] private int _startingMeteors = 5;
        [SerializeField] private int _meteorsPerLevel = 5;
        [SerializeField] private int _startingLives;
        [SerializeField] private float _playerSpawnDelay = 5f;
        [SerializeField] private float _levelStartDelay = 3f;
        [SerializeField] private float _gameOverDelay = 5f;
        [SerializeField] private float _edgeSpawnWidth = 100f;
        
        private int _score;
        private int _lives;
        private List<Meteor> _spawnedMeteors = new List<Meteor>();
        private int _level = 0;
        private Vector2 _screenBottomLeft;
        private Vector2 _screenTopRight;

        private void Awake()
        {
                if (_instance != null)
                {
                        Debug.LogWarning("Multiple Game Managers in the scene!");
                }

                _instance = this;
                _lives = _startingLives;
        }
        
        private void Start()
        {
                Camera cam = Camera.main;
                float zPos = transform.position.z;
                _screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zPos));
                _screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zPos));
                _player.PlayerHitEvent += HandlePlayerHit;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(StartLevelCo());
        }

        private void HandlePlayerHit()
        {
                StartCoroutine(PlayerDeathCo());
        }

        private IEnumerator PlayerDeathCo()
        {
                _player.gameObject.SetActive(false);
                _lives--;
                LivesUpdatedEvent?.Invoke(_lives);
                
                if (_lives <= 0)
                {
                        StartCoroutine(EndGame());
                        yield break;
                }

                yield return new WaitForSeconds(_playerSpawnDelay);
                yield return StartCoroutine(PlayerSpawnCo());

        }

        private IEnumerator PlayerSpawnCo()
        {
                // Ensure we don't spawn the player over a meteor
                var colliderAtSpawnPoint = Physics2D.OverlapPoint(_playerSpawnPos.position);
                while (colliderAtSpawnPoint != null)
                {
                        yield return new WaitForFixedUpdate();
                        colliderAtSpawnPoint = Physics2D.OverlapPoint(_playerSpawnPos.position);
                }
                
                _player.transform.position = _playerSpawnPos.position;
                _player.gameObject.SetActive(true);
        }
        


        private IEnumerator StartLevelCo()
        {
                _level++;
                LevelBeginEvent?.Invoke(_level);
                yield return new WaitForSeconds(_levelStartDelay);
                
                int numOfMeteors = _startingMeteors + (_level * _meteorsPerLevel);
                for (int i = 0; i < numOfMeteors; i++)
                {
                        Meteor meteor = _objectPoolManager.GetPooledObject(_meteorPrefabs.RandomItem(),GetRandomMeteorStartPos(),Quaternion.identity).GetComponent<Meteor>();
                        meteor.MeteorDestroyedEvent += HandleMeteorDestroyed;
                        _spawnedMeteors.Add(meteor);
                }
        }

        private Vector3 GetRandomMeteorStartPos()
        {
                switch (Random.Range(0,3))
                {
                        // BOTTOM
                        case 0: return new Vector3(Random.Range(_screenBottomLeft.x,_screenTopRight.x),_screenBottomLeft.y + Random.Range(0,_edgeSpawnWidth),0f);
                        // TOP
                        case 1: return new Vector3(Random.Range(_screenBottomLeft.x,_screenTopRight.x),_screenTopRight.y - Random.Range(0,_edgeSpawnWidth),0f);
                        // LEFT
                        case 2: return new Vector3(_screenBottomLeft.x + Random.Range(0,_edgeSpawnWidth),Random.Range(_screenBottomLeft.y,_screenTopRight.y),0f);
                        // RIGHT
                        default: return new Vector3(_screenTopRight.x - Random.Range(0,_edgeSpawnWidth),Random.Range(_screenBottomLeft.y,_screenTopRight.y),0f);
                }
        }
        
        private void HandleMeteorDestroyed(Meteor meteor)
        {
                meteor.MeteorDestroyedEvent -= HandleMeteorDestroyed;
                IncreaseScore(meteor.Points);
                _objectPoolManager.GetPooledObject(meteor.DestroyParticles, meteor.transform.position,
                        Quaternion.identity);
                
                SpawnChildMeteors(meteor);

                meteor.gameObject.SetActive(false);
                if (_spawnedMeteors.Count == 0)
                {
                        StartCoroutine(StartLevelCo());
                }
        }

        private void SpawnChildMeteors(Meteor meteor)
        {
                var newMeteors = meteor.SpawnChildren(_objectPoolManager);
                if (newMeteors != null)
                {
                        foreach (var newMeteor in newMeteors)
                        {
                                newMeteor.MeteorDestroyedEvent += HandleMeteorDestroyed;
                        }
                        _spawnedMeteors.AddRange(newMeteors);
                }

                _spawnedMeteors.Remove(meteor);
        }

        private void IncreaseScore(int points)
        {
                _score += points;
                ScoreUpdatedEvent?.Invoke(_score);
        }
        
        private IEnumerator EndGame()
        {
                if (_score > PlayerPrefs.GetInt("HighScore"))
                {
                        PlayerPrefs.SetInt("HighScore", _score);
                }

                GameOverEvent?.Invoke();
                
                yield return new WaitForSeconds(_gameOverDelay);
                
                SceneManager.LoadScene("MenuScene");
        }
}
