using Core.Commands;
using Newtonsoft.Json;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Commands;

namespace Core.Systems
{
    public sealed partial class SaveLoadSystem : BaseSystem, IReactGlobal<SaveCommand>, IReactGlobal<LoadProgressCommand>
    {
        private const string SAVE_KEY = "SAVE_KEY";
        
        public override void InitSystem() { }

        void IReactGlobal<SaveCommand>.ReactGlobal(SaveCommand command) => SaveEntitiesData();
        void IReactGlobal<LoadProgressCommand>.ReactGlobal(LoadProgressCommand command) => LoadEntitiesData();

        private void SaveEntitiesData()
        {
//             JSONEntityContainer container = new JSONEntityContainer();
//             container.SerializeEntitySavebleOnly(Owner);
//             if (container.Components.Count == 0 && container.Systems.Count == 0)
//             {
//                 HECSDebug.LogWarning($"Nothing to save on {Owner.ID}");
//                 return;
//             }
//             string json = JsonConvert.SerializeObject(container);
// #if UNITY_WEBGL && !UNITY_EDITOR
//             PlayerPrefs.SetString(SAVE_KEY, json);
//             PlayerPrefs.Save();           
// #else
//             string path = SavePathProvider.GetSavePath(ContainerIndex());
//             SaveManager.SaveJson(path, json);
// #endif
        }

        private void LoadEntitiesData()
        {
// #if UNITY_WEBGL && !UNITY_EDITOR
//             var hasSaves = PlayerPrefs.HasKey(SAVE_KEY);
//             var json = PlayerPrefs.GetString(SAVE_KEY);           
// #else
//             var hasSaves = SaveManager.TryLoadJson(SavePathProvider.GetSavePath(ContainerIndex()), out string json);
// #endif
//             if (hasSaves)
//             {
//                 JSONEntityContainer container = JsonConvert.DeserializeObject<JSONEntityContainer>(json);
//                 container.DeserializeToEntity(Owner);
//             }
        }

        // private string ContainerIndex() =>
            // Owner.GetComponent<ActorContainerID>().ContainerIndex.ToString();
    }
}