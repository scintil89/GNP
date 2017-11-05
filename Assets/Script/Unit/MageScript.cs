using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum MAGESTATE
{
    none,
    free = -1,
    walk = 0,
    attack,
    skill,
    death
}

public class MageScript : UnitScript
{
    public GameObject magicObject; //유닛의 스킬 이펙트 오브젝트

    float moveSpeed = 5.0f;         
    float rotationSpeed = 10.0f;     
    float attackAbleRange = 15.0f;
    public int normalDamage = 10;

    //스킬 관련 변수
    public int magicDamage = 25;
    public float skillAbleRange = 40.0f;
    float dist = 1000; //초기값
    bool skillCkr = false;
    float skillCoolTime = 5.0f;
   // public skillCoolTimeCkr = 5.0f;

    // Animation
    public MAGESTATE state = MAGESTATE.free;
    float stateTime = 0.0f;
    float idleStateMaxTime = 0.5f;
    public Animation anim;
    CharacterController characterController = null;

    Dictionary<MAGESTATE, System.Action> dicState = new Dictionary<MAGESTATE, System.Action>();


    // Use this for initialization
    void Start ()
    {
        characterController = GetComponent<CharacterController>();
    }
	
    void Awake()
    {
        //오브잭트 재스폰시 타겟초기화
        target = null;

        //애니메이션 자료구조 초기화
        anim = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        dicState[MAGESTATE.none]    = None;
        dicState[MAGESTATE.free]    = Idle;
        dicState[MAGESTATE.walk]    = Walk;
        dicState[MAGESTATE.attack]  = Attack;
        dicState[MAGESTATE.skill]   = Skill;
        dicState[MAGESTATE.death]   = Death;

        InitMage();       
    }

    void InitMage()
    {
        state = MAGESTATE.free;
    }

    void None()
    {
        //
    }

    void Idle()
    {
        //Debug.Log("Idle ================================= ");
        anim.Play("free");

        stateTime += Time.deltaTime;
        if (stateTime >= idleStateMaxTime)
        {
            //target이 있으면
            if (target && target.gameObject.activeSelf)
            {
                state = MAGESTATE.walk;
                stateTime = 0.0f;
            }
        }
    }

    void Walk()
    {
        //target과의 거리
        Vector3 targetdist = target.transform.position - transform.position;

        //target과의 거리 < 공격 가능 범위
        if (targetdist.magnitude < attackAbleRange)
        {
            stateTime = 2.0f;
            state = MAGESTATE.attack;

            return;
        }

        //Play Animation
        anim.Play("walk");

        //WayNode 방향 벡터
        Vector3 toward = dst - transform.position;

        //목적지 Node가 가까우면 새로운 목적지 설정
        if (toward.magnitude < 5.0f)
        {
            if (gameObject.GetComponent<WayPoint>().way.Count > 0)
            {
                dst = gameObject.GetComponent<WayPoint>().way.Pop();
            }
            else
            {
                Debug.Log("MageScript Walk : Way Zero" + " " + gameObject.name);
            }
        }

        //toward 방향으로 이동
        toward.Normalize();
        characterController.SimpleMove(toward * moveSpeed);

        //toward 방향으로 회전
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(toward),
            rotationSpeed * Time.deltaTime);
    }

    void Attack()
    {
        //target이 non active상태면
        if (!target || !target.gameObject.activeSelf)
        {
            state = MAGESTATE.free;

            return;
        }

        stateTime += Time.deltaTime;
        if (stateTime > 5.0f)
        {
            stateTime = 0.0f;
            anim.Play("skill");
            anim.PlayQueued("free", QueueMode.CompleteOthers);

            target.gameObject.GetComponent<DamageScript>().Hit(magicDamage);


            GameObject magic = Instantiate(magicObject) as GameObject;

            if (!magic)
            {
                Debug.LogError("MageScript magic Get Failed");
                return;
            }

            magic.transform.position = target.transform.position;
        }

        Vector3 dir = target.transform.position - transform.position;

        if (dir.magnitude > attackAbleRange)
        {
            state = MAGESTATE.free;
        }
    }

    void Skill()
    {
        stateTime += Time.deltaTime;
        if (skillCkr == false)
        {
	        if (stateTime > 0.1f)
	        {
                stateTime = 0.0f;

                skillCkr = true;
	            
	            anim.Play("skill");
	            anim.PlayQueued("free", QueueMode.CompleteOthers);

                target.gameObject.GetComponent<DamageScript>().Hit(magicDamage);

                GameObject magic = Instantiate(magicObject) as GameObject;
                //GameObject magic = MemoryPoolManager.Instance.Get("Flamestrike");

                if (!magic)
                {
                    Debug.LogError("MageScript magic Get Failed");
                    return;
                }

                magic.transform.position = target.transform.position;
            }
        }

        //after anim, change state
        if(stateTime > 1.5f)
        {
            stateTime = 0.0f;
            state = MAGESTATE.free;
            skillCkr = false;
        }
    }

    void Death()
    {
        state = MAGESTATE.none;
    }

    //void OnGUI()
    //{
    //    //set cool time
    //    if (isTouch && dist < skillAbleRange)
    //    {
    //        if (GUI.Button(new Rect(20, 30, 100, 30), "Magic"))
    //        {
    //            if (skillCoolTime > 5.0f)
    //            {
    //                state = MAGESTATE.skill;
    //
    //                skillCoolTime = 0.0f;
    //
    //                //Debug.Log(dist);
    //            }
    //        }
    //    }
    //}
   
    // Update is called once per frame
    void Update()
    {
        ////스킬 사용 거리
        //if (target != null)
        //{
        //    Vector3 a = target.transform.position - transform.position;
        //
        //    dist = a.magnitude;
        //}

        if (!target || !target.gameObject.activeSelf) //타겟이 없으면 Idle 상태
        {
            state = MAGESTATE.free;
            target = null;
        }
        else
        {
            dicState[state]();
        }

        skillCoolTime += Time.deltaTime;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    //충돌이 발생하면 충돌한 물체를 타겟으로 변경
    //    if (collision.gameObject.layer == enemyLayer)
    //    {
    //        target = collision.gameObject;
    //    }
    //}
}
