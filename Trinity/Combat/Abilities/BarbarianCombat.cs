using System;
using System.Linq;
using Trinity.Config.Combat;
using Trinity.Reference;
using Trinity.Technicals;
using Zeta.Bot;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;
using Logger = Trinity.Technicals.Logger;

namespace Trinity.Combat.Abilities
{
    class BarbarianCombat : CombatBase
    {
        private static bool _allowSprintOoc = true;
        private const float MaxFuriousChargeDistance = 300f;

        private static TrinityPower QueuedPower { get; set; }

        public static TrinityPower GetPower()
        {
            TrinityPower power;

            if (UseDestructiblePower)
                return DestroyObjectPower;

            if (UseOOCBuff)
            {
                // Call of The Ancients
                if (CanUseCallOfTheAncients && Sets.ImmortalKingsCall.IsFullyEquipped)
                    return PowerCallOfTheAncients;

                // Sprint OOC
                if (CanUseSprintOOC)
                    return PowerSprint;
            }
            else
            {
                if (QueuedPower != null && !Player.IsIncapacitated && PowerManager.CanCast(QueuedPower.SNOPower) && !Player.IsCastingOrLoading)
                {
                    Logger.LogVerbose(LogCategory.Behavior, "Casting Queued Power {0}", QueuedPower);
                    var next = QueuedPower;
                    QueuedPower.MaxFailedCastReTryAttempts = 5;
                    QueuedPower.WaitBeforeUseDelay = 750;
                    QueuedPower = null;
                    return next;
                }
            }

            // Ignore Pain when near Frozen
            if ((ZetaDia.Me.IsFrozen || ZetaDia.Me.IsRooted || Trinity.ObjectCache.Any(o => o.AvoidanceType == AvoidanceType.IceBall)) && CanCastIgnorePain)
            {
                Logger.Log("Used Ignore Pain to prevent Frozen");
                return PowerIgnorePain;
            }

            if (!UseOOCBuff)
            {
                // Refresh Frenzy
                if (CanCast(SNOPower.Barbarian_Frenzy) && TimeSincePowerUse(SNOPower.Barbarian_Frenzy) > 3000 && TimeSincePowerUse(SNOPower.Barbarian_Frenzy) < 4000)
                    return PowerFrenzy;

                // Refresh Bash - Punish
                if (CanCast(SNOPower.Barbarian_Bash) && TimeSincePowerUse(SNOPower.Barbarian_Bash) > 4000 && TimeSincePowerUse(SNOPower.Barbarian_Bash) < 5000)
                    return PowerBash;
            }

            // Ignore Pain when low on health
            if (CanCastIgnorePain)
                return PowerIgnorePain;

            // WOTB
            if (CanUseWrathOfTheBerserker)
                return PowerWrathOfTheBerserker;

            // Call of the Ancients
            if (CanUseCallOfTheAncients)
                return PowerCallOfTheAncients;

            // Leap with Earth Set.
            if (CanUseLeap && Sets.MightOfTheEarth.IsThirdBonusActive)
                return PowerLeap;

            // Earthquake
            if (CanUseEarthquake)
                return PowerEarthquake;

            // Avalanche
            if (CanUseAvalanche)
                return PowerAvalanche;

            // War Cry
            if (CanUseWarCry)
                return PowerWarCry;

            // Battle Rage
            if (CanUseBattleRage)
                return PowerBattleRage;

            // Rend
            if (CanUseRend)
                return PowerRend;

            // Overpower
            if (CanUseOverPower)
                return PowerOverpower;

            // Threatening Shout
            if (CanUseThreatingShout)
                return PowerThreateningShout;

            // Ground Stomp
            if (CanUseGroundStomp)
                return PowerGroundStomp;

            // Revenge
            if (CanUseRevenge)
                return PowerRevenge;

            // Ancient Spear
            if (CanUseAncientSpear)
                return PowerAncientSpear;

            // Sprint
            if (CanUseSprint)
                return PowerSprint;

            // Furious Charge
            if (CanUseFuriousCharge)
                return PowerFuriousCharge;

            // Leap
            if (CanUseLeap)
                return PowerLeap;

            // Seismic Slam
            if (CanUseSeismicSlam)
                return PowerSeismicSlam;

            // Bash to 3 stacks (Punish)
            if (CanUseBashTo3)
                return PowerBash;

            // Frenzy to 5 stacks (Maniac)
            if (CanUseFrenzyTo5)
                return PowerFrenzy;

            // HOTA Elites
            if (CanUseHammerOfTheAncientsElitesOnly)
                return PowerHammerOfTheAncients;

            // Whirlwind
            if (CanUseWhirlwind)
                return PowerWhirlwind;

            // Hammer of the Ancients
            if (CanUseHammerOfTheAncients)
                return PowerHammerOfTheAncients;

            // Weapon Throw
            if (CanUseWeaponThrow)
                return PowerWeaponThrow;

            // Frenzy Fury Generator
            if (CanUseFrenzy)
                return PowerFrenzy;

            // Bash Fury Generator
            if (CanUseBash)
                return PowerBash;

            // Cleave Fury Generator
            if (CanUseCleave)
                return PowerCleave;

            // Default Attacks
            return DefaultPower;

            return power;
        }

