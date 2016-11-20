using UnityEngine;
using System.Collections;

public class UnitClickScript : MonoBehaviour
{
//     public Transform target = null;

    public bool isTouch = false;

    public void Touching()
    {
        isTouch = !isTouch;
    }
    
	// Update is called once per frame
	void Update ()
    {
        if (isTouch == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Right Button Clicked");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitObj;

                if (Physics.Raycast(ray, out hitObj, 200.0f))
                {
                   // if (hitObj.transform.gameObject.layer == 5)
                        //Debug.Log(hitObj.transform.gameObject.name);

                    if (hitObj.transform.gameObject.layer == 11)
                    {
                        //Debug.Log(hitObj.transform.gameObject.name);

                        if( gameObject.GetComponent<MageScript>() )
                        {
                            gameObject.GetComponent<MageScript>().target = hitObj.transform;
                        }
                        else if ( gameObject.GetComponent<KnightScript>() )
                        {
                            gameObject.GetComponent<KnightScript>().target = hitObj.transform;
                        }

                        //target = hitObj.transform;
                    }
                }
            }
        }
    }
}
