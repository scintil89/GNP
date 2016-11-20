using UnityEngine;
using System.Collections;

public class ClickEvent : MonoBehaviour
{
    public GameObject TempObject;

    // Update is called once per frame
    void Update ()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObj;

            if (Physics.Raycast(ray, out hitObj, 200.0f))
            {
                //Debug.Log(hitObj.transform.gameObject.name);

                switch(hitObj.transform.gameObject.layer)
                {
                    case 5:
                        //Debug.Log(hitObj.transform.gameObject.name);
                        break;

                    case 10:
                        {
                            hitObj.transform.gameObject.GetComponent<UnitClickScript>().Touching();

                            if(TempObject)
                            {
                                TempObject.GetComponent<UnitClickScript>().Touching();
                                TempObject = hitObj.transform.gameObject;
                            }
                        } 
                        break;

                    case 11:
                        //Debug.Log(hitObj.transform.gameObject.name);
                        break;
                }
            }
        }
    }
}