        public static bool CanCastIgnorePain
        {
            get
            {
                if (UseOOCBuff)
                    return false;

                if (GetHasBuff(SNOPower.Barbarian_IgnorePain))
                    return false;

                if (!CanCast(SNOPower.Barbarian_IgnorePain))
                    return false;

                if (Settings.Combat.Barbarian.IgnorePainOffCooldown)
                    return true;

                if (Player.CurrentHealthPct <= Settings.Combat.Barbarian.IgnorePainMinHealthPct)
                    return true;

                if (Player.IsFrozen || Player.IsRooted || Player.IsJailed || Trinity.ObjectCache.Any(o => o.AvoidanceType == AvoidanceType.IceBall))
                    return true;

                return Sets.TheLegacyOfRaekor.IsFullyEquipped && ShouldFuryDump;
            }
        }

        public static bool CanUseCallOfTheAncients
        {
            get
            {
                return !UseOOCBuff &&
                    !IsCurrentlyAvoiding &&
                    CanCast(SNOPower.Barbarian_CallOfTheAncients) &&
                    !Player.IsIncapacitated &&
                    !GetHasBuff(SNOPower.Barbarian_CallOfTheAncients) &&
                    (Sets.ImmortalKingsCall.IsFullyEquipped && Trinity.PlayerOwnedAncientCount < 3 ||
                    CurrentTarget.IsEliteRareUnique ||
                    TargetUtil.EliteOrTrashInRange(V.F("Barbarian.CallOfTheAncients.MinEliteRange")) ||
                    TargetUtil.AnyMobsInRange(V.F("Barbarian.CallOfTheAncients.MinEliteRange"), 3) ||
                    TargetUtil.AnyElitesInRange(V.F("Barbarian.CallOfTheAncients.MinEliteRange")));
            }
        }

        public static bool CanUseWrathOfTheBerserker
        {
            get
            {
                /* WOTB should be used when the following conditions are met:
                 * If ignoring elites, when 3 monsters in 25 yards or 10 monsters in 50 yards are present, OR
                 * If using on hard elites only, when an elite with the required affix is present, OR
                 * If normal mode, when any elite is within 20 yards, OR
                 * If we have low health (use potion health)
                 * And not on the Heart of sin
                 */

                var anyTime = Settings.Combat.Barbarian.WOTBMode == BarbarianWOTBMode.WhenReady && !Player.IsInTown;
                var whenInCombat = Settings.Combat.Barbarian.WOTBMode == BarbarianWOTBMode.WhenInCombat &&
                                    TargetUtil.AnyMobsInRange(50) && !UseOOCBuff;
                var hasBuff = GetHasBuff(SNOPower.Barbarian_WrathOfTheBerserker);
                var hasInfiniteCasting = GetHasBuff(SNOPower.Pages_Buff_Infinite_Casting);

                var emergencyHealth = Player.CurrentHealthPct <= V.F("Barbarian.WOTB.EmergencyHealth") && Settings.Combat.Barbarian.WOTBEmergencyHealth;

                var result =
                    //Player.PrimaryResource >= V.I("Barbarian.WOTB.MinFury") && // WOTB is "free" !
                    // Don't still have the buff
                    !hasBuff && CanCast(SNOPower.Barbarian_WrathOfTheBerserker) &&
                    (WOTBGoblins || WOTBIgnoreElites || WOTBElitesPresent || emergencyHealth || hasInfiniteCasting ||
                     anyTime || whenInCombat);

                return result;
            }
        }

        /// <summary>
        /// If using WOTB on all elites, or if we should only use on "hard" affixes
        /// </summary>
        public static bool WOTBElitesPresent
        {
            get
            {
                //bool hardEliteOverride = Trinity.ObjectCache.Any(o => DataDictionary.ForceUseWOTBIds.Contains(o.ActorSNO)) ||
                //        TargetUtil.AnyElitesInRange(V.F("Barbarian.WOTB.HardEliteRangeOverride"), V.I("Barbarian.WOTB.HardEliteCountOverride"));

                //// WotB only used on Arcane, Frozen, Jailer, Molten, Electrified+Reflect Damage elites, or bosses and ubers, or when more than 4 elites are present
                //bool wotbHardElitesPresent = HardElitesPresent || hardEliteOverride;

                bool hardElitesOnly = Settings.Combat.Barbarian.WOTBMode == BarbarianWOTBMode.HardElitesOnly;

                bool elitesPresent = TargetUtil.AnyElitesInRange(V.F("Barbarian.WOTB.MinRange"), V.I("Barbarian.WOTB.MinCount"));

                return ((!hardElitesOnly && elitesPresent) || (hardElitesOnly && HardElitesPresent));
            }
        }

