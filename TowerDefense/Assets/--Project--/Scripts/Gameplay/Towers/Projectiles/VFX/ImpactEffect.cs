using System.Collections;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private ParticleSystem[] _particleSystems;

    private PoolFactory _poolFactory;
    private ImpactEffect _prefab;

    private void Awake()
    {
        if (_particleSystems == null || _particleSystems.Length == 0)
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
        }
    }

    public void Init(PoolFactory poolFactory, ImpactEffect prefab, float radius = 0f)
    {
        _poolFactory = poolFactory;
        _prefab = prefab;

        if (radius > 0f)
        {
            transform.localScale = Vector3.one * (radius * 2f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        if (_particleSystems != null)
        {
            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Play();
            }
        }

        StartCoroutine(ReturnToPoolRoutine());
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_duration);
        _poolFactory.Reclaim(this, _prefab);
    }
}