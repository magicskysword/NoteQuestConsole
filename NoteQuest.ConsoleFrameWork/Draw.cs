using System;
using System.Text;

namespace ConsoleRPG.Core;

public static class Draw
{
    private static int CurBufferIndex { get; set; } = 0;
    private static int ScreenWidth { get; set; } = 0;
    private static int ScreenHeight { get; set; } = 0;
    private static int CursorX = 0;
    private static int CursorY = 0;
    private static ConsoleBuffer ConsoleBuffer { get; set; } = null!;
    public static short ColorCode { get; set; } = 15;
    public static bool CursorVisible
    {
        get => Console.CursorVisible;
        set => Console.CursorVisible = value;
    }
    public static void Init()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        CursorVisible = false;
    }
    public static void Clear()
    {
        ConsoleBuffer.Clear();
    }
    public static void Print()
    {
        ConsoleBuffer.Print();
    }
    public static void SetScreenSize(int width, int height)
    {
        ScreenWidth = width;
        ScreenHeight = height;
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
        ConsoleBuffer = new ConsoleBuffer(width, height,width, height);
        Console.Clear();
    }
    public static void Write(char c)
    {
        ConsoleBuffer.Draw(c.ToString(),CursorX,CursorY,ColorCode);
    }
    public static void Write(string str)
    {
        ConsoleBuffer.Draw(str,CursorX,CursorY,ColorCode);
    }
    public static void SetCursorPosition(int x,int y)
    {
        CursorX = x;
        CursorY = y;
    }

    public static void DrawLineH(char getChar, int x, int y, int width) => DrawLineH(getChar.ToString(), x, y, width);
    public static void DrawLineH(string getStr, int x, int y, int width)
    {
        var strLen = DisplayLength(getStr);

        if (IsOuterOfScreen(x, y, strLen))
            return;
        for (int i = 0; i < width; i += strLen)
        {
            if (IsOuterOfScreen(x + i, y, strLen))
                break;
            SetCursorPosition(x + i, y);
            Write(getStr);
        }
    }

    public static void DrawLineV(char getChar, int x, int y, int height) => DrawLineV(getChar.ToString(), x, y, height);
    public static void DrawLineV(string getStr, int x, int y, int height)
    {
        var strLen = DisplayLength(getStr);

        for (int i = 0; i < height - 1; i++)
        {
            if (IsOuterOfScreen(x, y+i, strLen))
                break;
            SetCursorPosition(x, y + i);
            Write(getStr);
        }
    }

    public static void DrawFrameStyle1(int x, int y, int width, int height)
    {
        DrawLineH(BaseChar.FrameH, x + 2, y, width - 4);
        DrawLineH(BaseChar.FrameH, x + 2, y + height - 1, width - 4);

        DrawLineV(BaseChar.FrameV, x, y + 1, height - 1);
        DrawLineV(BaseChar.FrameV, x + width - 2, y + 1, height - 1);

        DrawText(BaseChar.FrameTL, x, y);
        DrawText(BaseChar.FrameTR, x + width - 2, y);
        DrawText(BaseChar.FrameBL, x, y + height - 1);
        DrawText(BaseChar.FrameBR, x + width - 2, y + height - 1);
    }

    public static void DrawRectangle(char getChar, int x, int y, int width, int height, bool fill = false) =>
        DrawRectangle(getChar.ToString(), x, y, width, height, fill);
    public static void DrawRectangle(string getStr, int x, int y, int width, int height, bool fill = false)
    {
        var strLen = DisplayLength(getStr);

        if (fill)
        {
            for (int i = 0; i < height; i++)
            {
                SetCursorPosition(x, y + i);
                for (int j = 0; j < width; j+=strLen)
                {
                    if (IsOuterOfScreen(x + i, y + j, strLen))
                        break;
                    Write(getStr);
                }
            }
        }
        else
        {
            DrawLineH(getStr, x, y, width);
            DrawLineH(getStr, x, y + height - 1, width);

            DrawLineV(getStr, x, y+1, height-1);
            DrawLineV(getStr, x + width-strLen, y+1, height-1);
        }
    }

    public static void DrawProgress(float value, int x, int y, int width)
    {
        value = Utils.Clamp(value, 0f, 1f);

        var progressCount = (width - 2) / 2;
        var progressInterval = 1f / progressCount;

        DrawText(BaseChar.ProgressL, x, y);
        DrawText(BaseChar.ProgressR, x + width - 1, y);
        for (int i = 0; i < progressCount; i++)
        {
            SetCursorPosition(x + 1 + i * 2, y);
            if (value >= progressInterval * i)
            {
                var per = (value - progressInterval * i) / progressInterval;
                if (per < 1f / 7)
                {
                    Write(BaseChar.Progress0);
                }
                else if (per < 2f / 7)
                {
                    Write(BaseChar.Progress1);
                }
                else if (per < 3f / 7)
                {
                    Write(BaseChar.Progress2);
                }
                else if (per < 4f / 7)
                {
                    Write(BaseChar.Progress3);
                }
                else if (per < 5f / 7)
                {
                    Write(BaseChar.Progress4);
                }
                else if (per < 6f / 7)
                {
                    Write(BaseChar.Progress5);
                }
                else if (per < 1)
                {
                    Write(BaseChar.Progress6);
                }
                else
                {
                    Write(BaseChar.Progress7);
                }
            }
            else
            {
                Write(BaseChar.Progress0);
            }
        }
    }
    public static void DrawText(char getStr, int x, int y, TextHorizontalAlign horizontal = TextHorizontalAlign.Left) =>
        DrawText(getStr.ToString(), x, y, horizontal);
    public static void DrawText(string getStr, int x, int y,TextHorizontalAlign horizontal = TextHorizontalAlign.Left)
    {
        var strLen = DisplayLength(getStr);
        switch (horizontal)
        {
            case TextHorizontalAlign.Left:
                break;
            case TextHorizontalAlign.Center:
                x -= strLen / 2;
                break;
            case TextHorizontalAlign.Right:
                x -= strLen;
                break;
        }

        x = Utils.Clamp(x, 0, Console.BufferWidth);
        SetCursorPosition(x, y);
        Write(getStr);
    }
    public static void DrawMText(string getStr, int x, int y,int width,int xInterval = 0,int yInterval = 0,TextHorizontalAlign horizontal = TextHorizontalAlign.Left)
    {
        
        switch (horizontal)
        {
            case TextHorizontalAlign.Left:
                break;
            case TextHorizontalAlign.Center:
                x -= width / 2;
                break;
            case TextHorizontalAlign.Right:
                x -= width;
                break;
        }

        x = Utils.Clamp(x, 0, Console.BufferWidth);
        var xIndex = 0;
        var yIndex = 0;
        foreach (var c in getStr)
        {
            if (c == '\n')
            {
                xIndex = 0;
                yIndex += 1 + yInterval;
                continue;
            }

            if (xIndex >= width)
            {
                xIndex = 0;
                yIndex += 1 + yInterval;
            }
            var strLen = DisplayLength(c);
            if (IsOuterOfScreen(x + xIndex, y + yIndex,strLen))
                break;

            SetCursorPosition(x + xIndex, y + yIndex);
            Write(c);
            xIndex += strLen;
            xIndex += xInterval;
        }
    }
    public static void DrawAndEraseText(string getStr, int x, int y, int width,
        TextHorizontalAlign horizontal = TextHorizontalAlign.Left)
    {
        var startX = x;
        switch (horizontal)
        {
            case TextHorizontalAlign.Left:
                break;
            case TextHorizontalAlign.Center:
                
                startX -= width / 2;
                break;
            case TextHorizontalAlign.Right:
                startX -= width;
                break;
        }
        for (int i = 0; i < width; i++)
        {
            SetCursorPosition(startX + i, y);
            Write(' ');
        }
        DrawText(getStr,x,y,horizontal);
    }

    public static void EraseText(char getChar, int x, int y,TextHorizontalAlign horizontal = TextHorizontalAlign.Left) => EraseText(getChar.ToString(), x, y,horizontal);
    public static void EraseText(string getStr, int x, int y,TextHorizontalAlign horizontal = TextHorizontalAlign.Left)
    {
        var strLen = DisplayLength(getStr);
        switch (horizontal)
        {
            case TextHorizontalAlign.Left:
                break;
            case TextHorizontalAlign.Center:
                x -= strLen / 2;
                break;
            case TextHorizontalAlign.Right:
                x -= strLen;
                break;
        }
        SetCursorPosition(x, y);
        for (int i = 0; i < strLen; i++)
        {
            SetCursorPosition(x + i, y);
            Write(' ');
        }
    }
    public static void Erase(int x, int y,int width = 1)
    {
        for (int i = 0; i < width; i ++)
        {
            SetCursorPosition(x+i, y);
            Write(' ');
        }
    }

    public static void EraseArea(int x, int y, int width, int height)
    {
        for (int i = 0; i < height; i ++)
        {
            for (int j = 0; j < width; j++)
            {
                SetCursorPosition(x + j, y + i);
                Write(' ');
            }
        }
    }
    public static bool IsOuterOfScreen(int x, int y, int length)
    {
        return x < 0 || y < 0 || x + length > Console.BufferWidth || y > Console.BufferHeight;
    }
    public static int DisplayLength(string str)
    {
        int lengthCount = 0;
        var splits = str.ToCharArray();
        for (int i = 0; i < splits.Length; i++)
        {
            if (splits[i] == '\t')
            {
                lengthCount += 8 - lengthCount % 8;
            }
            else
            {
                lengthCount += Encoding.GetEncoding("GB18030").GetByteCount(splits[i].ToString());
            }
        }
        return lengthCount;
    }
    public static int DisplayLength(char str)
    {
        switch (str)
        {
            case ' ':
                return 1;
            case >= 'a' and <= 'z':
            case >= 'A' and <= 'Z':
            case >= '0' and <= '9':
                return 1;
        }
        
        return Encoding.GetEncoding("GB18030").GetByteCount(str.ToString()); ;
    }
}