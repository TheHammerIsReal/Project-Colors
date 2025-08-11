using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [SerializeField] ColorTin _orangeTin , _violetTin , _greenTin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public ColorTin Merge(ColorTone color1 , ColorTone color2)
    {
        if (color1 == ColorTone.Red && color2 == ColorTone.Yellow)
        {  
            return _orangeTin;
        }

        else if (color1 == ColorTone.Yellow && color2 == ColorTone.Red)
        {
            return _orangeTin;
        }

        else if (color1 == ColorTone.Red && color2 == ColorTone.Blue)
        {
           
            return _violetTin;
        }

        else if (color1 == ColorTone.Blue && color2 == ColorTone.Red)
        {
            return _violetTin;
        }

        else if (color1 == ColorTone.Yellow && color2 == ColorTone.Blue)
        {
            return _greenTin;
        }

        else if (color1 == ColorTone.Blue && color2 == ColorTone.Yellow)
        {
            return _greenTin;
        }

        else return default;

    }
}
