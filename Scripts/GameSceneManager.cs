using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AudioManager.Instance == null)
        {
            Instantiate(Resources.Load<GameObject>("AudioManagerPrefab"));
        }
    }

  
}
