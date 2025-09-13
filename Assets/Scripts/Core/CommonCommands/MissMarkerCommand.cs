using System;
using System.Windows.Input;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Components;
using ICommand = SelfishFramework.Src.Core.CommandBus.ICommand;

namespace Core.Commands
{
    [Serializable]
    public struct MissMarkerCommand : IGlobalCommand
    {
    }
}