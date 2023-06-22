using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFirstLevel : MonoBehaviour
{
    public void LoadFirst()
    {
        SceneManager.LoadScene(1);
    }
}
