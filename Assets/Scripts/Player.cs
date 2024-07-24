using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject StartPoint;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.levelGenRestart.AddListener(onLevelReset);
        StartPoint = GameObject.Find("StartLevelSpawn");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        player.transform.position = StartPoint.transform.position;
        rb.velocity = Vector3.zero;
    }

    public void SetSpawnPoint(GameObject spawn)
    {
        StartPoint = spawn;
    }

    void onLevelReset()
    {
        StartPoint = GameObject.Find("StartLevelSpawn");
        ResetPlayer();
    }
}
