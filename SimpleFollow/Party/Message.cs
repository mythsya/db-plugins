using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QuestTools;
using SimpleFollow.Behaviors;
using SimpleFollow.Helpers;
using Zeta.Bot;
using Zeta.Bot.Logic;
using Zeta.Bot.Profile;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.Service;

namespace SimpleFollow.Party
{
    public class Message : IEquatable<Message>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Message)obj);
        }

        public int WorldId { get; set; }
        public int LevelAreaId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 ProfilePosition { get; set; }
        public int ProfileWaypointNumber { get; set; }
        public int ProfileActorSNO { get; set; }
        public float ProfilePathPrecision { get; set; }
        public bool IsInCombat { get; set; }
        public bool IsInTown { get; set; }
        public bool IsInGame { get; set; }
        public bool IsLoadingWorld { get; set; }
        public bool IsInParty { get; set; }
        public int ActorSNO { get; set; }
        public ActorClass ActorClass { get; set; }
        public double HitpointsMaxTotal { get; set; }
        public double HitpointsCurrent { get; set; }
        public GameId GameId { get; set; }
        public DateTime LastTimeInGame { get; set; }
        public DateTime LastTimeUpdated { get; set; }
        public int BattleTagHash { get; set; }
        public string ProfileTagName { get; set; }
        public int NumPartymembers { get; set; }
        public int CPlayerIndex { get; set; }

        public bool HasRiftKeys { get; set; }
        public int HighestLevelTieredRiftKey { get; set; }
        public bool IsInGreaterRift { get; set; }

        /* QuestTools Settings */
        public float MinimumGemChance { get; set; }
        public bool UseHighestKeystone { get; set; }
        public bool EnableLimitRiftLevel { get; set; }
        public int LimitRiftLevel { get; set; }
        public bool EnableTrialRiftMaxLevel { get; set; }
        public int TrialRiftMaxLevel { get; set; }
        public List<RiftKeyUsePriority> RiftKeyPriority { get; set; }

        /// <summary>
        /// Gets or sets the highest team rift key. This is the lowest common denominator of all rift keys held by party members.
        /// </summary>
        /// <value>The highest team rift key.</value>
        public int HighestTeamRiftKey { get; set; }

        public bool RequestOpenRift { get; set; }

        public bool IsInSameGame
        {
            get
            {
                if (!ZetaDia.IsInGame)
                    return false;

                return ZetaDia.Service.CurrentGameId == GameId;
            }
        }

        public bool IsVendoring { get; set; }

        public Message()
        {
            ActorClass = ActorClass.Invalid;
            LastTimeUpdated = DateTime.MinValue;
            IsInGame = false;
            IsLoadingWorld = false;
            Position = Vector3.Zero;
            ProfilePosition = Vector3.Zero;
            ProfilePathPrecision = 10f;
            IsInCombat = false;
            IsInTown = false;
            CPlayerIndex = -1;
        }

        /// <summary>
        /// Used by leaders and followers to pass updates
        /// </summary>
        /// <returns></returns>
        public static Message GetMessage()
        {
            try
            {
                Message m;

                if (!ZetaDia.Service.IsValid || !ZetaDia.Service.Platform.IsConnected)
                {
                    m = new Message
                    {
                        LastTimeUpdated = DateTime.UtcNow,
                        IsInGame = false
                    };
                    return m;
                }
                if (ZetaDia.IsInGame && !ZetaDia.IsLoadingWorld && ZetaDia.Me.IsFullyValid())
                {
                    m = new Message
                    {
                        LastTimeUpdated = DateTime.UtcNow,
                        LastTimeInGame = DateTime.UtcNow,
                        IsInGame = ZetaDia.IsInGame,
                        BattleTagHash = ZetaDia.Service.Hero.BattleTagName.GetHashCode(),
                        IsInParty = Social.IsInParty,
                        NumPartymembers = Social.NumPartyMembers,
                        IsLoadingWorld = ZetaDia.IsLoadingWorld,
                        ActorClass = ZetaDia.Me.ActorClass,
                        ActorSNO = ZetaDia.Me.ActorSNO,
                        GameId = ZetaDia.Service.CurrentGameId,
                        HitpointsCurrent = ZetaDia.Me.HitpointsCurrent,
                        HitpointsMaxTotal = ZetaDia.Me.HitpointsMaxTotal,
                        LevelAreaId = Player.LevelAreaId,
                        IsInTown = Player.LevelAreaId != 55313 && ZetaDia.IsInTown, // A2 Caldeum Bazaar
                        Position = ZetaDia.Me.Position,
                        ProfilePosition = GetProfilePosition(),
                        ProfileActorSNO = GetProfileActorSNO(),
                        ProfilePathPrecision = GetProfilePathPrecision(),
                        ProfileWaypointNumber = GetProfileWaypointNumber(),
                        ProfileTagName = GetProfileTagname(),
                        IsInCombat = GetIsInCombat(),
                        WorldId = ZetaDia.CurrentWorldId,
                        IsVendoring = BrainBehavior.IsVendoring,
                        IsInGreaterRift = Player.IsInGreaterRift,
                        CPlayerIndex = ZetaDia.CPlayer.Index,
                    };

                    if (m.IsInTown)
                    {
                        List<ACDItem> riftKeys = ZetaDia.Me.Inventory.StashItems.Where(i => i.IsValid && i.ItemType == ItemType.KeystoneFragment).ToList();
                        riftKeys.AddRange(ZetaDia.Me.Inventory.Backpack.Where(i => i.IsValid && i.GoodFood == 0xCEFAEDFE && i.ItemType == ItemType.KeystoneFragment).ToList());
                        m.HasRiftKeys = riftKeys.Any();
                        int maxLevel = -1;
                        try
                        {
                            // m.HighestLevelTieredRiftKey = riftKeys.Any() ? riftKeys.Max(i => i.TieredLootRunKeyLevel) : -1;
                            foreach (var key in riftKeys)
                            {
                                try
                                {
                                    int level = key.TieredLootRunKeyLevel;
                                    if (level > maxLevel)
                                        maxLevel = level;
                                }
                                catch { 
                                    // Tell me you love me
                                }
                            }
                        }
                        catch
                        {
                            // You lose!!
                        }
                        m.HighestLevelTieredRiftKey = maxLevel;

                        if (SimpleFollow.IsLeader)
                        {
                            m.RequestOpenRift = LeaderComposite.ShouldFollowerOpenRift();
                            m.RiftKeyPriority = QuestToolsSettings.Instance.RiftKeyPriority;
                            m.UseHighestKeystone = QuestToolsSettings.Instance.UseHighestKeystone;
                            m.MinimumGemChance = QuestToolsSettings.Instance.MinimumGemChance;
                            m.EnableLimitRiftLevel = QuestToolsSettings.Instance.EnableLimitRiftLevel;
                            m.LimitRiftLevel = QuestToolsSettings.Instance.LimitRiftLevel;
                            m.EnableTrialRiftMaxLevel = QuestToolsSettings.Instance.EnableTrialRiftMaxLevel;
                            m.TrialRiftMaxLevel = QuestToolsSettings.Instance.TrialRiftMaxLevel;
                        }
                    }
                }
                else if (ZetaDia.IsInGame && ZetaDia.IsLoadingWorld)
                {
                    m = new Message
                    {
                        IsInGame = true,
                        IsLoadingWorld = true,
                        GameId = ZetaDia.Service.CurrentGameId,
                        LastTimeInGame = DateTime.UtcNow,
                        BattleTagHash = ZetaDia.Service.Hero.BattleTagName.GetHashCode(),
                        IsInTown = false,
                        WorldId = -1,
                        LevelAreaId = -1,
                        LastTimeUpdated = DateTime.UtcNow,
                        IsInParty = Social.IsInParty,
                        NumPartymembers = Social.NumPartyMembers,
                        ActorClass = ZetaDia.Service.Hero.Class,
                    };
                }
                else
                {
                    m = new Message
                    {
                        IsInGame = false,
                        IsInTown = false,
                        BattleTagHash = ZetaDia.Service.Hero.BattleTagName.GetHashCode(),
                        WorldId = -1,
                        LastTimeUpdated = DateTime.UtcNow,
                        IsInParty = Social.IsInParty,
                        NumPartymembers = Social.NumPartyMembers,
                        ActorClass = ZetaDia.Service.Hero.Class,
                    };
                }
                return m;
            }
            catch (Exception ex)
            {
                Logr.Log("Exception in GetMessage() {0}", ex);
                return new Message();
            }
        }

        public double GetMillisecondsSinceLastUpdate()
        {
            return DateTime.UtcNow.Subtract(LastTimeUpdated).TotalMilliseconds;
        }

        public override string ToString()
        {
            return ToStringReflector.GetObjectString(this);

            //return String.Format(
            //    "Message: WorldID: {0} LevelAreaId: {1} Position: {2} IsInTown: {3} IsInGame: {4} ActorSNO: {5} ActorClass: {6}" +
            //    "Hitpointsmax: {7} HitpointsCurrent: {8} GameId: {9} LastUpdated: {10} IsVendoring: {11} IsLoadingWorld: {12} " +
            //    "ProfileTagName: {13} Class={14} IsInParty={15} BattleTagHash={16}",
            //    WorldId,
            //    LevelAreaId,
            //    Position,
            //    IsInTown,
            //    IsInGame,
            //    ActorSNO,
            //    ActorClass,
            //    HitpointsMaxTotal,
            //    HitpointsCurrent,
            //    GameId,
            //    LastTimeUpdated,
            //    IsVendoring,
            //    IsLoadingWorld,
            //    ProfileTagName,
            //    ActorClass,
            //    IsInParty,
            //    BattleTagHash);
        }


        public static Vector3 GetProfilePosition()
        {
            if (!ZetaDia.IsInGame)
                return Vector3.Zero;

            if (ProfileManager.CurrentProfileBehavior == null)
                return Vector3.Zero;

            ProfileBehavior currentBehavior = ProfileManager.CurrentProfileBehavior;

            Vector3 pos = Vector3.Zero;

            if (currentBehavior != null)
            {
                foreach (PropertyInfo pi in currentBehavior.GetType().GetProperties().ToList())
                {
                    if (pi.Name == "X")
                        pos.X = (float)pi.GetValue(currentBehavior, null);
                    if (pi.Name == "Y")
                        pos.Y = (float)pi.GetValue(currentBehavior, null);
                    if (pi.Name == "Z")
                        pos.Z = (float)pi.GetValue(currentBehavior, null);
                }
            }

            return pos;
        }

        public static int GetProfileWaypointNumber()
        {
            if (!ZetaDia.IsInGame)
                return -1;

            if (ProfileManager.CurrentProfileBehavior == null)
                return -1;

            ProfileBehavior currentBehavior = ProfileManager.CurrentProfileBehavior;

            int id = -1;

            if (currentBehavior == null)
                return id;
            foreach (PropertyInfo pi in currentBehavior.GetType().GetProperties().ToList().Where(pi => pi.Name == "waypointnumber"))
            {
                id = (int)pi.GetValue(currentBehavior, null);
            }

            return id;
        }

        public static int GetProfileActorSNO()
        {
            if (!ZetaDia.IsInGame)
                return -1;

            if (ProfileManager.CurrentProfileBehavior == null)
                return -1;

            ProfileBehavior currentBehavior = ProfileManager.CurrentProfileBehavior;

            int id = -1;

            if (currentBehavior == null)
                return id;
            foreach (PropertyInfo pi in currentBehavior.GetType().GetProperties().ToList().Where(pi => pi.Name == "actorid"))
            {
                id = (int)pi.GetValue(currentBehavior, null);
            }

            return id;
        }

        public static float GetProfilePathPrecision()
        {
            float pathPrecision = 10f;

            if (!ZetaDia.IsInGame)
                return pathPrecision;

            if (ProfileManager.CurrentProfileBehavior == null)
                return pathPrecision;

            ProfileBehavior currentBehavior = ProfileManager.CurrentProfileBehavior;

            if (currentBehavior == null)
                return pathPrecision;
            foreach (PropertyInfo pi in currentBehavior.GetType().GetProperties().ToList())
            {
                object val = null;
                if (pi.Name == "PathPrecision")
                    val = pi.GetValue(currentBehavior, null);

                if (val != null && val is float)
                    pathPrecision = (float)val;
            }
            return pathPrecision;
        }

        public static bool GetIsInCombat()
        {
            if (ZetaDia.Me.IsInCombat)
                return true;

            if (CombatTargeting.Instance == null)
                return false;

            if (CombatTargeting.Instance.FirstObject == null)
                return false;

            if (CombatTargeting.Instance.FirstObject == null)
                return false;

            if (CombatTargeting.Instance.FirstObject.IsValid && CombatTargeting.Instance.FirstObject.ActorType == Zeta.Game.Internals.SNO.ActorType.Monster)
                return true;

            return false;
        }

        public static string GetProfileTagname()
        {
            string name = string.Empty;

            if (ProfileManager.CurrentProfileBehavior != null)
            {
                name = ProfileManager.CurrentProfileBehavior.GetType().ToString();
            }

            return name;
        }

        public bool IsLeavingGame
        {
            get
            {
                if (ProfileTagName == null)
                    return false;

                return ProfileTagName.ToLower().Contains("leavegame");
            }
        }

        public bool IsTownPortalling
        {
            get
            {
                if (ProfileTagName == null)
                    return false;

                return ProfileTagName.ToLower().Contains("town");
            }
        }

        public bool IsTakingPortalBack
        {
            get
            {
                if (ProfileTagName == null)
                    return false;

                return ProfileTagName.ToLower().Contains("usetownportal");
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = WorldId;
                hashCode = (hashCode * 397) ^ LevelAreaId;
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ ProfilePosition.GetHashCode();
                hashCode = (hashCode * 397) ^ ProfileActorSNO;
                hashCode = (hashCode * 397) ^ ProfilePathPrecision.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInCombat.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInTown.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInGame.GetHashCode();
                hashCode = (hashCode * 397) ^ IsLoadingWorld.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInParty.GetHashCode();
                hashCode = (hashCode * 397) ^ ActorSNO;
                hashCode = (hashCode * 397) ^ (int)ActorClass;
                hashCode = (hashCode * 397) ^ HitpointsMaxTotal.GetHashCode();
                hashCode = (hashCode * 397) ^ HitpointsCurrent.GetHashCode();
                hashCode = (hashCode * 397) ^ GameId.GetHashCode();
                hashCode = (hashCode * 397) ^ LastTimeInGame.GetHashCode();
                hashCode = (hashCode * 397) ^ LastTimeUpdated.GetHashCode();
                hashCode = (hashCode * 397) ^ BattleTagHash;
                hashCode = (hashCode * 397) ^ (ProfileTagName != null ? ProfileTagName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ NumPartymembers;
                hashCode = (hashCode * 397) ^ CPlayerIndex;
                hashCode = (hashCode * 397) ^ HasRiftKeys.GetHashCode();
                hashCode = (hashCode * 397) ^ HighestLevelTieredRiftKey;
                hashCode = (hashCode * 397) ^ IsInGreaterRift.GetHashCode();
                hashCode = (hashCode * 397) ^ HighestTeamRiftKey;
                hashCode = (hashCode * 397) ^ RequestOpenRift.GetHashCode();
                hashCode = (hashCode * 397) ^ (RiftKeyPriority != null ? RiftKeyPriority.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UseHighestKeystone.GetHashCode();
                hashCode = (hashCode * 397) ^ IsVendoring.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Message message1, Message message2)
        {
            if (ReferenceEquals(message1, null))
            {
                return ReferenceEquals(message2, null);
            }
            return message1.Equals(message2);
        }

        public static bool operator !=(Message message1, Message message2)
        {
            return !(message1 == message2);
        }

        public bool Equals(Message other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return WorldId == other.WorldId && LevelAreaId == other.LevelAreaId && Position.Equals(other.Position) && ProfilePosition.Equals(other.ProfilePosition) && ProfileActorSNO == other.ProfileActorSNO && ProfilePathPrecision.Equals(other.ProfilePathPrecision) && IsInCombat.Equals(other.IsInCombat) && IsInTown.Equals(other.IsInTown) && IsInGame.Equals(other.IsInGame) && IsLoadingWorld.Equals(other.IsLoadingWorld) && IsInParty.Equals(other.IsInParty) && ActorSNO == other.ActorSNO && ActorClass == other.ActorClass && HitpointsMaxTotal.Equals(other.HitpointsMaxTotal) && HitpointsCurrent.Equals(other.HitpointsCurrent) && GameId.Equals(other.GameId) && LastTimeInGame.Equals(other.LastTimeInGame) && LastTimeUpdated.Equals(other.LastTimeUpdated) && BattleTagHash == other.BattleTagHash && string.Equals(ProfileTagName, other.ProfileTagName) && NumPartymembers == other.NumPartymembers && CPlayerIndex == other.CPlayerIndex && HasRiftKeys.Equals(other.HasRiftKeys) && HighestLevelTieredRiftKey == other.HighestLevelTieredRiftKey && IsInGreaterRift.Equals(other.IsInGreaterRift) && HighestTeamRiftKey == other.HighestTeamRiftKey && RequestOpenRift.Equals(other.RequestOpenRift) && Equals(RiftKeyPriority, other.RiftKeyPriority) && UseHighestKeystone.Equals(other.UseHighestKeystone) && IsVendoring.Equals(other.IsVendoring);
        }
    }
}