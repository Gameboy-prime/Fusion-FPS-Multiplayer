using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameMessageUiHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] messageBox;

    Queue messageQueue= new Queue();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMessageReceived(string message)
    {
        messageQueue.Enqueue(message);
        if(messageQueue.Count > 3 )
        {
            messageQueue.Dequeue();

        }

        int index = 0;

        foreach(string value in messageQueue)
        {
            messageBox[index].text = value;
            index++;

        }

    }
}
