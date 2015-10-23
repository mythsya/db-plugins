using System.Collections.Generic;
using System.Linq;
using Zeta.Common;
using Zeta.Game.Internals;
using Zeta.Game.Internals.SNO;

namespace Trinity.Framework.Grid
{
    public class WorldScene
    {


        public Scene Scene { get; private set; }
        public Vector2 Center { get; private set; }
        public List<TrinityNode> Nodes = new List<TrinityNode>();
        public string Name { get; private set; }
        public string HashName { get; private set; }
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        //public Rect Rect { get; private set; }
        public int LevelAreaId { get; set; }
        public bool HasParent { get; set; }
        public bool HasChild { get; set; }
        public bool IsTopLevel { get { return !HasParent; } }
        public bool GridCreated { get; private set; }
        public int DynamicWorldId { get; private set; }
        public int SceneId { get; private set; }

        public List<WorldSceneCell> Cells { get; private set; }

        public WorldScene SubScene { get; set; }

        public WorldScene(Scene scene)
        {
            //using (new PerformanceLogger("[WorldScene] ctor", false))
            //{
            //                Logger.Debug("[WorldScene] Scene GridSquare Size: {0} X:{1} Y:{2}", scene.Mesh.Zone.NavZoneDef.GridSquareSize,scene.Mesh.Zone.NavZoneDef.NavGridSquareCountX, scene.Mesh.Zone.NavZoneDef.NavGridSquareCountY);

            Scene = scene;
            Name = scene.Name;
            HashName = scene.GetSceneNameString();
            LevelAreaId = Scene.Mesh.LevelAreaSNO;
            Min = Scene.Mesh.Zone.ZoneMin;
            Max = Scene.Mesh.Zone.ZoneMax;
            Center = (Max + Min) / 2;
            //Rect = new Rect(new Point(Center.X, Center.Y), new Size(_boxSize, _boxSize));
            HasChild = Scene.Mesh.SubSceneId > 0;
            HasParent = Scene.Mesh.ParentSceneId > 0;
            DynamicWorldId = Scene.Mesh.DynamicWorldId;
            SceneId = scene.Mesh.SceneId;
            if (HasChild)
            {
                SubScene = new WorldScene(Scene.Mesh.SubScene);
                if (SubScene.HasChild)
                {
                    //Logger.Error("[ScenesStorage] Found sub sub scene!!!");
                    SubScene.SubScene = new WorldScene(SubScene.Scene.Mesh.SubScene);
                }
            }
            //Logger.Verbose("[WorldScene] Created a new world scene. Name: {0} LevelArea: {1} ({2})", Name, (SNOLevelArea)LevelAreaId, LevelAreaId);
            //if (LevelAreaId != AdvDia.CurrentLevelAreaId && !ExplorationData.OpenWorldIds.Contains(AdvDia.CurrentWorldId))
            //{
            //    Logger.Verbose("[WorldScene] The scene LevelAreaID is different than the CurrentLevelAreaID");
            //    Logger.Verbose("[WorldScene] Scene Name: {0}", Name);
            //    Logger.Verbose("[WorldScene] Scene: {0} ({1})", (SNOLevelArea)LevelAreaId, LevelAreaId);
            //    Logger.Verbose("[WorldScene] Current: {0} ({1})", (SNOLevelArea)AdvDia.CurrentLevelAreaId, AdvDia.CurrentLevelAreaId);
            //}
            CreateGrid();
            //}
        }

        public Vector3 GetRelativePosition(Vector3 worldPosition)
        {
            var v2Diff = worldPosition.ToVector2() - Min;
            return new Vector3(v2Diff.X, v2Diff.Y, worldPosition.Z);
        }

        public Vector3 GetWorldPosition(Vector3 relativePosition)
        {
            var v2Diff = relativePosition.ToVector2() + Min;
            return new Vector3(v2Diff.X, v2Diff.Y, relativePosition.Z);
        }

        public bool IsInScene(Vector3 position)
        {
            return position.X >= Min.X && position.X <= Max.X && position.Y >= Min.Y && position.Y <= Max.Y;
        }

        private void CreateGrid()
        {
            if (GridCreated) return;

            Cells = new List<WorldSceneCell>();

            foreach (var navCell in Scene.Mesh.Zone.NavZoneDef.NavCells)
            {
                Cells.Add(new WorldSceneCell(navCell, Min));
            }
            if (SubScene != null)
            {
                foreach (var navCell in SubScene.Scene.Mesh.Zone.NavZoneDef.NavCells)
                {
                    Cells.Add(new WorldSceneCell(navCell, SubScene.Min));
                }
                if (SubScene.SubScene != null)
                {
                    foreach (var navCell in SubScene.SubScene.Scene.Mesh.Zone.NavZoneDef.NavCells)
                    {
                        Cells.Add(new WorldSceneCell(navCell, SubScene.SubScene.Min));
                    }
                }
            }

            var navBoxSize = TrinityGrid.NodeBoxSize;
            var searchBeginning = navBoxSize / 2;

            for (var x = Min.X + searchBeginning; x <= Max.X; x = x + navBoxSize)
            {
                for (var y = Min.Y + searchBeginning; y <= Max.Y; y = y + navBoxSize)
                {
                    var cell = this.Cells.FirstOrDefault(c => c.IsInCell(x, y));
                    if (cell != null)
                    {
                        var navNode = new TrinityNode(new Vector3(x, y, cell.Z), navBoxSize, cell);
                        Nodes.Add(navNode);
                    }
                }
            }

            GridCreated = true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}