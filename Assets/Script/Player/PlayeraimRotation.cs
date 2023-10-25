using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayeraimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerREN;
    [SerializeField] private SpriteRenderer armREN;
    [SerializeField] private Transform armPivot;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraMaxDistance;
    private Vector3 center;

    private Player _player;
    private Camera _mainCam;

    private void Awake()
    {
        _player = GetComponent<PlayerInput>();
        _mainCam = Camera.main;
    }
    
    void Start()
    {
        _player.OnLookEvent += OnAim;
    }

    public void OnAim(Vector2 newAim)
    {
        RotateArm(newAim);
        TranslateCam(newAim);
    }

    private void RotateArm(Vector2 newAim)
    {
        Vector3 temp = new Vector3(newAim.x, newAim.y);

        float rotZ = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;

        armREN.flipY = Mathf.Abs(rotZ) > 90f;
        playerREN.flipX = armREN.flipY;
        armPivot.rotation = Quaternion.Euler(0,0,rotZ);
    }

    private void TranslateCam(Vector2 newAim)
    {
        Vector3 cameraPos = _mainCam.transform.position;

        Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = Vector3.ClampMagnitude(mousePos, cameraMaxDistance);
        mousePos.z = _mainCam.transform.position.z;

        Vector3 playerPos = _player.transform.position;

        center = new Vector3((playerPos.x + mousePos.x)*0.5f, (playerPos.y + mousePos.y)*0.5f, _mainCam.transform.position.z); 
        _mainCam.transform.position = Vector3.Lerp(cameraPos, center, Time.deltaTime * cameraSpeed);
    }

    private void OnDestroy()
    {
        _player.OnLookEvent -= OnAim;
    }
}
