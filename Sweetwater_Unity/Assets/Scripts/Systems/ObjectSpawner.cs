using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Spawning")]

        [SerializeField]
        private List<GameObject> _objectsToSpawn;

        [SerializeField]
        private int _maxSpawnCount;

        [Header("Positioning")]

        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private bool _parentToSpawnPoint;

        private int _currentSpawnCount;

        public void SpawnObject()
        {
            if (_objectsToSpawn.Count == 0)
            {
                Debug.LogWarning("No object to spawn");
                return;
            }

            if (_currentSpawnCount >= _maxSpawnCount)
            {
                return;
            }

            int randomIndex = Random.Range(0, _objectsToSpawn.Count);
            GameObject spawnedObject =
                Instantiate(_objectsToSpawn[randomIndex], _spawnPoint.position, _spawnPoint.rotation);

            if (_parentToSpawnPoint)
            {
                spawnedObject.transform.SetParent(_spawnPoint);
            }

            _currentSpawnCount++;
        }
    }
}