using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICall : MonoBehaviour
{

    [System.Serializable]
    public class ClientData
    {
        public List<Client> clients;
    }

    [System.Serializable]
    public class Client
    {
        public string id;
        public string name;
        // Add other properties as needed based on the JSON response
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetClientData());
    }

    IEnumerator GetClientData()
    {
        string url = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result!=UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error while sending request: " + www.error);
            }
            else
            {
                string jsonText = www.downloadHandler.text;
                ClientData clientData = JsonUtility.FromJson<ClientData>(jsonText);

                Debug.Log("First Client Name: " + clientData.clients[0].name);
            }
        }
    }

}