        /// <summary>
        /// Make sure we are allowed to use wrath on goblins, else make sure this isn't a goblin
        /// </summary>
        public static bool WOTBGoblins
        {
            get
            {
                if (CurrentTarget == null)
                    return false;
                return CurrentTarget.IsTreasureGoblin && Settings.Combat.Barbarian.UseWOTBGoblin;
            }
        }

        /// <summary>
        /// If ignoring elites completely, trigger on 3 trash within 25 yards, or 10 trash in 50 yards
        /// </summary>
        public static bool WOTBIgnoreElites
        {
            get
            {
                return
                    IgnoringElites &&
                    (TargetUtil.AnyMobsInRange(V.F("Barbarian.WOTB.RangeNear"), V.I("Barbarian.WOTB.CountNear")) ||
                    TargetUtil.AnyMobsInRange(V.F("Barbarian.WOTB.RangeFar"), V.I("Barbarian.WOTB.CountFar")) ||
                    TargetUtil.AnyMobsInRange(Settings.Combat.Misc.TrashPackClusterRadius, Settings.Combat.Misc.TrashPackSize));
            }
        }

        public static bool CanUseEarthquake
        {
            get
            {
                double minFury = 50f;
                bool hasCaveIn = CacheData.Hotbar.ActiveSkills.Any(p => p.Power == SNOPower.Barbarian_Earthquake && p.RuneIndex == 4);
                float range = hasCaveIn ? 24f : 14f;

                return
                       !UseOOCBuff &&
                       !IsCurrentlyAvoiding &&
                       !Player.IsIncapacitated &&
                       CanCast(SNOPower.Barbarian_Earthquake) &&
                       Player.PrimaryResource >= minFury &&
                       (TargetUtil.IsEliteTargetInRange(range) || TargetUtil.AnyMobsInRange(range, 10));

            }
        }

        public static bool CanUseBattleRage
        {
            get
            {
                var shouldRefreshTaeguk = GetHasBuff(SNOPower.ItemPassive_Unique_Gem_015_x1) && !Hotbar.Contains(SNOPower.Barbarian_Whirlwind) &&
                    Skills.Barbarian.BattleRage.TimeSinceUse >= 2300 && Skills.Barbarian.BattleRage.TimeSinceUse <= 3000;

                return !Player.IsIncapacitated && CanCast(SNOPower.Barbarian_BattleRage, CanCastFlags.NoTimer) &&
                    (!GetHasBuff(SNOPower.Barbarian_BattleRage) || ShouldFuryDump || shouldRefreshTaeguk) &&
                    Player.PrimaryResource >= V.F("Barbarian.BattleRage.MinFury");
            }
        }

