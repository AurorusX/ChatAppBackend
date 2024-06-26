using System;

public class RandomController
{
    private Random _random;

    public RandomController()
    {
        _random = new Random();
    }

    public string GetRandomMovement()
    {
        int direction = _random.Next(4); // Generate a random number between 0 and 3 (inclusive)

        switch (direction)
        {
            case 0:
                return "Forward";
            case 1:
                return "Backward";
            case 2:
                return "Left";
            case 3:
                return "Right";
            default:
                return "Stop"; // Handle unexpected outcome
        }
    }

    public int GetRandomSpeed()
    {
        return _random.Next(101); // Generate a random integer between 0 and 100 (inclusive)
    }

    public bool GetRandomAction()
    {
        return _random.NextDouble() > 0.5; // Generate a random boolean (true or false)
    }
}
