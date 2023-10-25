using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public SpriteRenderer playerREN;
    public GameObject target;
    Rigidbody2D rb;
    int speed = 2;
    public Animator spritAnim;
    int Hp = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spritAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isDeath();
        Tracking();
        MoveAnim();
    }

    void MoveAnim()
    {
        if (rb.velocity == new Vector2(0,0))
        {
            spritAnim.SetBool("isRun", false);
        }
        else
        {
            spritAnim.SetBool("isRun", true);
        }

        if (target.transform.position.x < transform.position.x)
        {
            playerREN.flipX = true;
        }
        else
        {
            playerREN.flipX = false;
        }
    }

    void Tracking()
    {
        Debug.DrawRay(transform.position, target.transform.position - transform.position,new Color(0,1,0));// 그려서 확인하게 편하게 해줌
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position,target.transform.position - transform.position, 5/* 길이*/, LayerMask.GetMask("Wall"));//벽의 실제로 충돌을 확인함LayerMask.GetMask("Wall")
        if (rayHit.collider == null)// 충돌 되지 않을 경우//1 << LayerMask.NameToLayer("Wall")
        {
            if (Vector3.Distance(target.transform.position, transform.position) > 1)// 거리가 2이상일때
            {
                rb.velocity = (target.transform.position - transform.position).normalized * speed;
            }
            else
            {
                //Debug.Log("거리 안에 들어옴");
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            //Debug.Log("벽이 감지 됨");
            rb.velocity = new Vector2(0, 0);
            //사이에 벽이 있다면 사용할 코드
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Hp -= 1;
        }
    }

    void isDeath()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
