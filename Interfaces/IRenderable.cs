public interface IRenderable
{
    int Height { get; }
    int Width { get; }
    char[,] Canvas { get; }
    string Render();
}
