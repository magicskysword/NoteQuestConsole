namespace NoteQuest.ConsoleFrameWork;

public class Game
{
    private static bool _endFlog = false;

    public static void StartGame(IScene startScene)
    {
        Input.Init();
        Time.Init();
        Draw.Init();

        Draw.SetScreenSize(102,31);

        SceneManager.PushScene(startScene);

        MainLoop();
    }

    public static void EndGame()
    {
        _endFlog = true;
    }

    private static void MainLoop()
    {
        while (true)
        {
            Time.StartFrame();
            Input.Update();
            SceneManager.Update();

            if (SceneManager.CurScene == null)
            {
                _endFlog = true;
            }

            if (_endFlog)
            {
                EndGame();
                return;
            }

            Draw.Print();
            Time.Sleep();
        }
    }
}