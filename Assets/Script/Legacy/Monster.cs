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
        Debug.DrawRay(transform.position, target.transform.position - transform.position,new Color(0,1,0));// �׷��� Ȯ���ϰ� ���ϰ� ����
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position,target.transform.position - transform.position, 5/* ����*/, LayerMask.GetMask("Wall"));//���� ������ �浹�� Ȯ����LayerMask.GetMask("Wall")
        if (rayHit.collider == null)// �浹 ���� ���� ���//1 << LayerMask.NameToLayer("Wall")
        {
            if (Vector3.Distance(target.transform.position, transform.position) > 1)// �Ÿ��� 2�̻��϶�
            {
                rb.velocity = (target.transform.position - transform.position).normalized * speed;
            }
            else
            {
                //Debug.Log("�Ÿ� �ȿ� ����");
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            //Debug.Log("���� ���� ��");
            rb.velocity = new Vector2(0, 0);
            //���̿� ���� �ִٸ� ����� �ڵ�
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
