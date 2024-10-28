using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    private Board board;
    private List<(ItemType[,] Types, ItemColor[,] Colors)> boardStates = new List<(ItemType[,], ItemColor[,])>();
    private int currentStateIndex = -1;

    private void OnEnable()
    {
        board = (Board)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Update Board Visualization"))
        {
            UpdateBoardVisualization();
        }

        EditorGUILayout.Space();

        if (boardStates.Count > 0)
        {
            EditorGUILayout.LabelField("Board Visualization", EditorStyles.boldLabel);
            DrawBoardGrid();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous State") && currentStateIndex > 0)
            {
                currentStateIndex--;
            }
            if (GUILayout.Button("Next State") && currentStateIndex < boardStates.Count - 1)
            {
                currentStateIndex++;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField($"State: {currentStateIndex + 1}/{boardStates.Count}");
        }
    }

    private void UpdateBoardVisualization()
    {
        if (board.Grid == null)
        {
            Debug.LogWarning("Board grid is null. Make sure the board is initialized.");
            return;
        }

        ItemType[,] currentTypes = new ItemType[board.Grid.GetLength(0), board.Grid.GetLength(1)];
        ItemColor[,] currentColors = new ItemColor[board.Grid.GetLength(0), board.Grid.GetLength(1)];

        for (int x = 0; x < board.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < board.Grid.GetLength(1); y++)
            {
                Cell cell = board.Grid[x, y];
                if (cell.IsItemPresent())
                {
                    ItemBase item = cell.GetItem();
                    currentTypes[x, y] = item.Type;
                    currentColors[x, y] = item.Color;
                }
                else
                {
                    currentTypes[x, y] = ItemType.NONE;
                    currentColors[x, y] = ItemColor.NONE;
                }
            }
        }

        boardStates.Add((currentTypes, currentColors));
        currentStateIndex = boardStates.Count - 1;
    }

    private void DrawBoardGrid()
    {
        if (currentStateIndex < 0 || currentStateIndex >= boardStates.Count)
            return;

        var (currentTypes, currentColors) = boardStates[currentStateIndex];

        for (int y = currentTypes.GetLength(1) - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentTypes.GetLength(0); x++)
            {
                ItemType itemType = currentTypes[x, y];
                ItemColor itemColor = currentColors[x, y];
                GUI.color = GetColorForItem(itemType, itemColor);
                GUILayout.Box(GetDisplayTextForItem(itemType, itemColor), GUILayout.Width(20), GUILayout.Height(20));
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private Color GetColorForItem(ItemType itemType, ItemColor itemColor)
    {
        if (itemType == ItemType.SOLID_COLOR)
        {
            switch (itemColor)
            {
                case ItemColor.YELLOW: return Color.yellow;
                case ItemColor.BLUE: return Color.blue;
                case ItemColor.RED: return Color.red;
                case ItemColor.GREEN: return Color.green;
                default: return Color.white;
            }
        }
        else
        {
            switch (itemType)
            {
         case ItemType.BOX: return new Color(1f, 0.5f, 0f);    // Orange
case ItemType.TNT: return new Color(0f, 0f, 0f);      // Black
case ItemType.STONE: return new Color(0.5f, 0.5f, 0.5f); // Gray (dark gray)
case ItemType.VASE: return new Color(0.5f, 0f, 0.5f); // Purple
                default: return Color.white;
            }
        }
    }

    private string GetDisplayTextForItem(ItemType itemType, ItemColor itemColor)
    {
        if (itemType == ItemType.SOLID_COLOR)
        {
            return itemColor.ToString();
        }
        else
        {
            return itemType.ToString();
        }
    }
}