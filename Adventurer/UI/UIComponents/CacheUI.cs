﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Adventurer.Game.Events;
using Adventurer.Game.Exploration;
using Adventurer.Util;
using Zeta.Bot;
using Zeta.Game;

namespace Adventurer.UI.UIComponents
{
    public class CacheUI
    {

        private static bool _isUpdating;

        public static CacheUIDataModel DataModel
        {
            get { return _dataModel ?? (_dataModel = CreateDataModel()); }
            set { _dataModel = value; }
        }

        private static DateTime _lastUpdatedDefault = DateTime.MinValue;
        private static DateTime _lastUpdatedLazy = DateTime.MinValue;
        private static DispatcherTimer _internalTimer;
        private static MapWindow _radarWindow;
        private static CacheUIDataModel _dataModel;

        public static void ToggleRadarWindow()
        {
            if (_radarWindow == null)
            {
                Initialize();
                CreateRadarWindow();
                DataModel.IsRadarWindowVisible = true;
            }
            else
            {
                if (_radarWindow.IsVisible)
                {
                    DataModel.IsRadarWindowVisible = false;
                }
                else
                {
                    DataModel.IsRadarWindowVisible = true;
                }
            }

        }

        public static CacheUIDataModel CreateDataModel()
        {
            var dataModel = new CacheUIDataModel();
            dataModel.PropertyChanged += DataModelOnPropertyChanged;
            Events.OnCacheUpdated += Update;

            return dataModel;
        }


        public static void Initialize()
        {
            _internalTimer = new DispatcherTimer();
            _internalTimer.Tick += InternalTimerTick;
            _internalTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            _internalTimer.Start();
        }


        private static void InternalTimerTick(object sender, EventArgs e)
        {
            try
            {
                if (_radarWindow == null)
                    return;

                if (!DataModel.Enabled && DataModel.IsLazyCacheVisible)
                    return;
                //Update();

                if (BotEvents.IsBotRunning)
                {
                    Update();
                }
                else
                {
                    //AdvDia.Update();
                    SafeFrameLock.ExecuteWithinFrameLock(() =>
                    {
                        ScenesStorage.Update();
                        Update();
                    }, true);
                }
                var userControl = _radarWindow.Content as UserControl;
                if (userControl == null) return;

                var canvas = userControl.Content as RadarCanvas.RadarCanvas;
                if (canvas != null) canvas.UpdateData();
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("ReadProcessMemory"))
                {
                    Logger.Debug("Exception in CacheUI.InternalTimerTick(). {0} {1} {2}", ex.Message, ex.InnerException,
                        ex);
                }
            }
        }

        private static void CreateRadarWindow()
        {
            if (_radarWindow == null)
            {
                Logger.Debug("Loading MapUI");

                _radarWindow = new MapWindow();

                DataModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsRadarWindowVisible")
                    {
                        if (DataModel.IsRadarWindowVisible)
                        {
                            _radarWindow.Show();
                        }
                        else
                        {
                            _radarWindow.Hide();
                        }
                    }
                };

                _radarWindow.Closed += (a, b) =>
                {
                    DataModel.IsRadarWindowVisible = false;
                    _radarWindow = null;
                };

            }
        }

        private static void DataModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var dataModel = sender as CacheUIDataModel;
            if (dataModel == null)
                return;

            if (propertyChangedEventArgs.PropertyName == "Enabled")
            {
                //if (DataModel.Enabled)
                //    DataModel.LazyCache.ForEach(CacheUtilities.UnFreeze);
                //else
                //    DataModel.LazyCache.ForEach(CacheUtilities.Freeze);
            }
        }

        private static void Update()
        {
            try
            {
                using (new PerformanceLogger("CacheUI Update"))
                {
                    if (_isUpdating)
                        return;

                    if ((DataModel.IsLazyCacheVisible || DataModel.IsRadarWindowVisible) && DateTime.UtcNow.Subtract(_lastUpdatedLazy).TotalMilliseconds > 50)
                    {
                        _isUpdating = true;
                        DataModel.LazyCache = new ObservableCollection<int> { (int)(DateTime.UtcNow - DateTime.Today).TotalSeconds };
                        _lastUpdatedLazy = DateTime.UtcNow;
                    }

                    _isUpdating = false;
                }
            }
            catch (Exception ex)
            {
                _isUpdating = false;
                Logger.Debug("Error in CacheUI Worker: " + ex);
            }
        }



    }

}
