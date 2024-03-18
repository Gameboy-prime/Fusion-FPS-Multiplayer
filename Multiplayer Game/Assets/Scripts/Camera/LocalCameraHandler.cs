using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    public Transform cameraAnchorPoint;
    Camera localCam;

    float camRotationX=0;
    float camRotationY=0;

    Vector2 viewInput;

    NetworkCharacterControllerPrototype prototype;
    private void Awake()
    {
        localCam = GetComponent<Camera>();
        prototype = GetComponentInParent<NetworkCharacterControllerPrototype>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (localCam.enabled)
        {
            localCam.transform.SetParent(null);
        }
        localCam.gameObject.transform.position = cameraAnchorPoint.position;
    }

    private void LateUpdate()
    {
        if (cameraAnchorPoint == null)
        {
            return;
        }

        if (!localCam.enabled)
        {
            return;
        }

        

        //set the possition of the camera to that of the anchor point
        Vector3 localAnchorPoint= cameraAnchorPoint.localPosition;
        Vector3 globalAnchorPoint = cameraAnchorPoint.transform.parent.TransformPoint(localAnchorPoint);
        localCam.gameObject.transform.position = globalAnchorPoint;
        //Calculate and set the rotation of the camera 

        camRotationX += viewInput.y * Time.deltaTime * prototype.rotationXSpeed;
        camRotationX = Mathf.Clamp(camRotationX, -90, 90);

        camRotationY += viewInput.x * Time.deltaTime * prototype.rotationSpeed;

        //Set the rotation of the local camera;

        localCam.transform.rotation = Quaternion.Euler(camRotationX, camRotationY, 0);


        
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;

    }
}
