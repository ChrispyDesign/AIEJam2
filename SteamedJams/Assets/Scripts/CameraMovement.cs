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
    public float zoomMax;
    [Space (20)]
    public float shakeAmount;
    public float shakeLength;
    [Space(20)]
    public float swayMax;
    public float swaySpeed;
    public float swayRotMax;
    public float swayRotSpeed;
    [Header("Speed")]
    public float camMovSpeed;

    [HideInInspector]
    public static float shakeTimer = 10;

    private Vector3 tarPos;
    private float tarZoom;
    private Vector3 swayTarget;
    private Vector3 swayRotateTarget;
    private Vector3 startRotation;
    private Vector3 currentRot;

    // Use this for initialization
    void Start()
    {
        startRotation = transform.GetChild(0).localEulerAngles;
        currentRot = startRotation;
        swayRotateTarget = startRotation;
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
        Vector3 offset = new Vector3(0, 0, -tarZoom * 0.5f);
        transform.position = Vector3.Lerp(transform.position, new Vector3(tarPos.x, tarZoom * zoomMultiplier, tarPos.z) + offset, camMovSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
            shakeTimer = 0;

        //CamShake
        if (shakeTimer < shakeLength)
            CamShake();
        else
            CamSway();


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
        if (tarZoom > zoomMax)
            tarZoom = zoomMax;
    }

    private void CamShake()
    {
        transform.GetChild(0).localPosition = Random.insideUnitSphere * shakeAmount;
        shakeTimer += Time.deltaTime;
    }

    private void CamSway()
    {
        /*
        Vector3 swayDif = swayTarget - transform.GetChild(0).localPosition;
        if (swayDif == Vector3.zero)
            swayTarget = Random.insideUnitSphere * swayMax;
        Vector3 targetPos = Vector3.ClampMagnitude(swayDif, Time.deltaTime * swaySpeed);
        transform.GetChild(0).localPosition += targetPos;
        */
        CamSwayRotate();
    }

    private void CamSwayRotate ()
    {
        Vector3 swayDif = swayRotateTarget - currentRot;
        if (swayDif == Vector3.zero)
            swayRotateTarget = (Random.insideUnitSphere * swayRotMax)+ startRotation;
        Vector3 targetRot = Vector3.ClampMagnitude(swayDif, Time.deltaTime * swayRotSpeed);
        currentRot += targetRot;
        transform.GetChild(0).localEulerAngles = currentRot;
    }
}