using NoteQuest.Core;

namespace NoteQuest;

public class Archive
{
    public static Archive I { get; set; }
    
    public GameManager GameManager;

    public void Init()
    {
        I = this;
        GameManager = new GameManager();
        GameManager.Init("Config");
    }
}