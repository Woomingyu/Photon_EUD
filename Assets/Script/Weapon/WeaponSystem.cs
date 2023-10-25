using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Pool;

public class WeaponSystem : MonoBehaviour
{
    private BulletObjectPool _pool;

    [SerializeField] private Transform bulletPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int maxBullet;
    [SerializeField] private float atkCooltime;

    private PlayerInput _input;
    private int _currentBulletCount;
    private Vector3 bulletDir;

    public GameObject Bullet { get { return bullet; } }
    public Transform BulletPos { get { return bulletPos; } }
    public Vector3 BulletDir { get { return bulletDir; } set { bulletDir = value; } }
    public int CurrentBulletCount { get { return _currentBulletCount; } set {  _currentBulletCount = value; } }

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _pool = GetComponent<BulletObjectPool>();
    }

    void Start()
    {
        _input.AtkCooldown = this.atkCooltime;
        _currentBulletCount = 0;
    }

    private void Update()
    {

    }

    public void GetProjectile(Vector3 bulletDir)
    {
        if (isCreate())
        {
            Vector3 weaponPos = BulletPos.position;
            Vector3 weaponScale = BulletPos.localScale * 0.5f;
            Vector3 spwanPos = new Vector3(weaponPos.x + (weaponScale.x) * bulletDir.x, 
                                           weaponPos.y + (weaponScale.y) * bulletDir.y);

            //GameObject obj = Instantiate(_weapon.Bullet, spwanPos, Quaternion.identity);
            _pool.SpawnBullet(spwanPos, Quaternion.identity, bulletDir);
            _currentBulletCount++;
        }
    }

    public bool isCreate()
    {
        Debug.Log(_currentBulletCount);
        return _currentBulletCount < maxBullet;
    }

    public GameObject CreateProjectile()
    {
        var obj = Instantiate(bullet);
        return obj;
    }

    public void DisableProjectile()
    {
        _currentBulletCount--;
    }


}
