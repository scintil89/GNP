using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour
{
    //데미지 처리를 하는 스크립트.
    public int hp;
    //public string destroyParticle = "FireExplosion";
    public GameObject destroyParticle;

    //the HP Particle
    public GameObject HPParticle;
    public Color particleColor;

    //Default Forces
    public Vector3 DefaultForce = new Vector3(0f, 1f, 0f);
    public float DefaultForceScatter = 0.5f;

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
            if(gameObject.activeSelf == true) //Don't Use .active
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
        //TODO : 파티클을 메모리풀로 관리했을시 사용된 후의 파티클을 재사용 못하는 문제 발생
        //GameObject particle = MemoryPoolManager.Instance.Get(destroyParticle);
        GameObject particle = Instantiate(destroyParticle) as GameObject;
        particle.SetActive(false);


        yield return new WaitForSeconds(0.9f);


        particle.transform.position = gameObject.transform.position;
        particle.SetActive(true);

        //Debug.Log("DeadProcess " + gameObject.name);

        //yield return new WaitForSeconds(1.0f);

        MemoryPoolManager.Instance.Free(gameObject);


        //MemoryPoolManager.Instance.Free(particle);
        //Debug.Log("DeadProcess after particle free" + gameObject.name);

        //destroyParticle.SetActive(false);
        //DestroyObject(particle);

        //TODO : 타워...  
        
       //Debug.Log("DeadProcess after gameObject free" + gameObject.name);
    }
}
