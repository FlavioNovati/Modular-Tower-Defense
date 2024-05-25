using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerBase PlayerBase;
    [SerializeField] private Canvas EndScreen;

    private void Awake()
    {
        //Set up Instance
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        EndScreen.gameObject.SetActive(false);
        PlayerBase.OnBaseDestroyed += EndGame;
    }

    private void EndGame()
    {
        Time.timeScale = 0.0f;
        EndScreen.gameObject.SetActive(true);
    }
}
