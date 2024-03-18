using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour,INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    



    async void StartGame(GameMode mode)
    {
        _runner= gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput= true;

        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager= gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }

    
    /*private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 50), "Host"))
            {
                
            }
            if (GUI.Button(new Rect(0, 50, 200, 50), "Join"))
            {
                
            }
        }
        
    }*/

    public void Join()
    {
        if(_runner==null)
        {
            StartGame(GameMode.Client);
        }
    }

    public void Host()
    {
        if(_runner== null)
        {
            StartGame(GameMode.Host);
        }
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("The application has been connected to the server");
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    CharacterInputHandler inputHandler;
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputHandler == null && NetworkPlayer.local != null)
        {
            inputHandler= NetworkPlayer.local.GetComponent<CharacterInputHandler>();
        }

        if (inputHandler != null)
        {
            input.Set(inputHandler.GetInputData());
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    [SerializeField] NetworkObject playerPrefab;

    private Dictionary<PlayerRef,NetworkObject> characters= new Dictionary<PlayerRef, NetworkObject>();

    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            Vector3 spawnPos = Utils.RandomSpawnPoint();// new Vector3(runner.Config.Simulation.DefaultPlayers * 3, 1, 0);
            NetworkObject playerObject = runner.Spawn(playerPrefab, spawnPos, Quaternion.identity, player);
            characters.Add(player, playerObject);

            

        }
        

        
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (characters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            characters.Remove(player);
        }
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

   
}
