using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquidMinigame
{
    /// <summary>
    ///     Handles spawning of fishing loot
    /// </summary>
    public class FishingLootSpawner : MonoBehaviour
    {
        [Serializable]
        public struct LootTableEntry
        {
            public GameObject Loot;
            public float Weight;
        }

        [InfoBox("Handles randomized loot spawning with variable weights")]
        [Header("Depends")]

        [SerializeField]
        private Transform _lootSpawnPoint;

        [Header("Config")]

        [SerializeField]
        private List<LootTableEntry> _fishingLootTable;

        private void OnDrawGizmos()
        {
            if (_lootSpawnPoint == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_lootSpawnPoint.position, 0.1f);
        }

        public void SpawnFish()
        {
            if (_fishingLootTable.Count == 0)
            {
                Debug.LogWarning("No loot table entries");
                return;
            }

            float totalWeight = 0;
            foreach (LootTableEntry entry in _fishingLootTable)
            {
                totalWeight += entry.Weight;
            }

            float randomValue = Random.Range(0, totalWeight);

            foreach (LootTableEntry entry in _fishingLootTable)
            {
                randomValue -= entry.Weight;
                if (randomValue <= 0)
                {
                    Instantiate(entry.Loot, _lootSpawnPoint.position, Quaternion.identity);
                    return;
                }
            }
        }
    }
}