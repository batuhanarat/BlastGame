using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager {

    private LevelLoader _levelLoader;
    private LevelData _levelData;
    private Board _board;
    private ItemFactory _itemFactory;
    private MoveManager _moveManager;
    private GoalManager _goalManager;


    public LevelManager(LevelData levelData, Board board, LevelLoader levelLoader, ItemFactory itemFactory, MoveManager moveManager, GoalManager goalManager)
    {
        _board = board;
        _levelData = levelData;
        _levelLoader = levelLoader;
        _itemFactory = itemFactory;
        _moveManager = moveManager;
        _goalManager = goalManager;
        InitializeLevel();
    }
    public void InitializeLevel()
    {
        if(_levelData == null ) return;
        _board.Initialize(_levelData.grid_width,_levelData.grid_height,_moveManager,_itemFactory);
        _moveManager.SetMoveCount(_levelData.move_count);

        for(int index = 0 ; index< _levelData.grid.Length; index++)   {
            ItemBase newItem = _itemFactory.GetItemWithString(_levelData.grid[index]);
            if(newItem == null) {
                return;
            }
            if (newItem is IGoal goalItem) {
                if (_goalManager.CheckGoalInitialized(goalItem.GoalType)) {
                    _goalManager.IncrementGoal(goalItem.GoalType);
                }
                else {
                    Goal goal = new Goal(goalItem.GoalType);
                    _goalManager.AddGoal(goal);
                }
            }
            _board.AddToBoard(newItem,index);
        }
        _goalManager.OnAllGoalsInitalized();
        _board.CheckAndChangeSpritesForLargeMatches();
        _board.FixSpriteOrders();
    }


    public void LevelWin() {
        _levelLoader.PrepareNextLevel();
        SceneManager.LoadScene((int)Scenes.MAIN_SCENE);
    }



}