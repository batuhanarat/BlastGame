using System.Collections.Generic;
using UnityEngine;

public static class CellVisitTracker
{
    public static Dictionary<Vector2Int ,Cell> tracker = new Dictionary<Vector2Int , Cell>();

    public static void AddAsVisited(Vector2Int index, Cell cell) {
        if(tracker.ContainsKey(index)) return;

        tracker.Add(index,cell);
    }

    public static bool HasVisited(Vector2Int index) {
        return tracker.ContainsKey(index);
    }

    public static void Reset() {
        tracker.Clear();
    }

}