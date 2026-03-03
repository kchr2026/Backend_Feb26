using System.Security.Cryptography;
using Spectre.Console;

namespace Basic_Week_One;

class Program
{
    static void Main(string[] args)
    {
        // var calc = new Calculator();

        // calc.RunCalculator();

        // var celcius = TemperatureConverter.ConvertToCelsius(20);
        // Console.WriteLine(celcius);
        // var fahren = TemperatureConverter.ConvertToFahrenheit(celcius);
        // Console.WriteLine(fahren);

        // var warrior = new CharacterClass(CharacterBuild.Warrior, 0.5, "Sword");
        // var player = new Player(name: "Rambo", characterClass: warrior);


        // var npc = new NPC
        // {
        //     TypeOfFoe = "Goblin",
        //     XPWhenDefeated = 30,
        //     HP = 100,
        //     BaseDamage = 7
        // };

        // npc.DropTable.Add(new LootEntry("Coins", weigth: 60, minimumAmount: 3, maximumAmount: 30));
        // npc.DropTable.Add(new LootEntry("Health Restoration Potion", weigth: 25, minimumAmount: 1, maximumAmount: 2));
        // npc.DropTable.Add(new LootEntry("Rusty Dagger", weigth: 10));

        // var battle = new BattleEngine();
        // battle.Fight(player, npc);

        var state = new GameState();

        // Create a new player
        AnsiConsole.Clear();

        var player = MainMenu.StartMenu();

        var goblin = NPCHelper.CreateNPCFromTempate(NPCHelper.CreateGoblinNPC());



        // Main game loop
        while (state.Screen != GameScreen.Exit)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold]{player.Player}[/] | {player.Player.CharacterClass} | HP : {player.Player.HP}");

            var choices = UI.MainActionPrompt(player.Player);

            switch (choices)
            {
                case "Walk":
                    NPCHelper.HandleEncounters(player);
                    break;
                case "Attack":
                    player.CurrentEnemy = goblin;
                    BattleEngine.RunBattle(player);
                    break;
                case "View inventory":
                    UI.ViewInventory(player.Player);
                    break;
                case "Party":
                    UI.ViewPartyMembers(player.Player);
                    break;
                case "Recruit":
                    NPCHelper.HandlePartyMemberRecruitment(player);
                    break;
                case "Save Game":
                    SaveGameService.Save(SaveGameService.DefaultSaveFileName(), SaveGameMapper.MapSave(player));
                    break;
                case "Exit":
                    state.Screen = GameScreen.Exit;
                    break;
            }
        }
        AnsiConsole.MarkupLine("Exiting game...");
    }
}