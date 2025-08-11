using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformsAdaptive : InteractuablesGlobal
{
    [SerializeField] int _platformScaleSmooth;
    [SerializeField] float _platformXValue;

    Sequence _platformSeq;

    public override void Interact(params object[] parameters)
    {
        float cooldown = (float)parameters[0];

        transform.localScale = new Vector3(0f, 0.231539f, 1f);

        var initialScale = transform.localScale;


        _platformSeq.Kill();

        _platformSeq = DOTween.Sequence();

        _platformSeq.Append(transform.DOScaleX(_platformXValue , _platformScaleSmooth));
        _platformSeq.AppendInterval(cooldown);
        
        _platformSeq.Append(transform.DOScaleX(initialScale.x, _platformScaleSmooth));
    }

    
}
