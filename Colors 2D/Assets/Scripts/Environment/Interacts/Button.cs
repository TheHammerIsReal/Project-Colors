using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour, IDamageable
{
    [SerializeField] InteractuablesGlobal _interactuable;
    SpriteRenderer _spriteRenderer;
    Sequence _platformSeq;
    [SerializeField] float _cooldown;
    [SerializeField] float _transitionValue;
    [SerializeField] bool _back;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDmg(int dmg)
    {
        _platformSeq.Kill();

        _platformSeq = DOTween.Sequence();

        _interactuable.Interact(_cooldown, _back);

        _platformSeq.Append(_spriteRenderer.DOColor(Color.white, _transitionValue));

        if (_back)
        {
            _platformSeq.AppendInterval(_cooldown);
            _platformSeq.Append(_spriteRenderer.DOColor(Color.red, _transitionValue));
        }
       
    }
}
