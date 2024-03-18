using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateManager : NetworkBehaviour
{

    public LayerMask aimLayer;

    public Transform aimPos;

    WeaponManager weaponManager;

    public Camera cam;

    Vector3 origin;
    public NetworkMecanimAnimator anime;

    private void Awake()
    {
        weaponManager= GetComponent<WeaponManager>();
        
    }

    private void Start()
    {
        //anime.SetTrigger("Aim");


    }

    public override void Render()
    {
        
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (Runner.LagCompensation.Raycast(weaponManager.originPoint.position, data.AimForwardVector, 300, Object.InputAuthority, out var hit, aimLayer, HitOptions.IncludePhysX))
            {
                if (hit.Collider != null)
                {
                    if (aimPos != null)
                    {
                        aimPos.transform.position = Vector3.Lerp(aimPos.transform.position, hit.Point, Runner.DeltaTime * 40);
                    }

                }

                if (hit.Hitbox != null)
                {
                    if (aimPos != null)
                    {
                        aimPos.transform.position = Vector3.Lerp(aimPos.transform.position, hit.Point, Runner.DeltaTime * 40);
                    }
                }


            }
        }
    }

    /*private void Update()
    {
        origin = new Vector3(Screen.width / 2, Screen.height / 2,0);

        Ray ray = cam.ScreenPointToRay(origin + new Vector3(0,0,1));

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayer))
        {

            aimPos.transform.position = hit.point;//Vector3.Lerp(aimPos.transform.position, hit.point, 12 * Time.deltaTime);


        }
    }*/
}
