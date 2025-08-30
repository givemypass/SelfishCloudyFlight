using SelfishFramework.Src.Core.CommandBus;
using UnityEngine;

namespace Core.Commands
{
    public struct ColorSelectedCommand : IGlobalCommand
    {
        public Color Color;
        
        public ColorSelectedCommand(Color color)
        {
            Color = color; 
        }
    }
}