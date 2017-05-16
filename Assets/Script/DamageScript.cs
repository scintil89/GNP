using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour
{
    //데미지 처리를 하는 스크립트.
    enum state
    {
        LIVE = 0,
        DEAD = 1,
        DYING = 2
    }

    state nowState;

    public int hp;
    public string destroyParticle = "FireExplosion";

    //the HP Particle
    public GameObject HPParticle;
    public Color particleColor;

    //Default Forces
    public Vector3 DefaultForce = new Vector3(0f, 1f, 0f);
    public float DefaultForceScatter = 0.5f;

    void Awake()
    {
        nowState = state.LIVE;
    }

    public bool isExist()
    {
        if (hp <= 0)
        {
            return false;
        }
        return true;
    }

    public void Hit(int damage)
    {
        hp -= damage;
        //Debug.Log(gameObject.name + " HP : " + hp);

        if(isExist() == true)
        {
            StartCoroutine( DamageFontProcess(damage) );
        }
        else
        {
            StartCoroutine( DeadProcess() );
        }
    }

    IEnumerator DamageFontProcess(int damage)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject NewHPP = Instantiate(HPParticle, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;


        TextMesh TM = NewHPP.transform.FindChild("HPLabel").GetComponent<TextMesh>();

        TM.text = damage.ToString();
        TM.characterSize = 20;
        TM.color = particleColor;

        NewHPP.GetComponent<Rigidbody>().AddForce(
            new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter, DefaultForceScatter),
                        DefaultForce.y + Random.Range(-DefaultForceScatter, DefaultForceScatter),
                        DefaultForce.z + Random.Range(-DefaultForceScatter, DefaultForceScatter)
                        ));
    }

    IEnumerator DeadProcess() //IEnumerator
    {
        //Debug.Log("DeadProcess " + gameObject.name);
        yield return new WaitForSeconds(0.5f);

        GameObject particle = MemoryPoolManager.Instance.Get(destroyParticle);
        particle.transform.position = gameObject.transform.position;

        yield return new WaitForSeconds(0.5f);

        MemoryPoolManager.Instance.Free(particle);

        //Debug.Log("DeadProcess after particle free" + gameObject.name);

        //yield return new WaitForSeconds(0.5f);

        MemoryPoolManager.Instance.Free(gameObject);
        
       //Debug.Log("DeadProcess after gameObject free" + gameObject.name);
    }
        
    // Update is called once per frame
    //void Update ()
    //{
    //    if (isExist() == false)
    //    {
    //        StartCoroutine(DeadProcess());
    //        //DeadProcess();
    //    }
    //}
}
