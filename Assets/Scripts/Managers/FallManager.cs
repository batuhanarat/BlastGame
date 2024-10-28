using System.Collections.Generic;
using UnityEngine;

public class FallManager
{
    private Board _board;
    private ItemFactory _itemFactory;
    private FallAnimationManager _animationManager;
    private const float NewItemStartHeight = 2f;

    public FallManager(Board board, ItemFactory itemFactory, FallAnimationManager animationManager)
    {
        _animationManager = animationManager;
        _board = board;
        _itemFactory = itemFactory;
    }

    public void ProcessFalling()
    {
        List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToFall = new List<(ItemBase, Vector3, Vector3)>();


        for (int x = 0; x < _board.Width; x++)
        {
            int emptyY = -1;
            for (int y = 0; y < _board.Height; y++)
            {
                if (!_board.Grid[x, y].IsItemPresent() && emptyY == -1)
                {
                    emptyY = y;
                }
                else if (_board.Grid[x, y].IsItemPresent() && emptyY != -1)
                {
                    var item = _board.Grid[x, y].GetItem();
                    if (item.CanFalltoEmptyCell())
                    {
                        Vector3 startPos = _board.Grid[x, y].transform.position;
                        Vector3 endPos = _board.Grid[x, emptyY].transform.position;
                        itemsToFall.Add((item, startPos, endPos));


                        _board.Grid[x, emptyY].SetItem(item,false);
                        _board.Grid[x, y].SetItem(null,false);
                        item.UpdatePosition(new Vector2Int(x, emptyY));
                        emptyY++;
                    }
                    else
                    {
                        emptyY = -1;
                    }
                }
            }

            if (emptyY != -1)
            {
                for (int y = emptyY; y < _board.Height; y++)
                {
                    ItemBase newItem = _itemFactory.GetItemWithType(ItemType.SOLID_COLOR);
                    Vector3 startPos = _board.Grid[x, _board.Height - 1].transform.position + Vector3.up * NewItemStartHeight;
                    Vector3 endPos = _board.Grid[x, y].transform.position;
                    itemsToFall.Add((newItem, startPos, endPos));

                    _board.Grid[x, y].SetItem(newItem);
                    newItem.UpdatePosition(new Vector2Int(x, y));
                }
            }
        }

        _animationManager.StartFallingItemsAnimation(itemsToFall);

    }




}