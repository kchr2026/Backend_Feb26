using Spectre.Console;

public sealed class BattleResults
{
    public bool PlayerWon { get; init; }
    public bool PlayerEscaped { get; init; }
}
public static class BattleEngineHandler
{
    /// <summary>
    /// Get the results of a given battle
    /// </summary>
    /// <param name="player">player</param>
    /// <param name="enemies">current enemy that is being battled</param>
    /// <param name="rng">Random Number Generator</param>
    /// <returns>BattleResults</returns>
    public static BattleResults GetBattleResult(Player player, List<NPC> enemies, Random rng)
    {
        // Clear the console before outputting back the results of a battle
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[red]Battle[/]"));

        while (player.IsAlive && enemies.Any(e => e.IsAlive))
        {
            // we reset at the start of each round (this applies to all entities as well as the player)
            player.IsCurrentlyDefending = false;

            foreach (var partyMember in player.PartyMembers)
            {
                partyMember.IsCurrentlyDefending = false;
            }

            foreach (var enemy in enemies)
            {
                enemy.IsCurrentlyDefending = false;
            }

            DrawStatus(player, enemies);

            var allEntities = new List<IBattleEngine> { player };
            allEntities.AddRange(player.PartyMembers);
            allEntities.AddRange(enemies);

            var turnOrder = TurnOrderHelper.BuildCombatOrder(rng, allEntities);

            foreach (var actor in turnOrder)
            {
                if (!player.IsAlive)
                {
                    break;
                }

                if (!enemies.Any(enemy => enemy.IsAlive))
                {
                    break;
                }

                if (!actor.IsAlive)
                {
                    continue;
                }

                if (ReferenceEquals(actor, player))
                {
                    var battleResult = PlayerCombatTurn(player, enemies, rng);

                    if (battleResult == PlayerActions.Run)
                    {
                        return new BattleResults { PlayerWon = false, PlayerEscaped = true };
                    }
                }
                else if (actor is Party member)
                {
                    PartyAI.TakeTurn(rng, member, player, enemies);
                }
                else if (actor is NPC npc)
                {
                    EnemyTurn(rng, npc, player);
                }
            }
        }
        // TODO: Finish this method.
        return new BattleResults();
    }

    private static void DrawStatus(Player player, List<NPC> enemies) { }

    private static PlayerActions PlayerCombatTurn(Player player, List<NPC> enemies, Random rng)
    {
        var action = CombatUI.GetPlayerAction(player);

        switch (action)
        {
            case PlayerActions.Attack:
                var target = (NPC)CombatUI.GetTarget("[bold]Choose a target[/]", enemies);
                PerformAttack(rng, player, target);
                break;
        }

        return PlayerActions.Run;
    }

    private static void EnemyTurn(Random rng, NPC npc, Player player)
    {

    }

    private static void PerformAttack(Random rng, Player player, NPC enemy)
    {

    }
}