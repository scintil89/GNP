using UnityEngine;
using System.Collections;

public class CameraWorking : MonoBehaviour
{
    public Transform characterTransform;
    Transform tempTransform;

//    bool test = false;

//     void OnGUI()
//     {
//         if (GUI.Button(new Rect(30, 30, 100, 30), "test"))
//         {
//             if (test == false)
//             {
//                 test = true;
//                 Debug.Log("set camera");
//                 setCamera();
//             }
//             else if (test == true)
//             {
//                 test = false;
//                 Debug.Log("reset camera");
//                 resetCamera();
//             }
//         }
//     }

//     void setCamera()
//     {
//         tempTransform = transform;
// 
//         transform.parent = characterTransform;
// 
//         transform.position = new Vector3(0.0f, 7.8f, -24.0f);
// 
//         //temp = cameraTransform.rotation;
// 
//         transform.localEulerAngles = new Vector3(40.0f, 0.0f, 0.0f);
//     }
// 
//     void resetCamera()
//     {
//         transform.parent = null;
// 
//         transform.position = new Vector3(0.0f, 12.0f, -38.0f);
// 
//         //transform = tempTransform;
// 
//         //cameraTransform.rotation = temp;
// 
//         //transform.localEulerAngles = new Vector3(20.0f, 0.0f, 0.0f);
//     }

    Vector2 beginPosition = Vector2.zero;
    Vector2 endPosition = Vector2.zero;
    public float moveSpeed = 0.4f;

    void MoveCamera()
    {
        //Debug.Log("enter MoveCamera");

        if (Input.GetButtonDown("Fire1"))
        {
            beginPosition = Input.mousePosition;
        }

        if(Input.GetButton("Fire1"))
        {
            endPosition = Input.mousePosition;

            float xGap = endPosition.x - beginPosition.x;
            //float yGap = endPosition.y - beginPosition.y;

            beginPosition = Input.mousePosition;

            //transform.position.x - xGap * moveSpeed
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - yGap * moveSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - xGap * moveSpeed);
        }

//         if(Input.GetButtonUp("Fire1"))
//         {
//             //
//         }
    }

    void Update()
    {
        MoveCamera();

        //camera z -60 ~ 45
        if(transform.position.z >= 45)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 45.0f); 
        }

        if (transform.position.z <= -60)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -60.0f);
        }
    }

}
