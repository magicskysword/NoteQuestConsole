using System;
using ConsoleRPG.Core;

namespace NoteQuest.Scenes;

public class TitleScene : SceneBase
{
    private int _selectIndex = 0;
    private int _indexLimit = 2;

    public override void Start()
    {
        Draw.Clear();

        Draw.DrawFrameStyle1(0, 0, 100, 30);
        Draw.DrawText("NoteQuest", 50, 5, TextHorizontalAlign.Center);

        Draw.DrawText("开始游戏", 50, 10, TextHorizontalAlign.Center);
        Draw.DrawText("退出游戏", 50, 12, TextHorizontalAlign.Center);
        
        Draw.DrawText("按[↑][↓]切换选项，按[Space]确认。", 50, 16, TextHorizontalAlign.Center);

        DrawCursor();
    }

    public override void Update()
    {
        var lastIndex = _selectIndex;

        if (Input.IsKeyDown(ConsoleKey.UpArrow))
            _selectIndex -= 1;
        if (Input.IsKeyDown(ConsoleKey.DownArrow))
            _selectIndex += 1;
        
        if (_selectIndex < 0) 
            _selectIndex = _indexLimit - 1;
        if (_selectIndex >= _indexLimit) 
            _selectIndex = 0;

        if (lastIndex != _selectIndex)
        {
            DrawCursor();
        }

        if (Input.IsKeyDown(ConsoleKey.Spacebar))
            HandleSelect();
        
        Draw.EraseArea(4, 4, 10, 1);
        Draw.DrawText($"FPS:{Time.Fps}", 4, 4);
    }

    public void DrawCursor()
    {
        Draw.Erase(42, 10, 2);

        Draw.Erase(42, 12, 2);

        switch (_selectIndex)
        {
            case 0:
                Draw.DrawText(BaseChar.Star, 42, 10);
                break;
            case 1:
                Draw.DrawText(BaseChar.Star, 42, 12);
                break;
        }
    }

    public void HandleSelect()
    {
        switch (_selectIndex)
        {
            case 0:
                SceneManager.SwitchScene(new CreateRoleScene());
                break;
            case 1:
                Game.EndGame();
                break;
        }
    }
}