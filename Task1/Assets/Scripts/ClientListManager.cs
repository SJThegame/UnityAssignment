using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class ClientData
{
    public List<Client> clients;
}

[System.Serializable]
public class Client
{
    public string label;
    public int points;
}

public class ClientListManager : MonoBehaviour
{
    public GameObject listItemPrefab;
    public Transform listContent;

    private readonly string url = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    private void Start()
    {
        StartCoroutine(GetClientData());
    }

    IEnumerator GetClientData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error while sending the request: " + www.error);
            }
            else
            {
                string jsonText = www.downloadHandler.text;
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
                ClientData clientData = JsonUtility.FromJson<ClientData>(jsonText);
                PopulateClientList(clientData);
            }
        }
    }

    void PopulateClientList(ClientData clientData)
    {
        foreach (Transform child in listContent)
        {
            Destroy(child.gameObject); // Clear existing list items if any
        }

        foreach (Client client in clientData.clients)
        {
            GameObject listItem = Instantiate(listItemPrefab, listContent);
            Text labelTxt = listItem.transform.Find("Label").GetComponent<Text>();
            Text pointsTxt = listItem.transform.Find("Points").GetComponent<Text>();

            labelTxt.text = client.label;
            pointsTxt.text = client.points.ToString();
        }
    }
}
