using System.Collections.Generic;

namespace NoteQuest.ConsoleFrameWork;

public static class SceneManager
{
    public static Stack<IScene> SceneStack { get; } = new Stack<IScene>();

    public static IScene CurScene => SceneStack.Count > 0 ? SceneStack.Peek() : null;

    private static Queue<IScene> _removeScenes = new Queue<IScene>();


    public static void PushScene(IScene sceneBase)
    {
        if (SceneStack.Count > 0)
        {
            SceneStack.Peek().IsStart = false;
        }
        SceneStack.Push(sceneBase);
    }

    public static void PopScene()
    {
        var scene = SceneStack.Pop();
        _removeScenes.Enqueue(scene);
    }

    public static void SwitchScene(IScene sceneBase)
    {
        while (SceneStack.Count > 0)
        {
            PopScene();
        }

        PushScene(sceneBase);
    }

    public static void Update()
    {
        var curScene = CurScene;
        if (curScene != null)
        {
            if(curScene.IsStart == false)
            {
                curScene.IsStart = true;
                curScene.Start();
            }

            curScene.Update();
        }

        while (_removeScenes.Count > 0)
        {
            var scene = _removeScenes.Dequeue();
            scene.Destroy();
        }
    }
}