using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
   public void LoadScene(string scene)
   {
        SceneManager.LoadScene(scene);
   }

    public void Quit()
    {
        Application.Quit();
    }
}
