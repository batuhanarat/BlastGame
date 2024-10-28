using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : IMatchManager
{
    private Func<Vector2Int,List<Cell>> checkNeighbours;
    private Func<Vector2Int,Cell> getCellFromBoard;

    public MatchManager(Func<Vector2Int,Cell> GetCellFromBoard, Func<Vector2Int,List<Cell>> CheckNeighbours ) {
        checkNeighbours = CheckNeighbours;
        getCellFromBoard = GetCellFromBoard;
    }

    public List<Cell> GetMatchGroup(Vector2Int index)
    {
        List<Cell> matchGroup = new List<Cell>();
        Queue<Cell> matchQueue = new Queue<Cell>();
        Cell initialCell = getCellFromBoard(index);
        var initialItem = initialCell.GetItem();
        matchQueue.Enqueue(initialCell);
        matchGroup.Add(initialCell);

        CellVisitTracker.AddAsVisited(initialCell.GetIndex(), initialCell);


        while(matchQueue.Count > 0) {
            var currentCell = matchQueue.Dequeue();
            List<Cell> neighbors = checkNeighbours(currentCell.GetIndex());
            foreach(var neighbor in neighbors) {
                var neighborItem = neighbor.GetItem();
                var neighborIndex = neighbor.GetIndex();

                if (CellVisitTracker.HasVisited(neighborIndex))
                continue;

                CellVisitTracker.AddAsVisited(neighborIndex, neighbor);
                if(neighborItem.CanBeMatched(initialItem.Type, initialItem.Color)){
                    matchQueue.Enqueue(neighbor);
                    matchGroup.Add(neighbor);
                }
            }
        }

        return matchGroup;
    }
}
public interface IMatchManager {
    List<Cell> GetMatchGroup(Vector2Int index);
}