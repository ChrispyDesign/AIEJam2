using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player;

    [Header("GameObjects")]
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> ActivateOnStart = new List<GameObject>();
    public List<GameObject> DeactivateOnStart = new List<GameObject>();
    [Header ("Variables")]
    public bool stayOnLevel;
    public Vector2 levelRange;
    public static List<bool> playerIn = new List<bool>();
    private bool inPlay = false;
    public static MainMenuManager activeManager;

    // Start is called before the first frame update
    void Start()
    {
        activeManager = this.GetComponent<MainMenuManager>();
        if (playerIn.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                playerIn.Add(false);
            }
        }
        else
        {
            for (int i = 0; i < playerIn.Count; i++)
            {
                if (playerIn[i] == true)
                {
                    GameObject GO = Instantiate(player, spawnPoints[0].position, player.transform.rotation);
                    GO.GetComponent<tempMovement>().playerNo = i;
                    CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inPlay == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && playerIn[0] == false)
            {
                playerIn[0] = true;
                GameObject GO = Instantiate(player, spawnPoints[0].position, player.transform.rotation);
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.S) && playerIn[0] == false)
            {
                playerIn[0] = true;
                GameObject GO = Instantiate(player, spawnPoints[0].position, player.transform.rotation);
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.A) && playerIn[0] == false)
            {
                playerIn[0] = true;
                GameObject GO = Instantiate(player, spawnPoints[0].position, player.transform.rotation);
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.D) && playerIn[0] == false)
            {
                playerIn[0] = true;
                GameObject GO = Instantiate(player, spawnPoints[0].position, player.transform.rotation);
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            /////////////////////
            if (Input.GetKeyDown(KeyCode.UpArrow) && playerIn[1] == false)
            {
                playerIn[1] = true;
                GameObject GO = Instantiate(player, spawnPoints[1].position, player.transform.rotation);
                GO.GetComponent<tempMovement>().playerNo = 1;
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && playerIn[1] == false)
            {
                playerIn[1] = true;
                GameObject GO = Instantiate(player, spawnPoints[1].position, player.transform.rotation);
                GO.GetComponent<tempMovement>().playerNo = 1;
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && playerIn[1] == false)
            {
                playerIn[1] = true;
                GameObject GO = Instantiate(player, spawnPoints[1].position, player.transform.rotation);
                GO.GetComponent<tempMovement>().playerNo = 1;
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && playerIn[1] == false)
            {
                playerIn[1] = true;
                GameObject GO = Instantiate(player, spawnPoints[1].position, player.transform.rotation);
                GO.GetComponent<tempMovement>().playerNo = 1;
                CameraMovement.activeCamera.CameraHooks.Add(GO.transform);
            }
        }
    }

    public void GameStart()
    {
        if (stayOnLevel == false)
            SceneManager.LoadScene(Mathf.RoundToInt(Random.Range(levelRange.x, levelRange.y)));
        else
        {
            for (int i = 0; i < ActivateOnStart.Count; i++)
            {
                ActivateOnStart[i].SetActive(true);
            }
            for (int i = 0; i < DeactivateOnStart.Count; i++)
            {
                DeactivateOnStart[i].SetActive(false);
            }
            inPlay = true;
        }
    }
}
