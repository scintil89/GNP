using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour
{
    public int hp;
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
            return false;

        return true;
    }

    public void Hit(int damage)
    {
        hp -= damage;
        //Debug.Log(gameObject.name + " HP : " + hp);

        StartCoroutine(DamageFontProcess(damage));
    }

    IEnumerator DeadProcess()
    {
        yield return new WaitForSeconds(0.9f);

        GameObject particle = Instantiate(destroyParticle) as GameObject;
        particle.transform.position = gameObject.transform.position;

        //Destroy(gameObject);
        gameObject.SetActive(false);
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

    // Update is called once per frame
    void Update ()
    {
        if (isExist() == false)
        {
            StartCoroutine(DeadProcess());
        }
    }
}
