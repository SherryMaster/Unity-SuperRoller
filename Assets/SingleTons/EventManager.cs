using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Singleton instance
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<EventManager>();
            return _instance;
        }
    }

    public UnityEvent levelGenRestart;
    public UnityEvent checkpointsCompleteEvent;
    public UnityEvent levelCompleteEvent;

    public int total_checkpoints = 0;
    public int ckeckpoints_checked = 0;

    private void Update()
    {
        // Debug.Log("Checkpoints Total: " + total_checkpoints + " Checkpoints Checked: " + ckeckpoints_checked + " Checkpoints Remaining: " + (total_checkpoints - ckeckpoints_checked));
    }

    public void LevelGenerarionRestart()
    {
        if (levelGenRestart != null)
        {
            levelGenRestart.Invoke();
            checkpointsReset();
        }
        else
        {
            Debug.LogWarning("EventManager: levelGenRestart event is null. Cannot invoke.");
        }
    }

    public void checkpointsComplete()
    {
        if (checkpointsCompleteEvent != null)
        {
            checkpointsCompleteEvent.Invoke();
        }
    }

    public void checkpointsReset()
    {
        total_checkpoints = 0;
        ckeckpoints_checked = 0;
    }

    public void levelComplete()
    {
        if (levelCompleteEvent != null)
        {
            levelCompleteEvent.Invoke();
            checkpointsReset();
        }
    }

}
