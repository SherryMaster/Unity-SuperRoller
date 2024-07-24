using UnityEngine;

public class WinPad : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Level Complete");
            EventManager.Instance.levelComplete();
        }
    }


}
