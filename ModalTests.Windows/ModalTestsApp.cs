using Stride.Engine;

namespace ModalTests
{
    class ModalTestsApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
