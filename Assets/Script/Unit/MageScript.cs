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

public class MageScript : MonoBehaviour
{
    public GameObject magicObject;
    public GameObject outline1;
    public GameObject outline2;
    public Transform target = null;

    float moveSpeed = 5.0f;         
    float rotationSpeed = 10.0f;     
    float attackableRange = 10.5f;
    int normalDamage = 10;
    int magicDamage = 25;
    bool skillCkr = false;
    float skillCoolTime = 5.0f;

//     bool isTouch = false;
// 
//     public void Touching()
//     {
//         isTouch = !isTouch;
//     }
// 
//     void Targeting()
//     {
//         target = gameObject.GetComponent<UnitClickScript>().target;
//     }

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
        outline1.SetActive(false);
        outline2.SetActive(false);
    }
	
    void Awake()
    {
//        target = GetComponent<UnitClickScript>().target;

        anim = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        dicState[MAGESTATE.none] = None;
        dicState[MAGESTATE.free] = Idle;
        dicState[MAGESTATE.walk] = Move;
        dicState[MAGESTATE.attack] = Attack;
        dicState[MAGESTATE.skill] = Skill;
        dicState[MAGESTATE.death] = Death;

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

        if (dir.magnitude > attackableRange)
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

        if (dir.magnitude > attackableRange)
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
        //Destroy(gameObject);
        gameObject.SetActive(false);

        state = MAGESTATE.none;
    }

    void OnGUI()
    {
        //set cool time
        if (gameObject.GetComponent<UnitClickScript>().isTouch == true)
        {
            if (GUI.Button(new Rect(20, 30, 100, 30), "Magic"))
            {
                if (skillCoolTime > 5.0f)
                {
                    state = MAGESTATE.skill;

                    skillCoolTime = 0.0f;
                }
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        //Animation
        if (target)
            dicState[state]();

        skillCoolTime += Time.deltaTime;


        if (gameObject.GetComponent<UnitClickScript>().isTouch == true)
        {
            outline1.SetActive(true);
            outline2.SetActive(true);
        }
        else
        {
            outline1.SetActive(false);
            outline2.SetActive(false);
        }
    }
}
