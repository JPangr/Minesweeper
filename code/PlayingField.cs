namespace Minesweeper.code;
using System;
public class PlayingField
{
    private bool _bombsSpawned;

    private readonly int _bombCount;
    private readonly int _height;
    private readonly int _width;
    private readonly int _covered;
    private readonly HashSet<Tuple<int, int>> _bombs = new();

    public Tile[,] Field
    {
        get;
        private set;
    }

    public PlayingField(int width, int height, int bombCount)
    {
        _height = height;
        _width = width;
        _covered = _height * _width - _bombCount;
        _bombCount = bombCount;
        Field = new Tile[_height, _width];
        for (int h = 0; h < _height; h++)
        {
            for (int w = 0; w < _width; w++)
            {
                Field[h, w] = new Tile();
            }
        }
    }

    private void SpawnBombs(int initialX, int initialY)
    {
        Random random = new Random();
        int i = 0;
        while (i < _bombCount)
        {
            // a better solution probably exists
            int x = random.Next(0, _width);
            int y = random.Next(0, _height);
            Tuple<int, int> bomb = new(x, y);
            if (_bombs.Contains(bomb) && !(x == initialX && y == initialY))
            {
                _bombs.Add(new Tuple<int, int>(x, y));
                i++;
            }
        }
    }

    public Outcome Reveal(int x, int y)
    {
        if (!_bombsSpawned)
        {
            SpawnBombs(x, y);
            _bombsSpawned = true;
            return Outcome.Ongoing;
        }

        if (_bombs.Contains(new Tuple<int, int>(x, y)))
        {
            return Outcome.Loss;
        }

        List<Tuple<int, int>> surrounding = ListSurrounding(x, y);
        Field[y, x].State = TileState.Empty;
        Field[y, x].AdjacentBombs = CountSurroundingBombs(surrounding);
        UncoverTiles(surrounding);
        
        return _covered == 0 ?  Outcome.Win : Outcome.Ongoing;
    }

    private List<Tuple<int, int>> ListSurrounding(int x, int y)
    {
        List<Tuple<int, int>> surrounding = new();
        int left = (x == 0) ? x : x - 1;
        int right = (x == _width - 1) ? x : x + 1;
        int bottom = (y == 0) ? y : y - 1;
        int top = (y == _height - 1) ? y : y + 1;
            
        for (int i = left; i <= right; i++)
        {
            for (int k = bottom; k <= top; k++)
            {
                Tuple<int, int> position = new(k, i);
                if (_bombs.Contains(position))
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
            count += _bombs.Contains(coords) ? 1 : 0;
        }
        
        return count;
    }

    private void UncoverTiles(List<Tuple<int, int>> surrounding)
    {
        foreach (Tuple<int, int> tile in surrounding)
        {
            if (Field[tile.Item1, tile.Item2].State == TileState.Empty || _bombs.Contains(tile))
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
    
    public void FlagTile(int x, int y)
    {
        Field[y, x].State = TileState.Flagged;
    }
}