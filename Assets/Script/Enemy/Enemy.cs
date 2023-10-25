using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public ParticleSystem _effect;

    //Enemy Value
    [SerializeField]
    public EnemyType enemyType;
    [SerializeField]
    protected int HP;
    [SerializeField]
    protected int walkSpeed;
    [SerializeField]
    protected int Exp;
    [SerializeField]
    protected int attackDamage;


    protected Vector3 destination; // ������

    //Need Component
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Animator anim;
    private GameObject target;
    NavMeshAgent nav;

    //State
    protected bool isAction; // ���� �ൿ�� ���������� �Ǻ�
    protected bool isWalking; // �ȱ����
    public bool isDead; // ������� (��ü ����-�׼���Ʈ�ѷ����� ���)
    protected bool isChasing; //�߰ݻ���(�ش� ���¿��� ���� x)
    protected bool isAttacking; // ���� ��

    [SerializeField]
    protected float walkTime; // �ȱ� �ð�
    [SerializeField]
    protected float waitTime; //��� �ð�

    protected float currentTime; // ���ð� ����


    //Attack
    public GameObject bulletSpawnPoint;
    public GameObject bullet;


    public enum EnemyType
    {
        IdleEnemy, // ���ڸ� ����
        MovingEnemy, // �̵�
        AggressiveEnemy //����+�̵�
    }

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void Start()
    {
        _effect.Play();
        target = UI_Manager.I.player;
        currentTime = waitTime;
        isAction = true;
        isDead = false;

        nav.updateRotation = false;
        nav.updateUpAxis = false;
    }



    protected virtual void Update()
    {
        //nav.SetDestination(target.transform.position);

        
        if (isDead)
            return;

        //MoveAnim();

        Move();
        ElapseTime(); // �ð� ���
    }

    protected void Move()
    {
        if (isWalking)
            nav.SetDestination(destination);
        //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        //������ٵ� �̵�(���� ��ġ���� ��������, 1�ʴ� walkSpeed ��ġ��ŭ �̵�


        if (destination.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    //�ൿ ���� �ð���� -> �ð� ���޽� ���½��� -> ���� �� �����ൿ ����
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking) // ���½ð� ����, �߰�, �������� �ƴҶ���
                ReSet();// �ൿ ���� �� �����׼� ����
        }
    }


    //�ൿ �Ұ� ����(�����Լ� == �ϼ��Լ����� �ڽ� Ŭ�������� ��������)
    protected virtual void ReSet()
    {
        //�� ���º��� �ʱ�ȭ
        isWalking = false;
        isAction = true;
        //�̵��ӵ� �ʱ�ȭ
        nav.speed = walkSpeed;

        //������ ����
        nav.ResetPath();

        //�ִϸ��̼� �ʱ�ȭ
        anim.SetBool("isRun", false); //anim.SetBool("Running", isRunning);


        destination.Set(Random.Range(-6f, 6f), Random.Range(-5f, 5f), 0); // ���� ������ ����

        //�����÷ο� : �ڵ尡 �߸���  ��� ��� ������(����/��/�׺�Ž� ����/��� ���)
        /*
        while (!NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            destination.Set(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0); // ���� ������ ����
        }
        */

    }
    //void Tracking()
    //{
    //    Debug.DrawRay(transform.position, target.transform.position - transform.position,new Color(0,1,0));// �׷��� Ȯ���ϰ� ���ϰ� ����
    //    RaycastHit2D rayHit = Physics2D.Raycast(transform.position,target.transform.position - transform.position, 5/* ����*/, LayerMask.GetMask("Wall"));//���� ������ �浹�� Ȯ����LayerMask.GetMask("Wall")
    //    if (rayHit.collider == null)// �浹 ���� ���� ���//1 << LayerMask.NameToLayer("Wall")
    //    {
    //        if (Vector3.Distance(target.transform.position, transform.position) > 1)// �Ÿ��� 2�̻��϶�
    //        {

    //            rb.velocity = (target.transform.position - transform.position).normalized * speed;
    //        }
    //        else
    //        {
    //            //Debug.Log("�Ÿ� �ȿ� ����");
    //            rb.velocity = new Vector2(0, 0);
    //        }
    //    }
    //    else
    //    {
    //        //Debug.Log("���� ���� ��");
    //        rb.velocity = new Vector2(0, 0);
    //        //���̿� ���� �ִٸ� ����� �ڵ�
    //    }
    //}


    public void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (collision.gameObject.tag == "Bullet")
        {
            Damage((int)obj.GetComponent<Bullet>()?.CurrentAtk);
        }
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("isRun", true);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("�ȱ�");       
    }



    //������ �޴°��(�����Լ�) = Enemy Ÿ�Կ� ���� �������� �޴� ��쿡 �� �߰����� �ൿ(����) ���� ����
    public virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            HP -= _dmg;

            if (HP <= 0)
            {
                Dead();
                //switch case / enemy type�� ���� ����ġ +
                Destroy(gameObject);
                return;
            }

            // �ǰ� ���� ���
            //anim.SetTrigger("Hurt"); // �ǰݸ�� ����
        }
    }

    public void Attack() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        if (!isDead)
        {
            
            Debug.Log("����");
            isAttacking = true; // ���ݻ��� ON
            nav.ResetPath(); // ���ڸ����� �����ϵ��� �߰����� (������ ����/�׺���̼� �����Լ�)    


            //�÷��̾ �ٶ󺸵��� ����

            //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

            //����() ���� �ٲ��� �� -> ����
            Vector3 direction = (target.transform.position - bulletSpawnPoint.transform.position).normalized;

            RotateArm(direction);

            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);

            isAttacking = false;
        }
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        bulletSpawnPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    protected void Dead()
    {
        if (HP <= 0)
        {
            UI_Manager.I.HP_Add(Exp);
            Instantiate(_effect,transform.position,Quaternion.identity);// ���� ���� �Է��ϸ� �����̼� ���� �Բ� �־������
            isWalking = false;
            isChasing = false;
            isAttacking = false;
            nav.ResetPath();
            isDead = true;
            //��� ���� ���
            //anim.SetTrigger("Dead"); // ������ ����        
        }
    }
}
