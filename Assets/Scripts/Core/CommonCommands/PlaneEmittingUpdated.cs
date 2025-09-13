using SelfishFramework.Src.Core.CommandBus;

namespace Core.CommonCommands
{
    public struct PlaneEmittingUpdated : IGlobalCommand
    {
        public bool Status; 
    }
}