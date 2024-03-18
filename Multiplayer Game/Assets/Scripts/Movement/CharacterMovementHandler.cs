using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    [Header("Metric Parameters")]
    [SerializeField] private float moveSpeed = 30f;
    [HideInInspector] public NetworkCharacterControllerPrototype prototype;
    HealthHandler healthHandler;

    [HideInInspector] public CharacterInputHandler characterInputHandler;

    bool isReSpawnRequest;

    public Animator anime;
    public RigBuilder rig;

    MovementBaseState currentState;

    public IdleState idle= new IdleState();
    public CrouchState crouch = new CrouchState();
    public WalkState walk = new WalkState();
    public RunState run= new RunState();
    

    private void Awake()
    {
        prototype= GetComponent<NetworkCharacterControllerPrototype>();
        healthHandler = GetComponent<HealthHandler>();
        characterInputHandler= GetComponent<CharacterInputHandler>();
        
        

    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Switch(idle);
        
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void FixedUpdateNetwork()
    {
        
        if(Object.HasStateAuthority)
        {
            if(isReSpawnRequest)
            {
                ReSpawn();
                return;
            }

            if (healthHandler.isDead)
            {
                return;
            }

        }
        
        if(GetInput(out NetworkInputData data))
        {
            //Set the transform forward vector so as to update it remotely


            
            transform.forward = data.AimForwardVector;

            //Cancel out the rotation on the X axis to prevent the player from tilting

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            Vector3 movement= transform.forward* data.MovementInput.y + transform.right* data.MovementInput.x;
            movement.Normalize();

            prototype.Move(movement);

            if(data.isJump)
            {
                prototype.Jump();
                anime.Play("Jumping");
            }

            if (data.sprint)
            {
                
                Switch(run);
            }
            else
            {
                if(prototype.Velocity.magnitude>0.2f)
                {
                    Switch(walk);
                }
                else
                {
                    Switch(idle);
                }
                


            }

            if(data.crouch)
            {
                Switch(crouch);
            }

            float vertical= Mathf.Clamp(prototype.Velocity.z,-1,1);
            float horizontal = Mathf.Clamp(prototype.Velocity.x,-1,1);

            anime.SetFloat("Vertical", vertical);
            anime.SetFloat("Horizontal", horizontal);

        }

        

        FallReSpawn();
    }

    void FallReSpawn()
    {
        if(transform.position.y<-15)
        {
            if(Object.HasStateAuthority)
            {
                ReSpawn();
            }
        }
    }

    public void GetReSpawnRequest()
    {
        isReSpawnRequest = true;
    }

    void ReSpawn()
    {
        prototype.TeleportToPosition(Utils.RandomSpawnPoint());

        healthHandler.OnReSpawn();
        isReSpawnRequest = false;

        Debug.Log($"{Time.time} The Object {transform.name} Has been Respawed");

    }

    public void NetworkCharacterControllerState(bool isEnabled)
    {
        prototype.enabled = isEnabled;
    }


    public void Switch(MovementBaseState state)
    {
        currentState= state;
        currentState.EnterState(this);

    }
    
}
