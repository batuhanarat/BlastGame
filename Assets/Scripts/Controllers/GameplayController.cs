using System;
using _Scripts.Pool;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    private LevelManager _levelManager;
    private LevelData _levelData;
    private LevelLoader _levelLoader;


    [SerializeField] private GameObject _boardPrefab;
    [SerializeField] private GameObject _objectPoolPrefab;
    [SerializeField] private GameObject _itemFactoryPrefab;
    [SerializeField] private GameObject _fallManagerPrefab;
    [SerializeField] private UIController _uiController;


    public void Awake()
    {
        _levelLoader = LevelLoader.Instance;
        _levelData =  _levelLoader.LoadLevelJson();
        _uiController = Instantiate<UIController>(_uiController);
        _uiController.InitializeLevelSceneUI();
        PrepareGameplay();
    }

    private void PrepareGameplay()
    {
        var boardgo = Instantiate(_boardPrefab);

        Board board = boardgo.GetComponent<Board>();

        MoveManager moveManager = new MoveManager(_uiController.UpdateMoveCount,_uiController.ShowLoseDialog);
        GoalManager goalManager = new GoalManager(() =>
            {
            moveManager.LockMove();
            _uiController.ShowWinDialog(OnNextLevel);
            },
            _uiController.UpdateGoalCount,
            _uiController.SetGoalUI);


        ItemPool itemPool = Instantiate(_objectPoolPrefab).GetComponent<ItemPool>();
        ItemFactory itemFactory = Instantiate(_itemFactoryPrefab).GetComponent<ItemFactory>();
        itemFactory.Prepare(itemPool,board.ItemHolder);
        itemPool.Initialize(board,goalManager,itemFactory);

        _levelManager = new LevelManager(_levelData,board,_levelLoader,itemFactory,moveManager,goalManager);

    }
    public void OnNextLevel() {
        _levelManager.LevelWin();

    }


}
