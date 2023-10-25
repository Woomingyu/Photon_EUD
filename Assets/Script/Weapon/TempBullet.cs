using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class TempBullet : Bullet
{
    private Rigidbody2D rb2D;
    private Vector3 _direction;

    private const string MONSTER_TAG = "Monster";
    private const string PLAYER_TAG = "Player";
    private const string BULLET_TAG = "Bullet";

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("color1: " + destinationColor.r + "/" + destinationColor.g + "/" + destinationColor.b);
        this.gameObject.SetActive(true);
    }

    void Start()
    {
        _currentSpeed = speed;
        _currentAtk = atk;
        _currentLifeTime = lifetime;
        _currentHitCount = 0;
        sourceColor = bulletRenderer.color;
    }

    void Update()
    {
        if (_currentLifeTime <= 0)
            this.ReturnToPool();
        else
            _currentLifeTime -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        _direction = rb2D.velocity.normalized;
    }

    // 오브젝트 풀링에 의한 활성화/비활성화
    void OnEnable()
    {
        rb2D.velocity *= _currentSpeed;
        //nvoke("ReturnToPool", lifetime);
    }

    void OnDisable()
    {
        ResetBullet();
    }

    // Pool 관련 처리
    void ResetBullet()
    {
        _direction = Vector3.zero;
        _currentSpeed = speed;
        _currentAtk = atk;
        _currentLifeTime = lifetime;
        _currentHitCount = 0;
        bulletRenderer.color = sourceColor;
    }

    void ReturnToPool()
    {
        Pool.Release(this);
    }

    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        //rb2D.velocity = Vector3.Reflect(_direction, contact.normal);

        rb2D.velocity = GetReflect(_direction, contact.normal)* _currentSpeed;
        HitProcess(collision.gameObject);
    }

    Vector3 GetReflect(Vector3 inDirection, Vector2 normalVec)
    {
        float dotVec = Vector3.Dot(inDirection, normalVec);
        Vector3 projecVec = normalVec * dotVec;

        Vector3 reflecVec = -2f* projecVec;
        reflecVec += _direction;

        return reflecVec;
    }

    void HitProcess(GameObject obj)
    {
        // changeHitCount affect Bullet's property. (ex: color, damage, speed)
        switch (obj.tag)
        {
            case MONSTER_TAG:
                _currentLifeTime -= decrementLifeTime;
                this._currentHitCount = 0;
                break;
            case PLAYER_TAG:
                this._currentLifeTime = lifetime;
                this._currentHitCount = maxHitCount;
                break;
            case BULLET_TAG:
                _currentLifeTime -= decrementLifeTime;
                this._currentHitCount++;
                break;
        }
        if ((_currentHitCount <= 0) || (_currentHitCount > maxHitCount))
            this._currentHitCount = 1;

        //Debug.Log("color1: " + bulletRenderer.color.r + "/" + bulletRenderer.color.g + "/" + bulletRenderer.color.b + "/" + bulletRenderer.color.a);
        bulletRenderer.color = Color.Lerp(sourceColor, destinationColor, _currentHitCount / maxHitCount);

        // increase
        _currentSpeed = speed + incrementSpd*(_currentHitCount-1);
        _currentAtk = atk + incrementAtk * (_currentHitCount-1);

        Debug.Log("SPD/ATK: "+_currentSpeed + "/" + _currentAtk);
    }
}
