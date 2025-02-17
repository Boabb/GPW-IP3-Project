using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScreen : MonoBehaviour
{

    [SerializeField]private float delayTime = 10f;

    private float timer;

    public GameObject levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.anyKeyDown || timer >= delayTime)
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        levelLoader.GetComponent<LevelLoader>().LoadNextLevel();
    }
}
