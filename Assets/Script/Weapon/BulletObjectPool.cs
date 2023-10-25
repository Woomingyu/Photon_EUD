using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Pool;

public class BulletObjectPool : MonoBehaviour
{
    [SerializeField] private int maxPoolSize = 10;
    [SerializeField] private int stackCapacity = 10;

    private WeaponSystem _handler;
    private IObjectPool<Bullet> _pool;

    public IObjectPool<Bullet> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool =
                    new ObjectPool<Bullet>(
                        CreatedPooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        stackCapacity,
                        maxPoolSize);
            }
            return _pool;
        }
    }

    private void Awake()
    {
        if (_pool != null) _pool.Clear();
        _handler = GetComponent<WeaponSystem>();
    }

    private Bullet CreatedPooledItem()
    {
        GameObject bullet = _handler.CreateProjectile();
        Bullet obj = bullet.GetComponent<Bullet>();

        obj.name = "Bullet";
        obj.Pool = Pool;

        return obj;
    }

    private void OnReturnedToPool(Bullet obj)
    {
        _handler.DisableProjectile();
        obj.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Bullet obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnDestroyPoolObject(Bullet obj)
    {
        Destroy(obj.gameObject);
    }

    public void SpawnBullet(Vector3 pos, Quaternion quat, Vector3 inDir)
    {
        var obj = Pool.Get();
        obj.gameObject.transform.position = pos;
        obj.gameObject.transform.rotation = quat;
        obj.gameObject.GetComponent<Rigidbody2D>().velocity = inDir * obj.Speed;
    }

    public void OnDestroy()
    {
        _pool = null;
    }
}
