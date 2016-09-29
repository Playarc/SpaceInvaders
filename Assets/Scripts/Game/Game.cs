using UnityEngine;
using System;

using Object = UnityEngine.Object;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameField _gameField = null;
    [SerializeField]
    private UnitsController _unitsCtrl = null;

    public event Action OnGameBegun;
    public event Action<int> OnScoreChanged;
    public event Action<bool> OnGameEnded;

    public bool IsGameActive { get; private set; }

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;

            if (OnScoreChanged != null)
            {
                OnScoreChanged(_score);
            }
        }
    }
    private int _score = 0;

    private GameScreen _screen = null;
    private MainCharacter _mainCharater = null;

    public void Init()
    {
        _screen = new GameScreen(Camera.main);

        _unitsCtrl.Init(Camera.main, _screen.Bounds, _gameField.DeathLineYPos, _gameField.LeftSideLineTransform, _gameField.RightSideLineTransform);
        _unitsCtrl.OnUnitPassedBottomLine += OnUnitPassedBottomLine;
        _unitsCtrl.OnUnitKilled += OnUnitKilled;
        _unitsCtrl.OnAllRowsEmpty += OnAllRowsEmpty;

        _gameField.Fit(_screen.Bounds);

        _mainCharater = CreateMainCharacter();
        _mainCharater.Init(_screen.Bounds, _gameField.LeftSideLineTransform, _gameField.RightSideLineTransform);
    }

    private void OnAllRowsEmpty()
    {
        EndGame(true);
    }

    private MainCharacter CreateMainCharacter()
    {
        Object loadedObj = Resources.Load("Prefabs/MainCharacter");
        GameObject instObj = Instantiate(loadedObj) as GameObject;
        instObj.transform.position = _gameField.MainCharacterSpawnPointTransform.position;

        return instObj.GetComponent<MainCharacter>();
    }

    private void OnUnitKilled(UnitBase unit)
    {
        Score += 10;
    }

    private void OnUnitPassedBottomLine(UnitBase unit)
    {
        EndGame(false);
    }

    public void BeginGame()
    {
        _unitsCtrl.CreateUnits();

        Resources.UnloadUnusedAssets();

        IsGameActive = true;

        if (OnGameBegun != null)
        {
            OnGameBegun();
        }
    }

    public void EndGame(bool win)
    {
        IsGameActive = false;

        if (OnGameEnded != null)
        {
            OnGameEnded(win);
        }
    }

    void OnDestroy()
    {
        _unitsCtrl.OnUnitPassedBottomLine -= OnUnitPassedBottomLine;
    }
}