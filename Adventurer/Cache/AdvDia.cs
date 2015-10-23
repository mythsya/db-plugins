﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adventurer.Game.Exploration;
using Adventurer.Game.Rift;
using Adventurer.Util;
using GreyMagic;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Logger = Adventurer.Util.Logger;

namespace Adventurer
{
    public static class AdvDia
    {
        //public static uint LastUpdatedFrame { get; private set; }


        //private static Lazy<RiftQuest> _riftQuest = new Lazy<RiftQuest>(() => new RiftQuest());

        //private static PropertyReader<int> _currentWorldId;
        //private static PropertyReader<int> _currentWorldDynamicId;
        //private static PropertyReader<int> _currentLevelAreaId;
        //private static PropertyReader<Vector3> _myPosition;
        //private static PropertyReader<Scene> _currentScene;
        //private static PropertyReader<bool> _isInTown;
        //private static PropertyReader<int> _battleNetHeroId;
        //private static PropertyReader<List<MapMarker>> _currentWorldMarkers;

        public static int CurrentWorldId
        {
            get { return PropertyReader<int>.SafeReadValue(() => ZetaDia.CurrentWorldId); }

        }

        public static int CurrentWorldDynamicId
        {
            get { return PropertyReader<int>.SafeReadValue(() => ZetaDia.CurrentWorldDynamicId); }

        }

        public static int CurrentLevelAreaId
        {
            get { return PropertyReader<int>.SafeReadValue(() => ZetaDia.CurrentLevelAreaId); }

        }

        public static Vector3 MyPosition
        {
            get { return PropertyReader<Vector3>.SafeReadValue(() => ZetaDia.Me.Position); }

        }

        public static WorldScene CurrentWorldScene
        {
            get { return ScenesStorage.CurrentScene; }

        }

        public static bool IsInTown
        {
            get { return PropertyReader<bool>.SafeReadValue(() => ZetaDia.IsInTown); }

        }

        public static int BattleNetHeroId
        {
            get { return PropertyReader<int>.SafeReadValue(() => ZetaDia.Memory.Read<int>(ZetaDia.Service.Hero.BaseAddress + 200)); }

        }

        public static List<MinimapMarker> CurrentWorldMarkers
        {
            get
            {
                return PropertyReader<List<MinimapMarker>>.SafeReadValue(() => ZetaDia.Minimap.Markers.CurrentWorldMarkers.Where(m => m.IsValid && m.NameHash != -1).ToList());
            }

        }

        public static RiftQuest RiftQuest { get { return PropertyReader<RiftQuest>.SafeReadValue(() => new RiftQuest()); } }

        public static IEnumerable<ACDItem> StashAndBackpackItems
        {
            get { return ZetaDia.Me.Inventory.Backpack.Union(ZetaDia.Me.Inventory.StashItems); }
        }

        //static AdvDia()
        //{
        //}

        //public static void Update()
        //{
        //    if (ZetaDia.IsInGame)
        //    {
        //        UpdateValues();
        //        ScenesStorage.Update();
        //        //Providers.GridProvider = GridProvider.Instance;
        //    }

        //}

        //private static void UpdateValues()
        //{
        //    var result = SafeFrameLock.ExecuteWithinFrameLock(() =>
        //     {
        //         _currentWorldId = new PropertyReader<int>(() => ZetaDia.CurrentWorldId, (v) => v == 0 || v == -1);
        //         _currentWorldDynamicId = new PropertyReader<int>(() => ZetaDia.CurrentWorldDynamicId, (v) => v == 0 || v == -1);
        //         _currentLevelAreaId = new PropertyReader<int>(() => ZetaDia.CurrentLevelAreaId, (v) => v == 0 || v == -1);
        //         _myPosition = new PropertyReader<Vector3>(() => ZetaDia.Me.Position, (v) => v == Vector3.Zero);
        //         _currentScene = new PropertyReader<Scene>(() => ZetaDia.Me.CurrentScene, (v) => v == null);
        //         _isInTown = new PropertyReader<bool>(() => ZetaDia.IsInTown);
        //         _currentWorldMarkers = new PropertyReader<List<MapMarker>>(() => ZetaDia.Minimap.Markers.CurrentWorldMarkers.Where(m => m.IsValid && m.NameHash != -1)
        //                         .Select(m => new MapMarker(m))
        //                         .ToList(), (v) => v == null || v.Count == 0);
        //         _battleNetHeroId = new PropertyReader<int>(() => ZetaDia.Memory.Read<int>(ZetaDia.Service.Hero.BaseAddress + 200), (v) => v == 0 || v == -1);

        //     }, true);
        //    if (!result.Success)
        //    {
        //        Logger.Error("[Cache][Update] " + result.Exception.Message);
        //    }

        //}
    }


    public class PropertyReader<T>
    {
        private readonly Func<T> _valueFactory;
        private readonly Func<T, bool> _failureCondition;
        private T _value;
        public PropertyReader(Func<T> valueFactory, Func<T, bool> failureCondition = null, bool lazyEvaluate = false)
        {
            _valueFactory = valueFactory;
            _failureCondition = failureCondition;
            if (!lazyEvaluate) _value = SafeReadValue(_valueFactory);
        }

        public T Value
        {
            get
            {
                if (_failureCondition != null && _failureCondition(_value))
                {
                    _value = SafeReadValue(_valueFactory.Invoke);
                }
                return _value;
            }
        }

        public static T SafeReadValue(Func<T> valueFactory)
        {
            try
            {
                return valueFactory.Invoke();
            }
            catch (ACDAttributeLookupFailedException acdEx)
            {
                Logger.Debug("[AdvDia] {0}", acdEx.Message);
            }
            catch (Exception ex)
            {
                Logger.Debug("[AdvDia] {0}", ex.Message);
                if (!ex.Message.Contains("ReadProcessMemory"))
                    throw;
            }
            return default(T);
        }

    }

}
