using Puffin.Core;
using Puffin.Core.Ecs;

namespace DeenGames.Champions.Scenes
{
    class PickUnitsScene : Scene
    {
        public PickUnitsScene()
        {
            this.Add(new Entity().Label("Hello from Puffin!"));
        }
    }
}