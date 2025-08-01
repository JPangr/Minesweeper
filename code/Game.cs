using System.Diagnostics;

namespace Minesweeper.code;

public class Game
{
    public required PlayingField PlayingField;
    private readonly Difficulty _selectedDifficulty;
    private readonly int _width;
    private readonly int _height;
    private readonly int _bombCount;
    public Game(Difficulty difficulty)
    {   
        _selectedDifficulty = difficulty;
        switch (_selectedDifficulty)
        {
            case Difficulty.Easy:
                _width = 9;
                _height = 9;
                _bombCount = 10;
                break;
            case Difficulty.Normal:
                _width = 16;
                _height = 16;
                _bombCount = 40;
                break;
            case Difficulty.Hard:
                _width = 30;
                _height = 16;
                _bombCount = 99;
                break;
        }
        PlayingField = new(_width, _height, _bombCount);
    }

    public Outcome Play()
    {
        while (true)
        {
            int x = 0; //input from UI here
            int y = 0; //input from UI here

            Outcome outcome = PlayingField.Reveal(x, y);
            if (outcome == Outcome.Ongoing) continue;
            
            return outcome;
        }
        Debug.Assert(false);
    }

    public void Restart()
    {
        PlayingField = new(_width, _height, _bombCount);
        Play();
    }
}