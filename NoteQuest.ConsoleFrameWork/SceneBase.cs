namespace NoteQuest.ConsoleFrameWork;

public interface IScene
{
    bool IsStart { get; set; }
    void Start();
    void Update();
    void Destroy();
}

public abstract class SceneBase : IScene
{
    public bool IsStart { get; set; } = false;
    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void Destroy() {}
}