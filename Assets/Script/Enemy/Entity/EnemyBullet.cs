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
        //�÷��̾� ������ �ٶ󺸵��� ȸ���� �ش�.
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

}
