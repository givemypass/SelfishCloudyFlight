using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.Features.LevelFeature.Commands
{
    public struct MarkerReachedCommand : IGlobalCommand
    {
        public bool MarkerIsEnd;
    }
}