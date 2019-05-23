using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStart : MonoBehaviour
{
    private List<GameObject> inArea = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inArea.Count == 2)
            MainMenuManager.activeManager.GameStart();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            inArea.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            inArea.Remove(other.gameObject);
    }
}
