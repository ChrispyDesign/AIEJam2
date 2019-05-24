using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInsideArea : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public Vector2 spawnRateBounds;

    private float spawnRate;
    private float spawnRateTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRateTimer >= spawnRate)
        {
            int sel = Mathf.RoundToInt(Random.Range(0, objects.Count));
            spawnRate = Random.Range(spawnRateBounds.x, spawnRateBounds.y);
            Collider col = GetComponent<Collider>();
            Vector3 pos = new Vector3(Random.Range(col.bounds.min.x, col.bounds.max.x), Random.Range(col.bounds.min.y, col.bounds.max.y), Random.Range(col.bounds.min.z, col.bounds.max.z));
            Instantiate(objects[sel], pos, Random.rotation);
            spawnRateTimer = 0;
        }
        spawnRateTimer += Time.deltaTime;
    }
}
