using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Material offMaterial;
    public Material onMaterial;
    public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.total_checkpoints++;
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            GetComponent<Renderer>().material = onMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = offMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !check)
        {
            check = true;
            other.gameObject.GetComponent<Player>().SetSpawnPoint(gameObject);
            EventManager.Instance.ckeckpoints_checked++;

            if (EventManager.Instance.ckeckpoints_checked == EventManager.Instance.total_checkpoints)
            {
                EventManager.Instance.checkpointsComplete();
            }
        }
    }
}
