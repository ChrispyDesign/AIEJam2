using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject teleportParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelGeneration.activeManager.freeBasePropHolders.Count == 0)
        {
            LevelGeneration.activeManager.freeBasePropHolders.Add(transform.parent);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int portalDestination = Random.Range(0, LevelGeneration.activeManager.freeBasePropHolders.Count);

            PlayerController player = other.GetComponent<PlayerController>();
            if (player)
            {
                player.CharacterController.enabled = false;
                other.transform.position = LevelGeneration.activeManager.freeBasePropHolders[portalDestination].position;
                player.CharacterController.enabled = true;
            }
            Instantiate(teleportParticles, LevelGeneration.activeManager.freeBasePropHolders[portalDestination]);
            CameraMovement.shakeTimer = 0;
        }
    }
}
