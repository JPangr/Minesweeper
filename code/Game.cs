namespace Minesweeper.code;

public class Game
{
    public required PlayingField PlayingField;
    
    public Game(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                PlayingField = new(width: 9, height: 9, bombCount: 10);
                break;
            case Difficulty.Normal:
                PlayingField = new(width: 16, height: 16, bombCount: 40);
                break;
            case Difficulty.Hard:
                PlayingField = new(width: 30, height: 16, bombCount: 99);
                break;
        }
    }

    public Outcome Play()
    {
        // main game loop here
        return Outcome.Win;
    }
}