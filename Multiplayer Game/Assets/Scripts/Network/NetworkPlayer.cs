using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNameText;

    bool hasSetMessage=false;

    NetworkInGameMessage networkMessage;

    public LocalCameraHandler localCameraHandler;

    public GameObject localUI;

    public GameObject visibleGun;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }
    public static NetworkPlayer local { get;  set; }

    private void Awake()
    {
        networkMessage= GetComponent<NetworkInGameMessage>();
    }
    public override void Spawned()
    {
        
        if (Object.HasInputAuthority)
        {
            local = this;

            Debug.Log("Has spawned a local player");

            Camera.main.gameObject.SetActive(false);

            FindObjectOfType<CanvasDisable>().gameObject.SetActive(false);
            FindFirstObjectByType<SoundManager>().ChangeMusic();

            RPC_SetNickName(PlayerPrefs.GetString("nickName"));

        }

        else
        {
            //This is neccessary to ensure that is is the perspective of the player that the camera is showing
            //Because in Unity, the latest camera is the one that Rendering and display takes place
            //So if a new player enters and it does not have input authority the camera will be disable
            Camera localCam= GetComponentInChildren<Camera>();
            localCam.enabled = false;

            localUI.SetActive(false);
            visibleGun.SetActive(false);

            AudioListener audioListener= GetComponentInChildren<AudioListener>();
            audioListener.enabled = false ;
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(Object.HasStateAuthority)
        {
            if(Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject))
            {
                if(playerLeftNetworkObject == Object)
                {
                    local.GetComponent<NetworkInGameMessage>().SendInGameMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>().nickName.ToString(), "Has Left");
                }
            }
        }
        if (player==Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }



        
    }


    //using the static function to call the function to set the name
    public static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {


        changed.Behaviour.NickNameChange();

    }

    private void NickNameChange()
    {
        Debug.Log("The player name has been set");
        playerNameText.text= nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]

    public void RPC_SetNickName(string nickName, RpcInfo info= default)
    {
        this.nickName = nickName;

        if(!hasSetMessage)
        {
            networkMessage.SendInGameMessage(nickName, "Has Joined");
            hasSetMessage= true;

        }


    }

    
}
