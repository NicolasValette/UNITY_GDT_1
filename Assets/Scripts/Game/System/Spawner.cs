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
        private List<Transform> _spawnPos;
        [SerializeField]
        private GameObject _objectToSpawn;
        [SerializeField]
        private float _timer = 60f;

        [SerializeField]
        private Vector2 _minBounds = new Vector2(-20, -20); // Default min bounds
        [SerializeField]
        private Vector2 _maxBounds = new Vector2(20, 20); // Default max bounds

        private int _currentSpawningInd = 0;
        private bool _spawningIsOn = false;

        // Start is called before the first frame update
        void Start()
        {
            if (_objectToSpawn == null)
            {
                Debug.LogError($"Error in spawner {gameObject.name}, missing prefab");
            }
            if (_spawnPos == null || _spawnPos.Count == 0)
            {
                Debug.LogError($"Error in spawner {gameObject.name}, missing spawn position");
            }
            StartCoroutine(SpawnUntilTimer());
        }

        private void Spawn()
        {
            switch (_spawnMode)
            {
                case SpawnMode.AllPosEachTime:
                    for (int i = 0; i < _spawnPos.Count; i++)
                    {
                        Vector3 spawnPosition = GetConstrainedSpawnPosition(_spawnPos[i].position);
                        Instantiate(_objectToSpawn, spawnPosition, _spawnPos[i].rotation);
                    }
                    break;

                case SpawnMode.OnePosbyOnePos:
                default:
                    Vector3 singleSpawnPosition = GetConstrainedSpawnPosition(_spawnPos[_currentSpawningInd].position);
                    Instantiate(_objectToSpawn, singleSpawnPosition, _spawnPos[_currentSpawningInd].rotation);
                    _currentSpawningInd = (_currentSpawningInd + 1) % _spawnPos.Count;
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
