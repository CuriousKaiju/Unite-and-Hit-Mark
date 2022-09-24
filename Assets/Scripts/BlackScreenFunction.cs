using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenFunction : MonoBehaviour
{
    [SerializeField] private string _nextScene;
    public void Destroyer()
    {
        Destroy(gameObject);
    }
    public void SetLoadedScene(string nameOfscene)
    {
        _nextScene = nameOfscene;
    }
    public void StartLoadScene()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
