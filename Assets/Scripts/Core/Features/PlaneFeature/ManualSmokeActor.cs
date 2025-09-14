using Core.CommonComponents;
using SelfishFramework.Src.Unity;

namespace Core.Features.PlaneFeature
{
    public partial class ManualSmokeActor : Actor
    {
        public ManualSmokeTagComponent ManualSmokeTagComponent = new(); 
        public WritingSmokeVFXMonoProvider WritingSmokeVFXMonoProvider = new();
    }
}