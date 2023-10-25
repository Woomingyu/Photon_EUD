using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBullt : MonoBehaviour
{
    public float speed;
    public Transform target;
    public int damage = 10;
    private void Awake()
    {
    }

    void Start()
    {
        //플레이어 방향을 바라보도록 회전을 준다.
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

}
