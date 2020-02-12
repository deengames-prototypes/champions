using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DeenGames.Champions.Models;
using DeenGames.Champions.Scenes;

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
        private BattleScene scene;
        
        // Boxed int
        private BoxedInt numPotions;

        public BattleSceneConsole(BattleScene scene, List<Unit> party, List<Unit> monsters, BoxedInt numPotions)
        {
            // This is bad. Use an event bus instead.
            this.scene = scene;
            this.party = party;
            this.monsters = monsters;
            this.numPotions = numPotions;
        }
        
        public void StartRepl()
        {
            this.replThread = new Thread(() => 
            {
                while (isRunning)
                {
                    Console.WriteLine("Your command? ");
                    var text = Console.ReadKey().KeyChar;
                    this.ProcessCommand(text);
                }
            });
            this.replThread.Start();
        }

        private void ProcessCommand(char input)
        {
            if (input == 'q') {
                this.PrintStats(this.monsters[0]);
            } else if (input == 'w') {
                this.PrintStats(this.monsters[1]);
            } else if (input == 'e') {
                this.PrintStats(this.monsters[2]);
            } else if (input == 'r') {
                this.PrintStats(this.monsters[3]);
            } else if (input == 't') {
                this.PrintStats(this.monsters[4]);
            }
            
            else if (input == 'a') {
                this.PrintStats(this.party[0]);
            } else if (input == 's') {
                this.PrintStats(this.party[1]);
            } else if (input == 'd') {
                this.PrintStats(this.party[2]);
            } else if (input == 'f') {
                this.PrintStats(this.party[3]);
            } else if (input == 'g') {
                this.PrintStats(this.party[4]);
            }

            else if (input == 'x')
            {
                Console.WriteLine("Bye!");
                Environment.Exit(0);
            }
            else if (input == 'h')
            {
                scene.IsActive = false;
                Console.WriteLine("Commands: h for help, i for inventory, p to pause, o to use a potion, g for info, x to quit. Q W E R T to check monster stats, A S D F G to check party member stats.");
                Thread.Sleep(3000);
                scene.IsActive = true;
            }
            else if (input ==  'i')
            {
                Console.WriteLine($"Inventory: {numPotions.Value} potions");
            }
            else if (input == 'p')
            {
                this.scene.IsActive = !this.scene.IsActive;
                if (this.scene.IsActive)
                {
                    Console.WriteLine("Unpaused. Press p to pause.");
                } else {
                    Console.WriteLine("Paused. Press p to unpause.");
                }
            }
            else if (input == 'o') // use potion
            {
                if (numPotions.Value == 0)
                {
                    Console.WriteLine("You're out of potions.");
                }
                else
                {
                    scene.IsActive = false;
                    Console.WriteLine("Press the number for the party member to use it on.");
                    var alive = this.party.Where(p => p.CurrentHealth > 0);
                    foreach (var i in Enumerable.Range(0, alive.Count()))
                    {
                        var member = alive.ElementAt(i);
                        Console.WriteLine($"{i}: {member.Name}, {member.CurrentHealth} out of {member.TotalHealth} health");
                    }
                    
                    var partyNum = -1;
                    while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out partyNum))
                    {
                        Console.WriteLine($"Try again - enter a number from 0 to {alive.Count() - 1}");
                    }

                    // Also bad: game logic shouldn't live here
                    this.numPotions.Value -= 1;

                    var target = alive.ElementAt(partyNum);
                    var healed = (int)Math.Ceiling(Constants.HEAL_POTION_PERCENT * target.TotalHealth);
                    target.CurrentHealth = Math.Min(target.TotalHealth, target.CurrentHealth + healed);
                    Console.WriteLine($"Healed {healed} HP, {target.Name} now has {target.CurrentHealth} out of {target.TotalHealth} health.");
                    Thread.Sleep(2000);
                    scene.IsActive = true;
                }
            }
            else if (input == 'g')
            {
                this.StateParties(true);
            }
        }

        public void Dispose()
        {
            this.isRunning = false;
            this.replThread.Join();
        }

        internal void PrintStats(Unit unit)
        {
            Console.WriteLine($"{unit.Name} has {unit.CurrentHealth} out of {unit.TotalHealth} health");
        }

        internal void StateParties(bool stateHealth = false)
        {
            this.scene.IsActive = false;

            StringBuilder partyText = new StringBuilder();
            
            partyText.Append("Your party is: ");
            foreach (var member in party)
            {
                var health = "";
                if (stateHealth)
                {
                    health = $" with {member.CurrentHealth} out of {member.TotalHealth} health";
                }
                partyText.Append($"A level {member.Level} {member.Specialization} {health}, ");
            }
            
            partyText.Append($"They are facing: {this.monsters.Count} slimes.");
            if (stateHealth)
            {
                partyText.Append(" Their health is ");
                foreach (var slime in monsters)
                {
                    partyText.Append($"{slime.Name}: {slime.CurrentHealth} out of {slime.TotalHealth}, ");
                }
            }
            partyText.Append('.');
            Console.WriteLine(partyText.ToString());

            Thread.Sleep(5000);
            this.scene.IsActive = true;
        }

        internal void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}