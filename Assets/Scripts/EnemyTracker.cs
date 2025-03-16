using System.Collections.Generic;

public static class EnemyTracker
{
    private static HashSet<string> defeatedEnemies = new HashSet<string>();

    public static void MarkEnemyAsDefeated(string enemyId)
    {
        defeatedEnemies.Add(enemyId);
    }

    public static bool IsEnemyDefeated(string enemyId)
    {
        return defeatedEnemies.Contains(enemyId);
    }

    public static void ClearAll()
    {
        defeatedEnemies.Clear();
    }
}