﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Text;

/*
 * Copyright [2012] [Jeff R Baker]

 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at    
 * 
 *          http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * v 1.2.0
 */
///<summary>
///This class allows for a double buffer in Visual C# cmd promt. 
///The buffer is persistent between frames.
///</summary>
class ConsoleBuffer
{
    private int width;
    private int height;
    private int windowWidth;
    private int windowHeight;
    private ConsoleHandle h;
    private CharInfo[] buf;
    private SmallRect rect;

    public const Int32 STD_OUTPUT_HANDLE = -11;

    [DllImportAttribute("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool WriteConsoleOutput(
        ConsoleHandle hConsoleOutput,
        CharInfo[] lpBuffer,
        Coord dwBufferSize,
        Coord dwBufferCoord,
        ref SmallRect lpWriteRegion);

    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
        private short X;
        private short Y;

        public Coord(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };


    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public char UnicodeChar;

        [FieldOffset(0)]
        public byte bAsciiChar;

        [FieldOffset(2)]
        public short Attributes;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect
    {
        private short Left;
        private short Top;
        private short Right;
        private short Bottom;

        public void setDrawCord(short l, short t)
        {
            Left = l;
            Top = t;
        }

        public short DrawCordX()
        {
            return Left;
        }

        public short DrawCordY()
        {
            return Top;
        }

        public void setWindowSize(short r, short b)
        {
            Right = r;
            Bottom = b;
        }
    }

    /// <summary>
    /// Consctructor class for the buffer. Pass in the width and height you want the buffer to be.
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    public
        ConsoleBuffer(int Width, int Height, int wWidth,
            int wHeight) // Create and fill in a multideminsional list with blank spaces.
    {
        if (Width > wWidth || Height > wHeight)
        {
            throw new System.ArgumentException(
                "The buffer width and height can not be greater than the window width and height.");
        }

        h = GetStdHandle(STD_OUTPUT_HANDLE);
        width = Width;
        height = Height;
        windowWidth = wWidth;
        windowHeight = wHeight;
        buf = new CharInfo[width * height];
        rect = new SmallRect();
        rect.setDrawCord(0, 0);
        rect.setWindowSize((short)windowWidth, (short)windowHeight);


        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Clear();
    }

    /// <summary>
    /// This method draws any text to the buffer with given color.
    /// To chance the color, pass in a value above 0. (0 being black text, 15 being white text).
    /// Put in the starting width and height you want the input string to be.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="attribute"></param>
    public void Draw(String str, int x, int y, short attribute) //Draws the image to the buffer
    {
        if (x > windowWidth - 1 || y > windowHeight - 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }

        if (str != null)
        {
            Char[] temp = str.ToCharArray();

            int tc = 0;
            foreach (Char le in temp)
            {
                buf[(x + tc) + (y * width)].UnicodeChar =
                    le; //Height * width is to get to the correct spot (since this array is not two dimensions).

                if (attribute != 0)
                    buf[(x + tc) + (y * width)].Attributes = attribute;

                var charLen = NoteQuest.ConsoleFrameWork.Draw.DisplayLength(le);
                if (charLen == 2)
                {
                    buf[(x + tc + 1) + (y * width)].Attributes = attribute;
                }
                tc+=charLen;
            }
        }
    }

    /// <summary>
    /// Prints the buffer to the screen.
    /// </summary>
    public void Print() //Paint the image
    {
        if (!h.IsInvalid)
        {
            bool b = WriteConsoleOutput(h, buf, new Coord((short)width, (short)height), new Coord((short)0, (short)0),
                ref rect);
        }
    }

    /// <summary>
    /// Clears the buffer and resets all character values back to 32, and attribute values to 1.
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < buf.Length; i++)
        {
            buf[i].Attributes = 15;
            buf[i].UnicodeChar = '\u0020';
        }
    }

    /// <summary>
    /// Pass in a buffer to change the current buffer.
    /// </summary>
    /// <param name="b"></param>
    public void setBuf(CharInfo[] b)
    {
        if (b == null)
        {
            throw new System.ArgumentNullException();
        }

        buf = b;
    }

    /// <summary>
    /// Set the x and y cordnants where you wish to draw your buffered image.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setDrawCord(short x, short y)
    {
        rect.setDrawCord(x, y);
    }

    /// <summary>
    /// Clear the designated row and make all attribues = 1.
    /// </summary>
    /// <param name="row"></param>
    public void clearRow(int row)
    {
        for (int i = (row * width); i < ((row * width + width)); i++)
        {
            if (row > windowHeight - 1)
            {
                throw new System.ArgumentOutOfRangeException();
            }

            buf[i].Attributes = 15;
            buf[i].UnicodeChar = '\u0020';
        }
    }

    /// <summary>
    /// Clear the designated column and make all attribues = 1.
    /// </summary>
    /// <param name="col"></param>
    public void clearColumn(int col)
    {
        if (col > windowWidth - 1)
        {
            throw new System.ArgumentOutOfRangeException();
        }

        for (int i = col; i < windowHeight * windowWidth; i += windowWidth)
        {
            buf[i].Attributes = 15;
            buf[i].UnicodeChar = '\u0020';
        }
    }

    /// <summary>
    /// This function return the character and attribute at given location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>
    /// byte character
    /// byte attribute
    /// </returns>
    public KeyValuePair<byte, byte> getCharAt(int x, int y)
    {
        if (x > windowWidth || y > windowHeight)
        {
            throw new System.ArgumentOutOfRangeException();
        }

        return new KeyValuePair<byte, byte>((byte)buf[((y * width + x))].UnicodeChar,
            (byte)buf[((y * width + x))].Attributes);
    }

    public class ConsoleHandle : SafeHandleMinusOneIsInvalid
    {
        public ConsoleHandle() : base(false)
        {
        }

        protected override bool ReleaseHandle()
        {
            return true; //releasing console handle is not our business
        }
    }
}