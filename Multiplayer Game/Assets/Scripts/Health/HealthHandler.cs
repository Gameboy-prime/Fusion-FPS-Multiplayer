using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HealthHandler : NetworkBehaviour
{
    [Networked (OnChanged = nameof(OnHealthChange))]
    byte health { get; set; }

    [Networked (OnChanged = nameof(OnStateChanged))]

    public bool isDead { get; set; }

    [SerializeField] const byte healthHp=5;

    bool initialized = false;

    //Reference for the UI and take damage

    public Color onHitColor;

    public Image onHitImage;

    public MeshRenderer defaultMeshRenderer;
    Color defaultBodyColor;

    public GameObject playerObject;
    public GameObject deathEffect;
    HitboxRoot hitRootBox;


    CharacterMovementHandler movementHandler;

    

    // Start is called before the first frame update

    private void Awake()
    {
        movementHandler=GetComponent<CharacterMovementHandler>();
        hitRootBox=GetComponentInChildren<HitboxRoot>();
        
    }
    void Start()
    {
        health = healthHp;

        isDead = false;

        initialized= true;

        defaultBodyColor= defaultMeshRenderer.material.color;


        
        
    }

    public void TakeDamage()
    {
        if(isDead)
        {
            return;
        }

        health -= 1;

        Debug.Log($"The Health of {transform.name} is {health}");

        if (health==0)
        {
            

            StartCoroutine(ServerSpawnCoRoutine());
            isDead = true;



        }




    }

    IEnumerator OnHitCoroutine()
    {
        defaultMeshRenderer.material.color = Color.green;

        

        if (Object.HasInputAuthority)
        {
            onHitImage.color = onHitColor;
        }

        yield return new WaitForSeconds(0.2f);

        defaultMeshRenderer.material.color = defaultBodyColor;

        if (Object.HasInputAuthority && !isDead)
        {
            onHitImage.color = new Color(0,0,0,0);
        }
    }

    IEnumerator ServerSpawnCoRoutine()
    {
        yield return new WaitForSeconds(2);

        movementHandler.GetReSpawnRequest();
    }

    static void OnHealthChange(Changed<HealthHandler> changed)
    {
        Debug.Log($"The Health of {changed.Behaviour.transform.name} is {changed.Behaviour.health}");


        byte newValue= changed.Behaviour.health;

        changed.LoadOld();

        byte oldValue= changed.Behaviour.health;

        if(newValue<oldValue)
        {
            changed.Behaviour.OnHealthReduce();
        }

    }

    void OnHealthReduce()
    {
        if(!initialized)
        {
            return;
        }

        StartCoroutine(OnHitCoroutine());

    }

    static void OnStateChanged(Changed<HealthHandler> changed)
    {
        Debug.Log($"The State of {changed.Behaviour.transform.name} is {changed.Behaviour.isDead}");
        bool newValue = changed.Behaviour.isDead;


        changed.LoadOld();

        bool oldValue= changed.Behaviour.isDead;

        if(newValue)
        {
            changed.Behaviour.Die();
        }

        if(oldValue && !newValue)
        {
            changed.Behaviour.Revive();
        }

    }

    void Die()
    {

        StartCoroutine(Dying());



    }

    IEnumerator Dying()
    {
        movementHandler.rig.enabled = false;
        movementHandler.anime.Play("Falling Back Death");
        yield return new WaitForSeconds(2);
        hitRootBox.enabled = false;
        playerObject.gameObject.SetActive(false);

        movementHandler.NetworkCharacterControllerState(false);
        //Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
    }

    void Revive()
    {
        movementHandler.rig.enabled= true;
        movementHandler.Switch(movementHandler.idle);
        if(Object.HasInputAuthority)
        {
            onHitImage.color = new Color(0, 0, 0, 0);

        }
        hitRootBox.enabled = true;
        playerObject.gameObject.SetActive(true);

        movementHandler.NetworkCharacterControllerState(true);

    }

    public void OnReSpawn()
    {
        health = healthHp;
        isDead= false;
    }
}
