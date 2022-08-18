using ConsoleRPG.Core;

namespace NoteQuest.Scenes;

public class InitScene : SceneBase
{
    public override void Start()
    {
        new Archive().Init();
        SceneManager.SwitchScene(new TitleScene());
    }
}