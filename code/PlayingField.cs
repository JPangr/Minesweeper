namespace Minesweeper.code;
using System;
public class PlayingField
{
    private bool BombsSpawned;

    public readonly int BombCount = 0;
    public int Height;
    public int Width;
    public int Covered;
    public HashSet<Tuple<int, int>> Bombs = new();
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

    private void SpawnBombs(int initialX, int initialY)
    {
        Random random = new Random();
        int i = 0;
        while (i < BombCount)
        {
            // a better solution probably exists
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            Tuple<int, int> bomb = new(x, y);
            if (Bombs.Contains(bomb) && !(x == initialX && y == initialY))
            {
                Bombs.Add(new Tuple<int, int>(x, y));
                i++;
            }
        }
    }

    public Outcome Reveal(int x, int y)
    {
        if (!BombsSpawned)
        {
            SpawnBombs(x, y);
            BombsSpawned = true;
            return Outcome.Ongoing;
        }

        if (Bombs.Contains(new Tuple<int, int>(x, y)))
        {
            return Outcome.Loss;
        }

        List<Tuple<int, int>> surrounding = ListSurrounding(x, y);
        Field[y, x].State = TileState.Empty;
        Field[y, x].AdjacentBombs = CountSurroundingBombs(surrounding);
        UncoverTiles(surrounding);
        
        return Covered == 0 ?  Outcome.Win : Outcome.Ongoing;
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
                Tuple<int, int> position = new(k, i);
                if (Bombs.Contains(position))
                {
                    surrounding.Add(position);
                }
            }
        }
        return surrounding;
    }

    private int CountSurroundingBombs(List<Tuple<int, int>> surrounding)
    {
        int count = 0;
        foreach (Tuple<int, int> coords in surrounding)
        {   
            count += Bombs.Contains(coords) ? 1 : 0;
        }
        
        return count;
    }

    private void UncoverTiles(List<Tuple<int, int>> surrounding)
    {
        //TODO: find out how empty tiles are uncovered
        foreach (Tuple<int, int> tile in surrounding)
        {
            if (Field[tile.Item1, tile.Item2].State == TileState.Empty || Bombs.Contains(tile))
            {
                continue;
            }
            List<Tuple<int, int>> nextSurrounding = ListSurrounding(tile.Item1, tile.Item2);
            Field[tile.Item1, tile.Item2].AdjacentBombs = CountSurroundingBombs(nextSurrounding);
            if (Field[tile.Item1, tile.Item2].AdjacentBombs == 0)
            {
                UncoverTiles(nextSurrounding);
            }
            
        }
    }
}