using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PathToGoal
{
    internal GameTiles spawnTile;
    internal List<GameTiles> pathToGoal = new List<GameTiles>();
    internal List<GameTiles> tempPathToGoal = new List<GameTiles>();
    //internal int leght;
}



public class PathFinder : MonoBehaviour
{
    GameTiles[,] gameTiles;
    internal List<GameTiles> spawnTile;
    internal GameTiles endTile;
    static internal List<PathToGoal> pathToGoal = new List<PathToGoal>();
    PathToGoal currentPath = new PathToGoal();

    int ColCount = 0;
    int RowCount = 0;

    ////constructeur
    //public PathFinder(GameTiles[,] NewGametiles, List<GameTiles> NewSpawnTile, GameTiles NewEndTil, int NewCol, int NewRow)
    //{
    //    gameTiles = NewGametiles;
    //    spawnTile = NewSpawnTile;
    //    endTile = NewEndTil;
    //    ColCount = NewCol;
    //    RowCount = NewRow;

    //}
    internal void SetValue(GameTiles[,] NewGametiles, List<GameTiles> NewSpawnTile, GameTiles NewEndTil, int NewCol, int NewRow)
    {
        gameTiles = NewGametiles;
        endTile = NewEndTil;
        ColCount = NewCol;
        RowCount = NewRow;

        foreach (var spawn in NewSpawnTile)
        {
            PathToGoal tempPath = new PathToGoal();
            tempPath.spawnTile = spawn;
            pathToGoal.Add(tempPath);
        }
    }


    internal void SetPath()
    {
        foreach (var t in gameTiles)
        {
            t.SetPathColor(false);
        }


        foreach (var spawn in pathToGoal)
        {
            spawn.pathToGoal.Clear();
            var path = PathFinding(spawn.spawnTile, endTile);
            var tile = endTile;

            Debug.Log($"base path = {path}");

            while (tile != null)
            {
                spawn.pathToGoal.Add(tile);
                tile.SetPathColor(true);
                tile = path[tile];
            }

            Debug.Log($"Path Created and count : {spawn.pathToGoal.Count}");
        }
    }

    internal List<GameTiles> EnemySetPath(GameTiles pos, GameTiles end)
    {
        foreach (var t in gameTiles)
        {
            t.SetPathColor(false);
        }

        List<GameTiles> newPath = new List<GameTiles>();
        var path = PathFinding(pos, end);
        var tile = end;

        Debug.Log($"base path = {path}");

        while (tile != null)
        {
            newPath.Add(tile);
            tile.SetPathColor(true);
            tile = path[tile];
        }

        //Debug.Log($"Path Created and count : {spawn.pathToGoal.Count}");

        return newPath;
    }


    internal List<int> SetTempPath()
    {
        List<int> leght = new List<int>();

        foreach (var spawn in pathToGoal)
        {
            spawn.tempPathToGoal.Clear();
            var path = PathFinding(spawn.spawnTile, endTile);
            var tile = endTile;

            if (path != null)
            {
                while (tile != null)
                {
                    spawn.tempPathToGoal.Add(tile);
                    //tile.SetPathColor(true);

                    //Debug.Log(tile);
                    //Debug.Log(path.Count);

                    tile = path[tile];
                }


                leght.Add(spawn.tempPathToGoal.Count);
            }
            else
            {
                leght.Add(0);
            }

            Debug.Log("Temp Path Created");
        }

        return leght;
    }
    private Dictionary<GameTiles, GameTiles> PathFinding(GameTiles sourceTile, GameTiles targetTile)
    {
        Dictionary<GameTiles, GameTiles> path = PathFindingAlgo(sourceTile, targetTile, avoidDamage: true, avoidSlow: true);

        // Si impossible de trouver un chemin �vitant les tuiles dangereuses, essayer d'�viter les tuiles lentes
        if (path == null)
        {
            path = PathFindingAlgo(sourceTile, targetTile, avoidDamage: true, avoidSlow: false);
        }

        // Si toujours impossible, trouver le chemin le plus court en ignorant les conditions
        if (path == null)
        {
            path = PathFindingAlgo(sourceTile, targetTile, avoidDamage: false, avoidSlow: false);
        }

        return path;
    }
    private Dictionary<GameTiles, GameTiles> PathFindingAlgo(GameTiles sourceTile, GameTiles targetTile, bool avoidDamage, bool avoidSlow)
    {
        var dist = new Dictionary<GameTiles, int>();
        var prev = new Dictionary<GameTiles, GameTiles>();
        var Q = new List<GameTiles>();

        foreach (var v in gameTiles)
        {
            dist[v] = int.MaxValue;
            prev[v] = null;
            Q.Add(v);
        }

        dist[sourceTile] = 0;

        while (Q.Count > 0)
        {
            GameTiles u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if (dist[v] < minDistance)
                {
                    minDistance = dist[v];
                    u = v;
                }
            }

            if (u == null || u == targetTile)
            {
                break;
            }

            Q.Remove(u);

            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBloced || (avoidDamage && v.IsDamaging) || (avoidSlow && v.IsSlowing))
                {
                    continue;
                }

                int alt = dist[u] + 1; // Vous pouvez ajuster cette valeur pour les tuiles lentes ou dangereuses

                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[targetTile] == null)
        {
            return null; // Aucun chemin trouv�
        }

        return prev;
    }

    private List<GameTiles> FindNeighbor(GameTiles u)
    {
        var result = new List<GameTiles>();

        //if (u.X - 1 >= 0)
        //{ result.Add(gameTiles[u.X - 1, u.Y]); }
        //if (u.X + 1 < ColCount)
        //{ result.Add(gameTiles[u.X + 1, u.Y]); }
        //if (u.Y - 1 >= 0)
        //{ result.Add(gameTiles[u.X, u.Y - 1]); }
        //if (u.Y + 1 < RowCount)
        //{ result.Add(gameTiles[u.X, u.Y + 1]); }

        if (u.X - 1 >= 0)
        { result.Add(gameTiles[u.Y, u.X - 1]); }
        if (u.X + 1 < ColCount)
        { result.Add(gameTiles[u.Y, u.X + 1]); }
        if (u.Y - 1 >= 0)
        { result.Add(gameTiles[u.Y - 1, u.X]); }
        if (u.Y + 1 < RowCount)
        { result.Add(gameTiles[u.Y + 1, u.X]); }

        return result;
    }

    internal void Reset()
    {
        spawnTile.Clear();
    }
}