        public static bool CanUseSprintOOC
        {
            get
            {
                return
                (Settings.Combat.Barbarian.SprintMode != BarbarianSprintMode.CombatOnly) &&
                AllowSprintOOC &&
                !Player.IsIncapacitated &&
                CanCast(SNOPower.Barbarian_Sprint) &&
                (Settings.Combat.Misc.AllowOOCMovement || GetHasBuff(SNOPower.Barbarian_WrathOfTheBerserker)) &&
                !GetHasBuff(SNOPower.Barbarian_Sprint) &&
                Player.PrimaryResource >= V.F("Barbarian.Sprint.MinFury");
            }
        }
        public static bool CanUseWarCry
        {
            get
            {
                return
                    CanCast(SNOPower.X1_Barbarian_WarCry_v2, CanCastFlags.NoTimer) &&
                    !Player.IsIncapacitated &&
                    (Player.PrimaryResource <= V.F("Barbarian.WarCry.MaxFury") || !GetHasBuff(SNOPower.X1_Barbarian_WarCry_v2) || Legendary.ChilaniksChain.IsEquipped);
            }
        }
        public static bool CanUseThreatingShout
        {
            get
            {
                var range = V.F("Barbarian.ThreatShout.Range");

                bool inCombat = !UseOOCBuff &&
                    CanCast(SNOPower.Barbarian_ThreateningShout) &&
                    !Player.IsIncapacitated && Skills.Barbarian.ThreateningShout.TimeSinceUse > Settings.Combat.Barbarian.ThreateningShoutWaitDelay &&
                    ((TargetUtil.AnyMobsInRange(range, Settings.Combat.Barbarian.MinThreatShoutMobCount, false)) || TargetUtil.IsEliteTargetInRange(range) ||

                        (Hotbar.Contains(SNOPower.Barbarian_Whirlwind) && Player.PrimaryResource <= V.I("Barbarian.Whirlwind.MinFury")) ||
                        (IsWaitingForSpecial && Player.PrimaryResource <= MinEnergyReserve)
                    );

                bool outOfCombat = UseOOCBuff &&
                     !Player.IsIncapacitated &&
                     Settings.Combat.Barbarian.ThreatShoutOOC && CanCast(SNOPower.Barbarian_ThreateningShout) &&
                     Player.PrimaryResource < V.D("Barbarian.ThreatShout.OOCMaxFury");

                return inCombat || outOfCombat;

            }
        }
        public static bool CanUseGroundStomp
        {
            get
            {
                return
                    !UseOOCBuff &&
                    !Player.IsIncapacitated &&
                    CanCast(SNOPower.Barbarian_GroundStomp) &&
                    (
                        TargetUtil.AnyElitesInRange(V.F("Barbarian.GroundStomp.EliteRange"), V.I("Barbarian.GroundStomp.EliteCount")) ||
                        TargetUtil.AnyMobsInRange(V.F("Barbarian.GroundStomp.TrashRange"), V.I("Barbarian.GroundStomp.TrashCount")) ||
                        (Player.CurrentHealthPct <= V.F("Barbarian.GroundStomp.UseBelowHealthPct") && TargetUtil.AnyMobsInRange(V.F("Barbarian.GroundStomp.TrashRange")))
                    );
            }
        }
        public static bool CanUseRevenge
        {
            get
            {
                return
                    !UseOOCBuff &&
                    CanCast(SNOPower.Barbarian_Revenge) &&
                    !Player.IsIncapacitated &&
                    // Don't use revenge on goblins, too slow!
                    (!CurrentTarget.IsTreasureGoblin || TargetUtil.AnyMobsInRange(V.F("Barbarian.Revenge.TrashRange"), V.I("Barbarian.Revenge.TrashCount")));
            }
        }
        public static bool CanUseFuriousCharge
        {
            get
            {
                if (UseOOCBuff)
                    return false;

                var bestTarget = TargetUtil.GetBestPierceTarget(MaxFuriousChargeDistance);
                var unitsInFrontOfBestTarget = 0;

                if (bestTarget != null)
                    unitsInFrontOfBestTarget = bestTarget.CountUnitsInFront();

                var currentEliteTargetInRange = CurrentTarget.RadiusDistance > 7f && CurrentTarget.IsBossOrEliteRareUnique && CurrentTarget.RadiusDistance <= 35f;
                var shouldRegenFury = CurrentTarget.NearbyUnitsWithinDistance(10) >= 3 && Player.PrimaryResource <= 40;

                if ((Sets.BastionsOfWill.IsFullyEquipped || Legendary.StrongarmBracers.IsEquipped) && !Sets.TheLegacyOfRaekor.IsFullyEquipped)
                    return CanCast(SNOPower.Barbarian_FuriousCharge, CanCastFlags.NoTimer) && !IsCurrentlyAvoiding &&
                        Skills.Barbarian.FuriousCharge.Charges > 0 && (TimeSincePowerUse(SNOPower.Barbarian_FuriousCharge) > 4000 || shouldRegenFury);

                return CanCast(SNOPower.Barbarian_FuriousCharge, CanCastFlags.NoTimer) && !IsCurrentlyAvoiding && Skills.Barbarian.FuriousCharge.Charges > 0 &&
                    (currentEliteTargetInRange || unitsInFrontOfBestTarget >= 3 || Sets.TheLegacyOfRaekor.IsFullyEquipped);

            }
        }
        public static bool CanUseLeap
        {
            get
            {
                bool leapresult = !UseOOCBuff && !Player.IsIncapacitated && CanCast(SNOPower.Barbarian_Leap);
                // This will now cast whenever leap is available and an enemy is around. 
                // Disable Leap OOC option. The last line will prevent you from leaping on destructibles
                if (Legendary.LutSocks.IsEquipped)
                {
                    return leapresult && TargetUtil.AnyMobsInRange(15f, 1);
                }
                return leapresult && (TargetUtil.ClusterExists(15f, 35f, V.I("Barbarian.Leap.TrashCount")) || CurrentTarget.IsBossOrEliteRareUnique);
            }
        }
        public static bool CanUseRend
        {
            get
            {
                if (UseOOCBuff || IsCurrentlyAvoiding || Player.IsIncapacitated || !CanCast(SNOPower.Barbarian_Rend))
                    return false;

                if (!CanCast(SNOPower.Barbarian_Rend))
                    return false;

                bool hasReserveEnergy = (!IsWaitingForSpecial && Player.PrimaryResource >= 20) || (IsWaitingForSpecial && Player.PrimaryResource > MinEnergyReserve);

                var mobCountThreshold = Trinity.ObjectCache.Count(o => o.IsUnit && (!o.HasDebuff(SNOPower.Barbarian_Rend)) && o.RadiusDistance <= 12) >= 3 || CurrentTarget.IsEliteRareUnique;
                if (!mobCountThreshold)
                    return false;

                // Spam with Bloodlust
                if (Runes.Barbarian.BloodLust.IsActive && Player.CurrentHealthPct <= .25)
                    return true;

                // If lamentation is equipped, cast twice in a row and then wait
                if (Legendary.Lamentation.IsEquipped)
                {
                    var castsWithinTime = SpellHistory.SpellUseCountInTime(SNOPower.Barbarian_Rend, TimeSpan.FromMilliseconds(Settings.Combat.Barbarian.RendWaitDelay));

                    Logger.LogVerbose(LogCategory.Behavior, "Casts within {0}ms = {1}", Settings.Combat.Barbarian.RendWaitDelay, castsWithinTime);

                    if (hasReserveEnergy && QueuedPower != PowerRend && castsWithinTime == 0)
                    {
                        Logger.LogVerbose("Double Rend!");
                        QueuedPower = PowerRend;
                        return true;
                    }

                    return false;                     
                }

                return Skills.Barbarian.Rend.TimeSinceUse > Settings.Combat.Barbarian.RendWaitDelay && hasReserveEnergy;
            }
        }
        public static bool CanUseOverPower
        {
            get
            {
                if (CurrentTarget == null || Player.IsIncapacitated || Player.IsInTown || !CanCast(SNOPower.Barbarian_Overpower))
                    return false;

                var overPowerHasBuffEffect = (Runes.Barbarian.KillingSpree.IsActive ||
                                        Runes.Barbarian.CrushingAdvance.IsActive);

                if (!GetHasBuff(SNOPower.Barbarian_Overpower) && overPowerHasBuffEffect)
                    return true;

                return  CurrentTarget.RadiusDistance <= V.F("Barbarian.OverPower.MaxRange") && !overPowerHasBuffEffect &&
                        TargetUtil.AnyMobsInRange(V.F("Barbarian.OverPower.MaxRange")) &&
                        (CurrentTarget.IsEliteRareUnique || CurrentTarget.IsMinion || CurrentTarget.IsBoss);
            }
        }
        public static bool CanUseSeismicSlam
        {
            get
            {
                return !UseOOCBuff && !IsWaitingForSpecial && CanCast(SNOPower.Barbarian_SeismicSlam) && !Player.IsIncapacitated &&
                    (!Hotbar.Contains(SNOPower.Barbarian_BattleRage) || (Hotbar.Contains(SNOPower.Barbarian_BattleRage) && GetHasBuff(SNOPower.Barbarian_BattleRage))) &&
                    Player.PrimaryResource >= V.I("Barbarian.SeismicSlam.MinFury") && CurrentTarget.Distance <= V.F("Barbarian.SeismicSlam.CurrentTargetRange") &&
                    (TargetUtil.AnyMobsInRange(V.F("Barbarian.SeismicSlam.TrashRange")) ||
                     TargetUtil.IsEliteTargetInRange(V.F("Barbarian.SeismicSlam.EliteRange")));
            }
        }
        public static bool CanUseAncientSpear
        {
            get
            {
                return !UseOOCBuff && !IsWaitingForSpecial && !IsCurrentlyAvoiding && CanCast(SNOPower.X1_Barbarian_AncientSpear) && Player.PrimaryResource >= 25 &&
                    // Only boulder toss as a rage dump if we have excess resource
                    (!Runes.Barbarian.BoulderToss.IsActive || Player.PrimaryResourcePct > 0.8) &&
                    CurrentTarget.HitPointsPct >= V.F("Barbarian.AncientSpear.MinHealthPct") &&
                    Skills.Barbarian.AncientSpear.TimeSinceUse > Settings.Combat.Barbarian.AncientSpearWaitDelay;
            }
        }
        public static bool CanUseSprint
        {
            get
            {
                return Trinity.Settings.Combat.Barbarian.SprintMode != BarbarianSprintMode.MovementOnly &&
                    !UseOOCBuff && CanCast(SNOPower.Barbarian_Sprint, CanCastFlags.NoTimer) && !Player.IsIncapacitated &&
                    (
                        // last power used was whirlwind and we don't have sprint up
                        (LastPowerUsed == SNOPower.Barbarian_Whirlwind && !GetHasBuff(SNOPower.Barbarian_Sprint)) ||
                        // Fury Dump Options for sprint: use at max energy constantly
                        ShouldFuryDump ||
                        // or on a timer
                        (
                         (SNOPowerUseTimer(SNOPower.Barbarian_Sprint) && !GetHasBuff(SNOPower.Barbarian_Sprint)) &&
                         // Always keep up if we are whirlwinding, if the target is a goblin, or if we are more than 16 feet away from the target
                         (Hotbar.Contains(SNOPower.Barbarian_Whirlwind) || CurrentTarget.IsTreasureGoblin ||
                          (CurrentTarget.Distance >= V.F("Barbarian.Sprint.SingleTargetRange") && Player.PrimaryResource >= V.F("Barbarian.Sprint.SingleTargetMinFury"))
                         )
                        )
                    ) &&
                    // minimum time between uses
                    TimeSincePowerUse(SNOPower.Barbarian_Sprint) >= V.I("Barbarian.Sprint.MinUseDelay") &&
                    // If they have battle-rage, make sure it's up
                    (!Hotbar.Contains(SNOPower.Barbarian_BattleRage) || (Hotbar.Contains(SNOPower.Barbarian_BattleRage) && GetHasBuff(SNOPower.Barbarian_BattleRage))) &&
                    // Check for minimum energy
                    Player.PrimaryResource >= V.F("Barbarian.Sprint.MinFury");
            }
        }
        public static bool CanUseFrenzyTo5
        {
            get
            {
                return !UseOOCBuff && !IsCurrentlyAvoiding && !Player.IsRooted && Hotbar.Contains(SNOPower.Barbarian_Frenzy) &&
                    !TargetUtil.AnyMobsInRange(15f, 3) && GetBuffStacks(SNOPower.Barbarian_Frenzy) < 5;
            }
        }
        public static bool CanUseBashTo3
        {
            get
            {
                return !UseOOCBuff && !IsCurrentlyAvoiding && !Player.IsRooted && Hotbar.Contains(SNOPower.Barbarian_Bash) &&
                    Runes.Barbarian.Punish.IsActive && !TargetUtil.AnyMobsInRange(15f, 3) && GetBuffStacks(SNOPower.Barbarian_Bash) < 3;
            }
        }
        public static bool CanUseWhirlwind
        {
            get
            {
                var alwaysWhirlwind = Sets.BulKathossOath.IsFullyEquipped || Sets.WrathOfTheWastes.IsFullyEquipped;

                if (Player.PrimaryResource < 10 || !CanCast(SNOPower.Barbarian_Whirlwind))
                    return false;

                if (!alwaysWhirlwind && (UseOOCBuff || IsCurrentlyAvoiding || Player.IsIncapacitated || Player.IsRooted))
                    return false;

                return (CurrentTarget != null && CurrentTarget.RadiusDistance <= 25f || TargetUtil.AnyMobsInRange(V.F("Barbarian.Whirlwind.TrashRange"), V.I("Barbarian.Whirlwind.TrashCount"))) &&
                    // Check for energy reservation amounts
                    Player.PrimaryResource >= V.D("Barbarian.Whirlwind.MinFury") &&
                    // If they have battle-rage, make sure it's up
                    (!Hotbar.Contains(SNOPower.Barbarian_BattleRage) || GetHasBuff(SNOPower.Barbarian_BattleRage));
            }
        }

