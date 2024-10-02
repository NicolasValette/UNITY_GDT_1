using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDT1.Game.System
{
    public enum SpawnMode
    {
        AllPosEachTime,
        OnePosbyOnePos
    }

    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private SpawnMode _spawnMode;
        [SerializeField]
        private GameObject _objectToSpawn;
        [SerializeField]
        private float _timer = 60f;

        [SerializeField]
        private Vector2 _minBounds = new Vector2(-20, -20); // Default min bounds
        [SerializeField]
        private Vector2 _maxBounds = new Vector2(20, 20); // Default max bounds

        private List<Transform> _spawningPositions = new List<Transform>();
        private int _currentSpawningInd = 0;
        private bool _spawningIsOn = false;

        void Start()
        {
            if (_objectToSpawn == null)
            {
                Debug.LogError($"Error in spawner {gameObject.name}, missing prefab");
            }

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                _spawningPositions.Add(transform.GetChild(i));
            }
        }

        private void Spawn()
        {
            GameObject spawned;
            switch (_spawnMode)
            {
                case SpawnMode.AllPosEachTime:
                    for (int i = 0; i < _spawningPositions.Count; i++)
                    {
                        Vector3 spawnPosition = GetConstrainedSpawnPosition(_spawningPositions[i].position);
                        spawned = Instantiate(_objectToSpawn, spawnPosition, _spawningPositions[i].rotation);
                        spawned.transform.parent = _spawningPositions[i];
                    }
                    break;

                case SpawnMode.OnePosbyOnePos:
                default:
                    Vector3 singleSpawnPosition = GetConstrainedSpawnPosition(_spawningPositions[_currentSpawningInd].position);
                    spawned = Instantiate(_objectToSpawn, singleSpawnPosition, _spawningPositions[_currentSpawningInd].rotation);
                    spawned.transform.parent = _spawningPositions[_currentSpawningInd];
                    _currentSpawningInd = (_currentSpawningInd + 1) % _spawningPositions.Count;
                    break;
            }
        }

        private Vector3 GetConstrainedSpawnPosition(Vector3 spawnPosition)
        {
            // Clamp the spawn position to ensure it is within the playground bounds
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, _minBounds.x, _maxBounds.x);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, _minBounds.y, _maxBounds.y);
            return spawnPosition;
        }

        private void StopSpawning()
        {
            _spawningIsOn = false;
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnUntilTimer());
        }

        private IEnumerator SpawnUntilTimer()
        {
            _spawningIsOn = true;
            while (_spawningIsOn)
            {
                Spawn();
                yield return new WaitForSeconds(_timer);
            }
        }
    }
}
