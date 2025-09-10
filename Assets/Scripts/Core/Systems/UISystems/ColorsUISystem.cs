using Core.Commands;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Systems
{
    public sealed partial class ColorsUISystem : BaseSystem, IAfterEntityInit
    {
        private LevelScreenUIMonoComponent _monoComponent;

        public override void InitSystem()
        {
        }

        public void AfterEntityInit()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            foreach (var colorButton in _monoComponent.ColorButtons)
            {
                colorButton.OnPressed += () =>
                {
                    var color = colorButton.Image.color;
                    Owner.GetWorld().Command(new ColorSelectedCommand(color));
                };
            }
        }
    }
}