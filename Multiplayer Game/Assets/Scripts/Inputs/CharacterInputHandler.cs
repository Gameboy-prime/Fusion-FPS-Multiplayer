using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    public Vector2 movementInput = Vector2.zero;
    Vector2 viewInput= Vector2.zero;

    //Keys was pressed
    private bool jumpKeyWasPressed;
    private bool fireKeyWasPressed;
    public  bool crouchKeyWasPressed;
    public bool sprintKeyWasPressed;

    LocalCameraHandler localCameraHandler;

    CharacterMovementHandler characterMovementHandler;

   


    private void Awake()
    {
        
        characterMovementHandler= GetComponent<CharacterMovementHandler>();
        localCameraHandler= GetComponentInChildren<LocalCameraHandler>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This is essestial to ensure that only players that has input authority can control the player object because the update function is run on every client that is connected to the server
        if (!characterMovementHandler.Object.HasInputAuthority)
        {
            return;
        }


        

        //Get View Input from the player

        viewInput.x = Input.GetAxis("Mouse X");
        viewInput.y = Input.GetAxis("Mouse Y") * -1;//Multiplying by -1 is essential to invert the view of the mouse onto he x axis

        localCameraHandler.SetViewInputVector(viewInput);

        //Get Movement Input from the player
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        



        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            fireKeyWasPressed = true;

        }


        
    }

    public NetworkInputData GetInputData()
    {
        NetworkInputData networkInputData= new NetworkInputData();


        //Set the aim data
        networkInputData.AimForwardVector = localCameraHandler.transform.forward;

        //set the movement input
        networkInputData.MovementInput= movementInput;

        //Set the sprint input

        networkInputData.sprint = sprintKeyWasPressed;

        //Set the crouch input

        networkInputData.crouch= crouchKeyWasPressed;

        //Set the jump input
        networkInputData.isJump= jumpKeyWasPressed;

        networkInputData.canFire = fireKeyWasPressed;


        //It is neccessary to set the jumpkey to false back after the input has been broadcast to the network because of the delay in the network
        jumpKeyWasPressed = false;
        fireKeyWasPressed = false;

        return networkInputData;
    }
}
