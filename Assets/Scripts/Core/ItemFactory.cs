using _Scripts.Pool;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    private ItemPool _pool;
    private SolidColorItemData _solidColorItemData;
    private Transform _itemHolder;

    void Awake()
    {
        _solidColorItemData =  Resources.Load<SolidColorItemData>("ScriptableObjects/SolidColorItemSO");
    }
    public void Prepare(ItemPool pool, Transform itemHolder)
    {
        _itemHolder = itemHolder;
        _pool = pool;
    }
    public ItemBase GetItemWithString(string element)
    {
        ItemBase newItem;
        switch (element)
        {
            case ItemData.BLUE_CUBE:
                newItem = GetItemWithType(ItemType.SOLID_COLOR, ItemColor.BLUE);
                return newItem;
            case ItemData.RED_CUBE:
                newItem = GetItemWithType(ItemType.SOLID_COLOR, ItemColor.RED);
                return newItem;
            case ItemData.YELLOW_CUBE:
                newItem = GetItemWithType(ItemType.SOLID_COLOR, ItemColor.YELLOW);
                return newItem;
            case ItemData.GREEN_CUBE:
                newItem = GetItemWithType(ItemType.SOLID_COLOR, ItemColor.GREEN);
                return newItem;
            case ItemData.RANDOM:
                newItem = GetItemWithType(ItemType.SOLID_COLOR);
                return newItem;
            case ItemData.TNT:
                newItem = GetItemWithType(ItemType.TNT);
                return newItem;
            case ItemData.STONE:
                newItem = GetItemWithType(ItemType.STONE);
                return newItem;
            case ItemData.VASE:
                newItem = GetItemWithType(ItemType.VASE);
                return newItem;
            case ItemData.BOX:
                newItem = GetItemWithType(ItemType.BOX);
                return newItem;
            default:
                newItem = GetItemWithType(ItemType.SOLID_COLOR, ItemColor.BLUE);
                return newItem;
        }
    }
    public ItemBase GetItemWithType(ItemType itemType, ItemColor itemColor = ItemColor.NONE)
    {
        if(itemType == ItemType.SOLID_COLOR && itemColor == ItemColor.NONE) {
                itemColor = GetRandomItemColor();
        }
        var itemGameObject = _pool.SpawnFromPool(itemType);

        if(itemGameObject == null) {
            Debug.LogWarning("Object spawned from pool is null");
        }
        itemGameObject.transform.SetParent(_itemHolder,false);

        ItemBase item = itemGameObject.GetComponent<ItemBase>();
        if(item == null) return null;
        item.SetItemData(itemType,itemColor);
        if(itemColor == ItemColor.NONE) {
            return item;
        }
        return PrepareSolidColorItem(itemColor, item as SolidColorItem);
    }
    private ItemBase PrepareSolidColorItem(ItemColor itemColor, SolidColorItem item)
    {
        Sprite cubeSprite = _solidColorItemData.GetNormalCubeSprite(itemColor);
        Sprite particleSprite = _solidColorItemData.GetParticleSprite(itemColor);
        Sprite tntCubeSprite = _solidColorItemData.GetTntCubeSprite(itemColor);

        item.SetSprite(cubeSprite);
        item.SetParticleSprite(particleSprite);
        item.SetTntSprite(tntCubeSprite);
        return item;
    }
    public ItemColor GetRandomItemColor()
    {
        ItemColor[] availableColors = { ItemColor.YELLOW, ItemColor.BLUE, ItemColor.RED, ItemColor.GREEN };
        return availableColors[Random.Range(0, availableColors.Length)];
    }


}