        public static bool CanUseHammerOfTheAncients
        {
            get
            {
                bool hotaresult = !UseOOCBuff && !IsCurrentlyAvoiding && !Player.IsIncapacitated && !IsWaitingForSpecial && CanCast(SNOPower.Barbarian_HammerOfTheAncients) &&
                    (Player.PrimaryResource >= V.F("Barbarian.HammerOfTheAncients.MinFury") || LastPowerUsed == SNOPower.Barbarian_HammerOfTheAncients) &&
                    (!Hotbar.Contains(SNOPower.Barbarian_Whirlwind) || (Player.CurrentHealthPct >= Settings.Combat.Barbarian.MinHotaHealth && Hotbar.Contains(SNOPower.Barbarian_Whirlwind)));

                if (Legendary.LutSocks.IsEquipped)
                {
                    return !CanUseLeap && hotaresult;
                }

                return hotaresult;

            }
        }
        public static bool CanUseHammerOfTheAncientsElitesOnly
        {
            get
            {
                bool canUseHota = CanUseHammerOfTheAncients;

                if (Legendary.LutSocks.IsEquipped)
                {
                    if (canUseHota)
                    {
                        bool hotaElites = (CurrentTarget.IsBossOrEliteRareUnique || CurrentTarget.IsTreasureGoblin) && TargetUtil.EliteOrTrashInRange(10f);

                        bool hotaTrash = IgnoringElites && CurrentTarget.IsTrashMob &&
                            (TargetUtil.EliteOrTrashInRange(6f) || CurrentTarget.MonsterSize == MonsterSize.Big);

                        return canUseHota && (hotaElites || hotaTrash);
                    }
                }
                else
                {
                    if (canUseHota && !CanUseLeap)
                    {
                        bool hotaElites = (CurrentTarget.IsBossOrEliteRareUnique || CurrentTarget.IsTreasureGoblin) && TargetUtil.EliteOrTrashInRange(10f);

                        bool hotaTrash = IgnoringElites && CurrentTarget.IsTrashMob &&
                            (TargetUtil.EliteOrTrashInRange(6f) || CurrentTarget.MonsterSize == MonsterSize.Big);

                        return canUseHota && (hotaElites || hotaTrash);
                    }
                }
                return false;
            }
        }
        public static bool CanUseWeaponThrow { get { return !UseOOCBuff && !IsCurrentlyAvoiding && CanCast(SNOPower.X1_Barbarian_WeaponThrow); } }
        public static bool CanUseFrenzy { get { return !UseOOCBuff && !IsCurrentlyAvoiding && CanCast(SNOPower.Barbarian_Frenzy); } }
        public static bool CanUseBash { get { return !UseOOCBuff && !IsCurrentlyAvoiding && CanCast(SNOPower.Barbarian_Bash); } }
        public static bool CanUseCleave { get { return !UseOOCBuff && !IsCurrentlyAvoiding && CanCast(SNOPower.Barbarian_Cleave); } }
        public static bool CanUseAvalanche
        {
            get
            {
                bool hasBerserker = CacheData.Hotbar.PassiveSkills.Any(p => p == SNOPower.Barbarian_Passive_BerserkerRage);
                double minFury = hasBerserker ? Player.PrimaryResourceMax * 0.99 : 0f;

                return !UseOOCBuff && !IsCurrentlyAvoiding && CanCast(SNOPower.X1_Barbarian_Avalanche_v2, CanCastFlags.NoTimer) &&
                    Player.PrimaryResource >= minFury && (TargetUtil.AnyMobsInRange(3) || TargetUtil.IsEliteTargetInRange());

            }
        }


