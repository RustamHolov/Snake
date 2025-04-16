using System.Text;
public class Food : Cell
{

    public Food(int size) : base(size, empty: false)
    {
    }

    public override string Render()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < Height; i++)
        {
            builder.AppendLine(new string('░', Width));
        }
        return builder.ToString();
    }

}