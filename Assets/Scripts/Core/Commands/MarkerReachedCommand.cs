using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.Commands
{
    public struct MarkerReachedCommand : IGlobalCommand
    {
        public Vector3 ScreenPosition;
        public bool MarkerIsEnd;
    }
}