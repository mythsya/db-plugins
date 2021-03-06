﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adventurer.Util;
using Zeta.Bot.Navigation;
using Zeta.Game;
using Zeta.Game.Internals;

namespace Adventurer.Game.Exploration
{
    public static class ScenesStorage
    {
        private static List<WorldScene> _currentWorldScenes;
        private static HashSet<string> _currentWorldSceneIds;
        //private static readonly ConcurrentDictionary<int, List<AdventurerScene>> Scenes = new ConcurrentDictionary<int, List<AdventurerScene>>();
        //private static readonly ConcurrentDictionary<int, HashSet<string>> SceneIds = new ConcurrentDictionary<int, HashSet<string>>();

        //public static List<AdventurerScene> CurrentWorldScenes
        //{
        //    get
        //    {
        //        return Scenes.GetOrAdd(AdvDia.CurrentWorldDynamicId, c => new List<AdventurerScene>());
        //    }
        //}

        //public static HashSet<string> CurrentWorldSceneIds
        //{
        //    get
        //    {
        //        return SceneIds.GetOrAdd(AdvDia.CurrentWorldDynamicId, c => new HashSet<string>());
        //    }
        //}

        public static List<WorldScene> CurrentWorldScenes
        {
            get { return _currentWorldScenes ?? (_currentWorldScenes = new List<WorldScene>()); }
        }
        public static HashSet<string> CurrentWorldSceneIds
        {
            get { return _currentWorldSceneIds ?? (_currentWorldSceneIds = new HashSet<string>()); }
        }

        public static WorldScene CurrentScene
        {
            get
            {
                var worldId = ZetaDia.CurrentWorldDynamicId;
                return
                    CurrentWorldScenes.FirstOrDefault(
                        s => s.DynamicWorldId == worldId && AdvDia.MyPosition.X >= s.Min.X && AdvDia.MyPosition.Y >= s.Min.Y && AdvDia.MyPosition.X <= s.Max.X && AdvDia.MyPosition.Y <= s.Max.Y);
            }
        }

        public static void Update()
        {

            using (new PerformanceLogger("[ScenesStorage] Update Scenes", true))
            {
                var addedScenes = new List<WorldScene>();
                List<Scene> newScenes;
                try
                {
                    newScenes = ZetaDia.Scenes.Where(s => s.IsAlmostValid() && !CurrentWorldSceneIds.Contains(s.GetSceneNameString())).ToList();
                }
                catch (NullReferenceException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ReadProcessMemory"))
                    {
                        return;
                    }
                    throw;
                }
                foreach (var scene in newScenes)
                {
                    try
                    {
                        if (!scene.IsAlmostValid()) continue;
                        var sceneHashName = scene.GetSceneNameString();
                        if (scene.IsAlmostValid() && scene.Mesh.ParentSceneId <= 0 &&
                            scene.Mesh.DynamicWorldId == AdvDia.CurrentWorldDynamicId &&
                            !scene.Name.ToLowerInvariant().Contains("fill"))
                        {
                            var adventurerScene = new WorldScene(scene, ExplorationData.ExplorationNodeBoxSize,
                                ExplorationData.ExplorationNodeBoxTolerance);
                            if (adventurerScene.Cells.Count > 0)
                            {
                                CurrentWorldScenes.Add(adventurerScene);
                                addedScenes.Add(adventurerScene);
                            }
                        }
                        CurrentWorldSceneIds.Add(sceneHashName);
                    }
                    catch (NullReferenceException)
                    {

                    }
                }
                if (addedScenes.Count > 0)
                {
                    Logger.Debug("[ScenesStorage] Found {0} new scenes", addedScenes.Count);
                    var nodes = addedScenes.SelectMany(s => s.Nodes).ToList();
                    //foreach (var explorationNode in nodes)
                    //{
                    //    if (ZetaDia.Minimap.IsExplored(explorationNode.NavigableCenter, explorationNode.DynamicWorldId))
                    //    {
                    //        explorationNode.IsVisited = true;
                    //    }
                    //}
                    ExplorationGrid.Instance.Update(nodes.Cast<INode>().ToList());
                    NavigationGrid.Instance.Update(nodes.SelectMany(n => n.Nodes).Cast<INode>().ToList());
                }
            }
        }



        public static IEnumerable<ExplorationNode> SceneNodes(Scene scene)
        {
            if (scene.IsAlmostValid())
            {
                var sceneNameHash = scene.GetSceneNameString();
                if (CurrentWorldSceneIds.Contains(sceneNameHash))
                {
                    var adventurerScene = CurrentWorldScenes.FirstOrDefault(s => s.HashName == sceneNameHash);
                    if (adventurerScene != null)
                    {
                        return adventurerScene.Nodes;
                    }
                }
            }
            return new List<ExplorationNode>();
        }

        public static void Reset()
        {
            Logger.Debug("[ScenesStorage] Reseting");
            CurrentWorldSceneIds.Clear();
            CurrentWorldScenes.Clear();
            ExplorationGrid.ResetAll();
            NavigationGrid.ResetAll();
            Update();
        }

        public static void ResetVisited()
        {
            foreach (var node in CurrentWorldScenes.SelectMany(s => s.Nodes))
            {
                node.IsVisited = false;
            }
        }

        public static bool IsAlmostValid(this Scene scene)
        {
            return scene != null && scene.Mesh != null && scene.Mesh.Zone != null;
        }

        public static bool IsFullyValid(this Scene scene)
        {
            return scene != null && scene.IsValid && scene.Mesh != null && scene.Mesh.Zone != null &&
                   scene.Mesh.Zone.IsValid && scene.Mesh.Zone.NavZoneDef != null && scene.Mesh.Zone.NavZoneDef.IsValid;
        }

        public static string GetSceneNameString(this Scene scene)
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", scene.Name, scene.Mesh.SceneSNO, scene.Mesh.LevelAreaSNO, scene.Mesh.Zone.ZoneMin, scene.Mesh.Zone.ZoneMax);
        }

        public static bool HasParent(this Scene scene)
        {
            return scene.Mesh.ParentSceneId > 0;
        }
    }
}
