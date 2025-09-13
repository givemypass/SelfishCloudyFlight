using System;
using System.IO;
using Core.Commands;
using Core.CommonComponents;
using Newtonsoft.Json;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.Features.Serialization;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Commands;
using UnityEngine;

namespace Core.Systems
{
    public sealed partial class SaveLoadSystem : BaseSystem, IReactGlobal<SaveCommand>, IReactGlobal<LoadProgressCommand>
    {
        public override void InitSystem() { }

        void IReactGlobal<SaveCommand>.ReactGlobal(SaveCommand command) => Save();
        void IReactGlobal<LoadProgressCommand>.ReactGlobal(LoadProgressCommand command) => Load();

        private void Save()
        {
            ref var playerProgress = ref Owner.Get<PlayerProgressComponent>(); 
            var saveModel = new SaveModel
            {
                LevelIndex = playerProgress.LevelIndex,
                TutorialPassed = playerProgress.TutorialPassed,
            };
            string json = JsonConvert.SerializeObject(saveModel);
            JsonHelper.SaveJson(SavePath(), json);
            SLog.Log("Game saved successfully.");
        }

        private void Load()
        {
            if (JsonHelper.TryLoadJson(SavePath(), out var json))
            {
                var saveModel = JsonConvert.DeserializeObject<SaveModel>(json);
                ref var playerProgress = ref Owner.Get<PlayerProgressComponent>();
                playerProgress.LevelIndex = saveModel.LevelIndex;
                playerProgress.TutorialPassed = saveModel.TutorialPassed;
                SLog.Log("Game loaded successfully.");
            }
            else
            {
                SLog.LogWarning("No save data found.");
            } 
        }

        private static string SavePath()
        {
            return Path.Combine(Application.persistentDataPath, "save.json");
        }
        
        [JsonObject]
        [Serializable]
        private struct SaveModel
        {
            [JsonProperty("LevelIndex")]
            public int LevelIndex;
            [JsonProperty("TutorialPassed")]
            public bool TutorialPassed;
        }
    }
}