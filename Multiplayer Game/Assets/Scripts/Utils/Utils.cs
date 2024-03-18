using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    
    
    public static Vector3 RandomSpawnPoint()
    {


        return new Vector3(Random.Range(-20, 20), 2, Random.Range(-3, 0));

    }
}
