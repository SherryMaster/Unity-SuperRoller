using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.checkpointsCompleteEvent.AddListener(Disable_Barrier);
    }

    void Disable_Barrier()
    {
        gameObject.SetActive(false);
    }
}
