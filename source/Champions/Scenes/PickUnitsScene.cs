using Puffin.Core;
using Puffin.Core.Ecs;
using Puffin.Core.Ecs.Components;

namespace DeenGames.Champions.Scenes
{
    class PickUnitsScene : Scene
    {
        public PickUnitsScene()
        {
            var e = new Entity().Label("");
            this.Add(e);

            this.Add(new Entity().Move(400, 300).Colour(0xFF0000, 64, 64)
                .Overlap(64, 64, 0, 0, 
                    () => e.Get<TextLabelComponent>().Text = "Mouse on",
                    () => e.Get<TextLabelComponent>().Text = ""));
        }
    }
}