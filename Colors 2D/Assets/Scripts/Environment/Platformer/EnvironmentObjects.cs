using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentObjects : MonoBehaviour
{
    public ColorTone color;

    public abstract void ChangeColor(ColorTone color);

    
}
