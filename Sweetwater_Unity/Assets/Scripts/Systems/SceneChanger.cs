using UnityEngine;

namespace Systems
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
