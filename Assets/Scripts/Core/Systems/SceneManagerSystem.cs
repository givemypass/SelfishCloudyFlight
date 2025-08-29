using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core.Systems;
using UnityEngine.SceneManagement;

namespace Core.Systems
{
    public sealed partial class SceneManagerSystem : BaseSystem
    {
        private string _loadedScene;

        public override void InitSystem()
        {
        }

        public async UniTask LoadScene(string sceneName)
        {
            if (_loadedScene != null)
                SceneManager.UnloadSceneAsync(_loadedScene);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
            _loadedScene = sceneName;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadedScene));
        } 
    }
}