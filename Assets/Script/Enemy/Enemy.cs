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


    protected Vector3 destination; // 목적지

    //Need Component
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Animator anim;
    private GameObject target;
    NavMeshAgent nav;

    //State
    protected bool isAction; // 여러 행동이 진행중인지 판별
    protected bool isWalking; // 걷기상태
    public bool isDead; // 사망상태 (해체 툴팁-액션컨트롤러에서 사용)
    protected bool isChasing; //추격상태(해당 상태에선 리셋 x)
    protected bool isAttacking; // 공격 중

    [SerializeField]
    protected float walkTime; // 걷기 시간
    [SerializeField]
    protected float waitTime; //대기 시간

    protected float currentTime; // 대기시간 계산용


    //Attack
    public GameObject bulletSpawnPoint;
    public GameObject bullet;


    public enum EnemyType
    {
        IdleEnemy, // 제자리 고정
        MovingEnemy, // 이동
        AggressiveEnemy //공격+이동
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
        ElapseTime(); // 시간 경과
    }

    protected void Move()
    {
        if (isWalking)
            nav.SetDestination(destination);
        //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        //리지드바디 이동(현재 위치에서 전방으로, 1초당 walkSpeed 수치만큼 이동


        if (destination.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    //행동 종료 시간계산 -> 시간 도달시 리셋실행 -> 리셋 시 랜덤행동 실행
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking) // 리셋시간 도달, 추격, 공격중이 아닐때만
                ReSet();// 행동 리셋 후 랜덤액션 실행
        }
    }


    //행동 불값 리셋(가상함수 == 완성함수지만 자식 클래스에서 수정가능)
    protected virtual void ReSet()
    {
        //각 상태변수 초기화
        isWalking = false;
        isAction = true;
        //이동속도 초기화
        nav.speed = walkSpeed;

        //목적지 리셋
        nav.ResetPath();

        //애니메이션 초기화
        anim.SetBool("isRun", false); //anim.SetBool("Running", isRunning);


        destination.Set(Random.Range(-6f, 6f), Random.Range(-5f, 5f), 0); // 랜덤 목적지 지정

        //오버플로우 : 코드가 잘못됨  경로 계산 참거짓(시작/끝/네비매쉬 영역/결과 경로)
        /*
        while (!NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            destination.Set(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0); // 랜덤 목적지 지정
        }
        */

    }
    //void Tracking()
    //{
    //    Debug.DrawRay(transform.position, target.transform.position - transform.position,new Color(0,1,0));// 그려서 확인하게 편하게 해줌
    //    RaycastHit2D rayHit = Physics2D.Raycast(transform.position,target.transform.position - transform.position, 5/* 길이*/, LayerMask.GetMask("Wall"));//벽의 실제로 충돌을 확인함LayerMask.GetMask("Wall")
    //    if (rayHit.collider == null)// 충돌 되지 않을 경우//1 << LayerMask.NameToLayer("Wall")
    //    {
    //        if (Vector3.Distance(target.transform.position, transform.position) > 1)// 거리가 2이상일때
    //        {

    //            rb.velocity = (target.transform.position - transform.position).normalized * speed;
    //        }
    //        else
    //        {
    //            //Debug.Log("거리 안에 들어옴");
    //            rb.velocity = new Vector2(0, 0);
    //        }
    //    }
    //    else
    //    {
    //        //Debug.Log("벽이 감지 됨");
    //        rb.velocity = new Vector2(0, 0);
    //        //사이에 벽이 있다면 사용할 코드
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
        Debug.Log("걷기");       
    }



    //데미지 받는경우(가상함수) = Enemy 타입에 따라 데미지를 받는 경우에 할 추가적인 행동(현상) 지정 가능
    public virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            HP -= _dmg;

            if (HP <= 0)
            {
                Dead();
                //switch case / enemy type에 따라 경험치 +
                Destroy(gameObject);
                return;
            }

            // 피격 사운드 재생
            //anim.SetTrigger("Hurt"); // 피격모션 실행
        }
    }

    public void Attack() // 피해량, 플레이어 위치 받아옴
    {
        if (!isDead)
        {
            
            Debug.Log("공격");
            isAttacking = true; // 공격상태 ON
            nav.ResetPath(); // 제자리에서 공격하도록 추격정지 (목적지 리셋/네비게이션 내장함수)    


            //플레이어를 바라보도록 설정

            //anim.SetTrigger("Attack"); // 공격 애니매이션

            //공격() 각도 바꿔준 후 -> 생성
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
            Instantiate(_effect,transform.position,Quaternion.identity);// 벡터 값을 입력하면 로테이션 값도 함께 넣어줘야함
            isWalking = false;
            isChasing = false;
            isAttacking = false;
            nav.ResetPath();
            isDead = true;
            //사망 사운드 재생
            //anim.SetTrigger("Dead"); // 사망모션 실행        
        }
    }
}
