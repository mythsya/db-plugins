#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Trinity.Config.Combat;
using TrinityCoroutines;
using TrinityCoroutines.Resources;
using Trinity.DbProvider;
using Trinity.Helpers;
using Trinity.ItemRules;
using Trinity.Items;
using Trinity.UI.UIComponents;
using Zeta.Bot;
using Zeta.Bot.Navigation;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals;
using Zeta.Game.Internals.Actors;
using Logger = Trinity.Technicals.Logger;
using UIElement = Zeta.Game.Internals.UIElement;

#endregion

namespace Trinity.UI
{
    internal class TabUi
    {
        private static UniformGrid _tabGrid;
        private static TabItem _tabItem;

        public static HashSet<int> CrafingMaterialIds = new HashSet<int>
        {
            361985, //Type: Item, Name: Arcane Dust
            361984, //Type: Item, Name: Reusable Parts
            361986, //Type: Item, Name: Veiled Crystal
            364281, //Type: Item, Name: Caldeum Nightshade
            361989, //Type: Item, Name: Death's Breath
            364975, //Type: Item, Name: Westmarch Holy Water
            364290, //Type: Item, Name: Arreat War Tapestry
            364305, //Type: Item, Name: Corrupted Angel Flesh
            365020, //Type: Item, Name: Khanduran Rune
            361988 //Type: Item, Name: Forgotten Soul
        };

        private static DateTime LastStartedConvert = DateTime.UtcNow;

        internal static void InstallTab()
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    var mainWindow = Application.Current.MainWindow;

                    _tabGrid = new UniformGrid
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        Columns = 4,
                        MaxHeight = 180
                    };

                    CreateButton("Configure", ShowMainTrinityUIEventHandler);
                    CreateButton("Sort Backpack", SortBackEventHandler);
                    CreateButton("Sort Stash", SortStashEventHandler);
                    CreateButton("Clean Stash", CleanStashEventHandler);
                    CreateButton("Reload Item Rules", ReloadItemRulesEventHandler);
                    CreateButton("Drop Legendaries", DropLegendariesEventHandler);
                    CreateButton("Find New ActorIds", GetNewActorSNOsEventHandler);
                    CreateButton("Dump My Build", DumpBuildEventHandler);
                    CreateButton("Show Cache", ShowCacheWindowEventHandler);
                    CreateButton("Reset TVars", ResetTVarsEventHandler);
                    CreateButton("Start Test", StartTestHandler);
                    CreateButton("Stop Test", StopTestHandler);
                    CreateButton("Cache Test", CacheTestCacheEventHandler);
                    CreateButton("Log Invalid Items", LogInvalidHandler);
                    //CreateButton("Special Test", btnClick_SpecialTestHandler);

                    CreateButton("Rare => Magic", btnClick_ConvertToBlue);
                    CreateButton("Rare => Common", btnClick_ConvertToCommon);

                    CreateButton("Scan UIElement", btnClick_ScanUIElement);

                    //CreateButton("1000 Rare => Magic", btnClick_MassConvertRareToMagic);
                    //CreateButton("Move to Stash", btnClick_MoveToStash);
                    //CreateButton("Move to Cube", btnClick_MoveToCube);
                    //CreateButton("Rares => Legendary", btnClick_UpgradeRares);
                    //CreateButton("Test1", btnClick_Test1);
                    //CreateButton("Test2", btnClick_HijackTest);
                    CreateButton("Log Run Time", btnClick_LogRunTime);

                    CreateButton("Test ItemList", btnClick_TestItemList);


                    _tabItem = new TabItem
                    {
                        Header = "Trinity",
                        ToolTip = "Trinity Functions",
                        Content = _tabGrid
                    };

                    var tabs = mainWindow.FindName("tabControlMain") as TabControl;
                    if (tabs == null)
                        return;

