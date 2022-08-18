using System;

namespace NoteQuest.ConsoleFrameWork;

public static class Input
{
    private static ConsoleKeyInfo? _lastKeyInfo = null;
    private static ConsoleKeyInfo? _keyInfo = null;

    private static ConsoleKey? _downKey = null;
    private static ConsoleKey? _pressKey = null;
    private static ConsoleKey? _upKey = null;

    public static void Init()
    {

    }

    public static void Update()
    {
        _lastKeyInfo = _keyInfo;
        if (Console.KeyAvailable)
        {
            _keyInfo = Console.ReadKey(true);
        }
        else
        {
            _keyInfo = null;
        }

        _pressKey = _keyInfo?.Key;
        _downKey = _lastKeyInfo?.Key != _keyInfo?.Key ? _keyInfo?.Key : null;
        _upKey = _lastKeyInfo?.Key != _keyInfo?.Key ? _lastKeyInfo?.Key : null;
    }

    public static bool IsKeyPress(ConsoleKey key)
    {
        return _pressKey == key;
    }

    public static bool IsKeyDown(ConsoleKey key)
    {
        return _downKey == key;
    }

    public static bool IsKeyUp(ConsoleKey key)
    {
        return _upKey == key;
    }

    public static char GetInputChar()
    {
        return _keyInfo?.KeyChar ?? '\0';
    }

    public static ConsoleKey? GetDownKey()
    {
        return _downKey;
    }
}