        public static TrinityPower PowerAvalanche { get { return new TrinityPower(SNOPower.X1_Barbarian_Avalanche_v2, 15f, TargetUtil.GetBestClusterUnit(15f, 45f).Position); } }
        public static TrinityPower PowerIgnorePain { get { return new TrinityPower(SNOPower.Barbarian_IgnorePain); } }
        public static TrinityPower PowerEarthquake
        {
            get
            {
                return new TrinityPower(SNOPower.Barbarian_Earthquake);
            }
        }
        public static TrinityPower PowerWrathOfTheBerserker { get { return new TrinityPower(SNOPower.Barbarian_WrathOfTheBerserker); } }
        public static TrinityPower PowerCallOfTheAncients { get { return new TrinityPower(SNOPower.Barbarian_CallOfTheAncients, V.I("Barbarian.CallOfTheAncients.TickDelay"), V.I("Barbarian.CallOfTheAncients.TickDelay")); } }
        public static TrinityPower PowerBattleRage { get { return new TrinityPower(SNOPower.Barbarian_BattleRage); } }
        public static TrinityPower PowerSprint { get { return new TrinityPower(SNOPower.Barbarian_Sprint, 0, 0); } }
        public static TrinityPower PowerWarCry { get { return new TrinityPower(SNOPower.X1_Barbarian_WarCry_v2); } }
        public static TrinityPower PowerThreateningShout { get { return new TrinityPower(SNOPower.Barbarian_ThreateningShout); } }
        public static TrinityPower PowerGroundStomp { get { return new TrinityPower(SNOPower.Barbarian_GroundStomp); } }
        public static TrinityPower PowerRevenge { get { return new TrinityPower(SNOPower.Barbarian_Revenge); } }

