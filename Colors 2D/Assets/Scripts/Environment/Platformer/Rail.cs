using UnityEngine;

public class Rail : EnvironmentObjects
{
    public override void ChangeColor(ColorTone color)
    {
        //En futuro implementar cambio con luz, extractor, etc.
    }
}

public enum ColorTone
{
    White,
    Red,
    Yellow,
    Blue,
    Green,
    Orange,
    Violet
}