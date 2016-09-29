using UnityEngine;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }

    public ScenesManager ScenesManager { get; private set; }
    public GameSession GameSession { get; private set; }

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(Instance);

        ScenesManager = new ScenesManager();

        GameSession = new GameSession();
        GameSession.MakeNewGame();
    }
}
