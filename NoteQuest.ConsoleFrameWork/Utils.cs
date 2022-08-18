using System;
using System.Runtime.CompilerServices;

namespace ConsoleRPG.Core;

public static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        if ((double)min > (double)max)
            throw new ArgumentException($"min value {min} greater max value {max}");
        if ((double) value < (double) min)
            return min;
        return (double) value > (double) max ? max : value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException($"min value {min} greater max value {max}");
        }

        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }

        return value;
    }
}