                    tabs.Items.Add(_tabItem);
                }
                );
        }

        /**************
         * 
         * WARNING
         * 
         * ALWAYS surround your RoutedEventHandlers in try/catch. Failure to do so will result in Demonbuddy CRASHING if an exception is thrown.
         * 
         * WARNING
         *  
         *************/

        private static void btnClick_LogRunTime(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Bot {0} has been running for {1} hours {2} minutes and {3} seconds", ZetaDia.CPlayer.HeroName, GameStats.Instance.RunTime.Hours, GameStats.Instance.RunTime.Minutes, GameStats.Instance.RunTime.Seconds);
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception {0}", ex);
            }
        }

        private static void btnClick_TestItemList(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                using (new MemoryHelper())
                {
                    DebugUtil.ItemListTest();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception {0}", ex);
            }
        }

        private static void LogInvalidHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                DebugUtil.LogInvalidItems();
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception {0}", ex);
            }
        }

        private static void CacheTestCacheEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Finished Cache Test");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception {0}", ex);
            }
        }

        private static void StartTestHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                var unitAtts = new Dictionary<int, HashSet<ActorAttributeType>>();
                var unitLastDamage = new Dictionary<int, float>();

                if (!ZetaDia.IsInGame)
                    return;

                Worker.Start(() =>
                {
                    using (new MemoryHelper())
                    {
                        Func<DiaUnit, bool> isValid = u => u != null && u.IsValid && u.CommonData != null && u.CommonData.IsValid && !u.CommonData.IsDisposed;

                        var testunits = ZetaDia.Actors.GetActorsOfType<DiaUnit>().Where(u => isValid(u) && u.RActorGuid != ZetaDia.Me.RActorGuid).ToList();
                        if (!testunits.Any())
                            return false;

                        var testunit = testunits.OrderBy(u => u.Distance).FirstOrDefault();
                        if (testunit == null || testunit.CommonData == null)
                        {
                            testunit = ZetaDia.Me;
                        }


                        //PowerBuff0VisualEffectNone (1) Power=MonsterAffix_ReflectsDamage
                        //if(PowerBuff3VisualEffectNone (1) Power=MonsterAffix_ReflectsDamageCast)

                        if (testunit.CommonData.GetAttribute<int>(((int) SNOPower.MonsterAffix_ReflectsDamageCast << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectNone & 0xFFF)) == 1)
                        {
                            Logger.Log("Unit {0} has has reflect buff", testunit.Name);
                        }


                        //var attrs = new HashSet<ActorAttributeType>
                        //{
                        //     ActorAttributeType.CustomTargetWeight,
                        //     ActorAttributeType.Hunter,
                        //     ActorAttributeType.PlatinumCapLastGain,
                        //};


                        //foreach (var att in attrs)
                        //{                                
                        //    Logger.Log("Unit {0} attr {1} ival={2} fval={3}", testunit.Name, att, testunit.CommonData.GetAttribute<int>(att), testunit.CommonData.GetAttribute<float>(att));
                        //}


                        var existingAtts = unitAtts.GetOrCreateValue(testunit.ACDGuid, new HashSet<ActorAttributeType>());
                        var atts = Enum.GetValues(typeof (ActorAttributeType)).Cast<ActorAttributeType>().ToList();

                        foreach (var att in atts)
                        {
                            //if (!att.ToString().ToLower().Contains("buff"))
                            //    continue;

                            try
                            {
                                var attiResult = testunit.CommonData.GetAttribute<int>(att);
                                var attfResult = testunit.CommonData.GetAttribute<float>(att);

                                var hasValue = attiResult > 0 || !float.IsNaN(attfResult) && attfResult > 0;

                                if (hasValue)
                                {
                                    if (!existingAtts.Contains(att))
                                    {
                                        Logger.Log("Unit {0} has gained {1} (i:{2} f:{3:00.00000})", testunit.Name, att.ToString(), attiResult, attfResult);
                                        existingAtts.Add(att);

                                        //if (att == ActorAttributeType.LastDamageACD)
                                        //{
                                        //    //var idmg = testunit.CommonData.GetAttribute<int>((attiResult << 12) + ((int) ActorAttributeType.LastDamageAmount & 0xFFF));
                                        //    //var fdmg = testunit.CommonData.GetAttribute<float>((attiResult << 12) + ((int) ActorAttributeType.LastDamageAmount & 0xFFF));
                                        //    //Logger.Log("DamageByUnit To LastACD {0} has gained {1} (i:{2} f:{3:00.00000})", testunit.Name, "LastDamageAmount", idmg, fdmg);

                                        //    var idmg = ZetaDia.Me.CommonData.GetAttribute<int>((attiResult << 12) + ((int)ActorAttributeType.LastDamageAmount & 0xFFF));
                                        //    var fdmg = ZetaDia.Me.CommonData.GetAttribute<float>((attiResult << 12) + ((int)ActorAttributeType.LastDamageAmount & 0xFFF));
                                        //    Logger.Log("DamageToPlayer By Unit {0} has gained {1} (i:{2} f:{3:00.00000})", testunit.Name, "LastDamageAmount", idmg, fdmg);

                                        //}        

                                        //foreach (var type in atts)
                                        //{
                                        //    //if (!att.ToString().ToLower().Contains("buff"))
                                        //    //    continue;

                                        //    try
                                        //    {                                                                            
                                        //        var ari = ZetaDia.Me.CommonData.GetAttribute<int>((testunit.ACDGuid << 12) + ((int)type & 0xFFF));
                                        //        var arf = ZetaDia.Me.CommonData.GetAttribute<float>((testunit.ACDGuid << 12) + ((int)type & 0xFFF));
                                        //        var hv = ari > 0 || !float.IsNaN(arf) && arf > 0;
                                        //        if (hv)
                                        //        {
                                        //            Logger.Log("PlayerOffset {0} has gained {1} (i:{2} f:{3:00.00000})", testunit.Name, type.ToString(), ari, arf);
                                        //        }
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //    }
                                        //}
                                    }
                                }
                                else
                                {
                                    if (existingAtts.Contains(att))
                                    {
                                        Logger.Log("Unit {0} has lost {1} (i:{2} f:{3:00.00000})", testunit.Name, att.ToString(), attiResult, attfResult);
                                        existingAtts.Remove(att);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }


                        //if (testunit.CommonData.GetAttribute<int>(((int) SNOPower.MonsterAffix_ReflectsDamage << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectNone & 0xFFF)) == 1)
                        //{
                        //    Logger.Log("Unit {0} has reflect damage", testunit.Name);
                        //}


                        //MonsterAffix_ReflectsDamageCast
                        var allpowers = Enum.GetValues(typeof (SNOPower)).Cast<SNOPower>().ToList();
                        var allBuffAttributes = Enum.GetValues(typeof(ActorAttributeType)).Cast<ActorAttributeType>().Where(a => a.ToString().StartsWith("PowerBuff")).ToList();

                        var checkpowers = new HashSet<SNOPower>
                        {
                            SNOPower.MonsterAffix_ReflectsDamage,
                            SNOPower.MonsterAffix_ReflectsDamageCast,
                            SNOPower.Monk_ExplodingPalm
                        };

                        foreach (var power in allpowers)
                        {
                            foreach (var buffattr in allBuffAttributes)
                            {
                                try
                                {
                                    if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int)buffattr & 0xFFF)) == 1)
                                    {
                                        Logger.Log("Unit {0} has {1} ({2})", testunit.Name, power, buffattr);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }

                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectA & 0xFFF)) == 1)
                            //    result = true;

                            //// Exploding Palm
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff0VisualEffectE & 0xFFF)) == 1)
                            //    result = true;

                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffectA & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff11VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff1VisualEffectE & 0xFFF)) == 1)
                            //    result = true;

                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectA & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff2VisualEffectE & 0xFFF)) == 1)
                            //    result = true;


                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectA & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff3VisualEffectE & 0xFFF)) == 1)
                            //    result = true;

                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectA & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff4VisualEffectE & 0xFFF)) == 1)
                            //    result = true;

                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffect & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectNone & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectA & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectB & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectC & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectD & 0xFFF)) == 1)
                            //    result = true;
                            //if (testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) ActorAttributeType.PowerBuff5VisualEffectE & 0xFFF)) == 1)
                            //    result = true;
                        }


                        //}
                        //var atts = Enum.GetValues(typeof(ActorAttributeType)).Cast<ActorAttributeType>().ToList();
                        //var powers = Enum.GetValues(typeof(SNOPower)).Cast<SNOPower>().ToList();


                        //foreach (var att in atts)
                        //{
                        //    //if (!att.ToString().ToLower().Contains("buff"))
                        //    //    continue;

                        //    try
                        //    {                                    
                        //        var attResult = testunit.CommonData.GetAttribute<int>(att);
                        //        if (attResult > 0)
                        //        {
                        //            if (existingAtts.Contains(att))
                        //            {
                        //                Logger.Log("Unit {0} has lost {1} ({2})", testunit.Name, att.ToString(), attResult);
                        //                existingAtts.Remove(att);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (!existingAtts.Contains(att))
                        //            {
                        //                Logger.Log("Unit {0} has gained {1} ({2})", testunit.Name, att.ToString(), attResult);
                        //                existingAtts.Add(att);
                        //            }
                        //        }

                        //        foreach (var power in powers)
                        //        {
                        //            try
                        //            {
                        //                var attPowerResult = testunit.CommonData.GetAttribute<int>(((int) power << 12) + ((int) att & 0xFFF));
                        //                if (attPowerResult > 0)
                        //                {
                        //                    if (existingAtts.Contains(att))
                        //                    {
                        //                        Logger.Log("Unit {0} has lost {1} ({2}) Power={3}", testunit.Name, att.ToString(), attPowerResult, power);
                        //                        existingAtts.Remove(att);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    if (!existingAtts.Contains(att))
                        //                    {
                        //                        Logger.Log("Unit {0} has gained {1} ({2}) Power={3}", testunit.Name, att.ToString(), attPowerResult, power);
                        //                        existingAtts.Add(att);
                        //                    }
                        //                }
                        //            }
                        //            catch (Exception)
                        //            {

                        //            }
                        //        }


                        //foreach (var power in powers)
                        //{
                        //    var attIntPowerResult = testunit.CommonData.GetAttribute<int>(((int)power << 12) + ((int)att & 0xFFF));                                        
                        //}


                        //var attIntResult = testunit.CommonData.GetAttribute<int>(((int) debuffSNO << 12) + ((int) att & 0xFFF));
                        //if(attIntResult > 0)
                        //    Logger.Log("Unit {0} has {1}", testunit.Name, att.ToString());
                        //    }
                        //    catch (Exception)
                        //    {

                        //    }

                        //}
                    }
                    return false;
                });

                //CacheManager.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Starting LazyCache: " + ex);
            }
        }

        private static void StopTestHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Worker.Stop();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Starting LazyCache: " + ex);
            }
        }

        private static void btnClick_SpecialTestHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                // A1 Open World Stash Location
                var stashlocation = new Vector3(388.16f, 509.63f, 23.94531f);

                CoroutineHelper.RunCoroutine(() => Navigator.MoveTo(TownRun.StashLocation));

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_MoveToStash(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                CoroutineHelper.RunCoroutine(() => Navigator.MoveTo(Town.Locations.Stash));

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_MoveToCube(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                CoroutineHelper.RunCoroutine(() => Navigator.MoveTo(Town.Locations.KanaisCube));

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_UpgradeRares(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                CoroutineHelper.RunCoroutine(() => CubeRaresToLegendary.Execute());

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_BuyVendorBlues(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                CoroutineHelper.RunCoroutine(() => BuyItemsFromVendor.Execute(ItemQualityColor.Yellow),
                    ret => BuyItemsFromVendor.CanRun(ItemQualityColor.Yellow));

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_HijackTest(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("HijackTest Started");

                CoroutineHelper.ForceRunCoroutine(() => CubeRaresToLegendary.Execute(), result => !CubeRaresToLegendary.CanRun());

                Logger.Log("HijackTest Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_Test1(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting");

                using (ZetaDia.Memory.AcquireFrame())
                {
                    ZetaDia.Actors.Update();

                    foreach (var item in ZetaDia.Me.Inventory.Backpack)
                    {
                        var stackHi = item.GetAttribute<int>(ActorAttributeType.ItemStackQuantityHi);
                        var stackLo = item.GetAttribute<int>(ActorAttributeType.ItemStackQuantityLo);

                        Logger.Log("Item: {0} {1} ItemStackQuantity={2} StackHi={3} StackLo={4}",
                            item.Name, item.ACDGuid, item.ItemStackQuantity, stackHi, stackLo);
                    }
                }

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_ConvertToBlue(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting Conversion of Backpack VeiledCrystals to Magic Dust");

                if (Trinity.Settings.Loot.Pickup.MiscItemQuality > TrinityItemQuality.Common)
                {
                    Logger.LogError("Aborting - Dangerous to pull craftin items to backpack when MiscItemQuality setting is set above common");
                    return;
                }

                var from = InventoryItemType.VeiledCrystal;
                var to = InventoryItemType.ArcaneDust;

                if (!UIElements.TransmuteItemsDialog.IsVisible || !ConvertMaterials.CanRun(from, to))
                {
                    Logger.LogError("You need to have the cube window open and all the required materials in your backpack.");
                    return;
                }

                LastStartedConvert = DateTime.UtcNow;

                CoroutineHelper.RunCoroutine(() => ConvertMaterials.Execute(from, to), result => !ConvertMaterials.CanRun(from, to) || CheckConvertTimeout());

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        public static bool CheckConvertTimeout()
        {
            if (DateTime.UtcNow.Subtract(LastStartedConvert).TotalSeconds > 20)
            {
                Logger.LogError("Timeout");
                return true;
            }
            return false;
        }

        private static void btnClick_ConvertToCommon(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting Conversion of Backpack VeiledCrystals to ReusableParts");

                if (Trinity.Settings.Loot.Pickup.MiscItemQuality > TrinityItemQuality.Common)
                {
                    Logger.LogError("Aborting - Too dangerous to put crafting items into backpack when MiscItemQuality setting is set above common");
                    return;
                }

                var from = InventoryItemType.VeiledCrystal;
                var to = InventoryItemType.ReusableParts;

                if (!UIElements.TransmuteItemsDialog.IsVisible || !ConvertMaterials.CanRun(from, to))
                {
                    Logger.LogError("You need to have the cube window open and all the required materials in your backpack.");
                    return;
                }

                LastStartedConvert = DateTime.UtcNow;

                CoroutineHelper.RunCoroutine(() => ConvertMaterials.Execute(from, to), result => !ConvertMaterials.CanRun(from, to) || CheckConvertTimeout());

                Logger.Log("Finished!");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void btnClick_MassConvertRareToMagic(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                Logger.Log("Starting Conversion of Backpack VeiledCrystals to ArcaneDust");


                if (Trinity.Settings.Loot.Pickup.MiscItemQuality > TrinityItemQuality.Common)
                {
                    Logger.LogError("Aborting - Too dangerous to put crafting items into backpack when MiscItemQuality setting is set above common");
                    return;
                }

                var from = InventoryItemType.VeiledCrystal;
                var to = InventoryItemType.ArcaneDust;

                if (!UIElements.TransmuteItemsDialog.IsVisible || !ConvertMaterials.CanRun(from, to))
                {
                    Logger.LogError("You need to have the cube window open and all the required materials in your backpack.");
                    return;
                }

                LastStartedConvert = DateTime.UtcNow;

                CoroutineHelper.RunCoroutine(() => ConvertMaterials.Execute(from, to), result => !ConvertMaterials.CanRun(from, to) || CheckConvertTimeout());

                Logger.Log("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception: " + ex);
            }
        }

        private static void ResetTVarsEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                var doReset = MessageBox.Show("This will reset all of the advanced Trinity Variables. Are you sure?", "Reset Trinity Variables",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (doReset == MessageBoxResult.OK)
                    V.ResetAll();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Resetting TVar's:" + ex);
            }
        }

        private static void ShowCacheWindowEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                CacheUI.CreateWindow();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error showing CacheUI:" + ex);
            }
        }

        private static void btnClick_ScanUIElement(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                //[1E8E8E20] Last clicked: 0x80E63C97B008F590, Name: Root.NormalLayer.vendor_dialog_mainPage.training_dialog
                //[1E94FCC0] Mouseover: 0x244BD04C84DF92F1, Name: Root.NormalLayer.vendor_dialog_mainPage

                using (new MemoryHelper())
                {
                    UIElement.FromHash(0x244BD04C84DF92F1).FindDecedentsWithText("jeweler");
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Error btnClick_ScanUIElement:" + ex);
            }
        }


        private static void ShowMainTrinityUIEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                var configWindow = UILoader.GetDisplayWindow();
                configWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error showing Configuration from TabUI:" + ex);
            }
        }

        private static void DumpBuildEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                DebugUtil.LogBuildAndItems();
            }
            catch (Exception ex)
            {
                Logger.LogError("DumpBuildEventHandler: " + ex);
            }
        }

        private static void GetNewActorSNOsEventHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                DebugUtil.LogNewItems();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error logging new items:" + ex);
            }
        }

        private static void SortBackEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemSort.SortBackpack();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error sorting backpack:" + ex);
            }
        }

        private static void DropLegendariesEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new MemoryHelper())
                {
                    ZetaDia.Me.Inventory.Backpack.Where(i => i.ItemQualityLevel == ItemQuality.Legendary).ForEach(i => i.Drop());

                    if (BotMain.IsRunning && !BotMain.IsPausedForStateExecution)
                        BotMain.PauseFor(TimeSpan.FromSeconds(2));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error dropping legendaries:" + ex);
            }
        }

        private static void SortStashEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemSort.SortStash();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error dropping legendaries:" + ex);
            }
        }

        private static void CleanStashEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure? This may remove and salvage/sell items from your stash! Permanently!", "Clean Stash Confirmation",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    CleanStash.RunCleanStash();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Cleaning Stash:" + ex);
            }
        }

        private static void ReloadItemRulesEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Trinity.StashRule == null)
                    Trinity.StashRule = new Interpreter();

                if (Trinity.StashRule != null)
                {
                    BotMain.PauseWhile(Trinity.StashRule.reloadFromUI);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error Reloading Item Rules:" + ex);
            }
        }

        #region TabMethods

        internal static void RemoveTab()
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    var tabs = mainWindow.FindName("tabControlMain") as TabControl;
                    if (tabs == null)
                        return;
                    tabs.Items.Remove(_tabItem);
                }
                );
        }

        private static void CreateButton(string buttonText, RoutedEventHandler clickHandler)
        {
            var button = new Button
            {
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(3),
                Content = buttonText
            };
            button.Click += clickHandler;
            _tabGrid.Children.Add(button);
        }

        #endregion
    }
}