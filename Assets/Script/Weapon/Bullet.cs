using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> Pool { get; set; }

    [SerializeField] protected float speed;
    [SerializeField] protected float atk;
    [SerializeField] protected float lifetime;
    [SerializeField] protected int maxHitCount;
    [SerializeField] protected Color destinationColor = Color.red;
    [SerializeField] protected float incrementAtk;
    [SerializeField] protected float incrementSpd;
    [SerializeField] protected float decrementLifeTime;

    protected float _currentSpeed;
    protected float _currentAtk;
    protected float _currentHitCount;
    protected float _currentLifeTime;
    protected Color sourceColor;

    protected SpriteRenderer bulletRenderer;

    public float Speed { get { return speed; } }
    public float Atk { get {  return _currentAtk; } }
    public float CurrentAtk {  get { return _currentAtk; } }

    private void Awake()
    {
        Pool = gameObject.GetComponent<BulletObjectPool>().Pool;
    }
}