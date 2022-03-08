using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject Panel;

    public void OpenPanel()
    {
        if(Panel != null)
        {
            bool isActive  = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
