using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleRPG.Core;

public static class Time
{
    [DllImport("winmm.dll")] 
    internal static extern uint timeBeginPeriod(uint period);

    [DllImport("winmm.dll")]
    internal static extern uint timeEndPeriod(uint period);

    /// <summary>
    /// 游戏启动的时间，单位毫秒
    /// </summary>
    private static long _startTime;

    /// <summary>
    /// 上一帧开始时间，单位毫秒
    /// </summary>
    private static long _lastFrameStartTime;
    /// <summary>
    /// 当前帧开始时间，单位毫秒
    /// </summary>
    private static long _curFrameStartTime;

    private static long _totalPassTimeOneSecond;
    private static int _totalFpsThisSecond;
    private static int _totalFpsLastSecond;

    public static int MaxFps { get; set; } = 30;
    public static int Fps => _totalFpsLastSecond;
    public static float FrameInterval => 1f / MaxFps;

    /// <summary>
    /// 从帧开始已经过去的时间
    /// </summary>
    public static float DeltaTime => (GetCurTime() - _lastFrameStartTime) /1000f;

    public static void Init()
    {
        _startTime = GetCurTime();
        _lastFrameStartTime = _startTime;
        _curFrameStartTime = _startTime;
    }

    public static void StartFrame()
    {
        var curTime = GetCurTime();
        _totalPassTimeOneSecond += curTime - _curFrameStartTime;
        _lastFrameStartTime = _curFrameStartTime;
        _curFrameStartTime = curTime;

        if (_totalPassTimeOneSecond > 1000)
        {
            _totalPassTimeOneSecond = 0;
            _totalFpsLastSecond = _totalFpsThisSecond;
            _totalFpsThisSecond = 0;
        }

        _totalFpsThisSecond += 1;
    }

    public static void Sleep()
    {
        long sleepTime = (long) (FrameInterval * 1000 - (GetCurTime() - _curFrameStartTime));
        if (sleepTime > 0)
        {
            timeBeginPeriod(1);
            Thread.Sleep((int)sleepTime);
            timeEndPeriod(1);
        }
    }

    private static long GetCurTime() => DateTime.Now.Ticks / 10000L;
}