        public static TrinityPower PowerFuriousCharge
        {
            get
            {
                var bestTarget = TargetUtil.GetBestPierceTarget(MaxFuriousChargeDistance);

                if (bestTarget != null && Sets.TheLegacyOfRaekor.IsMaxBonusActive)
                    return new TrinityPower(SNOPower.Barbarian_FuriousCharge, V.F("Barbarian.FuriousCharge.UseRange"), bestTarget.Position);
                return new TrinityPower(SNOPower.Barbarian_FuriousCharge, V.F("Barbarian.FuriousCharge.UseRange"), CurrentTarget.Position);
            }
        }
        public static TrinityPower PowerLeap
        {
            get
            {
                // For Call of Arreat rune. Will do all quakes on top of each other
                if (Legendary.LutSocks.IsEquipped && Runes.Barbarian.CallOfArreat.IsActive)
                {
                    Vector3 aoeTarget = TargetUtil.GetBestClusterPoint(7f, 9f, false);
                    return new TrinityPower(SNOPower.Barbarian_Leap, V.F("Barbarian.Leap.UseRange"), aoeTarget);
                }
                else
                {
                    Vector3 aoeTarget = TargetUtil.GetBestClusterPoint(15f, 35f, false);
                    return new TrinityPower(SNOPower.Barbarian_Leap, V.F("Barbarian.Leap.UseRange"), aoeTarget);
                }
            }
        }
        public static TrinityPower PowerRend { get { return new TrinityPower(SNOPower.Barbarian_Rend, V.I("Barbarian.Rend.TickDelay"), V.I("Barbarian.Rend.TickDelay")); } }
        public static TrinityPower PowerOverpower { get { return new TrinityPower(SNOPower.Barbarian_Overpower); } }

        public static TrinityPower PowerSeismicSlam
        {
            get
            {
                var target = TargetUtil.UnitsPlayerFacing(20f) >= 6 ? TargetUtil.GetClosestUnit(12f) : TargetUtil.GetBestClusterUnit(12f, 12f);

                if (target != null)
                    return new TrinityPower(SNOPower.Barbarian_SeismicSlam, V.F("Barbarian.HammerOfTheAncients.UseRange"), target.Position);
                return new TrinityPower(SNOPower.Barbarian_SeismicSlam, V.F("Barbarian.HammerOfTheAncients.UseRange"), CurrentTarget.Position);
            }
        }

