using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Pool
{
    public class ItemPool : MonoBehaviour
    {
        public bool IsInitialized { get; private set; } = false;
        public Dictionary<ItemType, Queue<GameObject>> poolDictionary = new Dictionary<ItemType, Queue<GameObject>>();
        private PoolData _poolData;
        private Board _board;
        private GoalManager _goalManager;
        private ItemFactory _itemFactory;

        public void Awake() {
            _poolData = Resources.Load<PoolData>("ScriptableObjects/PoolSO");

            if(_poolData == null) {
            Debug.LogWarning("Pool data is null");
            }
        }

        public void Initialize(Board board,GoalManager goalManager,ItemFactory itemFactory)
        {
            _board = board;
            _goalManager = goalManager;
            _itemFactory = itemFactory;
            InitializePools();
        }

        public void InitializePools()
        {
            foreach(var pool in _poolData.pool)
            {
                Queue<GameObject> queue = new Queue<GameObject>(pool.size);

                for(int i = 0 ; i<pool.size; i++) {
                    GameObject gameObject = Instantiate(pool.ItemPrefab,transform);
                    ItemBase item = gameObject.GetComponent<ItemBase>();
                    if(item is IInteractable interactable) {
                        interactable.Prepare(_board,this,_itemFactory);
                    }else if(item is IGoal goalItem){
                        goalItem.Prepare(_board,this, _goalManager.DecrementGoal);
                    }

                    gameObject.SetActive(false);
                    queue.Enqueue(gameObject);
                }
                poolDictionary.Add(pool.itemType,queue);
            }
            IsInitialized = true;
        }
        public GameObject SpawnFromPool(ItemType itemType)
        {
            if(!IsPoolPresent(itemType)) return null;
            if (!IsInitialized) return null;

            GameObject objectToSpawn = poolDictionary[itemType].Dequeue();
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
        public void ReturnToPool(ItemType itemType, ItemBase itemToReturn)
        {
            if (!IsInitialized)  return;

            var cell = _board.Grid[itemToReturn._position.x, itemToReturn._position.y];
            cell.SetItem(null,false);
            itemToReturn.gameObject.SetActive(false);
            poolDictionary[itemType].Enqueue(itemToReturn.gameObject);
            itemToReturn.transform.SetParent(transform);
           // itemToReturn.SetAlphaToNormal();
        }
        private bool IsPoolPresent(ItemType itemType)
        {
            if (!poolDictionary.ContainsKey(itemType))
            {
                Debug.LogWarning("Pool with tag " + itemType + " doesn't exist.");
                return false;
            }
            return poolDictionary[itemType].Count > 0;
        }

    }
}