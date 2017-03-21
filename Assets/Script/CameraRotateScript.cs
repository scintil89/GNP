using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour
{
    public GameObject rotatePoint;

	// Update is called once per frame
	void Update ()
    {
        this.transform.RotateAround(rotatePoint.transform.position, Vector3.up, 0.1f);
	}
}
