﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using Adventurer.Game.Actors;
using Adventurer.Game.Events;
using Adventurer.Game.Exploration;
using Adventurer.Game.Quests;
using Adventurer.UI.UIComponents;
using Adventurer.Util;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.Actors.Gizmos;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Logger = Adventurer.Util.Logger;
using TabControl = System.Windows.Controls.TabControl;

namespace Adventurer.UI
{
    class DeveloperUI
    {
        private static TabItem _tabItem;

        internal static void RemoveTab()
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    Window mainWindow = Application.Current.MainWindow;
                    var tabs = mainWindow.FindName("tabControlMain") as TabControl;
                    if (tabs == null)
                        return;
                    tabs.Items.Remove(_tabItem);

                }
            );
        }

        internal static void InstallTab()
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {

                    var mainWindow = Application.Current.MainWindow;

                    var dumpers = new StackPanel { Background = Brushes.DimGray, Height = 176, Margin = new Thickness(2, 2, 0, 2) };
                    dumpers.Children.Add(CreateTitle("Dumpers"));
                    dumpers.Children.Add(CreateButton("Map Markers", DumpMapMarkers_Click));
                    dumpers.Children.Add(CreateButton("All Actors", DumpObjects_Click));
                    dumpers.Children.Add(CreateButton("Specific Actor", DumpActor_Click));
                    dumpers.Children.Add(CreateButton("Unsupported Bounties", DumpUnsupportedBounties_Click));
                    dumpers.Children.Add(CreateButton("Scenes", DumpLevelAreaScenes_Click));
                    dumpers.Children.Add(CreateButton("Toggle MapUI", ToggleRadarUI_Click, default(Thickness), new SolidColorBrush(Colors.NavajoWhite) { Opacity = 0.2 }));

                    var coroutineHelpers = new StackPanel { Background = Brushes.DimGray, Height = 176, Margin = new Thickness(2, 2, 0, 2) };
                    coroutineHelpers.Children.Add(CreateTitle("Coroutines"));
                    coroutineHelpers.Children.Add(CreateButton("Move To Position", MoveToPosition_Click));
                    coroutineHelpers.Children.Add(CreateButton("Move To Map Marker", MoveToMapMarker_Click));
                    coroutineHelpers.Children.Add(CreateButton("Move To Actor", MoveToActor_Click));
                    coroutineHelpers.Children.Add(CreateButton("Enter LevelA rea", EnterLevelArea_Click));
                    coroutineHelpers.Children.Add(CreateButton("Clear Level Area", ClearLevelArea_Click));
                    coroutineHelpers.Children.Add(CreateButton("Clear Area For N Seconds", ClearAreaForNSeconds_Click));

                    var coroutineHelpers2 = new StackPanel { Background = Brushes.DimGray, Height = 176, Margin = new Thickness(0, 2, 2, 2) };
                    coroutineHelpers2.Children.Add(CreateTitle(" "));
                    coroutineHelpers2.Children.Add(CreateButton("Wait For N Seconds", WaitForNSeconds_Click, new Thickness(0, 2.5, 5, 2.5)));
                    coroutineHelpers2.Children.Add(CreateButton("Interact With Gizmo", InteractWithGizmo_Click, new Thickness(0, 2.5, 5, 2.5)));
                    coroutineHelpers2.Children.Add(CreateButton("Interact With Unit", InteractWithUnit_Click, new Thickness(0, 2.5, 5, 2.5)));
                    coroutineHelpers2.Children.Add(CreateButton("MoveToScene", MoveToScene_Click, new Thickness(0, 2.5, 5, 2.5)));
                    coroutineHelpers2.Children.Add(CreateButton("MoveToScenePosition", MoveToScenePosition_Click, new Thickness(0, 2.5, 5, 2.5)));

                    var tests = new StackPanel { Background = Brushes.DimGray, Height = 176, Margin = new Thickness(0, 2, 2, 2) };
                    tests.Children.Add(CreateTitle("Tests"));
                    tests.Children.Add(CreateButton("Dump Experience", DumpExperience_Click));
                    tests.Children.Add(CreateButton("Dump Me", DumpMe_Click));
                    tests.Children.Add(CreateButton("Dump Bounty Quests", DumpBountyQuests_Click));
                    tests.Children.Add(CreateButton("Dump Backpack", DumpBackpack_Click));
                    tests.Children.Add(CreateButton("Dump Party Members", DumpParty_Click));


                    //var mapUiContainer = new StackPanel { Background = Brushes.DimGray, Height = 176, Margin = new Thickness(0, 2, 0, 2)};

                    //var mapUiButton = new Button();
                    //mapUiButton.Click += ToggleRadarUI_Click;
                    //mapUiButton.Content = "Toggle\r\nMap UI";
                    //mapUiButton.Margin = new Thickness(5);
                    //mapUiButton.Background = new SolidColorBrush(Colors.NavajoWhite) {Opacity = 0.2};
                    //mapUiButton.Height = 166;
                    //mapUiButton.FontWeight = FontWeights.Bold;
                    //mapUiButton.FontSize = 26;
                    //mapUiButton.Width = 140;
                    //mapUiButton.HorizontalAlignment = HorizontalAlignment.Left;
                    //mapUiButton.VerticalAlignment = VerticalAlignment.Top;
                    //mapUiContainer.Children.Add(mapUiButton);

                    var uniformGrid = new UniformGrid
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        MaxHeight = 180,
                        Columns = 4
                    };

                    //uniformGrid.Children.Add(mapUiContainer);
                    uniformGrid.Children.Add(dumpers);
                    uniformGrid.Children.Add(coroutineHelpers);
                    uniformGrid.Children.Add(coroutineHelpers2);
                    uniformGrid.Children.Add(tests);


                    _tabItem = new TabItem
                    {
                        Header = "Adventurer",
                        ToolTip = "Developer Tools",
                        Content = uniformGrid,
                    };

                    var tabs = mainWindow.FindName("tabControlMain") as TabControl;
                    if (tabs == null)
                        return;

                    tabs.Items.Add(_tabItem);
                }
            );
        }

        #region UI Elements
        static TextBlock CreateTitle(string title)
        {
            return new TextBlock
            {
                Text = title,
                Width = 140,
                Height = 18,
                Padding = new Thickness(0, 2, 0, 0),
                Margin = new Thickness(2.5),
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
        }

        static Button CreateButton(string title, RoutedEventHandler eventFunc, Thickness margin = default(Thickness), Brush backrground = null)
        {
            if (margin == default(Thickness))
            {
                margin = new Thickness(5, 2.5, 5, 2.5);
            }
            var button = new Button
            {
                Width = 140,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = margin,
                Content = title,
                Height = 20
            };
            if (backrground != null)
            {
                button.Background = backrground;
            }
            button.Click += eventFunc;
            return button;
        }
        #endregion

        private static void DumpExperience_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {

                if (!ZetaDia.IsInGame)
                    return;

                using (ZetaDia.Memory.AcquireFrame(true))
                {
                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();

                    Logger.Raw("ExperienceGranted {0}", ZetaDia.Me.ExperienceGranted);
                    Logger.Raw("ExperienceNextHi {0}", ZetaDia.Me.ExperienceNextHi);
                    Logger.Raw("ExperienceNextLevel {0}", ZetaDia.Me.ExperienceNextLevel);
                    Logger.Raw("ExperienceNextLo {0}", ZetaDia.Me.ExperienceNextLo);
                    Logger.Raw("CurrentExperience {0}", ZetaDia.Me.CurrentExperience);
                    Logger.Raw("ParagonCurrentExperience {0}", (long)ZetaDia.Me.ParagonCurrentExperience);
                    Logger.Raw("ParagonExperienceNextLevel {0}", ZetaDia.Me.ParagonExperienceNextLevel);
                    Logger.Raw("RestExperience {0}", ZetaDia.Me.RestExperience);
                    Logger.Raw("RestExperienceBonusPercent {0}", ZetaDia.Me.RestExperienceBonusPercent);
                    Logger.Raw("AltExperienceNextHi {0}", ZetaDia.Me.CommonData.GetAttribute<int>(ActorAttributeType.AltExperienceNextHi));
                    Logger.Raw("AltExperienceNextLo {0}", ZetaDia.Me.CommonData.GetAttribute<int>(ActorAttributeType.AltExperienceNextLo));

                    var high = ZetaDia.Me.CommonData.GetAttribute<int>(ActorAttributeType.AltExperienceNextHi);
                    var low = ZetaDia.Me.CommonData.GetAttribute<int>(ActorAttributeType.AltExperienceNextLo);
                    long result = (long)high << 32 + low;
                    ulong result2 = (ulong)high << 32 + low;
                    Logger.Raw("Result {0}", result);
                    Logger.Raw("Result2 {0}", result2);


                    Logger.Raw(" ");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void ToggleRadarUI_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(CacheUI.ToggleRadarWindow);
        }

        static void DumpLevelAreaScenes_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {
                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    Logger.Raw("\nCurrent Level Area {0} ({1})", AdvDia.CurrentLevelAreaId,
                        (SNOLevelArea)AdvDia.CurrentLevelAreaId);

                    ScenesStorage.Update();
                    var scenes = ScenesStorage.CurrentWorldScenes.Where(s => s.LevelAreaId == AdvDia.CurrentLevelAreaId);
                    foreach (var adventurerScene in scenes)
                    {
                        Logger.Raw("{0}", adventurerScene.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpActor_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                var mbox = InputBox.Show("Enter ActorSNO", "Adventurer", string.Empty);
                if (mbox.ReturnCode == DialogResult.Cancel)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(mbox.Text))
                {
                    Logger.Info("Enter an actorId");
                    return;
                }
                int actorId;
                if (!int.TryParse(mbox.Text, out actorId))
                {
                    Logger.Info("Invalid actorId");
                    return;
                }
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var actor =
                        ZetaDia.Actors.GetActorsOfType<DiaObject>(true)
                            .Where(a => a.ActorSNO == actorId)
                            .OrderBy(a => a.Distance)
                            .FirstOrDefault();
                    if (actor == null)
                    {
                        Logger.Info("Actor not found");
                        return;
                    }

                    //foreach (var attribute in Enum.GetValues(typeof(ActorAttributeType)).Cast<ActorAttributeType>())
                    //{
                    //    Logger.Raw("ActorAttributeType.{0}: {1}", attribute.ToString(), actor.CommonData.GetAttribute<int>(attribute));
                    //}
                    Logger.Raw(" ");
                    Logger.Raw("Actor Details for actorId: {0}", actorId);
                    ObjectDumper.Write(actor, 1);
                    Logger.Raw("Untargetable: {0}", actor.CommonData.GetAttribute<int>(ActorAttributeType.Untargetable));

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpMe_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();


                    //foreach (var attribute in Enum.GetValues(typeof(ActorAttributeType)).Cast<ActorAttributeType>())
                    //{
                    //    Logger.Raw("ActorAttributeType.{0}: {1}", attribute.ToString(), actor.CommonData.GetAttribute<int>(attribute));
                    //}
                    Logger.Raw(" ");
                    ObjectDumper.Write(ZetaDia.Me, 1);
                    Logger.Raw("Bnet Hero Id: {0}", ZetaDia.Service.Hero.HeroId);
                    var heroId = ZetaDia.Service.Hero.HeroId;
                    var baseAddress = ZetaDia.Me.BaseAddress;
                    for (int i = 0; i < 10000; i = i + 4)
                    {
                        if (ZetaDia.Memory.Read<int>(IntPtr.Add(baseAddress, i)) == heroId)
                        {
                            Logger.Raw("Offset for BnetHeroId: {0}", i);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpBountyQuests_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                using (ZetaDia.Memory.AcquireFrame(true))
                {
                    if (!ZetaDia.IsInGame)
                        return;
                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    Dictionary<Act, SNOQuest> actBountyFinishingQuests = new Dictionary<Act, SNOQuest>
                                                    {
                                                        {Act.A1,SNOQuest.x1_AdventureMode_BountyTurnin_A1},
                                                        {Act.A2,SNOQuest.x1_AdventureMode_BountyTurnin_A2},
                                                        {Act.A3,SNOQuest.x1_AdventureMode_BountyTurnin_A3},
                                                        {Act.A4,SNOQuest.x1_AdventureMode_BountyTurnin_A4},
                                                        {Act.A5,SNOQuest.x1_AdventureMode_BountyTurnin_A5},
                                                    };

                    var quests =
                        ZetaDia.ActInfo.AllQuests.Where(q => actBountyFinishingQuests.ContainsValue(q.Quest)).ToList();
                    foreach (var quest in quests)
                    {
                        ObjectDumper.Write(quest, 1);
                    }


                    Logger.Raw(" ");
                    ObjectDumper.Write(ZetaDia.Me, 1);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpObjects_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var minimapActors =
                        ZetaDia.Actors.GetActorsOfType<DiaObject>(true)
                            .Where(
                                o =>
                                    o.IsValid && o.CommonData != null && o.CommonData.IsValid &&
                                    o.CommonData.MinimapVisibilityFlags != 0)
                            .OrderBy(o => o.Distance)
                            .ToList();
                    Logger.Raw(" ");
                    Logger.Raw("Minimap Actors (Count: {0})", minimapActors.Count);
                    foreach (var actor in minimapActors)
                    {
                        var gizmo = actor as DiaGizmo;

                        Logger.Raw(
                            "ActorId: {0}, Type: {1}, Name: {2}, Distance2d: {3}, CollisionRadius: {4}, MinimapActive: {5}, MinimapIconOverride: {6}, MinimapDisableArrow: {7} ",
                            actor.ActorSNO,
                            actor.ActorType,
                            actor.Name,
                            actor.Position.Distance2D(AdvDia.MyPosition),
                            actor is DiaGizmo ? gizmo.CollisionSphere.Radius : 0,
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapActive),
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapIconOverride),
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapDisableArrow)
                            );
                    }


                    var objects =
                        ZetaDia.Actors.GetActorsOfType<DiaObject>(true)
                            .Where(o => o.IsValid && o.CommonData != null && o.CommonData.IsValid)
                            .OrderBy(o => o.Distance)
                            .ToList();
                    Logger.Raw(" ");
                    Logger.Raw("Actors (Count: {0})", objects.Count);

                    foreach (var actor in objects)
                    {
                        var gizmo = actor as DiaGizmo;

                        Logger.Raw(
                            "ActorId: {0}, Type: {1}, Name: {2}, Distance2d: {3}, CollisionRadius: {4}, MinimapActive: {5}, MinimapIconOverride: {6}, MinimapDisableArrow: {7} ",
                            actor.ActorSNO,
                            actor.ActorType,
                            actor.Name,
                            actor.Position.Distance2D(AdvDia.MyPosition),
                            actor is DiaGizmo ? gizmo.CollisionSphere.Radius : 0,
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapActive),
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapIconOverride),
                            actor.CommonData.GetAttribute<int>(ActorAttributeType.MinimapDisableArrow)
                            );
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpBackpack_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var objects = ZetaDia.Me.Inventory.Backpack.ToList();
                    Logger.Raw(" ");
                    Logger.Raw("Actors (Count: {0})", objects.Count);

                    foreach (var actor in objects)
                    {

                        Logger.Raw(
                            "ActorId: {0}, Type: {1}, Name: {2}",
                            actor.ActorSNO,
                            actor.ActorType,
                            actor.Name

                            );
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private static Dictionary<int, int> _partyMembersFirstHit = new Dictionary<int, int>();
        private static Dictionary<int, int> _partyMembersSecondtHit = new Dictionary<int, int>();
        private static bool isFirstHit = true;
        static void DumpParty_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    Logger.Raw("Hooks:");
                    foreach (var hook in TreeHooks.Instance.Hooks)
                    {
                        Logger.Raw("{0}: {1}", hook.Key,string.Join(", " ,hook.Value));
                    }

                    var party = ZetaDia.Players.ToList();

                    Logger.Raw(" ");
                    Logger.Raw("Party Members (Count: {0})", party.Count);

                    foreach (var actor in party)
                    {
                        Logger.Raw("=======================================================");
                        ObjectDumper.Write(actor, 1);
                    }
                    for (var i = 0; i <= 40000; i = i + 4)
                    {
                        if (isFirstHit)
                        {
                            _partyMembersFirstHit.Add(i, ZetaDia.Memory.Read<int>(ZetaDia.Service.Party.BaseAddress + i));
                        }
                        else
                        {
                            _partyMembersSecondtHit.Add(i, ZetaDia.Memory.Read<int>(ZetaDia.Service.Party.BaseAddress + i));
                        }
                    }
                    Logger.Raw("IsFirstHit: {0}", isFirstHit);
                    if (!isFirstHit)
                    {
                        for (var i = 0; i <= 40000; i = i + 4)
                        {
                            if (_partyMembersFirstHit[i] != _partyMembersSecondtHit[i])
                            {
                                Logger.Raw("{0}: {1} - {2}", i, _partyMembersFirstHit[i], _partyMembersSecondtHit[i]);
                            }
                        }
                        _partyMembersFirstHit = new Dictionary<int, int>();
                        _partyMembersSecondtHit = new Dictionary<int, int>();
                    }
                    isFirstHit = !isFirstHit;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpMapMarkers_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    var myPosition = ZetaDia.Me.Position;

                    Logger.Raw(" ");
                    Logger.Raw("Dumping Minimap Markers");

                    foreach (
                        var mapMarker in
                            ZetaDia.Minimap.Markers.CurrentWorldMarkers.OrderBy(
                                m => Vector3.Distance(myPosition, m.Position)).Take(100))
                    {

                        var locationInfo = string.Format("x=\"{0:0}\" y=\"{1:0}\" z=\"{2:0}\" ", mapMarker.Position.X,
                            mapMarker.Position.Y, mapMarker.Position.Z);
                        Logger.Raw(
                            "Id={0} MinimapTexture={1} NameHash={2} IsPointOfInterest={3} IsPortalEntrance={4} IsPortalExit={5} IsWaypoint={6} Location={7} Distance={8:N0}",
                            mapMarker.Id,
                            mapMarker.MinimapTexture,
                            mapMarker.NameHash,
                            mapMarker.IsPointOfInterest,
                            mapMarker.IsPortalEntrance,
                            mapMarker.IsPortalExit,
                            mapMarker.IsWaypoint,
                            locationInfo,
                            Vector3.Distance(myPosition, mapMarker.Position));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpUnsupportedBounties_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    Logger.Raw("Unsupported Bounties:");
                    Logger.Raw(" ");

                    var bounties =
                        ZetaDia.ActInfo.Bounties.Where(
                            b =>
                                BountyDataFactory.GetBountyData((int)b.Quest) == null &&
                                b.State != QuestState.Completed)
                            .ToList();

                    var wp = ZetaDia.Actors.GetActorsOfType<GizmoWaypoint>().OrderBy(g => g.Distance).FirstOrDefault();
                    var wpNr = 0;
                    if (wp != null)
                    {
                        wpNr = wp.WaypointNumber;
                    }

                    foreach (var bountyInfo in bounties)
                    {
                        DumpBountyInfo(bountyInfo, wpNr);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void DumpBountyInfo(BountyInfo bountyInfo, int waypointNumber)
        {
            Logger.Raw("// {0} - {1} ({2})", bountyInfo.Act, bountyInfo.Info.DisplayName, (int)bountyInfo.Quest);
            Logger.Raw("Bounties.Add(new BountyData");
            Logger.Raw("{");
            Logger.Raw("    QuestId = {0},", (int)bountyInfo.Quest);
            Logger.Raw("    Act = Act.{0},", bountyInfo.Act);
            Logger.Raw("    WorldId = 0, // Enter the final worldId here");
            Logger.Raw("    QuestType = BountyQuestType.SpecialEvent,");
            Logger.Raw("    WaypointNumber = {0},", waypointNumber);
            Logger.Raw("    Coroutines = new List<ISubroutine>");
            Logger.Raw("    {");
            Logger.Raw("        // Coroutines goes here");
            Logger.Raw("    }");
            Logger.Raw("});");
            Logger.Raw(" ");

        }

        static void MoveToPosition_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    Logger.Raw(" ");
                    Logger.Raw("new MoveToPositionCoroutine({3}, new Vector3({0}, {1}, {2})),",
                        (int)AdvDia.MyPosition.X, (int)AdvDia.MyPosition.Y, (int)AdvDia.MyPosition.Z,
                        AdvDia.CurrentWorldId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void MoveToScene_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                ScenesStorage.Update();
                SafeFrameLock.ExecuteWithinFrameLock(() =>
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;
                    //AdvDia.Update();
                    Logger.Raw(" ");
                    Logger.Raw("new MoveToSceneCoroutine({0}, {1}, \"{2}\"),", activeBounty,
                        AdvDia.CurrentWorldId, AdvDia.CurrentWorldScene.Name);
                }, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void MoveToScenePosition_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                ScenesStorage.Update();
                SafeFrameLock.ExecuteWithinFrameLock(() =>
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;
                    //AdvDia.Update();
                    Logger.Raw(" ");
                    var currentScenePosition = AdvDia.CurrentWorldScene.GetRelativePosition(AdvDia.MyPosition);
                    Logger.Raw("new MoveToScenePositionCoroutine({0}, {1}, \"{2}\", new Vector3({3}f, {4}f, {5}f)),", activeBounty,
                        AdvDia.CurrentWorldId, AdvDia.CurrentWorldScene.Name, currentScenePosition.X,
                        currentScenePosition.Y, currentScenePosition.Z);
                }, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void MoveToMapMarker_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    var objectiveMarkers = AdvDia.CurrentWorldMarkers.Where(m => m.Id >= 0 && m.Id <= 200);

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;


                    Logger.Raw(" ");
                    foreach (var objectiveMarker in objectiveMarkers)
                    {
                        Logger.Raw("new MoveToMapMarkerCoroutine({0}, {1}, {2}),", activeBounty, AdvDia.CurrentWorldId,
                            objectiveMarker.NameHash);
                        Logger.Raw(" ");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void MoveToActor_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;
                    var actors =
                        ZetaDia.Actors.GetActorsOfType<DiaObject>(true)
                            .Where(
                                a =>
                                    a.IsFullyValid() &&
                                    (a.IsInteractableQuestObject() ||
                                     (a is DiaUnit && (a as DiaUnit).CommonData.IsUnique)))
                            .OrderBy(a => a.Distance)
                            .ToList();

                    if (actors.Count == 0)
                    {
                        Logger.Raw("// Could not detect an active quest actors, you must be out of range.");
                    }
                    foreach (var actor in actors)
                    {
                        Logger.Raw("// {0} ({1}) Distance: {2}", (SNOActor)actor.ActorSNO, actor.ActorSNO,
                            actor.Distance);
                        Logger.Raw("new MoveToActorCoroutine({0}, {1}, {2}),", activeBounty, AdvDia.CurrentWorldId,
                            actor.ActorSNO);
                        Logger.Raw(" ");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void EnterLevelArea_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;

                    var objectiveMarkers = AdvDia.CurrentWorldMarkers.Where(m => m.Id >= 0 && m.Id <= 200).ToList();

                    if (objectiveMarkers.Count == 0)
                    {
                        Logger.Raw(
                            "// Could not detect an active objective marker, you are either out of range or to close to it.");
                    }
                    foreach (var marker in objectiveMarkers)
                    {
                        //new EnterLevelAreaCoroutine(int questId, int sourceWorldId, int destinationWorldId, int portalMarker, int portalActorId)
                        var portal =
                            ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true)
                                .FirstOrDefault(
                                    a => a.IsFullyValid() && a.IsPortal && a.Position.Distance2D(marker.Position) <= 5);
                        if (portal != null)
                        {
                            Logger.Raw("new EnterLevelAreaCoroutine({0}, {1}, {2}, {3}, {4}),", activeBounty,
                                AdvDia.CurrentWorldId, 0, marker.NameHash, portal.ActorSNO);
                        }
                        else
                        {
                            Logger.Raw(
                                "// Could not detect the portal near the marker. You need to get a bit closer to the level entrance");
                        }
                    }
                    Logger.Raw(" ");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void ClearLevelArea_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;

                    Logger.Raw("new ClearLevelAreaCoroutine({0}),", activeBounty);
                    Logger.Raw(" ");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void ClearAreaForNSeconds_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    int seconds;
                    var mbox = InputBox.Show("How many seconds?", "Adventurer", "60");
                    if (mbox.ReturnCode == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(mbox.Text))
                    {
                        seconds = 60;
                    }
                    else
                    {
                        if (!int.TryParse(mbox.Text, out seconds))
                        {
                            Logger.Raw("// Invalid number");
                            return;
                        }
                    }

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();

                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;

                    //ClearAreaForNSecondsCoroutine(int questId, int seconds, int actorId, int marker, int radius = 30, bool increaseRadius = true)
                    Logger.Raw("new ClearAreaForNSecondsCoroutine({0}, {1}, {2}, {3}, {4}),", activeBounty, seconds, 0,
                        0, 45);
                    Logger.Raw(" ");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void WaitForNSeconds_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    int seconds;
                    var mbox = InputBox.Show("How many seconds?", "Adventurer", "5");
                    if (mbox.ReturnCode == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(mbox.Text))
                    {
                        seconds = 60;
                    }
                    else
                    {
                        if (!int.TryParse(mbox.Text, out seconds))
                        {
                            Logger.Raw("// Invalid number");
                            return;
                        }
                    }

                    Logger.Raw("new WaitCoroutine({0}),", seconds * 1000);
                    Logger.Raw(" ");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void InteractWithUnit_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {

                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;
                    var actors =
                        ZetaDia.Actors.GetActorsOfType<DiaUnit>(true)
                            .Where(a => a.IsFullyValid() && a.IsInteractableQuestObject())
                            .OrderBy(a => a.Distance)
                            .ToList();

                    if (actors.Count == 0)
                    {
                        Logger.Raw("// Could not detect an active quest unit, you must be out of range.");
                    }
                    foreach (var actor in actors)
                    {
                        var marker =
                            AdvDia.CurrentWorldMarkers.FirstOrDefault(m => m.Position.Distance2D(actor.Position) < 30);
                        int markerId = 0;
                        if (marker != null)
                        {
                            markerId = marker.NameHash;
                        }
                        //InteractWithUnitCoroutine(int questId, int worldId, int actorId, int marker, int interactAttemps = 1, int secondsToSleepAfterInteraction = 1, int secondsToTimeout = 4)
                        Logger.Raw("// {0} ({1}) Distance: {2}", (SNOActor)actor.ActorSNO, actor.ActorSNO,
                            actor.Distance);
                        Logger.Raw("new InteractWithUnitCoroutine({0}, {1}, {2}, {3}, {4}),", activeBounty,
                            AdvDia.CurrentWorldId, actor.ActorSNO, markerId, 5);
                        Logger.Raw(" ");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        static void InteractWithGizmo_Click(object sender, RoutedEventArgs e)
        {
            if (BotEvents.IsBotRunning)
            {
                BotMain.Stop();
                Thread.Sleep(500);
            }
            try
            {
                if (!ZetaDia.IsInGame)
                    return;
                using (ZetaDia.Memory.AcquireFrame(true))
                {


                    if (ZetaDia.Me == null)
                        return;
                    if (!ZetaDia.Me.IsValid)
                        return;

                    ZetaDia.Actors.Update();
                    //AdvDia.Update();
                    var activeBounty = ZetaDia.ActInfo.ActiveBounty != null
                        ? (int)ZetaDia.ActInfo.ActiveBounty.Quest
                        : 0;
                    var actors =
                        ZetaDia.Actors.GetActorsOfType<DiaGizmo>(true)
                            .Where(a => a.IsFullyValid() && a.IsInteractableQuestObject())
                            .OrderBy(a => a.Distance)
                            .ToList();

                    if (actors.Count == 0)
                    {
                        Logger.Raw("// Could not detect an active quest gizmo, you must be out of range.");
                    }
                    foreach (var actor in actors)
                    {
                        var marker =
                            AdvDia.CurrentWorldMarkers.FirstOrDefault(m => m.Position.Distance2D(actor.Position) < 30);
                        int markerId = 0;
                        if (marker != null)
                        {
                            markerId = marker.NameHash;
                        }
                        //InteractWithGizmoCoroutine(int questId, int worldId, int actorId, int marker, int interactAttemps = 1, int secondsToSleepAfterInteraction = 1, int secondsToTimeout = 10)
                        Logger.Raw("// {0} ({1}) Distance: {2}", (SNOActor)actor.ActorSNO, actor.ActorSNO,
                            actor.Distance);
                        Logger.Raw("new InteractWithGizmoCoroutine({0}, {1}, {2}, {3}, {4}),", activeBounty,
                            AdvDia.CurrentWorldId, actor.ActorSNO, markerId, 5);
                        Logger.Raw(" ");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

    }
}
