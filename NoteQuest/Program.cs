using NoteQuest.ConsoleFrameWork;
using NoteQuest.Scenes;

namespace NoteQuest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Game.StartGame(new InitScene());
        }
    }
}