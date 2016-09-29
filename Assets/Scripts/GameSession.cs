using UnityEngine;

public class GameSession
{
    public void MakeNewGame()
    {
        App.Instance.ScenesManager.LoadScene(SceneName.Game, null, OnGameSceneLoaded);
    }

    public void RestartCurrentGame()
    {
        MakeNewGame();
    }

    private void OnGameSceneLoaded()
    {
        Game game = Object.FindObjectOfType<Game>();
        game.Init();
        game.BeginGame();
    }
}