using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableObject : SightableObject, IEffectable
{
    [Header("�N�������ɍĐ������Particle")]
    [SerializeField] GameObject _popParticle;

    GameObject _particle;
    Actor _effectedActor;

    void Init()
    {
        if (_particle == null)
        {
            _particle = Instantiate(_popParticle, transform.position, Quaternion.identity);
        }
        else
        {
            _particle.SetActive(true);
        }
    }

    void Start()
    {
        /* Treasure��Enemy�̏��������傤���ԂԂ���ʂ������@ */
    }

    void Update()
    {
        
    }

    public override bool IsAvailable(Actor actor)
    {
        if (_effectedActor == null)
        {
            _effectedActor = actor;
            return true;
        }
        else if (_effectedActor == actor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void IEffectable.Effect(string message) => Play(message);

    protected virtual void Play(string message)
    {

    }
}
