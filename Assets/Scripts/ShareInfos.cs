using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UI;
using Proyecto26;

public class ShareInfos : MonoBehaviour
{
    [SerializeField]
    private InputField name;

    [SerializeField]
    private InputField email;

    private readonly string basePath = "https://epsi-419434555953333401.myfreshworks.com/crm/sales/api/contacts";
	private readonly string token = "8sV1aiDbcLZb-cSFqqcUFQ";
	private RequestHelper currentRequest;

    public void SendInfos()
    {
        RestClient.DefaultRequestHeaders["Authorization"] = "Token token=" + token;
		currentRequest = new RequestHelper
		{
			Uri = basePath,

			Body = new Contact
            {
				last_name = name.text,
				email = email.text,
			},
	
			EnableDebug = true
		};

		RestClient.Post<Contact>(currentRequest)
		.Then(res => {

			// And later we can clear the default query string params for all requests
			//RestClient.ClearDefaultParams();
			// this.LogMessage("Success", "enregistrement ok");
			//this.LogMessage("Success", JsonUtility.ToJson(res, true));
			Debug.Log("Envoie OK");
		})
		.Catch(err => Debug.Log("Error" + err.Message));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
	public class Contact
	{

		public string last_name;

		public string email;

		public override string ToString()
		{
			return UnityEngine.JsonUtility.ToJson(this, true);
		}
	}
}