    public static TrinityPower PowerAncientSpear
        {
            get
            {
                var bestAoEUnit = TargetUtil.GetBestPierceTarget(35f);

                if (Runes.Barbarian.BoulderToss.IsActive)
                    bestAoEUnit = TargetUtil.GetBestClusterUnit(9f);

                return new TrinityPower(SNOPower.X1_Barbarian_AncientSpear, V.F("Barbarian.AncientSpear.UseRange"), bestAoEUnit.ACDGuid);
            }
        }
        public static TrinityPower PowerWhirlwind
        {
            get
            {
                return new TrinityPower(SNOPower.Barbarian_Whirlwind, 20f, TargetUtil.GetZigZagTarget(CurrentTarget.Position, 20), Trinity.CurrentWorldDynamicId, -1, 0, 1);
            }
        }

        public static TrinityPower PowerHammerOfTheAncients
        {
            get
            {
                var target = TargetUtil.UnitsPlayerFacing(20f) >= 6 ? TargetUtil.GetClosestUnit(15f) : TargetUtil.GetBestClusterUnit(15f, 15f);

                if (target != null)
                    return new TrinityPower(SNOPower.Barbarian_HammerOfTheAncients, V.F("Barbarian.HammerOfTheAncients.UseRange"), target.Position);
                return new TrinityPower(SNOPower.Barbarian_HammerOfTheAncients, V.F("Barbarian.HammerOfTheAncients.UseRange"), CurrentTarget.Position);
            }
        }
        public static TrinityPower PowerWeaponThrow { get { return new TrinityPower(SNOPower.X1_Barbarian_WeaponThrow, V.F("Barbarian.WeaponThrow.UseRange"), CurrentTarget.ACDGuid); } }
        public static TrinityPower PowerFrenzy { get { return new TrinityPower(SNOPower.Barbarian_Frenzy, V.F("Barbarian.Frenzy.UseRange"), CurrentTarget.ACDGuid); } }
        public static TrinityPower PowerBash { get { return new TrinityPower(SNOPower.Barbarian_Bash, V.F("Barbarian.Bash.UseRange"), CurrentTarget.ACDGuid); } }
        public static TrinityPower PowerCleave { get { return new TrinityPower(SNOPower.Barbarian_Cleave, V.F("Barbarian.Cleave.UseRange"), CurrentTarget.ACDGuid); } }

        private static TrinityPower DestroyObjectPower
        {
            get
            {
                if (CanCast(SNOPower.Barbarian_Frenzy))
                    return new TrinityPower(SNOPower.Barbarian_Frenzy, 4f);

                if (CanCast(SNOPower.Barbarian_Bash))
                    return new TrinityPower(SNOPower.Barbarian_Bash, 4f);

                if (CanCast(SNOPower.Barbarian_Cleave))
                    return new TrinityPower(SNOPower.Barbarian_Cleave, 4f);

                if (CanCast(SNOPower.X1_Barbarian_WeaponThrow))
                    return new TrinityPower(SNOPower.X1_Barbarian_WeaponThrow, 4f);

                if (CanCast(SNOPower.Barbarian_Overpower))
                    return new TrinityPower(SNOPower.Barbarian_Overpower, 9);

                if (CanCast(SNOPower.Barbarian_Whirlwind))
                    return new TrinityPower(SNOPower.Barbarian_Whirlwind, V.F("Barbarian.Whirlwind.UseRange"), CurrentTarget.Position);

                if (CanCast(SNOPower.Barbarian_Rend) && Player.PrimaryResourcePct >= 0.65)
                    return new TrinityPower(SNOPower.Barbarian_Rend, V.F("Barbarian.Rend.UseRange"));

                return DefaultPower;
            }
        }

        public static bool AllowSprintOOC
        {
            get { return _allowSprintOoc; }
            set { _allowSprintOoc = value; }
        }

        private static bool ShouldFuryDump
        {
            get
            {
                bool ignoranceIsBliss = Runes.Barbarian.IgnoranceIsBliss.IsActive && GetHasBuff(SNOPower.Barbarian_IgnorePain);
                bool lifePerFuryIK = Sets.ImmortalKingsCall.IsMaxBonusActive && (Legendary.TheGavelOfJudgment.IsEquipped || Legendary.FuryOfTheVanishedPeak.IsEquipped);

                return ((lifePerFuryIK || ignoranceIsBliss) && Trinity.Player.CurrentHealthPct <= 1 &&
                        ((Settings.Combat.Barbarian.FuryDumpWOTB && Player.PrimaryResourcePct >= V.F("Barbarian.WOTB.FuryDumpMin") && GetHasBuff(SNOPower.Barbarian_WrathOfTheBerserker)) ||
                         Settings.Combat.Barbarian.FuryDumpAlways && Player.PrimaryResourcePct >= V.F("Barbarian.WOTB.FuryDumpMin")));
            }
        }


    }
}
