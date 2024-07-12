using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ScenesManager", menuName = "SceneManagement/ScenesManager")]
public class ScenesManager : ScriptableObject
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
    public void LoadScene(String sceneName) => SceneManager.LoadScene(sceneName.Value);
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void Quit() => Application.Quit();

    public static void ReloadSceneStatic() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
