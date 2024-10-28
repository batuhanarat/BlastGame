using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0 , menuName = "DreamGamesCase/PoolData")]
public class PoolData : ScriptableObject
{
    public List<PoolableItemData> pool;

}

[Serializable]
public class PoolableItemData{
    public ItemType itemType;
    public GameObject ItemPrefab;
    public int size;
}

