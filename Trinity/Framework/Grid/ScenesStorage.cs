using System;
using System.Collections.Generic;
using System.Linq;
using Trinity.Technicals;
using Zeta.Game;
using Zeta.Game.Internals;

namespace Trinity.Framework.Grid
{
    public static class ScenesStorage
    {
        private static List<WorldScene> _currentWorldScenes;
        private static HashSet<string> _currentWorldSceneIds;

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
                        s => s.DynamicWorldId == worldId && ZetaDia.Me.Position.X >= s.Min.X && ZetaDia.Me.Position.Y >= s.Min.Y && ZetaDia.Me.Position.X <= s.Max.X && ZetaDia.Me.Position.Y <= s.Max.Y);
            }
        }

        public static void Update()
        {

            var addedScenes = new List<WorldScene>();
            List<Scene> newScenes;
            try
            {
                newScenes =
                    ZetaDia.Scenes.Where(
                        s => s.IsAlmostValid() && !CurrentWorldSceneIds.Contains(s.GetSceneNameString())).ToList();
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
                    if (scene.IsAlmostValid() && scene.Mesh.ParentSceneId <= 0 &&
                        scene.Mesh.DynamicWorldId == ZetaDia.CurrentWorldDynamicId &&
                        !scene.Name.ToLowerInvariant().Contains("fill"))
                    {
                        var adventurerScene = new WorldScene(scene);
                        if (adventurerScene.Cells.Count > 0)
                        {
                            CurrentWorldScenes.Add(adventurerScene);
                            addedScenes.Add(adventurerScene);
                        }
                    }
                    CurrentWorldSceneIds.Add(scene.GetSceneNameString());
                }
                catch (NullReferenceException)
                {

                }
            }
            if (addedScenes.Count > 0)
            {
                //Logger.Debug("[ScenesStorage] Found {0} new scenes", addedScenes.Count);
                var nodes = addedScenes.SelectMany(s => s.Nodes).ToList();
                TrinityGrid.Instance.Update(nodes);
            }

        }


        public static void Reset()
        {
            //Logger.Debug("[ScenesStorage] Reseting");
            CurrentWorldSceneIds.Clear();
            CurrentWorldScenes.Clear();
            TrinityGrid.ResetAll();
            Update();
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
