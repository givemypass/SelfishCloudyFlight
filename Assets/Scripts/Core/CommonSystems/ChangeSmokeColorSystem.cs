using Core.Commands;
using Core.CommonComponents;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;

namespace Core.Systems
{
    public sealed partial class ChangeSmokeColorSystem : BaseSystem, IReactGlobal<ColorSelectedCommand>
    {
        private Filter _filter;

        public override void InitSystem()
        {
            _filter = Owner.GetWorld().Filter.With<WritingSmokeVFXMonoProvider>().Build();
        }

        void IReactGlobal<ColorSelectedCommand>.ReactGlobal(ColorSelectedCommand command)
        {
            foreach (var entity in _filter)
            {
                ref var provider = ref entity.Get<WritingSmokeVFXMonoProvider>();
                provider.MonoComponent.SetColor(command.Color);
            }             
        }
    }
}