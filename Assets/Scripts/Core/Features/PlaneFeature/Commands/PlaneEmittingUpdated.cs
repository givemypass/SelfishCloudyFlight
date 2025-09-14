using SelfishFramework.Src.Core.CommandBus;

namespace Core.Features.PlaneFeature.Commands
{
    public struct PlaneEmittingUpdated : IGlobalCommand
    {
        public bool Status; 
    }
}