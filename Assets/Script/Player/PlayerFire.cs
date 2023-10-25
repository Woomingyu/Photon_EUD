using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFire : MonoBehaviour
{
    private PlayerInput _controller;
    private WeaponSystem _weapon;

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<PlayerInput>();
        _weapon = GetComponent<WeaponSystem>();
    }

    private void Start()
    {
        _controller.OnFireEvent += Fire;
    }

    void Fire()
    {
        _weapon.GetProjectile(_controller.BulletDir);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        _controller.OnFireEvent -= Fire;
    }
}
