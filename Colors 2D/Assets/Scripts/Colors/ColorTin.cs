using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTin : MonoBehaviour
{
    public ColorTone color;
    public ColorType colorType;

    public Sprite image;


    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player != null)
        {
            //Logica UI
        }
    }
}

public enum ColorType
{
    Primary,
    Secundary
}
