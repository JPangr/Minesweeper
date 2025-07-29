namespace Minesweeper.code;

public struct Tile
{
    public TileState State;
    public int AdjacentBombs;

    public Tile()
    {
        State = TileState.Unknown;
        AdjacentBombs = 0;
    }
}