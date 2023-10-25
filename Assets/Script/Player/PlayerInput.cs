using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerInput : Player
{
    private Camera _camera;
    private float _timeSinceLastAttack = float.MaxValue;
    private float _timeSinceLastDash = float.MaxValue;
    private bool isAttacking = false;
    private bool isDashing = false;
    private float atkCooldown = 0.2f;
    private float dashCooldown = 1f;
    private Vector3 bulletDir;

    public float AtkCooldown { get { return atkCooldown; } set { atkCooldown = value; } }
    public Vector3 BulletDir { get { return bulletDir; } set { bulletDir = value; } }


    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        HandleAttackDelay();
        HandleDashDelay();
    }

    private void HandleAttackDelay()
    {
        if (_timeSinceLastAttack <= atkCooldown)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }
        else if (isAttacking)
        {
            _timeSinceLastAttack = 0;
            CallFireEvent();
        }
    }

    private void HandleDashDelay()
    {
        if (_timeSinceLastDash <= dashCooldown)
        {
            _timeSinceLastDash += Time.deltaTime;
        }
        else if (isDashing)
        {
            _timeSinceLastDash = 0;
            CallDashEvent();
        }
    }

    void OnMove(InputValue InputVec)
    {
        Vector2 moveVec = InputVec.Get<Vector2>();
        CallMoveEvent(moveVec);
    }

    void OnLook(InputValue InputVec)
    {
        Vector2 newAim = InputVec.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;
        bulletDir = newAim;

        if (newAim.magnitude >= .9f)
        {
            CallLookEvent(newAim);
        }
    }

    void OnFire(InputValue value)
    {
        isAttacking = value.isPressed;
    }

    void OnDash(InputValue value)
    {
        isDashing = value.isPressed;
    }
}
