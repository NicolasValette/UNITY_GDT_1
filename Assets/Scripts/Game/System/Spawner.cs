using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        private int _currentSpawningInd = 0;
        private bool _spawningIsOn = false;

        // Start is called before the first frame update
        void Start()
        {
            if (_objectToSpawn == null)
            {
                Debug.LogError($"Errow in spawner {gameObject.name}, missing prefab");
            }
            if (_spawnPos == null)
            {
                Debug.LogError($"Errow in spawner {gameObject.name}, missing spawn postion");
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
                        Instantiate(_objectToSpawn, _spawnPos[i].position, _spawnPos[i].rotation);
                    }
                    break;
               
                case SpawnMode.OnePosbyOnePos:
                default:
                    Instantiate(_objectToSpawn, _spawnPos[_currentSpawningInd].position, _spawnPos[_currentSpawningInd].rotation);
                    _currentSpawningInd = (_currentSpawningInd + 1) % _spawnPos.Count;
                    break;
            }
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
