using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject car;
    public List<Transform> spawnBoundaries;
    public float spawnPerMinute;

    private float spawnTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer >= 60/spawnPerMinute)
        {
            int spawnAreaNo = Mathf.RoundToInt(Random.Range(0, spawnBoundaries.Count));
            Collider spawnArea = spawnBoundaries[spawnAreaNo].GetComponent<Collider>();
            Vector3 spawnPos = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));
            Instantiate(car, spawnPos, spawnBoundaries[spawnAreaNo].rotation);
            spawnTimer = 0;
        }
        spawnTimer += Time.deltaTime;
    }
}
