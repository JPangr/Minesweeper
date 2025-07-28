namespace Minesweeper.code;
using System;
public class PlayingField
{
    private bool generated;

    public readonly int BombCount = 0;
    public int Height;
    public int Width;
    public int Covered;
    public Tile[,] Field
    {
        get;
        private set;
    }

    public PlayingField(int width, int height)
    {
        Height = height;
        Width = width;
        Covered = height * width;
        Field = new Tile[height, width];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Field[h, w] = new Tile();
            }
        }
    }

    private void GenerateField(int initialX, int initialY)
    {
        Random random = new Random();
        int i = 0;
        while (i < BombCount)
        {
            // a better solution probably exists
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            if (Field[y, x].State == TileState.Empty && !(x == initialX && y == initialY))
            {
                Field[y, x].State = TileState.Bomb;
                i++;
            }
        }
    }

    public Outcome Reveal(int x, int y)
    {
        if (!generated)
        {
            GenerateField(x, y);
            generated = true;
        }

        if (Field[y, x].State == TileState.Bomb)
        {
            return Outcome.Loss;
        }
        
        Field[y, x].AdjacentBombs = CountSurroundingBombs(x, y);
        UncoverTiles(x, y);
        
        return Covered == 0 ?  Outcome.Win : Outcome.Loss;
    }

    private List<Tuple<int, int>> ListSurrounding(int x, int y)
    {
        List<Tuple<int, int>> surrounding = new();
        int left = (x == 0) ? x : x - 1;
        int right = (x == Width - 1) ? x : x + 1;
        int bottom = (y == 0) ? y : y - 1;
        int top = (y == Height - 1) ? y : y + 1;
            
        for (int i = left; i <= right; i++)
        {
            for (int k = bottom; k <= top; k++)
            {
                if (Field[k, i].State == TileState.Bomb)
                {
                    surrounding.Add(new Tuple<int,int>(i, k));
                }
            }
        }
        return surrounding;
    }

    private int CountSurroundingBombs(int x, int y)
    {
        int count = 0;
        foreach (Tuple<int, int> coords in ListSurrounding(x, y))
        {   
            int newX = coords.Item1;
            int newY = coords.Item2;
            count += Field[newY, newX].State == TileState.Bomb ? 1 : 0;
        }
        
        return count;
    }

    private void UncoverTiles(int x, int y)
    {
        //TODO: find out how empty tiles are uncovered
    }
}