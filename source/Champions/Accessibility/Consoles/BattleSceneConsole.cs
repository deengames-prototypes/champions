using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DeenGames.Champions.Models;

namespace DeenGames.Champions.Accessibility.Consoles
{
    /// <summary>
    /// Encapsulates the state and reads/writes commands to the console for the battle scene.
    /// </summary>
    public class BattleSceneConsole : IDisposable
    {
        private Thread replThread;
        private bool isRunning = true;
        private List<Unit> party;
        private List<Unit> monsters;
        
        // Boxed int
        private BoxedInt numPotions;

        public BattleSceneConsole(BoxedInt numPotions)
        {
            this.numPotions = numPotions;

            this.replThread = new Thread(() => 
            {
                while (isRunning)
                {
                    Console.WriteLine("Your command? ");
                    var text = Console.ReadLine();
                    this.ProcessCommand(text);
                }
            });
            this.replThread.Start();
        }

        private void ProcessCommand(string text)
        {
            if (text == "help" || text == "h")
            {
                Console.WriteLine("Commands: h for help, i for inventory");
            }
            else if (text == "i" || text == "inv")
            {
                Console.WriteLine($"Inventory: {numPotions.Value} potions");
            }
        }

        public void Dispose()
        {
            this.isRunning = false;
            this.replThread.Join();
        }

        internal void StateParties(List<Unit> party, List<Unit> monsters)
        {
            this.party = party;
            this.monsters = monsters;

            StringBuilder partyText = new StringBuilder();
            
            partyText.Append("Your party includes: ");
            foreach (var member in party)
            {
                partyText.Append($"A level {member.Level} {member.Specialization}, ");
            }
            
            partyText.Append("They are facing: 5 slimes.");
            Console.WriteLine(partyText.ToString());
        }
    }
}