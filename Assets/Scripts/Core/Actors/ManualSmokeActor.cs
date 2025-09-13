using Core.CommonComponents;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class ManualSmokeActor : Actor
    {
        public ManualSmokeTagComponent ManualSmokeTagComponent = new(); 
        public WritingSmokeVFXMonoProvider WritingSmokeVFXMonoProvider = new();
    }
}