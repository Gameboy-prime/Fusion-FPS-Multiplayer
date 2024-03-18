using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static Fusion.NetworkCharacterController;

public class WeaponManager : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    [SerializeField] float fireRate = 1.2f;


    //References
    
    public float range = 100f;
    public LayerMask shootables;
    public Transform originPoint;

    HealthHandler healthHandler;


    //Effects Example
    public GameObject bulletHole;
    public GameObject bulletHolePlayer;
    public GameObject muzzleFlash;
    public Transform attackPoint;

    //NetworkRunner runner;

    //Audio

    public AudioSource source;
    public AudioClip gunShot;



    float timePastFire;

    private void Awake()
    {
        //runner= GetComponent<NetworkRunner>();
        healthHandler= GetComponent<HealthHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    public override void FixedUpdateNetwork()
    {
        if(healthHandler.isDead) return;
        
        if(GetInput(out NetworkInputData data))
        {
            if(data.canFire)
            {
                
                Fire(data.AimForwardVector);
                
            }
        }
    }

    void Fire(Vector3 aimForward)
    {

        //This is to limit the fire rate and private autofire, this is neccessay because some client might be disadvataged based on thier ping and network
        if(Time.time-timePastFire< fireRate)
        {
            return;

        }



        StartCoroutine(FireEffects());

        Runner.LagCompensation.Raycast(originPoint.position, aimForward, range, Object.InputAuthority, out var hit, shootables, HitOptions.IncludePhysX);


        bool hasHitOther = false;
        float distance = 100f;

        if(hit.Distance>0)
        {
            distance = hit.Distance;
        }

        if(hit.Hitbox != null)
        {
            Debug.Log($"{transform.name} Has hit HitBox {hit.Hitbox.transform.root.name}");

            if (Object.HasStateAuthority)
            {
                hit.Hitbox.transform.root.GetComponent<HealthHandler>().TakeDamage();
            }
            hasHitOther = true;

            GameObject instantiatedBulletHole = Instantiate(bulletHolePlayer, hit.Point, Quaternion.identity);
            Destroy(instantiatedBulletHole,2f);

        }

        if(hit.Collider != null)
        {
            Debug.Log($"{transform.name} Has Hit Vollider {hit.Collider.transform.name}");

            // Instantiate the bullet hole object at the position where the raycast hit
            GameObject instantiatedBulletHole = Instantiate(bulletHole, hit.Point, Quaternion.identity);
            Destroy(instantiatedBulletHole,2f);

            // Ensure that the instantiated object is spawned with the correct network authority
            //Runner.Spawn(instantiatedBulletHole,hit.Point,Quaternion.identity, Object.InputAuthority);
        }
        //Runner.Spawn(bulletHole, hit.Point, Quaternion.identity, Object.InputAuthority);
        //Instantiate(bulletHole, hit.Normal, Quaternion.identity);
        
        
        if(hasHitOther)
        {
            Debug.DrawRay(originPoint.position, aimForward * distance, Color.red, 1);

        }

        else
        {
            Debug.DrawRay(originPoint.position , aimForward * distance, Color.green, 1);
        }
        
        

        timePastFire = Time.time;

    }

    IEnumerator FireEffects()
    {
        Debug.Log("The Player can fire");

        //WHen this is set to true, the OnFireChanged function is called
        isFiring =true;
        
        GameObject instantiatedMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        Destroy(instantiatedMuzzleFlash, .2f);
        if(Object.HasInputAuthority)
        {
            source.PlayOneShot(gunShot);
        }
        //This wait is essentital so that fusion and the server can update anything that needs to be updated
        yield return new WaitForSeconds(0.09f);

        isFiring=false;
    }

    static void OnFireChanged(Changed<WeaponManager> change)
    {

        bool currentFiring = change.Behaviour.isFiring;

        Debug.Log($"The Value of the isfiring After it was triggered is {change.Behaviour.isFiring}");


        //This is used to check the previous value of the isfiring variable
        change.LoadOld();

        bool previousFiring = change.Behaviour.isFiring;

        Debug.Log($"The value of the isFireing before it was triggerd id {change.Behaviour.isFiring}");
        

        //Check to ensure that a change has occured to the firing variable before calling onthe remote fire

        if(currentFiring  && !previousFiring)
        {
            change.Behaviour.OnFireRemote();

        }

    }

    private void OnFireRemote()
    {

        //This is neccessary to update the effect on server as welll as other clients
        if (!Object.InputAuthority)
        {
            GameObject instantiatedBulletHole = Instantiate(bulletHolePlayer, attackPoint.position, Quaternion.identity);
            
        }
    }


}
