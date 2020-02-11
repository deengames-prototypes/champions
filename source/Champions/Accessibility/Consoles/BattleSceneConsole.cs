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

        public BattleSceneConsole()
        {
            this.replThread = new Thread(() => 
            {
                while (isRunning)
                {
                    Console.WriteLine("Your command? ");
                    var text = Console.ReadLine();
                    Console.WriteLine($"You said: {text}!");
                }
            });
            this.replThread.Start();
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
                partyText.Append($"A level {member.Level} {member.Specialization}, ")
            }
            
            partyText.Append("They are facing: 5 slimes.");
            Console.WriteLine(partyText.ToString());
        }
    }
}