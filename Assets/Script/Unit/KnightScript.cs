using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum KNIGHTSTATE
{
    none,
    idle = -1,
    walk = 0,
    attack,
}

public class KnightScript : MonoBehaviour
{
    public GameObject outline;

    public Transform target = null;
    float moveSpeed = 5.0f;
    float rotationSpeed = 10.0f;
    float attackableRange = 12.0f;
    int normalDamage = 15;

//     void Targeting()
//     {
//         target = gameObject.GetComponent<UnitClickScript>().target;
//     }

    // Animation
    public KNIGHTSTATE state = KNIGHTSTATE.attack;
    float stateTime = 0.0f;
    float idleStateMaxTime = 0.5f;
    public Animation anim;
    CharacterController characterController = null;

    Dictionary<KNIGHTSTATE, System.Action> dicState = new Dictionary<KNIGHTSTATE, System.Action>();


    // Use this for initialization
    void Start ()
    {
        characterController = GetComponent<CharacterController>();

        outline.SetActive(false);
    }

    void Awake()
    {
        anim = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        dicState[KNIGHTSTATE.none] = None;
        dicState[KNIGHTSTATE.idle] = Idle;
        dicState[KNIGHTSTATE.walk] = Walk;
        dicState[KNIGHTSTATE.attack] = Attack;

        InitKnight();
    }

    void InitKnight()
    {
        anim.Play("WK_heavy_infantry_05_combat_idle");

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
            state = KNIGHTSTATE.walk;
        }

        anim.Play("WK_heavy_infantry_05_combat_idle");
    }

    void Walk()
    {
        //Debug.Log("Move ================================= ");
        anim.Play("WK_heavy_infantry_06_combat_walk");

        Vector3 dir = target.position - transform.position;

        if (dir.magnitude > attackableRange)
        {
            //Debug.Log("======================" + dir.magnitude);

            dir.Normalize();
            characterController.SimpleMove(dir * moveSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    Quaternion.LookRotation(dir),
                                                    rotationSpeed * Time.deltaTime);
        }
        else
        {
            stateTime = 2.0f;
            state = KNIGHTSTATE.attack;
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
            anim.Play("WK_heavy_infantry_08_attack_B");
            anim.PlayQueued("WK_heavy_infantry_05_combat_idle", QueueMode.CompleteOthers);

            target.gameObject.GetComponent<DamageScript>().Hit(normalDamage);
        }

        Vector3 dir = target.position - transform.position;

        if (dir.magnitude > attackableRange)
        {
            state = KNIGHTSTATE.idle;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //Animation
        if (target)
            dicState[state]();

        if (gameObject.GetComponent<UnitClickScript>().isTouch == true)
        {
            outline.SetActive(true);
        }
        else
        {
            outline.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != gameObject.layer)
        {
            target = collision.gameObject.transform;
        }
    }
}
