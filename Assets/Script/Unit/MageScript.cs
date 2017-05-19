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

    public GameObject outline1; //유닛 자체의 아웃라인
    public GameObject outline2; //유닛이 들고 있는 지팡이의 아웃라인

    float moveSpeed = 5.0f;         
    float rotationSpeed = 10.0f;     
    float attackAbleRange = 10.5f;
    public int normalDamage = 10;

    //스킬 관련 변수
    public int magicDamage = 25;
    public float skillAbleRange = 40.0f;
    float dist = 1000; //초기값
    bool skillCkr = false;
    float skillCoolTime = 5.0f;
   // public skillCoolTimeCkr = 5.0f;

    //bool isTouch = false;
    //
    //public void Touching()
    //{
    //    isTouch = !isTouch;
    //}
    //
    //void Targeting()
    //{
    //    target = gameObject.GetComponent<UnitClickScript>().target;
    //}

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
        //오브잭트 재스폰시 타겟초기화
        //Debug.Log("eeeeee");
        //target = null;

        characterController = GetComponent<CharacterController>();
        outline1.SetActive(false);
        outline2.SetActive(false);

        if (gameObject.layer == 10)
        {
            enemyLayer = 11;
        }

        else if (gameObject.layer == 11)
        {
            enemyLayer = 10;
        }
    }
	
    void Awake()
    {
        //애니메이션 자료구조 초기화
        //target = GetComponent<UnitClickScript>().target;

        anim = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        dicState[MAGESTATE.none]    = None;
        dicState[MAGESTATE.free]    = Idle;
        dicState[MAGESTATE.walk]    = Move;
        dicState[MAGESTATE.attack]  = Attack;
        dicState[MAGESTATE.skill]   = Skill;
        dicState[MAGESTATE.death]   = Death;

        InitMage();       
    }

    void InitMage()
    {
        anim.Play("free");
    }

    void None()
    {
        //
    }

    void Idle()
    {
        //Debug.Log("Idle ================================= ");

        stateTime += Time.deltaTime;
        if (stateTime >= idleStateMaxTime)
        {
            stateTime = 0.0f;
            state = MAGESTATE.walk;
        }
    }

    void Move()
    {
        //Debug.Log("Move ================================= ");
        anim.Play("walk");

        Vector3 dir = target.position - transform.position;

        if (dir.magnitude > attackAbleRange)
        {
            //Debug.Log("======================" + dir.magnitude);

            dir.Normalize();
            characterController.SimpleMove(dir * moveSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        }
        else
        {
            stateTime = 2.0f;
            state = MAGESTATE.attack;
        }
    }

    void Attack()
    {
        //Debug.Log("Attack ================================= ");
        //anim.Play("attack");

        stateTime += Time.deltaTime;
        if (stateTime > 2.0f)
        {
            stateTime = 0.0f;
            anim.Play("attack");
            anim.PlayQueued("free", QueueMode.CompleteOthers);

            target.gameObject.GetComponent<DamageScript>().Hit(normalDamage);
        }

        Vector3 dir = target.position - transform.position;

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

                magic.transform.position = target.position;
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

    void OnGUI()
    {
        //set cool time
        if (isTouching() == true && dist < skillAbleRange)
        {
            if (GUI.Button(new Rect(20, 30, 100, 30), "Magic"))
            {
                if (skillCoolTime > 5.0f)
                {
                    state = MAGESTATE.skill;

                    skillCoolTime = 0.0f;

                    //Debug.Log(dist);
                }
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        //if (target = null)
        //{
        //    Debug.Log("eeeeee");
        //    if (gameObject.layer == 10)
        //        target = GameObject.Find("GameObject").GetComponent<GameManagerScript>().MainTower_Enemy.transform;
        //    else if (gameObject.layer == 11)
        //        target = GameObject.Find("GameObject").GetComponent<GameManagerScript>().MainTower_My.transform;
        //}

        //스킬 사용 거리
        if (target != null)
        {
            float deltaX = target.position.x - transform.position.x;
            float deltaZ = target.position.z - transform.position.z;
            dist = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        }

        //OutLine 제어
        if (isTouching() == true)
        {
            outline1.SetActive(true);
            outline2.SetActive(true);
        }
        else
        {
            outline1.SetActive(false);
            outline2.SetActive(false);
        }

        //Animation
        if (!target) //타겟이 없으면 Idle 상태
        {
            return;
        }
        else if (target.gameObject.activeSelf == false)
        {
            target = null;
        }
        else
            dicState[state]();

        skillCoolTime += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        //충돌이 발생하면 충돌한 물체를 타겟으로 변경
        if (collision.gameObject.layer == enemyLayer)
        {
            target = collision.gameObject.transform;
        }
    }
}
