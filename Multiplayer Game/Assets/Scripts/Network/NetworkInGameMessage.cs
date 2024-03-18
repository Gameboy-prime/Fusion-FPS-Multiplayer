using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkInGameMessage : NetworkBehaviour
{

    InGameMessageUiHandler inGameMessageUiHandler;


    public void SendInGameMessage(string nickName, string message)
    {
        RPC_InGameMessage($"<b>{nickName}</b>  {message}");
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_InGameMessage(string message, RpcInfo info= default)
    {
        if(inGameMessageUiHandler == null)
        {
            inGameMessageUiHandler= NetworkPlayer.local.localCameraHandler.GetComponentInChildren<InGameMessageUiHandler>();
        }
        if(inGameMessageUiHandler != null)
        {
            inGameMessageUiHandler.OnMessageReceived(message);

        }

        

    }
}
