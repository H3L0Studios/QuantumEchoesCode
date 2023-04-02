using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] int nextScene;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(nextScene);
    }


}
