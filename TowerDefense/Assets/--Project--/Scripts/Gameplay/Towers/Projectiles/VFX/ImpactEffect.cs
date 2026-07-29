using System.Collections;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private ParticleSystem _particleSystem;

    private PoolFactory _poolFactory;
    private ImpactEffect _prefab;

    public void Init(PoolFactory poolFactory, ImpactEffect prefab, float radius = 0f)
    {
        _poolFactory = poolFactory;
        _prefab = prefab;

        if (radius > 0f)
        {
            transform.localScale = Vector3.one * (radius * 2f);

            if (_particleSystem != null)
            {
                var shape = _particleSystem.shape;
                if (shape.enabled)
                {
                    shape.radius = radius;
                }
            }
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        if (_particleSystem != null)
        {
            _particleSystem.Play();
        }

        StartCoroutine(ReturnToPoolRoutine());
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_duration);
        _poolFactory.Reclaim(this, _prefab);
    }
}