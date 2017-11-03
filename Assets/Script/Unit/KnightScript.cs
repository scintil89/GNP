using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum KNIGHTSTATE
{
    none,
    idle = -1,
    walk = 0,
    attack,
    opponent,
}

public class KnightScript : UnitScript
{
    float moveSpeed = 5.0f;
    float rotationSpeed = 10.0f;
    float attackAbleRange = 8.0f;
    int normalDamage = 15;

    // Animation
    public KNIGHTSTATE state = KNIGHTSTATE.attack;
    float stateTime = 0.0f;
    float idleStateMaxTime = 0.2f;
    public Animation anim;
    CharacterController characterController = null;

    [SerializeField]
    Dictionary<KNIGHTSTATE, System.Action> dicState = new Dictionary<KNIGHTSTATE, System.Action>();

    //이동중 마주친 상대
    public GameObject opponent = null;

    // Use this for initialization
    void Start ()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Awake()
    {
        //오브잭트 재스폰시 타겟초기화
        target = null;
        opponent = null;

        //애니메이션 자료구조 초기화
        anim = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        dicState[KNIGHTSTATE.none] = None;
        dicState[KNIGHTSTATE.idle] = Idle;
        dicState[KNIGHTSTATE.walk] = Walk;
        dicState[KNIGHTSTATE.attack] = Attack;
        dicState[KNIGHTSTATE.opponent] = Opponent;

        InitKnight();
    }

    void InitKnight()
    {
        state = KNIGHTSTATE.idle;
    }

    void None()
    {
        //
    }

    void Idle()
    {
        //Debug.Log("Idle ================================= ");
        anim.Play("WK_heavy_infantry_05_combat_idle");

        stateTime += Time.deltaTime;
        if (stateTime >= idleStateMaxTime)
        {
            //target이 있으면
            if (target && target.gameObject.activeSelf)
            {
                state = KNIGHTSTATE.walk;
                stateTime = 0.0f;
            }
        }
    }

    void Walk()
    {
        //Debug.Log("KnightScript Walk");

        //Play Animation
        anim.Play("WK_heavy_infantry_06_combat_walk");

        //WayNode 방향 벡터
        Vector3 toward = dst - transform.position;

        //목적지 Node dst가 가까우면 새로운 목적지 설정
        if (toward.magnitude < 2.0f)
        {
            if (gameObject.GetComponent<WayPoint>().way.Count > 0)
            {
                dst = gameObject.GetComponent<WayPoint>().way.Pop();
            }
            else
            {
                Debug.Log("KnightScript Walk : Way Zero");
            }
        }
        
        //toward 방향으로 이동
        toward.Normalize();
        characterController.SimpleMove(toward * moveSpeed);
        //toward 방향으로 회전
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(toward),
            rotationSpeed * Time.deltaTime);

        //target과의 거리
        Vector3 targetdist = target.transform.position - transform.position;

        //target과의 거리 < 공격 가능 범위
        if (targetdist.magnitude < attackAbleRange)
        {
            stateTime = 2.0f;
            state = KNIGHTSTATE.attack;
        }
    }

    void Attack()
    {
        //Debug.Log("Attack ================================= ");
        //anim.Play("attack");

        //target이 non active상태면
        if (!target || !target.gameObject.activeSelf)
        {
            target = RayCastFront(100.0f).gameObject;

            state = KNIGHTSTATE.idle;

            return;
        }

        //Play Attack Animation
        stateTime += Time.deltaTime;
        if (stateTime > 2.0f)
        {
            stateTime = 0.0f;
            anim.Play("WK_heavy_infantry_08_attack_B");
            anim.PlayQueued("WK_heavy_infantry_05_combat_idle", QueueMode.CompleteOthers);

            target.gameObject.GetComponent<DamageScript>().Hit(normalDamage);
        }

        Vector3 dir = target.transform.position - transform.position;

        if (dir.magnitude > attackAbleRange)
        {
            state = KNIGHTSTATE.idle;
        }
    }

    void Opponent()
    {
        if(!opponent || !opponent.gameObject.activeSelf)
        {
            state = KNIGHTSTATE.idle;
        }

        stateTime += Time.deltaTime;
        if (stateTime > 2.0f)
        {
            stateTime = 0.0f;
            anim.Play("WK_heavy_infantry_08_attack_B");
            anim.PlayQueued("WK_heavy_infantry_05_combat_idle", QueueMode.CompleteOthers);

            opponent.gameObject.GetComponent<DamageScript>().Hit(normalDamage);
        }

        Vector3 dir = opponent.transform.position - transform.position;

        if (dir.magnitude > attackAbleRange)
        {
            state = KNIGHTSTATE.idle;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        opponent = RayCastFront(5.0f);
        if(opponent)
        {

        }

        if (!target || !target.gameObject.activeSelf) //타겟이 없으면 Idle 상태
        {
            //find target
            var t =  RayCastFront(100.0f);
        
            if(t)
            {
                Targeting(t);
            }
        }
        else
        {
            dicState[state]();
        }
    }

    
    //void OnCollisionEnter(Collision collision)
    //{
    //    //충돌이 발생하면 충돌한 물체를 타겟으로 변경
    //    if(collision.gameObject.layer != gameObject.layer)
    //    {
    //        Targeting(collision.gameObject);
    //    }
    //}
}
