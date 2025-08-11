using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView
{
    Player _player;
    Dictionary<ColorTone, Color> _colorSkins = new Dictionary<ColorTone, Color>();
    SpriteRenderer _renderer;

    List<Image> _nullImages = new List<Image>();
    List<Image> _fillImages = new List<Image>();

    public PlayerView(Player player, SpriteRenderer renderer)
    {
        _player = player;
        _renderer = renderer;
        _nullImages = player.nullImages;
        _fillImages = player.fillImages;
    }

    public void ChangeSkin(ColorTone color)
    {
         _renderer.color = _colorSkins[color];
    }

    public void BlindColors()
    {
        _colorSkins.Add(ColorTone.White, Color.white);
        _colorSkins.Add(ColorTone.Red, Color.red);
        _colorSkins.Add(ColorTone.Blue, Color.blue);
        _colorSkins.Add(ColorTone.Yellow, Color.yellow);
        _colorSkins.Add(ColorTone.Green, Color.green);
        _colorSkins.Add(ColorTone.Orange, _player.orange);
        _colorSkins.Add(ColorTone.Violet, Color.magenta);
    }

    public void ChangeUI(ColorTin tin , int index)
    {
        if (index != 2)
        {
            _nullImages[index].gameObject.SetActive(false);
            _fillImages[index].sprite = tin.image;
            _fillImages[index].gameObject.SetActive(true);
        }
    }

    public void DeleteTin(int index)
    {
        _fillImages[index].gameObject.SetActive(false);
        _nullImages[index].gameObject.SetActive(true);
    }

    public void ExchangeImages(List<ColorTin> colorTins)
    {
        for (int i = 0; i < _fillImages.Count; i++)
        {
            _fillImages[i].sprite = colorTins[i].image;
        }
        
    }

    public void TargetSwitch(GameObject target, bool activate)
    {
        target.gameObject.SetActive(activate);
    }

    public void TargetUpdate(GameObject target,Transform transform, Vector3 mousePos, float radius, Transform spawnPointShoot)
    {
        Vector3 direction = mousePos - transform.position;

        if (direction.magnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 offset = new Vector3(radius, 0, 0);

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 newPos = transform.position + rotation * offset;

        target.transform.position = newPos;

        spawnPointShoot.position = newPos;
    }

    public void LandAnimation(bool isInWall)
    {
        //if(isInWall) animacion, sprite etc.
    }

    public void JumpAnimation(ColorTone color)
    {
        // particles(color) 
    }
}
