namespace NoteQuest.Core;

public class GameManager
{
    public CoreConfigManager Config { get; private set; } = new CoreConfigManager();
    
    public void Init(string configPath)
    {
        Config.Init(configPath);
    }
}