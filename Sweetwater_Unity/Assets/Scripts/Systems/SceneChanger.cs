using Base;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class SceneChanger : DescriptionMono
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}