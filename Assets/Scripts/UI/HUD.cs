using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Game _game = null;
    [SerializeField]
    private GameObject _restartBtnObj = null;
    [SerializeField]
    private Text _scoreLabel = null;
    [SerializeField]
    private GameObject _winWindow = null, _loseWindow = null;

    void Awake()
    {
        SetRestartBtnObjState(false);
    }

    void Start()
    {
        _game.OnScoreChanged += OnScoreChanged;
        _game.OnGameEnded += OnGameEnded;
    }

    void OnDestroy()
    {
        _game.OnScoreChanged -= OnScoreChanged;
        _game.OnGameEnded -= OnGameEnded;
    }

    private void OnScoreChanged(int newScore)
    {
        _scoreLabel.text = string.Format("Score:{0}{1}", System.Environment.NewLine, newScore);
    }

    private void OnGameEnded(bool win)
    {
        SetRestartBtnObjState(true);

        if (win)
        {
            _winWindow.SetActive(true);
        }
        else
        {
            _loseWindow.SetActive(true);
        }
    }

    public void OnResetBtnClicked()
    {
        App.Instance.GameSession.RestartCurrentGame();
    }

    private void SetRestartBtnObjState(bool isActive)
    {
        _restartBtnObj.SetActive(isActive);
    }
}