namespace Minesweeper.code;
using System
public class PlayingField
{
    private bool generated = false;
    
    public readonly bombCount = 0
    public int Height;
    public int Width;
    public int Covered;
    public int[,] Field
    {
        get;
        private set;
    }

    public PlayingField(int width, int height)
    {
        Height = height;
        Width = width;
        Covered = height * width;
        Field = new int[height, width];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Field[h, w] = 0;
            }
        }
    }

    private void GenerateField(int initialX, int initialY)
    {
        Random random = new Random();
        int i = 0;
        while (i < bombCount)
        {
            // a better solution probably exists
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            if (Field[y, x] == 0 and !(x == initialX and y == initialY))
            {
                Field[y, x] = -1;
                i++;
            }
        }
    }

    public Outcome Reveal(int x, int y)
    {
        if (!generated)
        {
            this.GenerateField(x, y);
            generated = true;
        }

        if (Field[y, x] == Tile.Bomb)
        {
            return Outcome.Loss;
        }
        
        Field[y, x] = CountSurroundingBombs(x, y);
        UncoverTiles(x, y);
        
        return Covered == 0 ?  Outcome.Win : Outcome.Loss;
    }

    private int CountSurroundingBombs(int x, int y)
    {
        int count = 0;

        int left = (x == 0) ? x : x - 1;
        int right = (x == Width - 1) ? x : x + 1;
        int bottom = (y == 0) ? y : y - 1;
        int top = (y == Height - 1) ? y : y + 1;
            
        for (int i = left; i <= right; i++)
        {
            for (int k = bottom; k <= top; k++)
            {
                if (Field[k, i] == Tile.Bomb)
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    //TODO: find out how empty tiles are uncovered
    private void UncoverTiles(int x, int y);
}