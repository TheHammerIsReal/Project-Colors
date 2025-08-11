using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : InteractuablesGlobal
{
    Sequence _doorSeq;
    [SerializeField] float _doorSmooth;
    [SerializeField] float _doorYValue;

    public override void Interact(params object[] parameters)
    {
        float cooldown = (float)parameters[0];
        bool back = (bool)parameters[1];

        var initialScale = transform.localScale;


        _doorSeq.Kill();

        _doorSeq = DOTween.Sequence();

        _doorSeq.Append(transform.DOScaleY(_doorYValue, _doorSmooth));

        if (back)
        {
            _doorSeq.AppendInterval(cooldown);
            _doorSeq.Append(transform.DOScaleY(initialScale.y, _doorSmooth));
        }
    }
}

