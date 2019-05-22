using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Transforms")]
    public List<Transform> CameraHooks = new List<Transform>();
    [Header("Variables")]
    public float zoomMultiplier;
    public float zoomMin;
    public float shakeAmount;
    public float shakeLength;
    public Vector3 posOffset;
    [Header("Speed")]
    public float camMovSpeed;

    [HideInInspector]
    public static float shakeTimer = 10;

    private Vector3 tarPos;
    private float tarZoom;

    // Use this for initialization
    void Start()
    {
        GetPlayerObjects();
    }

    private void GetPlayerObjects()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //CameraHooks = PlayerManager.Instance.m_players;
        
        for (int i = 0; i < players.Length; i++)
        {
            CameraHooks.Add(players[i].transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetPlayerObjects();
        if (CameraHooks.Count > 0)
        {
            CamMov();
            CamZoom();
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(tarPos.x, tarZoom * zoomMultiplier, tarPos.z) + posOffset, camMovSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
            shakeTimer = 0;

        //CamShake
        if (shakeTimer < shakeLength)
            CamShake();
        else
            transform.GetChild(0).localPosition = Vector3.zero;


    }

    private void CamMov()
    {
        tarPos = Vector3.zero;

        for (int i = 0; i < CameraHooks.Count; i++)
        {
            tarPos += CameraHooks[i].transform.position;
        }
        tarPos /= CameraHooks.Count;
    }

    private void CamZoom()
    {
        tarZoom = 0;
        for (int i = 0; i < CameraHooks.Count; i++)
        {
            for (int j = 0; j < CameraHooks.Count; j++)
            {
                if (j != i)
                {
                    float tempZoom = Vector3.Distance(CameraHooks[j].transform.position, CameraHooks[i].transform.position);
                    if (tempZoom > tarZoom)
                        tarZoom = tempZoom;
                }
            }
        }
        if (tarZoom < zoomMin)
            tarZoom = zoomMin;
    }

    private void CamShake()
    {
        transform.GetChild(0).localPosition = Random.insideUnitSphere * shakeAmount;
        shakeTimer += Time.deltaTime;
    }
}