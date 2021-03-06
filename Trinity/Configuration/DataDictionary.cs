using System;
using System.Collections.Generic;
using Trinity.Objects;
using Trinity.Reference;
using Zeta.Common;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.Game.Internals.SNO;

namespace Trinity
{
    /// <summary>
    /// Contains hard-coded meta-lists of ActorSNO's, Spells and other non-dynamic info
    /// </summary>
    internal partial class DataDictionary
    {

        public static HashSet<int> ArchonSkillIds = new HashSet<int>
        {
            135166, 135238, 135663, 166616, 167355, 167648, 392883, 392884, 392885, 392886, 392887, 392888, 392889, 392890, 392891,
        };



        /// <summary>
        /// Monsters who pretend to be dead/hidden
        /// </summary>
        public static readonly HashSet<int> FakeDeathMonsters = new HashSet<int>()
        {
            //morluMelee_A
            4755,
            //morluMelee_B
            4757,
            //morluMelee_C
            4758,
            //bloodScratch_morluMelee_attack01
            //219793,
            //bloodScratch_morluMelee_attack02
            //219808,
            //morluMelee_A_Unique_01
            219925,
            //morluMelee_A_Unique_02
            219936,
            //morluMelee_asheyState_glowSphere
            //221119,
            //morluMelee_B_asheyState_glowSphere
            //221120,
            //x1_Spawner_MorluMelee_A_Challenge
            //307092,
            //x1_Spawner_MorluMelee_C_Ressurect
            //344038,
            //x1_Event_SpeedKill_morluMelee_B_Spawner
            //370327,
            //P2_morluMelee_A_Unique_01
            409614,
            //P2_morluMelee_A_Unique_02
            409843,
        };

        public const int WALLER_SNO = 226808; //monsterAffix_waller_model (226808)

        public const int PLAYER_HEADSTONE_SNO = 4860; // PlayerHeadstone

        public static HashSet<int> PandemoniumFortressWorlds { get { return _pandemoniumFortressWorlds; } }
        private static readonly HashSet<int> _pandemoniumFortressWorlds = new HashSet<int>
        {
            271233, // Adventure Pand Fortress 1
            271235, // Adventure Pand Fortress 2
        };

        public static HashSet<int> PandemoniumFortressLevelAreaIds { get { return _pandemoniumFortressLevelAreaIds; } }
        private static readonly HashSet<int> _pandemoniumFortressLevelAreaIds = new HashSet<int>
        {
            333758, //LevelArea: X1_LR_Tileset_Fortress
        };

        public static HashSet<int> NoCheckKillRange { get { return _noCheckKillRange; } }
        private static readonly HashSet<int> _noCheckKillRange = new HashSet<int>
        {
            210120, // A4 Corrupt Growth
            210268, // A4 Corrupt Growth
        };

        public const int RiftTrialLevelAreaId = 405915;

        /// <summary>
        /// Contains a list of Rift WorldId's
        /// </summary>
        public static List<int> RiftWorldIds { get { return DataDictionary.riftWorldIds; } }
        private static readonly List<int> riftWorldIds = new List<int>()
        {
            288454,
            288685,
            288687,
            288798,
            288800,
            288802,
            288804,
            288806,
        };

        /// <summary>
        /// Contains all the Exit Name Hashes in Rifts
        /// </summary>
        public static List<int> RiftPortalHashes { get { return DataDictionary.riftPortalHashes; } }
        private static readonly List<int> riftPortalHashes = new List<int>()
        {
            1938876094,
            1938876095,
            1938876096,
            1938876097,
            1938876098,
            1938876099,
            1938876100,
            1938876101,
            1938876102,
        };

        public static HashSet<int> BountyTurnInQuests { get { return DataDictionary.bountyTurnInQuests; } }
        private static readonly HashSet<int> bountyTurnInQuests = new HashSet<int>()
        {
            356988, //x1_AdventureMode_BountyTurnin_A1 
            356994, //x1_AdventureMode_BountyTurnin_A2 
            356996, //x1_AdventureMode_BountyTurnin_A3 
            356999, //x1_AdventureMode_BountyTurnin_A4 
            357001, //x1_AdventureMode_BountyTurnin_A5 
        };

        public static HashSet<int> EventQuests { get { return DataDictionary.eventQuests; } }
        private static readonly HashSet<int> eventQuests = new HashSet<int>()
        {
            365821, // [D7499CC] Quest: x1_Catacombs_NS_06Mutant_Evant, QuestSNO: 365821, QuestMeter: -1, QuestState: InProgress, QuestStep: 10, KillCount: 0, BonusCount: 0 
            369381, // [2ECD96F4] Quest: x1_Event_Horde_HunterKillers, QuestSNO: 369381, QuestMeter: 0.004814815, QuestState: InProgress, QuestStep: 14, KillCount: 0, BonusCount: 0
            369431, // [2ECD9860] Quest: x1_Event_WaveFight_AncientEvils, QuestSNO: 369431, QuestMeter: -1, QuestState: InProgress, QuestStep: 13, KillCount: 0, BonusCount: 0
            336293, // [417DD860] Quest: X1_Graveyard_GraveRobber_Event, QuestSNO: 336293, QuestMeter: -1, QuestState: InProgress, QuestStep: 46, KillCount: 0, BonusCount: 0
            369414, // [33955B38] Quest: X1_Pand_Ext_ForgottenWar_Adventure, QuestSNO: 369414, QuestMeter: -1, QuestState: InProgress, QuestStep: 2, KillCount: 1, BonusCount: 0

            368306, // x1_Event_Horde_ArmyOfHell,
            369332, // x1_Event_Horde_Bonepit,
            365252, // x1_Event_Horde_DeathCellar,
            365150, // x1_Event_Horde_Deathfire,
            365695, // x1_Event_Horde_DesertFortress,
            367979, // x1_Event_Horde_Dustbowl,
            364880, // x1_Event_Horde_FleshpitGrove,
            369525, // x1_Event_Horde_FlyingAssasins,
            365796, // x1_Event_Horde_FoulHatchery,
            365305, // x1_Event_Horde_GhoulSwarm,
            369366, // x1_Event_Horde_GuardSlaughter,
            369381, // x1_Event_Horde_HunterKillers,
            368035, // x1_Event_Horde_InfernalSky,
            // 365269, // x1_Event_Horde_SpiderTrap,
            366331, // x1_Event_Horde_UdderChaos,
            239301, // x1_Event_Jar_Of_Souls_NecroVersion,
            370334, // x1_Event_SpeedKill_Angel_Corrupt_A,
            370316, // x1_Event_SpeedKill_BileCrawler_A,
            369841, // x1_Event_SpeedKill_Bloodhawk_A,
            370556, // x1_Event_SpeedKill_Boss_Adria,
            370373, // x1_Event_SpeedKill_Boss_Despair,
            370154, // x1_Event_SpeedKill_Boss_Ghom,
            369892, // x1_Event_SpeedKill_Boss_Maghda,
            365630, // x1_Event_SpeedKill_Boss_SkeletonKing,
            370349, // x1_Event_SpeedKill_Champion_BigRed_A,
            370082, // x1_Event_SpeedKill_Champion_FallenHound_D,
            369895, // x1_Event_SpeedKill_Champion_FleshPitFlyer_C,
            365586, // x1_Event_SpeedKill_Champion_GhostA,
            365593, // x1_Event_SpeedKill_Champion_GoatmanB,
            370364, // x1_Event_SpeedKill_Champion_MalletDemon_A,
            369906, // x1_Event_SpeedKill_Champion_SandShark_A,
            370135, // x1_Event_SpeedKill_Champion_SoulRipper_A,
            370837, // x1_Event_SpeedKill_Champion_SquiggletA,
            365617, // x1_Event_SpeedKill_Champion_SummonableA,
            370077, // x1_Event_SpeedKill_Champion_azmodanBodyguard_A,
            370066, // x1_Event_SpeedKill_Champion_creepMob_A,
            370341, // x1_Event_SpeedKill_Champion_morluSpellcaster_A,
            370516, // x1_Event_SpeedKill_Champion_x1_FloaterAngel_A,
            370544, // x1_Event_SpeedKill_Champon_x1_Rockworm_Pand_A,
            370320, // x1_Event_SpeedKill_CoreEliteDemon_A,
            370038, // x1_Event_SpeedKill_Fallen_C,
            365551, // x1_Event_SpeedKill_GhostHumansA,
            370053, // x1_Event_SpeedKill_Ghoul_E,
            364644, // x1_Event_SpeedKill_GoatmanA,
            365509, // x1_Event_SpeedKill_Goatman_Melee_A_Ghost,
            370044, // x1_Event_SpeedKill_Goatmutant_B,
            369873, // x1_Event_SpeedKill_Lacuni_B,
            370049, // x1_Event_SpeedKill_Monstrosity_Scorpion_A,
            369910, // x1_Event_SpeedKill_Rare_Ghoul_B,
            365622, // x1_Event_SpeedKill_Rare_Skeleton2HandA,
            370147, // x1_Event_SpeedKill_Rare_ThousandPounder,
            370359, // x1_Event_SpeedKill_Rare_demonTrooper_C,
            370499, // x1_Event_SpeedKill_Rare_x1_westmarchBrute_C,
            370060, // x1_Event_SpeedKill_Skeleton_E,
            364635, // x1_Event_SpeedKill_SkeletonsA,
            369856, // x1_Event_SpeedKill_Snakeman_A,
            369884, // x1_Event_SpeedKill_Spiderling_B,
            369863, // x1_Event_SpeedKill_Swarm_A,
            370666, // x1_Event_SpeedKill_TentacleBears,
            369832, // x1_Event_SpeedKill_TriuneCultist_C,
            365526, // x1_Event_SpeedKill_TriuneVesselA,
            365547, // x1_Event_SpeedKill_ZombieB,
            370033, // x1_Event_SpeedKill_demonFlyer_B,
            369817, // x1_Event_SpeedKill_electricEel_A,
            369837, // x1_Event_SpeedKill_fastMummy_A,
            370329, // x1_Event_SpeedKill_morluMelee_B,
            370482, // x1_Event_SpeedKill_x1_BileCrawler_Skeletal_A,
            370435, // x1_Event_SpeedKill_x1_BogFamily_A,
            370452, // x1_Event_SpeedKill_x1_Monstrosity_ScorpionBug_A,
            370427, // x1_Event_SpeedKill_x1_Skeleton_Ghost_A,
            370561, // x1_Event_SpeedKill_x1_Tentacle_A,
            370445, // x1_Event_SpeedKill_x1_bogBlight_Maggot_A,
            370466, // x1_Event_SpeedKill_x1_leaperAngel_A,
            370489, // x1_Event_SpeedKill_x1_portalGuardianMinion_A,
            370476, // x1_Event_SpeedKill_x1_westmarchHound_A,
            369431, // x1_Event_WaveFight_AncientEvils,
            365751, // x1_Event_WaveFight_ArmyOfTheDead,
            368092, // x1_Event_WaveFight_BloodClanAssault,
            365300, // x1_Event_WaveFight_ChamberOfBone,
            365033, // x1_Event_WaveFight_CultistLegion,
            368056, // x1_Event_WaveFight_DeathChill,
            365678, // x1_Event_WaveFight_FallenWarband,
            368124, // x1_Event_WaveFight_ForsakenSoldiers,
            369482, // x1_Event_WaveFight_HostileRealm,
            368334, // x1_Event_WaveFight_Juggernaut,
            365133, // x1_Event_WaveFight_KhazraWarband,
            365953, // x1_Event_WaveFight_SunkenGrave,
        };



        public static HashSet<string> VanityItems { get { return DataDictionary.vanityItems; } }
        private static readonly HashSet<string> vanityItems = new HashSet<string>()
        {
            "x1_AngelWings_Imperius", // Wings of Valor
            "X1_SpectralHound_Skull_promo", // Liber Canis Mortui
            "WoDFlag", // Warsong Pennant
        };

        public static HashSet<int> NeverTownPortalLevelAreaIds { get { return neverTownPortalLevelAreaIds; } }
        private static readonly HashSet<int> neverTownPortalLevelAreaIds = new HashSet<int>()
        {
            202446, // A1 New Tristram "Attack Area"
            //19947, // A1 New Tristram "Attack Area"

            284069, // A5 Westmarch Overlook
            308323, // A5 Westmarch Wolf Gate
            315938, // A5 Westmarch Wolf Gate
            316374, // A5 Westmarch Storehouse
            311624, // A5 Westmarch Cathedral Courtyard
            311623, // A5 Streets of Westmarch
            309413, // A5 Westmarch Cathedral

        };


        public static HashSet<int> ForceTownPortalLevelAreaIds { get { return DataDictionary.forceTownPortalLevelAreaIds; } }
        private static readonly HashSet<int> forceTownPortalLevelAreaIds = new HashSet<int>
        {
            55313, // Act 2 Caldeum Bazaar
        };



        /// <summary>
        /// Contains the list of Boss Level Area ID's
        /// </summary>
        public static HashSet<int> BossLevelAreaIDs { get { return bossLevelAreaIDs; } }
        private static readonly HashSet<int> bossLevelAreaIDs = new HashSet<int>
        {
            109457, 185228, 60194, 130163, 60714, 19789, 62726, 90881, 195268, 58494, 81178, 60757, 111232, 112580,
            119656, 111516, 143648, 215396, 119882, 109563, 153669, 215235, 55313, 60193, 19789, 330576,
        };

        /// <summary>
        /// A list of LevelAreaId's that the bot should always use Straight line pathing (no navigator)
        /// </summary>
        public static HashSet<int> StraightLinePathingLevelAreaIds { get { return DataDictionary.straightLinePathingLevelAreaIds; } }
        private static readonly HashSet<int> straightLinePathingLevelAreaIds = new HashSet<int>
        {
            60757, // Belial's chambers
            405915, // p1_TieredRift_Challenge
        };

        public static HashSet<int> QuestLevelAreaIds { get { return DataDictionary.questLevelAreaIds; } }
        private static readonly HashSet<int> questLevelAreaIds = new HashSet<int>
        {
            202446, // A1 New Tristram "Attack Area"
            19947, // A1 New Tristram
            109457, // A1 New Tristram Inn
            109457, // A1 The Slaughtered Calf Inn
            62968, // A1 The Hidden Cellar
            60714, // A1 Leoric's Passage
            83110, // A1 Cellar of the Damned
            19935, // A1 Wortham
            100854, // A1 Khazra Den
            94672, // A1 Cursed Hold

            60757, // A2 Belial's chambers
            55313, // A2 Caldeum Bazaar
            102964, // A2 City of Caldeum

            309413, // A5 Westmarch Cathedral

            336846, // x1_westm_KingEvent01 - Westmarch Commons Contested Villa
            405915, // p1_TieredRift_Challenge
        };


        /// <summary>
        /// This list is used when an actor has an attribute BuffVisualEffect=1, e.g. fire floors in The Butcher arena
        /// </summary>
        public static HashSet<int> ButcherFloorPanels { get { return butcherFloorPanels; } }
        private static readonly HashSet<int> butcherFloorPanels = new HashSet<int>
        {
            // Butcher Floor Panels
            201454, 201464, 201426, 201438, 200969, 201423, 201242,
        };

        /// <summary>
        /// This list is used for Units with specific Animations we want to treat as avoidance
        /// </summary>
        public static HashSet<DoubleInt> AvoidanceAnimations { get { return DataDictionary.avoidanceAnimations; } }
        private static readonly HashSet<DoubleInt> avoidanceAnimations = new HashSet<DoubleInt>
        {
            // Fat guys that explode into worms
            // Stitch_Suicide_Bomb State=Transform By: Corpulent_C (3849)
            new DoubleInt((int)SNOActor.Corpulent_A, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_A_Unique_01, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_A_Unique_02, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_A_Unique_03, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_B, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_B_Unique_01, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_C, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_D_CultistSurvivor_Unique, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_C_OasisAmbush_Unique, (int)SNOAnim.Stitch_Suicide_Bomb),
            new DoubleInt((int)SNOActor.Corpulent_D_Unique_Spec_01, (int)SNOAnim.Stitch_Suicide_Bomb),

            new DoubleInt(330824, (int)SNOAnim.x1_Urzael_attack_06), // Urzael flame 
            new DoubleInt(330824, 348109), // Urzael Cannonball Aim
            new DoubleInt(330824, 344952), // Urzael Flying

            // Spinny AOE Attack
            new DoubleInt((int)SNOActor.x1_LR_DeathMaiden_A, (int)SNOAnim.x1_deathMaiden_attack_special_360_01),

            new DoubleInt((int)SNOActor.x1_portalGuardianMinion_Melee_A, (int)SNOAnim.x1_portalGuardianMinion_attack_charge_01), // x1_portalGuardianMinion_Melee_A (279052)
            new DoubleInt((int)SNOActor.X1_BigRed_Chronodemon_Burned_A, (int)SNOAnim.X1_BigRed_attack_02), // X1_BigRed_Chronodemon_Burned_A (326670)
            
            // Big guys with blades on their arms who jump accross the screen and stun you
            // x1_westmarchBrute_attack_02_out State=Attacking By: x1_westmarchBrute_A (258678)
            new DoubleInt((int)SNOActor.x1_westmarchBrute_A, (int)SNOAnim.x1_westmarchBrute_attack_02_in),
            new DoubleInt((int)SNOActor.x1_westmarchBrute_A, (int)SNOAnim.x1_westmarchBrute_attack_02_mid),
            new DoubleInt((int)SNOActor.x1_westmarchBrute_A, (int)SNOAnim.x1_westmarchBrute_attack_02_out),   
           
            // snakeMan_melee_generic_cast_01 State=Transform By: X1_LR_Boss_Snakeman_Melee_Belial (360281)
            new DoubleInt((int)SNOActor.X1_LR_Boss_Snakeman_Melee_Belial, (int)SNOAnim.snakeMan_melee_generic_cast_01),  
 
            //x1_Squigglet_Generic_Cast State=Transform By: X1_LR_Boss_Squigglet (353535)
            new DoubleInt((int)SNOActor.X1_LR_Boss_Squigglet, (int)SNOAnim.x1_Squigglet_Generic_Cast),
       };

        /// <summary>
        /// This list is used for Units with specific Animations we want to treat as avoidance
        /// </summary>
        public static readonly HashSet<DoubleInt> DirectionalAvoidanceAnimations = new HashSet<DoubleInt>
        {
            // Beast Charge
            new DoubleInt((int)SNOActor.Beast_A, (int)SNOAnim.Beast_start_charge_02),
            new DoubleInt((int)SNOActor.Beast_A, (int)SNOAnim.Beast_charge_02),
            new DoubleInt((int)SNOActor.Beast_A, (int)SNOAnim.Beast_charge_04),
            new DoubleInt((int)SNOActor.Beast_B, (int)SNOAnim.Beast_start_charge_02),
            new DoubleInt((int)SNOActor.Beast_B, (int)SNOAnim.Beast_charge_02),
            new DoubleInt((int)SNOActor.Beast_B, (int)SNOAnim.Beast_charge_04),
            new DoubleInt((int)SNOActor.Beast_C, (int)SNOAnim.Beast_start_charge_02),
            new DoubleInt((int)SNOActor.Beast_C, (int)SNOAnim.Beast_charge_02),
            new DoubleInt((int)SNOActor.Beast_C, (int)SNOAnim.Beast_charge_04),
            new DoubleInt((int)SNOActor.Beast_D, (int)SNOAnim.Beast_start_charge_02),
            new DoubleInt((int)SNOActor.Beast_D, (int)SNOAnim.Beast_charge_02),
            new DoubleInt((int)SNOActor.Beast_D, (int)SNOAnim.Beast_charge_04),

            // Nobody wants to get hit by a mallet demon
            new DoubleInt(343767, (int)SNOAnim.malletDemon_attack_01), // X1_LR_Boss_MalletDemon
            new DoubleInt(106709, (int)SNOAnim.malletDemon_attack_01), // MalletDemon_A
            new DoubleInt(219736, (int)SNOAnim.malletDemon_attack_01), // MalletDemon_A_Unique_01  
            new DoubleInt(219751, (int)SNOAnim.malletDemon_attack_01), // MalletDemon_A_Unique_02 

           
            // Angels with those big clubs with a dashing attack
            // Angel_Corrupt_attack_dash_in State=Transform By: Angel_Corrupt_A (106711)
            new DoubleInt((int)SNOActor.Angel_Corrupt_A, (int)SNOAnim.Angel_Corrupt_attack_dash_in),
            new DoubleInt((int)SNOActor.Angel_Corrupt_A, (int)SNOAnim.Angel_Corrupt_attack_dash_middle),
            new DoubleInt((int)SNOActor.Angel_Corrupt_A, (int)SNOAnim.Angel_Corrupt_attack_dash_out), 
           

            //] Triune_Berserker_specialAttack_loop_01 State=TakingDamage By: Triune_Berserker_A (6052)
            new DoubleInt((int)SNOActor.Triune_Berserker_A, (int)SNOAnim.Triune_Berserker_specialAttack_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_A, (int)SNOAnim.Triune_Berserker_specialAttack_loop_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_B, (int)SNOAnim.Triune_Berserker_specialAttack_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_B, (int)SNOAnim.Triune_Berserker_specialAttack_loop_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_C, (int)SNOAnim.Triune_Berserker_specialAttack_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_C, (int)SNOAnim.Triune_Berserker_specialAttack_loop_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_D, (int)SNOAnim.Triune_Berserker_specialAttack_01),
            new DoubleInt((int)SNOActor.Triune_Berserker_D, (int)SNOAnim.Triune_Berserker_specialAttack_loop_01),
       };

        /// <summary>
        /// This list is used for animations where the avoidance point should be the player's current location
        /// </summary>
        public static HashSet<int> AvoidAnimationAtPlayer { get { return avoidAnimationAtPlayer; } }
        private static readonly HashSet<int> avoidAnimationAtPlayer = new HashSet<int>
        {
            (int)SNOAnim.Beast_start_charge_02, // A1 Savage Beast Charge - needs special handling!
            (int)SNOAnim.Beast_charge_02, // A1 Savage Beast Charge - needs special handling!
            (int)SNOAnim.Beast_charge_04, // A1 Savage Beast Charge - needs special handling!
            (int)SNOAnim.morluSpellcaster_attack_AOE_01, //morluSpellcaster_D
            (int)SNOAnim.X1_LR_Boss_morluSpellcaster_generic_cast, //morluSpellcaster_D
            (int)SNOAnim.snakeMan_melee_generic_cast_01, //X1_LR_Boss_Snakeman_Melee_Belial (360281)
       };

        public static Dictionary<int, float> DefaultAvoidanceAnimationCustomRadius { get { return defaultAvoidanceAnimationCustomRadius; } }
        private static readonly Dictionary<int, float> defaultAvoidanceAnimationCustomRadius = new Dictionary<int, float>()
        {
            {(int)SNOAnim.morluSpellcaster_attack_AOE_01, 20f },
            {(int)SNOAnim.x1_deathMaiden_attack_special_360_01, 15f},
            {(int)SNOAnim.x1_Squigglet_Generic_Cast, 40f}, // Rift Boss Slime AOE
        };

        /// <summary>
        /// A list of all the SNO's to avoid - you could add
        /// </summary>
        public static HashSet<int> Avoidances { get { return avoidances; } }
        private static readonly HashSet<int> avoidances = new HashSet<int>
        {
            219702, // Arcane
            221225, // Arcane 2
            5482,   // Poison Tree
            6578,   // Poison Tree
            4803,   // monsterAffix_Molten_deathStart_Proxy
            4804,   // monsterAffix_Molten_deathExplosion_Proxy 
            4806,   // monsterAffix_Electrified_deathExplosion_proxy
            224225, // Molten Core 2
            247987, // Molten Core 2
            95868,  // Molten Trail
            108869, // Plague Cloud
            402,    // Ice Balls
            223675, // Ice Balls
            5212,   // Bees-Wasps
            3865,   // Plague-Hands
            123124, // Azmo Pools
            123842, // Azmo fireball
            123839, // Azmo bodies
            161822, // Belial 1
            161833, // Belial 2
            4103,   // Sha-Ball
            160154, // Mol Ball
            432,    // Mage Fire
            168031, // Diablo Prison
            214845, // Diablo Meteor
            260377, // Ice-trail
            //185924, // Zolt Bubble
            139741, // Zolt Twister
            93837,  // Ghom Gas
            166686, // Maghda Proj
            226350, // Diablo Ring of Fire
            226525, // Diablo Ring of Fire
            250031, // Mortar MonsterAffix_Mortar_Pending

            84608,  // Desecrator monsterAffix_Desecrator_damage_AOE
            84606,  // Desecrator monsterAffix_Desecrator_telegraph

            /* 2.0 */
            349774, // FrozenPulse x1_MonsterAffix_frozenPulse_monster
            343539, // Orbiter X1_MonsterAffix_Orbiter_Projectile
            316389, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_projectile (316389)
            340319, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_bomb_start (340319)
            341512, // Thunderstorm x1_MonsterAffix_Thunderstorm_Impact
            337109, // Wormhole X1_MonsterAffix_TeleportMines

            338889, // x1_Adria_bouncingProjectile
            360738, // X1_Adria_arcanePool
            358404, // X1_Adria_blood_large

            360598, // x1_Urzael_CeilingDebris_DamagingFire_wall
            359205, // x1_Urzael_ceilingDebris_Impact_Beam
            360883, // x1_Urzael_ceilingDebris_Impact_Circle

            362850, // x1_Urzael_Cannonball_burning_invisible
            346976, // x1_Urzael_Cannonball_burning_impact
            346975, // x1_Urzael_Cannonball_burning

            335505, // x1_malthael_drainSoul_ghost
            325136, // x1_Malthael_DeathFogMonster
            340512, // x1_Malthael_Mephisto_LightningObject

            359703, // X1_Unique_Monster_Generic_AOE_DOT_Cold_10foot
            363356, // X1_Unique_Monster_Generic_AOE_DOT_Cold_20foot
            359693, // X1_Unique_Monster_Generic_AOE_DOT_Fire_10foot
            363357, // X1_Unique_Monster_Generic_AOE_DOT_Fire_20foot
            364542, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_10foot
            364543, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_20foot
            377537, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_hardpoints
            360046, // X1_Unique_Monster_Generic_AOE_DOT_Poison_10foot
            363358, // X1_Unique_Monster_Generic_AOE_DOT_Poison_20foot
            368156, // X1_Unique_Monster_Generic_AOE_Lightning_Ring
            358917, // X1_Unique_Monster_Generic_AOE_Sphere_Distortion
            377086, // X1_Unique_Monster_Generic_Projectile_Arcane
            377087, // X1_Unique_Monster_Generic_Projectile_Cold
            377088, // X1_Unique_Monster_Generic_Projectile_Fire
            377089, // X1_Unique_Monster_Generic_Projectile_Holy
            377090, // X1_Unique_Monster_Generic_Projectile_Lightning
            377091, // X1_Unique_Monster_Generic_Projectile_Physical
            377092, // X1_Unique_Monster_Generic_Projectile_Poison
            (int)SNOActor.x1_LR_boss_terrorDemon_A_projectile,

        };

        /// <summary>
        /// A list of SNO's that are projectiles (so constantly look for new locations while avoiding)
        /// </summary>
        public static HashSet<int> AvoidanceProjectiles { get { return avoidanceProjectiles; } }
        private static readonly HashSet<int> avoidanceProjectiles = new HashSet<int>
        {
            5212,   // Bees-Wasps
            4103,   // Sha-Ball
            160154, // Molten Ball
            123842, // Azmo fireball
            139741, // Zolt Twister
            166686, // Maghda Projectile
            185999, // Diablo Expanding Fire
            196526, // Diablo Expanding Fire
            136533, // Diablo Lightning Breath
            343539, // Orbiter X1_MonsterAffix_Orbiter_Projectile
            341512, // Thunderstorm 
            316389, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_projectile (316389)
            340319, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_bomb_start (340319)
            
            // A5

            338889, // x1_Adria_bouncingProjectile

            362850, // x1_Urzael_Cannonball_burning_invisible
            346976, // x1_Urzael_Cannonball_burning_impact
            346975, // x1_Urzael_Cannonball_burning

            335505, // x1_malthael_drainSoul_ghost
            325136, // x1_Malthael_DeathFogMonster
            340512, // x1_Malthael_Mephisto_LightningObject

            377086, // X1_Unique_Monster_Generic_Projectile_Arcane
            377087, // X1_Unique_Monster_Generic_Projectile_Cold
            377088, // X1_Unique_Monster_Generic_Projectile_Fire
            377089, // X1_Unique_Monster_Generic_Projectile_Holy
            377090, // X1_Unique_Monster_Generic_Projectile_Lightning
            377091, // X1_Unique_Monster_Generic_Projectile_Physical
            377092, // X1_Unique_Monster_Generic_Projectile_Poison

            3528,   // Butcher_hook

            // 4394, //g_ChargedBolt_Projectile-200915 (4394) Type=Projectile
            // 368392, // x1_Cesspool_Slime_Posion_Attack_Projectile-222254 (368392) Type=Projectile
            (int)SNOActor.x1_LR_boss_terrorDemon_A_projectile,
        };

        /// <summary>
        /// A list of SNO's that spawn AoE then disappear from the object manager
        /// </summary>
        public static HashSet<int> AvoidanceSpawners { get { return avoidanceSpawners; } }
        private static readonly HashSet<int> avoidanceSpawners = new HashSet<int>
        {
            5482,   // Poison Tree
            6578,   // Poison Tree
            316389, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_projectile (316389)
            340319, // PoisonEnchanted x1_MonsterAffix_CorpseBomber_bomb_start (340319)
            159369, //MorluSpellcaster_Meteor_Pending-178011 (159369)
        };

        /// <summary>
        /// The duration the AoE from AvoidanceSpawners should be avoided for
        /// </summary>
        public static Dictionary<int, TimeSpan> AvoidanceSpawnerDuration { get { return avoidanceSpawnerDuration; } }
        private static readonly Dictionary<int, TimeSpan> avoidanceSpawnerDuration = new Dictionary<int, TimeSpan>
        {
            {5482, TimeSpan.FromSeconds(15)},   // Poison Tree
            {6578, TimeSpan.FromSeconds(15)},   // Poison Tree
            {316389, TimeSpan.FromSeconds(6)}, // PoisonEnchanted 
            {340319, TimeSpan.FromSeconds(6)}, // PoisonEnchanted 
            {4803, TimeSpan.FromSeconds(10)}, // Molten Core
            {4804, TimeSpan.FromSeconds(10)}, // Molten Core
            {224225, TimeSpan.FromSeconds(10)}, // Molten Core
            {247987, TimeSpan.FromSeconds(10)}, // Molten Core
            {159369, TimeSpan.FromSeconds(3)}, // Morlu Meteor
        };

        public static Dictionary<int, float> DefaultAvoidanceCustomRadius { get { return defaultAvoidanceCustomRadius; } }
        private static readonly Dictionary<int, float> defaultAvoidanceCustomRadius = new Dictionary<int, float>()
        {
            {330824, 35f }, // A5 Urzael animations
            {360598, 10f }, // x1_Urzael_CeilingDebris_DamagingFire_wall
            {359205, 20f }, // x1_Urzael_ceilingDebris_Impact_Beam
            {360883, 20f }, // x1_Urzael_ceilingDebris_Impact_Circle
            {362850, 12f }, // x1_Urzael_Cannonball_burning_invisible
            {346976, 12f }, // x1_Urzael_Cannonball_burning_impact
            {346975, 12f }, // x1_Urzael_Cannonball_burning

            {360738, 12f}, // X1_Adria_arcanePool
            {338889, 5f}, // x1_Adria_bouncingProjectile
            {358404, 15f}, // X1_Adria_blood_large

            {335505, 5f}, // x1_malthael_drainSoul_ghost
            {325136, 20f}, // x1_Malthael_DeathFogMonster
            {340512, 8f}, // x1_Malthael_Mephisto_LightningObject
            {343767, 35f }, // Mallet Demons
            {106709, 35f }, // Mallet Demons
            {219736, 35f }, // Mallet Demons
            {219751, 35f }, // Mallet Demons

            {159369, 20f }, //MorluMeteor
            {4103, 25f}, // Meteor
            {3528, 15f}, // Butcher_hook

            {359703, 13f}, // X1_Unique_Monster_Generic_AOE_DOT_Cold_10foot
            {363356, 25f}, // X1_Unique_Monster_Generic_AOE_DOT_Cold_20foot
            {359693, 13f}, // X1_Unique_Monster_Generic_AOE_DOT_Fire_10foot
            {363357, 25f}, // X1_Unique_Monster_Generic_AOE_DOT_Fire_20foot
            {364542, 13f}, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_10foot
            {364543, 25f}, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_20foot
            {377537, 13f}, // X1_Unique_Monster_Generic_AOE_DOT_Lightning_hardpoints
            {360046, 13f}, // X1_Unique_Monster_Generic_AOE_DOT_Poison_10foot
            {363358, 25f}, // X1_Unique_Monster_Generic_AOE_DOT_Poison_20foot
            {368156, 13f}, // X1_Unique_Monster_Generic_AOE_Lightning_Ring
            {358917, 13f}, // X1_Unique_Monster_Generic_AOE_Sphere_Distortion
            {377086, 13f}, // X1_Unique_Monster_Generic_Projectile_Arcane
            {377087, 13f}, // X1_Unique_Monster_Generic_Projectile_Cold
            {377088, 13f}, // X1_Unique_Monster_Generic_Projectile_Fire
            {377089, 13f}, // X1_Unique_Monster_Generic_Projectile_Holy
            {377090, 13f}, // X1_Unique_Monster_Generic_Projectile_Lightning
            {377091, 13f}, // X1_Unique_Monster_Generic_Projectile_Physical
            {377092, 13f}, // X1_Unique_Monster_Generic_Projectile_Poison
			{123842,60f},

            {(int)SNOActor.x1_LR_boss_terrorDemon_A_projectile, 10f},


            {4803, 25f}, // Molten Core
            {4804, 25f}, // Molten Core
            {224225, 25f}, // Molten Core
            {247987, 25f}, // Molten Core
            {139741, 15}, 

        };

        public readonly static HashSet<TrinityItemType> NoPickupClickTypes = new HashSet<TrinityItemType>
        {
            TrinityItemType.PowerGlobe,
            TrinityItemType.HealthGlobe,
            TrinityItemType.ProgressionGlobe,
            TrinityItemType.HoradricRelic,
        };

        /*
         * Combat-related dictionaries/defaults
         */

        /// <summary>
        /// ActorSNO's of Very fast moving mobs (eg wasps), for special skill-selection decisions
        /// </summary>
        public static HashSet<int> FastMovingMonsterIds { get { return fastMovementMonsterIds; } }
        private static readonly HashSet<int> fastMovementMonsterIds = new HashSet<int>
        {
            5212
         };

        /// <summary>
        /// A list of crappy "summoned mobs" we should always ignore unless they are very close to us, eg "grunts", summoned skeletons etc.
        /// </summary>
        public static HashSet<int> ShortRangeAttackMonsterIds { get { return shortRangeAttackMonsterIds; } }
        private static readonly HashSet<int> shortRangeAttackMonsterIds = new HashSet<int>
        {
            4084, 4085, 5395, 144315,
         };

        /// <summary>
        /// Dictionary of Actor SNO's and cooresponding weighting/Priority 
        /// </summary>
        public static Dictionary<int, int> MonsterCustomWeights { get { return monsterCustomWeights; } }
        private static readonly Dictionary<int, int> monsterCustomWeights = new Dictionary<int, int>
        {
            // Wood wraiths all this line (495 & 496 & 6572 & 139454 & 139456 & 170324 & 170325)
            {495, 901}, {496, 901}, {6572, 901}, {139454, 901}, {139456, 901}, {170324, 901}, {170325, 901},
            // -- added 4099 (act 2 fallen shaman)
            // Fallen Shaman prophets goblin Summoners (365 & 4100)
            {365, 1901}, {4099, 1901}, {4100, 1901},
            // The annoying grunts summoned by the above
            {4084, -250},
            // Wretched mothers that summon zombies in act 1 (6639)
            {6639, 951},
            // Fallen lunatic (4095)
            {4095, 2999},
            // Pestilence hands (4738)
            {4738, 1901},
            // Belial Minions
            {104014, 1500},
            // Act 1 doors that skeletons get stuck behind
            {454, 1500},
            // Cydaea boss (95250)
            {95250, 1501},
            
            //Cydaea Spiderlings (137139)
            {137139, -301},
            // GoatMutantshaman Elite (4304)
            {4304, 999},
            // GoatMutantshaman (4300)
            {4300, 901},
            // Succubus (5508)
            {5508, 801},
            // skeleton summoners (5387, 5388, 5389)
            {5387, 951}, {5388, 951}, {5389, 951},
            // Weak skeletons summoned by the above
            {5395, -401},
            // Wasp/Bees - Act 2 annoying flyers (5212)
            {5212, 751},
            // Dark summoner - summons the helion dogs (6035)
            {6035, 501},
            // Dark berserkers - has the huge damaging slow hit (6052)
            {6052, 501},
            // The giant undead fat grotesques that explode in act 1 (3847)
            {3847, 401},
            // Hive pods that summon flyers in act 1 (4152, 4153, 4154)
            {4152, 901}, {4153, 901}, {4154, 901},
            // Totems in act 1 that summon the ranged goatmen (166452)
            {166452, 901},
            // Totems in act 1 dungeons that summon skeletons (176907)
            {176907, 901},
            // Act 2 Sand Monster + Zultun Kulle (kill sand monster first)
            {226849, 20000}, {80509, 1100},
            // Maghda and her minions
            {6031, 801}, {178512, 901},
            // Uber Bosses - Skeleton King {255929}, Maghda {256189} & Pets {219702} which must be killed first
            {255929, 2999}, {219702, 1999}, {256189, 999},
            // Uber Bosses - Zoltun Kulle {256508} & Siegebreaker {256187}
            // Siegebreaker removed so the focus remains on Zoltun Kulle until he is dead
            {256508, 2999},
            //{256508, 2999}, {256187, 1899},
            // Uber Bosses - Ghom {256709} & Rakanot {256711}
            {256709, 2999}, {256711, 1899},

            // A5 Forgotton War trash
            { 300864, -300 },
         };


        /// <summary>
        /// A list of all known SNO's of treasure goblins/bandits etc.
        /// </summary>
        public static HashSet<int> GoblinIds { get { return goblinIds; } }
        private static readonly HashSet<int> goblinIds = new HashSet<int>
        {
            //5984, 5985, 5987, 5988, 405186, 380657

        // treasureGoblin_A
        5984,
        // treasureGoblin_B
        5985,
        // treasureGoblin_backpack
        5986,
        // treasureGoblin_C
        5987,
        // treasureGoblin_Portal
        //54862,
        // treasureGoblin_Portal_Proxy
        //54887,
        // treasureGoblin_portal_emitter
        //59948,
        // treasureGoblin_portal_opening
        //60549,
        // treasureGoblin_portal_closing
        //60558,
        // treasureGoblin_portal_summon_trailActor
        //108403,
        // treasureGoblin_stunImmune_trailActor
        //129286,
        // Lore_Bestiary_TreasureGoblin
        //147316,
        // treasureGoblin_A_Slave
        //326803,
        // treasureGoblin_C_Unique_DevilsHand
        343046,
        // p1_treasureGobin_A_Unique_GreedMinion
        380657,
        // treasureGoblin_G
        391593,
        // p1_treasureGoblin_inBackpack_A
        394196,
        // p1_treasureGoblin_jump_trailActor
        //403549,
        // p1_treasureGoblin_tentacle_A
        405186,
        // p1_treasureGoblin_tentacle_backpack
        405189,
        // treasureGoblin_D_Splitter
        408354,
        // treasureGoblin_E
        408655,
        // treasureGoblin_F
        408989,
        // treasureGoblin_Portal_Open
        //410460,
        // treasureGoblin_D_Splitter_02
        410572,
        // treasureGoblin_D_Splitter_03
        410574,
        // treasureGoblin_H
        413289,
        // p1_treasureGoblin_teleport_shell
        //428094,
        // p1_treasureGoblin_backpack_B
        428205,
        // p1_treasureGoblin_backpack_F
        428206,
        // p1_treasureGoblin_backpack_C
        428211,
        // p1_treasureGoblin_backpack_H
        428213,
        // p1_treasureGoblin_backpack_D
        428247,
        // treasureGoblin_I
        428663,
        // treasureGoblin_J
        429161,
        // p1_treasureGoblin_backpack_J
        429526,
        // treasureGoblin_B_WhatsNew
        429615,
        // treasureGoblin_F_WhatsNew
        429619,
        // treasureGoblin_C_WhatsNew
        429620,
        // treasureGoblin_B_FX_WhatsNew
        429624,
        // p1_treasureGoblin_backpack_E
        429660,
        // treasureGoblin_A_LegacyPuzzleRing
        //429689,
        // p1_treasureGoblin_backpack_I
        433905,
        // treasureGoblin_J_WhatsNew
        434630,
        // treasureGoblin_J_FX_WhatsNew
        434631,
        // treasureGoblin_E_WhatsNew
        434632,
        // treasureGoblin_D_WhatsNew
        434633,
        // treasureGoblin_Anniversary_Event
        434745,

         };

        /// <summary>
        /// Contains ActorSNO of ranged units that should be attacked even if outside of kill radius
        /// </summary>
        public static HashSet<int> RangedMonsterIds { get { return rangedMonsterIds; } }
        private static readonly HashSet<int> rangedMonsterIds = new HashSet<int>
        {
            365, 4100, // fallen
            4304, 4300, // goat shaman 
            4738, // pestilence 
            4299, // goat ranged
            62736, 130794, // demon flyer
            5508, // succubus 
            5388, 4286, 256015, 256000, 255996,
            5984, 5985, 5987, 5988, 405186, //goblins
       };
        // A list of bosses in the game, just to make CERTAIN they are treated as elites
        /// <summary>
        /// Contains ActorSNO of known Bosses
        /// </summary>
        public static HashSet<int> BossIds { get { return bossIds; } }
        private static readonly HashSet<int> bossIds = new HashSet<int>
        {
            // Siegebreaker (96192), Azmodan (89690), Cydea (95250), Heart-thing (193077), 
            96192,                   89690,           95250,         193077, 
            //Kulle (80509), Small Belial (220160), Big Belial (3349), Diablo 1 (114917), terror Diablo (133562)
            80509,           220160,                3349,              114917,            133562,
            62975, // Belial TrueForm
            //Maghda, Kamyr (MiniBoss before Belial)
            6031, 51298,
            // Ghom
            87642,
            // I dunno?
            255929, 256711, 256508, 256187, 256189, 256709,
            // Another Cydaea
            137139,
            // Diablo shadow clones (needs all of them, there is a male & female version of each class!)
            144001, 144003, 143996, 143994, 
            // Jondar, Chancellor, Queen Araneae (act 1 dungeons), Skeleton King, Butcher
            86624, 156353, 51341, 5350, 3526,
            361347, //Jondar from the Adventure mode
            215103, // Istaku            
            4630, // Rakanoth
            256015, // Xah'Rith Keywarden
            115403, // A1 Cain Skeleton boss
            4373, 4376, 177539, // A1 Robbers
            168240, // A2 Jewler quest
            84919, // Skeleton King
            108444, // ZombieFemale_A_TristramQuest (Wretched Mothers)
            176889, // ZombieFemale_Unique_WretchedQueen
            129439, //Arsect The Venomous
            164502, // sandMonster_A_Head_Guardian
            378665, // Greed

            // A5
            316839, // x1_deathOrb_bodyPile
            375106, // A5 x1_Death_Orb_Monster
            375111, // A5 x1_Death_Orb_Master_Monster
            279394, // A5 Adria 

            300862, // X1_BigRed_Chronodemon_Event_ForgottenWar
            318425, // X1_CoreEliteDemon_Chronodemon_Event_ForgottenWar

            300866, // X1_Angel_TrooperBoss_Event_ForgottenWar

            346482, // X1_PandExt_TimeTrap 
            367456, // x1_Pand_Ext_Event_Hive_Blocker

            347276, // x1_Fortress_Soul_Grinder_A

            374751, // x1_PortalGuardian_A
            307339, // X1_Rockworm_Pand_Unique_HexMaze
            297730, // x1_Malthael_Boss
        };

        // Three special lists used purely for checking for the existance of a player's summoned mystic ally, gargantuan, or zombie dog

        public static HashSet<int> MysticAllyIds { get { return mysticAllyIds; } }
        private static readonly HashSet<int> mysticAllyIds = new HashSet<int>
        {
            169123, 123885, 169890, 168878, 169891, 169077, 169904, 169907, 169906, 169908, 169905, 169909
        };

        public static HashSet<int> GargantuanIds { get { return gargantuanIds; } }
        private static readonly HashSet<int> gargantuanIds = new HashSet<int>
        {
            179780, 179778, 179772, 179779, 179776, 122305 };

        public static HashSet<int> ZombieDogIds { get { return zombieDogIds; } }
        private static readonly HashSet<int> zombieDogIds = new HashSet<int>
        {
            110959, 103235, 103215, 105763, 103217, 51353,
        };

        public static HashSet<int> DemonHunterPetIds { get { return demonHunterPetIds; } }
        private static readonly HashSet<int> demonHunterPetIds = new HashSet<int>
        {
            178664,
            173827,
            133741,
            159144,
            181748,
            159098,
            159102,
            159144,
            334861,

        };

        public static HashSet<int> DemonHunterSentryIds { get { return demonHunterSentryIds; } }
        private static readonly HashSet<int> demonHunterSentryIds = new HashSet<int>
        {
           141402, 150025, 150024, 168815, 150026, 150027,
        };

        public static HashSet<int> WizardHydraIds { get { return wizardHydraIds; } }
        private static readonly HashSet<int> wizardHydraIds = new HashSet<int>
        { 
            // Some hydras are 3 monsters, only use one of their heads.
            82972, //Type: Monster Name: Wizard_HydraHead_Frost_1-1037 ActorSNO: 82972
            83959, // Type: Monster Name: Wizard_HydraHead_Big-1168 ActorSNO: 83959
            325807, // Type: Monster Name: Wizard_HydraHead_fire2_1-1250 ActorSNO: 325807
            82109, // Type: Monster Name: Wizard_HydraHead_Lightning_1-1288 ActorSNO: 82109, 
            81515, // Type: Monster Name: Wizard_HydraHead_Arcane_1-1338 ActorSNO: 81515, 
            80745, // Type: Monster Name: Wizard_HydraHead_Default_1-1364 ActorSNO: 80745, 
        };

        public static HashSet<int> AncientIds { get { return ancientIds; } }
        private static readonly HashSet<int> ancientIds = new HashSet<int>
        {
            90443, //Name: Barbarian_CallOfTheAncients_1-1028
            90535, //Name: Barbarian_CallOfTheAncients_2-1029
            90536, //Name: Barbarian_CallOfTheAncients_3-1030
        };

        /// <summary>
        /// World-object dictionaries eg large object lists, ignore lists etc.
        /// A list of SNO's to *FORCE* to type: Item. (BE CAREFUL WITH THIS!).
        /// </summary>
        public static HashSet<int> ForceToItemOverrideIds { get { return forceToItemOverrideIds; } }
        private static readonly HashSet<int> forceToItemOverrideIds = new HashSet<int>
        {
            166943, // DemonTrebuchetKey, infernal key
            255880, // DemonKey_Destruction
            255881, // DemonKey_Hate
            255882, // DemonKey_Terror
        };

        /// <summary>
        /// Interactable whitelist - things that need interacting with like special wheels, levers - they will be blacklisted for 30 seconds after one-use
        /// </summary>
        public static HashSet<int> InteractWhiteListIds { get { return interactWhiteListIds; } }
        private static readonly HashSet<int> interactWhiteListIds = new HashSet<int>
        {
            209133, // TentacleLord (209133)  QuestSNO: 434753 QuestStep: 1, Description: Slay the Infernal Bovine herd!
            363725, // Special Event Chest

            56686, // a3dun_Keep_Bridge_Switch 
            211999, // a3dun_Keep_Bridge_Switch_B 
            52685, // a3dun_Keep_Bridge
            54882, // a3dun_Keep_Door_Wooden_A
            105478, // a1dun_Leor_Spike_Spawner_Switch
            102927, // A1 Cursed Hold Prisoners
            5747, // A1 Cathedral Switch
            365097, // Cursed Chest - Damp Cellar

            // A5
            348096, // Paths of the Drowned - portal switches - x1_Bog_Beacon_B
            361364, // A5 Siege Rune Path of War

            274457, // A5 Spirit of Malthael - Tower of Korelan
            368515, // A5 Nephalem Switch - Passage to Corvus 

            354407, // X1_Angel_Common_Event_GreatWeapon

        };

        public static HashSet<int> HighPriorityInteractables { get { return highPriorityInteractables; } }
        private static readonly HashSet<int> highPriorityInteractables = new HashSet<int>
        {
            56686, // a3dun_Keep_Bridge_Switch 
            211999, // a3dun_Keep_Bridge_Switch_B 
        };

        public static Dictionary<int, int> InteractEndAnimations { get { return interactEndAnimations; } }
        private static readonly Dictionary<int, int> interactEndAnimations = new Dictionary<int, int>()
        {
            {348096, 348093}, // x1_Bog_Beacon_B
        };

        /// <summary>
        /// NOTE: you don't NEED interactable SNO's listed here. But if they are listed here, *THIS* is the range at which your character will try to walk to within the object
        /// BEFORE trying to actually "click it". Certain objects need you to get very close, so it's worth having them listed with low interact ranges
        /// </summary>
        public static Dictionary<int, float> CustomObjectRadius { get { return customObjectRadius; } }
        private static readonly Dictionary<int, float> customObjectRadius = new Dictionary<int, float>
        {
            {56686, 4},
            {52685, 4},
            {54882, 40},
            {3349, 25}, // Belial
            {225270, 35},
            {180575, 10},  // Diablo Arena Health Well
            {375111, 45f}, // A5 x1_Death_Orb_Master_Monster
            {301177, 15f}, // x1_PandExt_Time_Activator
            {368515, 5f}, // A5 Nephalem Switch -  Passage to Corvus
            {309432, 37f}, // x1_westm_Bridge
            {54850, 14f}, // a3dun_Keep_SiegeTowerDoor
            {325136, 15f},
        };

        /// <summary>
        /// Navigation obstacles for standard navigation down dungeons etc. to help DB movement
        /// MAKE SURE you add the *SAME* SNO to the "size" dictionary below, and include a reasonable size (keep it smaller rather than larger) for the SNO.
        /// </summary>
        public static HashSet<int> NavigationObstacleIds { get { return navigationObstacleIds; } }
        private static readonly HashSet<int> navigationObstacleIds = new HashSet<int>
        {
            104596, //GizmoType: DestroyableObject Name: trOut_FesteringWoods_Neph_Column_B-27477 ActorSNO: 104596 

            174900, 185391, // demonic forge
            198977,293900, // Azmo room center
            194682, 81699, 3340, 123325,

            158681, // A1 Blacksmith_Lore
            104596, // A1 trOut_FesteringWoods_Neph_Column_B
            104632, // A1 trOut_FesteringWoods_Neph_Column_B_Broken_Base
            105303, // A1 trOut_FesteringWoods_Neph_Column_C_Broken_Base_Bottom
            104827, // A1 trOut_FesteringWoods_Neph_Column_C_Broken_Base 

            332924, // x1_Bog_bloodSpring_small, Blood Spring - Overgrown Ruins
            332922, // x1_Bog_bloodSpring_medium
            332923, // x1_Bog_bloodSpring_large     
            321855, // x1_Pand_Ext_Ordnance_Mine
            355898, // x1_Bog_Family_Guard_Tower_Stump
            376917, // [1FA3B814] Type: ServerProp Name: x1_Westm_Hub_Stool_A-381422 ActorSNO: 376917, Distance: 2.337004
            (int)SNOActor.px_Bounty_Camp_Hellportals_Frame, // A4 bounties
            (int)SNOActor.px_Bounty_Camp_Pinger, // A4 bounties

            // DyingHymn A4 Bounties
            433402,
            434971,

            // Bulba
            433383, // A3 - Bounty - Catapult Command - Catapults
            433384, // A3 - Bounty - Catapult Command - Catapults
            210433 //A3 -  Catapult_a3dunKeep_WarMachines_Snow_Firing
        };

        /// <summary>
        /// Size of the navigation obstacles above (actual SNO list must be matching the above list!)
        /// </summary>
        public static Dictionary<int, float> ObstacleCustomRadius { get { return obstacleCustomRadius; } }
        private static readonly Dictionary<int, float> obstacleCustomRadius = new Dictionary<int, float>
        {
            {174900, 25}, {194682, 20}, {81699, 20}, {3340, 12}, {123325, 25}, {185391, 25},
            {104596, 15}, // trOut_FesteringWoods_Neph_Column_B
            {104632, 15}, // trOut_FesteringWoods_Neph_Column_B_Broken_Base
            {105303, 15}, // trOut_FesteringWoods_Neph_Column_C_Broken_Base_Bottom
            {104827, 15}, // trOut_FesteringWoods_Neph_Column_C_Broken_Base 
            {355898, 12}, // x1_Bog_Family_Guard_Tower_Stump
            {376917, 10},
            {293900, 20f}, // Azmo Blocking Shit


            // DyingHymn A4 Bounties
            {433402, 8},
            {434971, 10},

         };

        public static HashSet<int> ForceDestructibles { get { return forceDestructibles; } }
        private static HashSet<int> forceDestructibles = new HashSet<int>()
        {
            273323, // x1_westm_Door_Wide_Clicky
            55325, // a3dun_Keep_Door_Destructable

            225252, // Shamanic Ward - Revenge of Gharbad bounty

            331397, // x1_westm_Graveyard_Floor_Sarcophagus_Undead_Husband_Event

            386274, // Tgoblin_Gold_Pile_C

            211861, //Pinata

            437152, // A3 - Bounty: Khazra Guardians (436284)

            121821, // A2 - Bounty: The Putrid Waters (433017)
        };

        /// <summary>
        /// This is the RadiusDistance at which destructibles and barricades (logstacks, large crates, carts, etc.) are added to the cache
        /// </summary>
        public static Dictionary<int, float> DestructableObjectRadius { get { return destructableObjectRadius; } }
        private static readonly Dictionary<int, float> destructableObjectRadius = new Dictionary<int, float>
        {
            {2972, 10}, {80357, 16}, {116508, 10}, {113932, 8}, {197514, 18}, {108587, 8}, {108618, 8}, {108612, 8}, {116409, 18}, {121586, 22},
            {195101, 10}, {195108, 25}, {170657, 5}, {181228, 10}, {211959, 25}, {210418, 25}, {174496, 4}, {193963, 5}, {159066, 12}, {160570, 12},
            {55325, 5}, {5718, 14}, {5909, 10}, {5792, 8}, {108194, 8}, {129031, 30}, {192867, 3.5f}, {155255, 8}, {54530, 6}, {157541, 6},
            {93306, 10},
         };

        /// <summary>
        /// Destructible things that need targeting by a location instead of an ACDGUID (stuff you can't "click on" to destroy in-game)
        /// </summary>
        public static HashSet<int> DestroyAtLocationIds { get { return destroyAtLocationIds; } }
        private static readonly HashSet<int> destroyAtLocationIds = new HashSet<int>
        {
            170657, 116409, 121586, 155255, 104596, 93306,
         };

        /// <summary>
        /// Resplendent chest SNO list
        /// </summary>
        public static HashSet<int> ResplendentChestIds { get { return resplendentChestIds; } }
        private static readonly HashSet<int> resplendentChestIds = new HashSet<int>
        {
            62873, 95011, 81424, 108230, 111808, 111809, 211861, 62866, 109264, 62866, 62860, 96993,
            // Magi
			112182,
            363725, 357331, // chests after Cursed Chest

             301177, // A5 Timeless Prison Switch

            433670, //[1FA93634] GizmoType: Chest Name: x1_Global_Chest_BossBounty-6118 ActorSNO: 433670 Distance: 14.60398 
        };
        /// <summary>
        /// Objects that should never be ignored due to no Line of Sight (LoS) or ZDiff
        /// </summary>
        public static HashSet<int> LineOfSightWhitelist { get { return lineOfSightWhitelist; } }
        private static readonly HashSet<int> lineOfSightWhitelist = new HashSet<int>
        {
            116807, // Butcher Health Well
            180575, // Diablo arena Health Well
            129031, // A3 Skycrown Catapults
            220160, // Small Belial (220160), 
            3349,   // Big Belial (3349),    
            210268, // Corrupt Growths 2nd Tier
            193077, // a3dun_Crater_ST_GiantDemonHeart_Mob

            375106, // A5 x1_Death_Orb_Monster
            375111, // A5 x1_Death_Orb_Master_Monster

            329390, // x1_Pand_BatteringRam_Hook_B_low
            368515, // A5 Nephalem Switch -  Passage to Corvus 
            347276, // x1_Fortress_Soul_Grinder_A
            326937, // x1_Pand_BatteringRam_Hook_B
            291368, // x1_Urzael_Boss
        };

        /// <summary>
        /// Chests/average-level containers that deserve a bit of extra radius (ie - they are more worthwhile to loot than "random" misc/junk containers)
        /// </summary>
        public static HashSet<int> ContainerWhiteListIds { get { return containerWhiteListIds; } }
        private static readonly HashSet<int> containerWhiteListIds = new HashSet<int>
        {
            (int)SNOActor.x1_Catacombs_Weapon_Rack_Raise,
            62859,  // TrOut_Fields_Chest
            62865,  // TrOut_Highlands_Chest
            62872,  // CaOut_Oasis_Chest
            78790,  // trOut_wilderness_chest
            79016,  // trOut_Tristram_chest
            94708,  // a1dun_Leor_Chest
            96522,  // a1dun_Cath_chest
            130170, // a3dun_Crater_Chest
            108122, // caOut_StingingWinds_Chest
            111870, // A3_Battlefield_Chest_Snowy
            111947, // A3_Battlefield_Chest_Frosty
            213447, // Lore_AzmodanChest3
            213446, // Lore_AzmodanChest2
            51300,  // a3dun_Keep_Chest_A
            179865, // a3dun_Crater_ST_Chest
            109264, // a3dun_Keep_Chest_Rare
            212491, // a1dun_Random_Cloud
            210422, // a1dun_random_pot_of_gold_A
            211861, // Pinata
			196945, // a2dun_Spider_EggSack__Chest
            70534,  // a2dun_Spider_Chest
            289794, // Weaponracks on battlefields of eternity --> best place to farm white crafting materials
            103919, // Demonic Vessels         
            78030,  // GizmoType: Chest Name: trOut_Wilderness_Scarecrow_A-3924 ActorSNO: 78030 
            173325, // Anvil of Fury

            301177, // A5 Timeless Prison Switch

            // Kevin Spacey was here
            193023, //LootType3_GraveGuard_C_Corpse_03
			156682, //Adventurer_A_Corpse_01_WarrivEvent
			5758, //trDun_Cath_FloorSpawner_01
			5724, //trDun_Cath_BookcaseShelves_A
			85790, //Cath_Lecturn_ LachdanansScroll
			227305, //Lore_Inarius_Corrupt
			137125, //FesteringWoods_WarriorsRest_Lore
        };

        /// <summary>
        /// Contains ActorSNO's of world objects that should be blacklisted
        /// </summary>
        public static HashSet<int> BlackListIds { get { return blacklistIds; } }
        private static HashSet<int> blacklistIds = new HashSet<int>
        {
            5674, //trDun_book_pile_b ActorSNO=5674 
            362323, // x1_WestmHub_GuardNoHelmUnarmed
            // World Objects
            163449, 2909, 58283, 58321, 87809, 90150, 91600, 97023, 97350, 97381, 72689, 121327, 54515, 3340, 122076, 123640,
            60665, 60844, 78554, 86400, 86428, 81699, 86266, 86400, 6190, 80002, 104596, 58836, 104827, 74909, 6155, 6156, 6158, 6159, 75132,
            181504, 91688, 3007, 3011, 3014, 130858, 131573, 214396, 182730, 226087, 141639, 206569, 15119, 54413, 54926, 2979, 5776, 3949,
            108490, 52833, 200371, 153752, 2972, 206527, 3628,
            //a3dun_crater_st_Demo_ChainPylon_Fire_Azmodan, a3dun_crater_st_Demon_ChainPylon_Fire_MistressOfPain
            201680,
            217285,  //trOut_Leor_painting
            5902, // trDun_Magic_Painting_H_NoSpawn
            // uber fire chains in Realm of Turmoil  
            263014,
            249192, 251416, 249191, 251730, // summoned skeleton from ring   
            // Units below here
            111456, 5013, 5014, 205756, 205746, 4182, 4183, 4644, 4062, 4538, 52693, 162575, 2928, 51291, 51292,
            96132, 90958, 90959, 80980, 51292, 51291, 2928, 3546, 129345, 81857, 138428, 81857, 60583, 170038, 174854, 190390,
            194263, 87189, 90072, 107031, 106584, 186130, 187265,
            108012, 103279, 74004, 84531, 84538,  190492, 209133, 6318, 107705, 105681, 89934,
            89933, 182276, 117574, 182271, 182283, 182278, 128895, 81980, 82111, 81226, 81227, 107067, 106749,
            107107, 107112, 106731, 107752, 107829, 90321, 107828, 121327, 249320, 81232, 81231, 81239, 81515, 210433, 195414,
            80758, 80757, 80745, 81229, 81230, 82109, 83024, 83025, 82972, 83959, 249190, 251396, 138472, 118260, 200226, 192654, 245828,
            215103, 132951, 217508, 199998, 199997, 114527, 245910, 169123, 123885, 169890, 168878, 169891, 169077, 169904, 169907,
            169906, 169908, 169905, 169909, 179780, 179778, 179772, 179779, 179776, 122305, 80140, 110959, 103235, 103215, 105763, 103217, 51353,
            4176, 178664, 173827, 133741, 159144, 181748, 159098, 206569, 200706, 5895, 5896, 5897, 5899, 4686, 85843, 249338,
            251416, 249192, 4798, 183892,196899, 196900, 196903, 223333, 220636, 218951, 245838,
            //bone pile
            218951,245838,
            // rrrix act 1
            108882, 245919, 5944, 165475, 199998, 168875, 105323, 85690, 105321, 108266, 89578,
            175181, // trDun_Crypt_Skeleton_King_Throne_Parts 
            // rrrix act 2
            213907, 92519, 61544, 105681, 113983, 114527, 114642, 139933, 144405, 156890, 164057, 164195, 180254, 180802, 180809, 181173, 181174, 181177, 181181,
            181182, 181185, 181290, 181292, 181306, 181309, 181313, 181326, 181563, 181857, 181858, 187265, 191433, 191462, 191641, 192880, 192881, 196413, 196435,
            197280, 199191, 199264, 199274, 199597, 199664, 200979, 200982, 201236, 201580, 201581, 201583, 204183, 205746, 205756, 210087, 213907,  219223,
            220114, 3011, 3205, 3539, 3582, 3584, 3595, 3600, 4580, 52693, 5466, 55005, 5509, 62522,
            205756, 5509, 200371, 167185, 181195, 217346, 178161, 60108, 
            // rrrix act 3
            182443, 211456,
            // uber fire chains in Realm of Turmoil and Iron Gate in Realm of Chaos
            263014,
            5724, 5727,
            5869, // trDun_Gargoyle_01
            5738, // trDun_Cath_Breakable_pillar

            105478, // a1dun_Leor_Spike_Spawner_Switch
            /*
             * A5
             */

            // Pandemonium Fortress
            357297, // X1_Fortress_Rack_C
            //374196, // X1_Fortress_Rack_C_Stump
            357295, // X1_Fortress_Rack_B
            //374195, // X1_Fortress_Rack_B_Stump
            357299, // X1_Fortress_Rack_D
            //374197, // X1_Fortress_Rack_D_Stump
            357301, // X1_Fortress_Rack_E
            //374198, // X1_Fortress_Rack_E_Stump
            357306, // X1_Fortress_Rack_F
            //374199, // X1_Fortress_Rack_F_Stump
            365503, // X1_Fortress_FloatRubble_A
            365562, // X1_Fortress_FloatRubble_B
            365580, // X1_Fortress_FloatRubble_C
            365602, // X1_Fortress_FloatRubble_D
            365611, // X1_Fortress_FloatRubble_E
            365739, // X1_Fortress_FloatRubble_F

            355365, // x1_Abattoir_furnaceWall

            304313, // x1_abattoir_furnace_01 
            375383, // x1_Abattoir_furnaceSpinner_Event_Phase2 -- this is a rotating avoidance, with a fire "beam" about 45f in length

            265637, // x1_Catacombs_Weapon_Rack_Raise

            321479, // x1_Westm_HeroWorship03_VO

            328008, // X1_Westm_Door_Giant_Closed
            312441, // X1_Westm_Door_Giant_Opening_Event

            328942, // x1_Pand_Ext_ImperiusCharge_Barricade 
            324867, // x1_Westm_DeathOrb_Caster_TEST
            313302, // X1_Westm_Breakable_Wolf_Head_A

            368268, // x1_Urzael_SoundSpawner
            368626, // x1_Urzael_SoundSpawner
            368599, // x1_Urzael_SoundSpawner
            368621, // x1_Urzael_SoundSpawner

            377253, // x1_Fortress_Crystal_Prison_Shield
            316221, // X1_WarpToPortal 
            370187, // x1_Malthael_Boss_Orb_Collapse
            328830, // x1_Fortress_Portal_Switch
            374174, // X1_WarpToPortal

            356639, // x1_Catacombs_Breakable_Window_Relief

            //x1_Westm_HeroWorship01_VO Dist=10 IsElite=False LoS=True HP=1,00 Dir=SW - Name=x1
            //It's trying to attack that in town
            //x1_Westm_HeroWorship01_VO = 321451,
            //x1_Westm_HeroWorship02_VO = 321454,
            //x1_Westm_HeroWorship03_VO = 321479,
            321451, 321454, 321479,


            // Bulba
            430733, //A1 Templar Inquisition
            432259, //A1 The Triune Reborn
            434366, //A1 : Wortham Survivors 
            432770, //A1: Queen's Desert
            433051, //A2: Prisoners of the Cult
            432331, //A2: Blood and Iron
            432885, //A2: The Ancient Devices
            433184, //A3: The Lost Patrol
            433295, //A3: The Demon Gates
            433385, //A3: Catapult Command (It's a switch instead of chest, so might be pre 2.2 bounty)
            433402, //A4: Hell Portals
            433124, //A4: Tormented Angels
            433246, //A5: Death's Embrace
            433316, //A5: Rathma's Gift
			
			435630, // Pinger 1
			434971, // Pinger 2

            175603, //A3 - Oasis - a2dun_Aqd_Act_Waterwheel_Lever_A_01_WaterPuzzle

        };

        /// <summary>
        /// A list of LevelAreaId's that the bot should always ignore Line of Sight
        /// </summary>
        public static HashSet<int> NeverRaycastLevelAreaIds { get { return neverRaycastLevelAreaIds; } }
        private static readonly HashSet<int> neverRaycastLevelAreaIds = new HashSet<int>()
        {
            405915, // p1_TieredRift_Challenge 
        };


        public static HashSet<int> AlwaysRaycastWorlds { get { return DataDictionary.alwaysRaycastWorlds; } }
        private static readonly HashSet<int> alwaysRaycastWorlds = new HashSet<int>()
        {
            271233, // Pandemonium Fortress 1
            271235, // Pandemonium Fortress 2
        };



        /// <summary>
        /// Last used-timers for all abilities to prevent spamming D3 memory for cancast checks too often
        /// These should NEVER need manually editing
        /// But you do need to make sure every skill used by Trinity is listed in here once!
        /// </summary>
        public static Dictionary<SNOPower, DateTime> LastUseAbilityTimeDefaults
        {
            get { return lastUseAbilityTimeDefaults; }
            internal set { lastUseAbilityTimeDefaults = value; }
        }
        private static Dictionary<SNOPower, DateTime> lastUseAbilityTimeDefaults = new Dictionary<SNOPower, DateTime>
        {
                {SNOPower.DrinkHealthPotion, DateTime.MinValue},
                {SNOPower.Weapon_Melee_Instant, DateTime.MinValue},
                {SNOPower.Weapon_Ranged_Instant, DateTime.MinValue},
            };

        public static HashSet<int> ForceUseWOTBIds { get { return DataDictionary.forceUseWOTBIds; } }
        private static readonly HashSet<int> forceUseWOTBIds = new HashSet<int>()
        {
            256015, 256000, 255996
        };

        public static HashSet<int> IgnoreUntargettableAttribute { get { return DataDictionary.ignoreUntargettableAttribute; } }
        private static readonly HashSet<int> ignoreUntargettableAttribute = new HashSet<int>()
        {
            5432, // A2 Snakem
        };

        public static HashSet<string> WhiteItemCraftingWhiteList { get { return whiteItemCraftingWhiteList; } }

        private static HashSet<string> whiteItemCraftingWhiteList = new HashSet<string>()
        {
            "Ascended Pauldrons",
            "Ascended Armor",
            "Ascended Bracers",
            "Ascended Crown",
            "Ascended Pauldrons",
            "Archon Sash",
            "Ascended Faulds",
            "Ascended Greaves",
            "Ascended Gauntlets",

            "Limb Cleaver",
            "Doubleshot",
            "Whirlwind Staff",
            "Flesh Render",
            "Penetrator",
            "Ascended Shield",
            "Punyal",
            "Dire Axe",
            "Tsunami Blade",
            "Kerykeion",
            "Steppes Smasher",
            "Grandfather Flail",
            "Oxybeles",
            "Persuader",
            "Skullsplitter",
            "Suwaiya",
            "Tecpatl",
            "Diabolic Wand"
        };
        // Chamber of Suffering (Butcher's Lair)
        public static HashSet<Vector3> ChamberOfSufferingSafePoints = new HashSet<Vector3>
        {
            new Vector3(122.3376f, 120.1721f, 0), // Center
            new Vector3(138.504f,  88.64854f, 0), // Top Left
            new Vector3(98.61596f, 95.93278f, 0), // Top
            new Vector3(93.04589f, 134.9459f, 0), // Top Right
            new Vector3(107.9791f, 150.6952f, 0), // Bottom Right
            new Vector3(146.8563f, 144.0836f, 0), // Bottom
            new Vector3(151.9562f, 104.8417f, 0), // Bottom Left
        };

        public static HashSet<int> TownLevelAreaIds = new HashSet<int>()
        {
            332339,168314,92945,270011
        };

        public static Dictionary<int, Vector3> ButcherPanelPositions = new Dictionary<int, Vector3>
        {
            { 201426, new Vector3(121, 121, 0)}, // ButcherLair_FloorPanel_MidMiddle_Base
            { 201242, new Vector3(158, 111, 0)}, // ButcherLair_FloorPanel_LowerLeft_Base
            { 200969, new Vector3(152, 151, 0)}, // ButcherLair_FloorPanel_LowerMid_Base
            { 201438, new Vector3(91, 91, 0)}, // ButcherLair_FloorPanel_UpperMid_Base
            { 201423, new Vector3(133, 78, 0)}, // ButcherLair_FloorPanel_UpperLeft_Base
            { 201464, new Vector3(107, 160, 0)}, // ButcherLair_FloorPanel_LowerRight_Base
            { 201454, new Vector3(80, 134, 0)}, // ButcherLair_FloorPanel_UpperRight_Base
        };

        public readonly static Dictionary<Item, SNOPower> PowerByItem = new Dictionary<Item, SNOPower>
        {
            { Legendary.HarringtonWaistguard, SNOPower.ItemPassive_Unique_Ring_685_x1 },
            { Legendary.PoxFaulds, SNOPower.itemPassive_Unique_Pants_007_x1 },
            { Legendary.RechelsRingOfLarceny, SNOPower.ItemPassive_Unique_Ring_508_x1 },
            //{ Legendary.BottomlessPotionofKulleAid, SNOPower.X1_Legendary_Potion_06 },
            //{ Legendary.PridesFall, SNOPower.ItemPassive_Unique_Helm_017_x1 },
            { Legendary.KekegisUnbreakableSpirit, SNOPower.ItemPassive_Unique_Ring_569_x1 },
        };

        public readonly static Dictionary<Item, string> MinionInternalNameTokenByItem = new Dictionary<Item, string>
        {
            { Legendary.Maximus, "DemonChains_ItemPassive" },
            { Legendary.HauntOfVaxo, "_shadowClone_" }
        };

        public readonly static Dictionary<Skill, Set> AllRuneSetsBySkill = new Dictionary<Skill, Set>
        {
            { Skills.Barbarian.FuriousCharge, Sets.TheLegacyOfRaekor },
            { Skills.Wizard.Archon, Sets.VyrsAmazingArcana },
        };

        public readonly static HashSet<TrinityObjectType> InteractableTypes = new HashSet<TrinityObjectType>
        {
            TrinityObjectType.Item,
            TrinityObjectType.Door,
            TrinityObjectType.Container,
            TrinityObjectType.HealthWell,
            TrinityObjectType.CursedChest,
            TrinityObjectType.Interactable,
            TrinityObjectType.Shrine,
            TrinityObjectType.CursedShrine
        };

        public readonly static HashSet<TrinityObjectType> DestroyableTypes = new HashSet<TrinityObjectType>
        {
            TrinityObjectType.Barricade,
            TrinityObjectType.Destructible,
            TrinityObjectType.HealthGlobe,
            TrinityObjectType.ProgressionGlobe
        };

        public readonly static HashSet<SNOActor> Shrines = new HashSet<SNOActor>
        {
            SNOActor.Shrine_Global_Fortune,
            SNOActor.Shrine_Global_Blessed,
            SNOActor.Shrine_Global_Frenzied,
            SNOActor.Shrine_Global_Reloaded,
            SNOActor.Shrine_Global_Enlightened,
            SNOActor.Shrine_Global_Hoarder,
            SNOActor.x1_LR_Shrine_Infinite_Casting,
            SNOActor.x1_LR_Shrine_Electrified,
            SNOActor.x1_LR_Shrine_Invulnerable,
            SNOActor.x1_LR_Shrine_Run_Speed,
            SNOActor.x1_LR_Shrine_Damage
        };

        internal static HashSet<string> ActorIgnoreNames = new HashSet<string>
        {
            "MarkerLocation",
            "Generic_Proxy",
            "Hireling",
            "Start_Location",
            "SphereTrigger",
            "Checkpoint",
            "ConductorProxyMaster",
            "BoxTrigger",
            "SavePoint",
            "TriggerSphere",
            "minimapicon",
        };

        internal static HashSet<string> ActorIgnoreNameParts = new HashSet<string>
        {
            "markerlocation",
            "start_location",
            "checkpoint",
            "savepoint",
            "triggersphere",
            "minimapicon",
            "proxy",
            "invisbox",
            "trigger",
            "invisible"
        };

        internal static HashSet<SNOAnim> ActorChargeAnimations = new HashSet<SNOAnim>
        {
            SNOAnim.Beast_start_charge_02,
            SNOAnim.Beast_charge_02,
            SNOAnim.Beast_charge_04,
            SNOAnim.Butcher_Attack_Charge_01_in,
            SNOAnim.Butcher_Attack_Chain_01_out,
            SNOAnim.Butcher_Attack_05_telegraph,
            SNOAnim.Butcher_Attack_Chain_01_in,
            SNOAnim.Butcher_BreakFree_run_01,
        };

        /// <summary>
        /// Actor types that we dont wan't to even look at from DB's ACD List.
        /// </summary>
        public static HashSet<ActorType> ExcludedActorTypes = new HashSet<ActorType>
        {
            ActorType.Environment,
            ActorType.ClientEffect,
            ActorType.AxeSymbol,
            ActorType.CustomBrain,
            ActorType.Invalid,
            //ActorType.ServerProp,
            //ActorType.Player,
            //ActorType.Projectile, //lots of avoidance are classified as projectile
            ActorType.Critter,
        };

        /// <summary>
        /// ActorSNO that we want to completely ignore
        /// </summary>
        public static HashSet<int> ExcludedActorIds = new HashSet<int>
        {
            -1,
            4176, // Generic Proxy
            5502, // Start Location
            375658, // Banter Trigger
            3462, // Box Trigger
            5466, // Sphere Trigger
            3461, // OneShot Box Trigger
            //6442, // Waypoint
            3795, // Checkpoint
            5992, // OneShot Trigger Sphere
            180941 // SavePoint
        };

        public static HashSet<MonsterType> NonHostileMonsterTypes = new HashSet<MonsterType>
        {
            MonsterType.Ally,
            MonsterType.Scenery,
            MonsterType.Team,
            MonsterType.None,
            MonsterType.Helper,
        };

        public static Dictionary<int, MonsterType> MonsterTypeOverrides = new Dictionary<int, MonsterType>
        {
            { 86624, MonsterType.Undead }, // Jondar, DB thinks he's permanent ally
        };

        public static HashSet<int> AvoidanceBuffSNO = new HashSet<int>
        {
            201454,
            201464,
            201426,
            201438,
            200969,
            201423,
            201242,
        };

        public static HashSet<int> WhirlWindIgnoreSNO = new HashSet<int>
        {
            4304,
            5984,
            5985,
            5987,
            5988,
        };

        public static HashSet<int> FastMonsterSNO = new HashSet<int>
        {
            5212
        };

        public static Dictionary<int, AvoidanceType> AvoidanceTypeSNO = new Dictionary<int, AvoidanceType>
        {
            {349774, AvoidanceType.FrozenPulse},
            {343539, AvoidanceType.Orbiter},
            {316389, AvoidanceType.PoisonEnchanted},
            {340319, AvoidanceType.PoisonEnchanted},
            {341512, AvoidanceType.Thunderstorm},
            {337109, AvoidanceType.Wormhole},
            {123839, AvoidanceType.AzmodanBody},
            {123124, AvoidanceType.AzmodanPool},
            {123842, AvoidanceType.AzmoFireball},
            {219702, AvoidanceType.Arcane},
            {221225, AvoidanceType.Arcane},
            {3337, AvoidanceType.BeastCharge},
            {5212, AvoidanceType.BeeWasp},
            {161822, AvoidanceType.Belial},
            {161833, AvoidanceType.Belial},
            {201454, AvoidanceType.ButcherFloorPanel},
            {201464, AvoidanceType.ButcherFloorPanel},
            {201426, AvoidanceType.ButcherFloorPanel},
            {201438, AvoidanceType.ButcherFloorPanel},
            {200969, AvoidanceType.ButcherFloorPanel},
            {201423, AvoidanceType.ButcherFloorPanel},
            {201242, AvoidanceType.ButcherFloorPanel},
            {226350, AvoidanceType.DiabloRingOfFire},
            {226525, AvoidanceType.DiabloRingOfFire},
            {84608, AvoidanceType.Desecrator},
            {93837, AvoidanceType.GhomGas},
            {3847, AvoidanceType.Grotesque},
            {168031, AvoidanceType.DiabloPrison},
            {214845, AvoidanceType.DiabloMeteor},
            {402, AvoidanceType.IceBall},
            {223675, AvoidanceType.IceBall},
            {260377, AvoidanceType.IceTrail},
            {432, AvoidanceType.MageFire},
            {166686, AvoidanceType.MaghdaProjectille},
            {160154, AvoidanceType.MoltenBall},
            {4803, AvoidanceType.MoltenCore},
            {4804, AvoidanceType.MoltenCore},
            {224225, AvoidanceType.MoltenCore},
            {247987, AvoidanceType.MoltenCore},
            {95868, AvoidanceType.MoltenTrail},
            {250031, AvoidanceType.Mortar},
            {108869, AvoidanceType.PlagueCloud},
            {3865, AvoidanceType.PlagueHand},
            {5482, AvoidanceType.PoisonTree},
            {6578, AvoidanceType.PoisonTree},
            {4103, AvoidanceType.ShamanFire},
            //{185924, AvoidanceType.ZoltBubble},
            {139741, AvoidanceType.ZoltTwister}
        };

        public static Dictionary<int, AvoidanceType> AvoidanceProjectileSNO = new Dictionary<int, AvoidanceType>
        {
            { 343539, AvoidanceType.Orbiter },
            { 316389, AvoidanceType.PoisonEnchanted },
            { 340319, AvoidanceType.PoisonEnchanted },
            { 5212, AvoidanceType.BeeWasp },
            { 4103, AvoidanceType.ShamanFire },
            { 160154, AvoidanceType.MoltenBall },
            { 123842, AvoidanceType.AzmoFireball },
            { 139741, AvoidanceType.ZoltTwister },
            { 166686, AvoidanceType.MaghdaProjectille },
            { 185999, AvoidanceType.DiabloRingOfFire },
            { 136533, AvoidanceType.DiabloLightning },
        };

        public static HashSet<SNOPower> CheckVariantBuffs = new HashSet<SNOPower>
        {
            SNOPower.ItemPassive_Unique_Ring_735_x1,
            SNOPower.ItemPassive_Unique_Ring_735_x1,
        };

        public static HashSet<int> GoldSNO = new HashSet<int>
        {
            //GoldCoin
            376,
            //GoldLarge
            4311,
            //GoldMedium
            4312,
            //GoldSmall
            4313,
            //PlacedGold
            166389,
            //GoldCoins
            209200,
        };

        public static HashSet<int> HealthGlobeSNO = new HashSet<int>
        {
            //HealthGlobe
            4267,
            //HealthGlobe_02
            85798,
            //healthGlobe_swipe
            85816,
            //HealthGlobe_03
            209093,
            //HealthGlobe_04
            209120,
            //X1_NegativeHealthGlobe
            333196,
            //x1_healthGlobe
            366139,
            //x1_healthGlobe_playerIsHealed_attract
            367978,
            //HealthGlobe_steak
            375124,
            //HealthGlobe_steak_02
            375125,
            //x1_healthGlobe_steak_model
            375132,
        };

        public static HashSet<int> HealthWellSNO = new HashSet<int>
        {
            //caOut_Healthwell
            3648,
            //HealthWell_Global
            138989,
            //HealthWell_Water_Plane
            139129,
            //a4_Heaven_HealthWell_Global
            218885,
            //a4dun_DIablo_Arena_Health_Well
            180575,
        };

        public static HashSet<int> ShrineSNO = new HashSet<int>
        {
            //shrine_fxSphere_corrupt
            5333,
            //Shrine_Global
            135384,
            //shrine_fxGeo_model_Global
            139931,
            //Shrine_Global_Glow
            156680,
            //Shrine_Global_Blessed
            176074,
            //Shrine_Global_Enlightened
            176075,
            //Shrine_Global_Fortune
            176076,
            //Shrine_Global_Frenzied
            176077,
            //a4_Heaven_Shrine_Global_Blessed
            225025,
            //a4_Heaven_Shrine_Global_Fortune
            225027,
            //a4_Heaven_Shrine_Global_Frenzied
            225028,
            //a4_Heaven_Shrine_Global_Enlightened
            225030,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Blessed
            225261,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Enlightened
            225262,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Fortune
            225263,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Frenzied
            225266,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Hoarder
            260342,
            //a4_Heaven_Shrine_Global_DemonCorrupted_Reloaded
            260343,
            //a4_Heaven_Shrine_Global_Hoarder
            260344,
            //a4_Heaven_Shrine_Global_Reloaded
            260345,
            //Shrine_Global_Hoarder
            260346,
            //Shrine_Global_Reloaded
            260347,
            //PVP_Shrine_Murderball
            275729,
            //x1_LR_Shrine_Damage
            330695,
            //x1_LR_Shrine_Electrified
            330696,
            //x1_LR_Shrine_Infinite_Casting
            330697,
            //x1_LR_Shrine_Invulnerable
            330698,
            //x1_LR_Shrine_Run_Speed
            330699,
            //x1_Event_CursedShrine
            364601,
            //x1_Event_CursedShrine_Heaven
            368169,
            //x1_player_isShielded_riftShrine_model
            369696,
            //x1_LR_Shrine_Electrified_TieredRift
            398654,
            //shrine_Shadow
            434722,
        };

        public static HashSet<int> CursedShrineSNO = new HashSet<int>
        {
            //x1_Event_CursedShrine
            364601,
            //x1_Event_CursedShrine_Heaven
            368169,
        };

        public static HashSet<int> CursedChestSNO = new HashSet<int>
        {
            //x1_Global_Chest_CursedChest
            364559,
            //x1_Global_Chest_CursedChest_B
            365097,
            //x1_Global_Chest_CursedChest_B_MutantEvent
            374391,
        };

        public static HashSet<int> AvoidanceSNO = new HashSet<int>
        {
            //monsterAffix_Frozen_deathExplosion_Proxy
            402,
            //monsterAffix_Molten_deathStart_Proxy
            4803,
            //monsterAffix_Molten_deathExplosion_Proxy
            4804,
            //monsterAffix_Electrified_deathExplosion_proxy
            4806,
            //monsterAffix_Desecrator_telegraph
            84606,
            //monsterAffix_Desecrator_damage_AOE
            84608,
            //monsterAffix_Vortex_proxy
            85809,
            //monsterAffix_Vortex_model
            89862,
            //monsterAffix_Molten_trail
            95868,
            //monsterAffix_healthLink_jumpActor
            98220,
            //monsterAffix_Plagued_endCloud
            108869,
            //monsterAffix_frenzySwipe
            143266,
            //monsterAffix_vortex_target_trailActor
            210407,
            //monsterAffix_missileDampening_shield_add
            219458,
            //MonsterAffix_ArcaneEnchanted_PetSweep
            219702,
            //monsterAffix_missileDampening_outsideGeo
            220191,
            //MonsterAffix_ArcaneEnchanted_PetSweep_reverse
            221225,
            //MonsterAffix_ArcaneEnchanted_Proxy
            221560,
            //MonsterAffix_ArcaneEnchanted_trailActor
            221658,
            //monsterAffix_frozen_iceClusters
            223675,
            //monsterAffix_plagued_groundGeo
            223933,
            //monsterAffix_molten_fireRing
            224225,
            //monsterAffix_waller_wall
            226296,
            //monsterAffix_Avenger_glowSphere
            226722,
            //monsterAffix_ghostly_distGeo
            226799,
            //monsterAffix_waller_model
            226808,
            //monsterAffix_invulnerableMinion_distGeo
            227697,
            //monsterAffix_linked_chainHit
            228275,
            //monsterAffix_entangler_ringGlow_geo
            228885,
            //monsterAffix_molten_bomb_buildUp_geo
            247980,
            //monsterAffix_invulnerableMinion_colorGeo
            248043,
            //MonsterAffix_Mortar_Pending
            250031,
            //x1_MonsterAffix_CorpseBomber_projectile
            316389,
            //X1_MonsterAffix_corpseBomber_bomb
            325761,
            //X1_MonsterAffix_LightningStorm_Wanderer
            328307,
            //X1_MonsterAffix_TeleportMines
            337109,
            //x1_MonsterAffix_CorpseBomber_bomb_start
            340319,
            //x1_MonsterAffix_Thunderstorm_Impact
            341512,
            //X1_MonsterAffix_Orbiter_Projectile
            343539,
            //X1_MonsterAffix_Orbiter_FocalPoint
            343582,
            //x1_Spawner_Skeleton_MonsterAffix_World_1
            345764,
            //x1_Spawner_Skeleton_MonsterAffix_World_2
            345765,
            //x1_Spawner_Skeleton_MonsterAffix_World_3
            345766,
            //x1_Spawner_Skeleton_MonsterAffix_World_4
            345767,
            //x1_Spawner_Skeleton_MonsterAffix_World_5
            345768,
            //x1_MonsterAffix_orbiter_projectile_orb
            346805,
            //x1_MonsterAffix_orbiter_projectile_focus
            346837,
            //x1_MonsterAffix_orbiter_glowSphere
            346839,
            //x1_MonsterAffix_frozenPulse_monster
            349774,
            //x1_MonsterAffix_frozenPulse_shard
            349779,
            //x1_monsteraffix_mortar_blastwave
            365830,
            //x1_MonsterAffix_frozenPulse_shard_search
            366924,
            //x1_monsterAffix_generic_coldDOT_runeGeo
            377326,
            //x1_monsterAffix_generic_coldDOT_rune_emitter
            377374,
            //MonsterAffix_Avenger_ArcaneEnchanted_PetSweep
            384431,
            //MonsterAffix_Avenger_ArcaneEnchanted_PetSweep_reverse
            384433,
            //X1_MonsterAffix_Avenger_Orbiter_Projectile
            384575,
            //X1_MonsterAffix_Avenger_Orbiter_FocalPoint
            384576,
            //x1_MonsterAffix_Avenger_CorpseBomber_bomb_start
            384614,
            //x1_MonsterAffix_Avenger_CorpseBomber_projectile
            384617,
            //x1_MonsterAffix_Avenger_frozenPulse_monster
            384631,
            //x1_MonsterAffix_Avenger_arcaneEnchanted_dummySpawn
            386997,
            //x1_MonsterAffix_Avenger_ArcaneEnchanted_trailActor
            387010,
            //x1_MonsterAffix_Avenger_orbiter_projectile_orb
            387679,
            //x1_MonsterAffix_Avenger_orbiter_projectile_focus
            388435,
            //x1_MonsterAffix_Avenger_corpseBomber_slime
            389483,    
	        //x1_Bog_bloodSpring_medium
            332922,
	        //x1_Bog_bloodSpring_large
            332923,
	        //x1_Bog_bloodSpring_small
            332924,
            //p4_RatKing_RatBallMonster
            427170,
            //x1_LR_Boss_MalletDemon_FallingRocks
            368453,
            //X1_LR_Boss_FireNova_projectile
            373937,
            //X1_LR_Boss_AsteroidRain
            378237,
            //x1_LR_boss_terrorDemon_A_projectile
            434843,

        };

        public static HashSet<int> PlayerBannerSNO = new HashSet<int>
        {
            //Banner_Player_1
            123714,
            //Banner_Player_2
            123715,
            //Banner_Player_3
            123716,
            //Banner_Player_4
            123717,
            //Banner_Player_1_Act2
            212879,
            //Banner_Player_2_Act2
            212880,
            //Banner_Player_3_Act2
            212881,
            //Banner_Player_4_Act2
            212882,
            //Banner_Player_1_Act5
            367451,
            //Banner_Player_2_Act5
            367452,
            //Banner_Player_3_Act5
            367453,
            //Banner_Player_4_Act5
            367454,
        };

        public static HashSet<int> PlayerSNO = new HashSet<int>
        {
            //Wizard_Female
            6526,
            //Wizard_Female_characterSelect
            6527,
            //Wizard_Male
            6544,
            //Wizard_Male_characterSelect
            6545,
            //Wizard_Male_FrontEnd
            218883,
            //Wizard_Female_FrontEnd
            218917,
            //Barbarian_Female
            3285,
            //Barbarian_Female_characterSelect
            3287,
            //Barbarian_Male
            3301,
            //Barbarian_Male_characterSelect
            3302,
            //Barbarian_Male_FrontEnd
            218882,
            //Barbarian_Female_FrontEnd
            218909,
            //Demonhunter_Female_FrontEnd
            218911,
            //Demonhunter_Male_FrontEnd
            218912,
            //Demonhunter_Female
            74706,
            //Demonhunter_Male
            75207,
            //X1_Crusader_Male
            238284,
            //X1_Crusader_Female
            238286,
            //X1_Crusader_Male_FrontEnd
            238287,
            //X1_Crusader_Female_FrontEnd
            238288,
            //Crusader_Female_characterSelect
            279361,
            //Crusader_Male_characterSelect
            279362,
            //WitchDoctor_Male
            6485,
            //WitchDoctor_Male_characterSelect
            6486,
            //WitchDoctor_Female
            6481,
            //WitchDoctor_Female_characterSelect
            6482,
            //Monk_Male
            4721,
            //Monk_Male_characterSelect
            4722,
            //Monk_Female
            4717,
            //Monk_Female_characterSelect
            4718,
        };

        public static HashSet<int> SummonerSNO = new HashSet<int>
        {
            //TriuneSummoner_fireBall_obj
            467,
            //SkeletonSummoner_A
            5387,
            //SkeletonSummoner_B
            5388,
            //SkeletonSummoner_C
            5389,
            //SkeletonSummoner_D
            5390,
            //TriuneSummoner_A
            6035,
            //TriuneSummoner_B
            6036,
            //TriuneSummoner_C
            6038,
            //TriuneSummoner_D
            6039,
            //SkeletonSummoner_A_TemplarIntro
            104728,
            //TownAttack_TriuneSummonerBoss_C
            105539,
            //triuneSummoner_summonRope_glow
            111554,
            //TriuneSummoner_B_RabbitHoleEvent
            111580,
            //Spawner_SkeletonSummoner_A_Immediate_Chand
            117947,
            //TriuneSummoner_A_Unique_SwordOfJustice
            131131,
            //a4_heaven_hellportal_summoner_loc
            143502,
            //TownAttack_SummonerSpawner
            173527,
            //TownAttack_Summoner
            178297,
            //TownAttack_Summoner_Unique
            178619,
            //SkeletonSummoner_E
            182279,
            //TriuneSummoner_A_CainEvent
            186039,
            //TriuneSummoner_A_Unique_01
            218662,
            //TriuneSummoner_A_Unique_02
            218664,
            //TriuneSummoner_C_Unique_01
            222001,
            //x1_TriuneSummoner_WestMCultist
            288215,
            //x1_Spawner_TriuneSummoner_A
            290730,
            //x1_Spawner_SkeletonSummoner_A
            292297,
            //x1_westm_Soul_Summoner
            298827,
            //x1_westm_Soul_Summoner_Hands
            301425,
            //x1_Spawner_SkeletonSummoner_D
            303859,
            //x1_TEST_FallenChampion_LR_Summoner
            308285,
            //x1_westm_Soul_Summoner_Spawner
            308823,
            //x1_soul_summoner_hands_trail
            316395,
            //x1_westm_Soul_Summoner_twoHands
            316560,
            //x1_Soul_Summoner_glowSphere
            316716,
            //x1_westm_Soul_Summoner_GhostChase
            330609,
            //x1_devilshand_unique_SkeletonSummoner_B
            332432,
            //x1_devilshand_unique_TriuneSummoner_C
            332433,
            //x1_TriuneSummoner_C_Unique_01
            341240,
            //X1_LR_Boss_SkeletonSummoner_C
            359094,
            //x1_Heaven_Soul_Summoner
            361480,
            //TriuneSummoner_Unique_Cultist_Leader_Hershberg
            396812,
            //TriuneSummoner_Unique_Cultist_Leader_Son_of_Jacob
            396836,
            //TriuneSummoner_Unique_Cultist_Leader_Poirier
            396849,
            //TriuneSummoner_Unique_Cultist_Leader_Buckley
            396863,
        };




        /// <summary>
        /// SNOs that belong to a player, spells, summons, effects, projectiles etc.
        /// </summary>
        public static HashSet<int> PlayerOwnedSNO = new HashSet<int>
        {
            493, 3368, 6509, 6511, 6513, 6514, 6515, 6516, 6519, 6522, 6523, 6524, 6528, 6535, 6542, 6543, 6550, 6551, 6553, 6554,
            6558, 6560, 6561, 6562, 6563, 58362, 61419, 61445, 62054, 71129, 75631, 75642, 75726, 75731, 75732, 76019, 77097, 77098,
            77116, 77333, 77805, 80600, 80745, 80757, 80758, 81103, 81229, 81230, 81231, 81232, 81238, 81239, 81515, 82109, 82111,
            82660, 82972, 83024, 83025, 83028, 83043, 83959, 83964, 84504, 86082, 86769, 86790, 87621, 87913, 88032, 90364, 91424,
            91440, 91441, 91702, 92030, 92031, 92032, 93067, 93076, 93560, 93582, 93592, 93718, 97691, 97821, 98010, 99565, 99566,
            99567, 99572, 99574, 99629, 107916, 112560, 112572, 112585, 112588, 112594, 112675, 112697, 112806, 112808, 112811,
            117557, 130029, 130030, 130035, 130073, 130074, 130668, 141936, 144003, 147977, 148060, 148070, 148077, 148220, 148634,
            148700, 149837, 154769, 161695, 161772, 162301, 164699, 166051, 166130, 166172, 167260, 167261, 167262, 167263, 167382,
            167397, 167419, 167463, 167564, 167628, 167724, 167807, 167814, 167817, 167978, 170169, 170199, 170268, 170287, 170385,
            170405, 170445, 170496, 170574, 170592, 170935, 171179, 171180, 171184, 171185, 171225, 171226, 176247, 176248, 176262,
            176265, 176287, 176288, 176356, 176390, 176407, 176440, 176600, 176653, 185106, 185125, 185226, 185233, 185263, 185273,
            185283, 185301, 185309, 185316, 185459, 185513, 185660, 185661, 185662, 185663, 189372, 189373, 189375, 189458, 189460,
            191967, 192126, 192210, 192211, 192271, 199154, 201526, 210804, 210896, 215324, 215420, 215488, 215511, 215516, 215700,
            215711, 215809, 215853, 216040, 216069, 216462, 216529, 216817, 216818, 216851, 216874, 216890, 216897, 216905, 216941,
            216956, 216975, 216988, 217121, 217130, 217139, 217142, 217172, 217180, 217287, 217307, 217311, 217457, 217458, 217459,
            219070, 219196, 219200, 219254, 219292, 219295, 219300, 219314, 219315, 219316, 219391, 219392, 219393, 226648, 249225,
            249226, 249227, 249228, 249975, 249976, 251688, 251689, 251690, 261341, 261342, 261343, 261344, 261616, 261617, 299099,
            300476, 302468, 314792, 315588, 316207, 316239, 316270, 316271, 317398, 317409, 317501, 317507, 317652, 317809, 319692,
            319698, 319732, 319771, 322022, 322236, 322350, 322406, 322488, 323029, 323092, 323149, 323897, 324143, 324451, 324459,
            324466, 325154, 325552, 325804, 325807, 325813, 325815, 326285, 326305, 326308, 326313, 326755, 328146, 328161, 328171,
            328199, 336410, 337757, 339443, 339473, 341373, 341381, 341396, 341410, 341411, 341412, 341426, 341427, 341441, 341442,
            342082, 343197, 343293, 343300, 347101, 362960, 362961, 366983, 381908, 381915, 381917, 381919, 385215, 385216, 394102,
            396290, 396291, 396292, 396293, 409287, 409352, 409430, 409523, 322, 3276, 3277, 3280, 3281, 3282, 3283, 3289, 3290,
            3291, 3297, 3298, 3303, 3305, 3306, 3308, 3309, 3310, 3314, 3315, 3316, 3317, 3319, 51303, 74636, 79400, 90443, 90535,
            90536, 92895, 93481, 93903, 99541, 100800, 100832, 100839, 100934, 101068, 108742, 108746, 108767, 108772, 108784,
            108789, 108808, 108819, 108868, 108907, 108920, 109151, 136261, 143994, 158990, 159030, 159614, 159626, 159631, 159940,
            160587, 160685, 160818, 160893, 161452, 161457, 161599, 161607, 161654, 161657, 161890, 161892, 161893, 161894, 161960,
            162005, 162087, 162114, 162386, 162387, 162548, 162577, 162590, 162593, 162621, 162622, 162623, 162766, 162839, 162920,
            162929, 163353, 163462, 163494, 163501, 163541, 163552, 163783, 163792, 163861, 163925, 163949, 163968, 164066, 164112,
            164708, 164709, 164710, 164712, 164713, 164714, 164747, 164770, 164788, 164804, 165040, 165043, 165069, 165381, 165382,
            165514, 165515, 165560, 165561, 165988, 166214, 166222, 166223, 166438, 168307, 168440, 168460, 173342, 174723, 189078,
            189094, 198346, 209487, 220559, 220562, 220565, 220569, 220632, 252478, 252479, 317733, 356731, 358574, 360571, 362283,
            362949, 362951, 363301, 363760, 363765, 364460, 364953, 365194, 365291, 365338, 365340, 365342, 365534, 365789, 373063,
            373074, 373211, 374667, 374683, 375757, 396470, 408515, 408532, 410567, 75678, 75887, 77569, 77604, 77813, 87564, 88244,
            88251, 111307, 111330, 111503, 129228, 129603, 129621, 129785, 129787, 129788, 129934, 131701, 141937, 143995, 147809,
            148845, 148846, 148847, 149944, 149946, 149947, 149948, 151832, 151842, 153029, 153864, 153865, 153866, 153867, 153868,
            154674, 155092, 155147, 155749, 155938, 158843, 158844, 158916, 158917, 158940, 158941, 160932, 165558, 166549, 166550,
            166556, 166557, 166582, 166583, 166584, 166585, 166618, 166619, 166620, 166621, 166636, 166637, 167169, 167171, 167172,
            167218, 194565, 194566, 200561, 200672, 200808, 212547, 314804, 347447, 349626, 360546, 360547, 360548, 360549, 360550,
            360561, 360563, 360564, 360697, 360773, 361118, 361213, 361214, 362954, 362955, 428572, 428574, 129784, 129932, 130366,
            130572, 130661, 131664, 131672, 132068, 132615, 132732, 133714, 133741, 134841, 134917, 135207, 136149, 141402, 141681,
            141734, 143853, 143854, 147960, 148788, 148900, 149338, 149770, 149790, 149935, 149949, 149975, 150024, 150025, 150026,
            150027, 150036, 150037, 150038, 150039, 150061, 150062, 150063, 150064, 150065, 150449, 151591, 151805, 151929, 151998,
            152116, 152269, 152589, 152736, 152857, 152863, 153075, 153352, 154093, 154194, 154198, 154199, 154200, 154201, 154227,
            154292, 154590, 154591, 154592, 154593, 154595, 154657, 154736, 154750, 154811, 155096, 155149, 155159, 155276, 155280,
            155353, 155374, 155376, 155734, 155848, 156100, 157728, 159098, 159102, 159144, 160612, 162563, 165340, 165467, 165767,
            166462, 166613, 166732, 167223, 167235, 168815, 173827, 178664, 180622, 180640, 181748, 182234, 182263, 186050, 193463,
            193493, 193496, 193497, 193499, 193500, 196030, 196615, 200810, 215242, 215727, 218467, 218504, 219494, 219509, 219534,
            219577, 219580, 219609, 219610, 220527, 220800, 221261, 221440, 222109, 222115, 222117, 222122, 222128, 222130, 222135,
            222141, 222143, 222151, 230674, 251704, 251710, 261665, 366893, 366897, 366921, 366933, 366935, 367223, 367232, 367251,
            367258, 370495, 370496, 375869, 375871, 376739, 376743, 396318, 405388, 406116, 408096, 408100, 408103, 408300, 408327,
            408333, 408335, 408379, 408422, 408568, 410274, 410299, 428075, 253211, 253416, 253717, 255056, 256083, 256180, 257376,
            257777, 257782, 258133, 263866, 265474, 266503, 277722, 277805, 277808, 280196, 280462, 280702, 284686, 284872, 284920,
            285209, 285380, 285476, 286805, 286925, 289010, 289991, 290291, 290315, 290325, 290330, 290340, 290460, 290494, 290508,
            290521, 290532, 291672, 292175, 292608, 292837, 292849, 293223, 293293, 293307, 293312, 293318, 293332, 293342, 293644,
            293682, 293701, 293725, 293762, 293769, 293773, 293797, 293843, 293850, 293866, 293872, 293895, 293912, 293915, 293919,
            293927, 293930, 293947, 293998, 294005, 294009, 294012, 294023, 294047, 294052, 294057, 294169, 294182, 294330, 294333,
            294336, 294340, 294348, 294351, 294355, 294358, 294361, 294365, 294368, 294371, 294375, 294378, 294381, 294436, 294439,
            294442, 294446, 294449, 294452, 294456, 294459, 294462, 294466, 294469, 294472, 294476, 294479, 294482, 294517, 294520,
            294523, 294527, 294530, 294533, 294537, 294540, 294543, 294547, 294550, 294553, 294557, 294560, 294563, 294732, 295009,
            295012, 295015, 300142, 304115, 306040, 306210, 306225, 308143, 312522, 312552, 312658, 312661, 314802, 316079, 316835,
            319402, 323582, 324046, 324050, 324059, 324081, 324126, 324856, 324878, 325092, 325528, 326101, 327375, 327987, 329016,
            330019, 330042, 330082, 330728, 332236, 332450, 332465, 332631, 332644, 332702, 332705, 332759, 332760, 335993, 335995,
            336138, 336201, 336202, 336203, 336279, 336285, 336286, 336339, 336356, 336360, 336361, 336468, 336710, 336968, 337088,
            337184, 338224, 338225, 338226, 338227, 338246, 338252, 338598, 338678, 338807, 339343, 340460, 342209, 342213, 342257,
            342332, 342336, 342562, 342581, 342587, 342919, 342938, 342940, 342945, 343022, 343099, 343180, 343250, 343262, 343614,
            343792, 343879, 343881, 343954, 344224, 344305, 344330, 344543, 344546, 344571, 344573, 344577, 344588, 345114, 345224,
            345228, 345232, 345249, 345295, 345296, 345305, 345682, 345787, 345800, 345892, 346151, 346291, 346293, 346915, 347135,
            347248, 347249, 347360, 347421, 347466, 347643, 347647, 347677, 347792, 347798, 348203, 348262, 348731, 348735, 348766,
            348991, 348993, 349465, 349483, 349485, 349498, 349509, 349510, 349534, 349540, 349705, 349895, 349932, 349942, 349943,
            349959, 349977, 349989, 349991, 350052, 350072, 350083, 350135, 350158, 350204, 350219, 350461, 350468, 350631, 350634,
            350636, 350685, 350686, 350706, 350808, 351031, 351051, 351093, 351139, 352414, 352433, 352455, 352677, 352680, 352683,
            352687, 352723, 352737, 352757, 352871, 352933, 352942, 352969, 352985, 353516, 353843, 354249, 354412, 354579, 354606,
            354969, 355126, 355171, 356719, 356879, 356904, 357248, 357258, 357261, 357317, 357350, 357351, 357355, 357356, 357358,
            357369, 357381, 357382, 357387, 357388, 357426, 357432, 357436, 357439, 357472, 357475, 357574, 357584, 357587, 357645,
            357802, 357803, 357903, 357914, 357916, 357946, 357949, 358001, 358010, 358106, 358235, 358236, 358243, 358244, 358245,
            358246, 359040, 359041, 359042, 359043, 359044, 359045, 359709, 362665, 362952, 362953, 363775, 363890, 369795, 369975,
            369976, 369977, 369978, 369979, 369980, 428280, 6443, 6450, 6451, 6452, 6453, 6457, 6459, 6463, 6465, 6489, 51353,
            59155, 61398, 71336, 102723, 103215, 103217, 103235, 104079, 105606, 105763, 105772, 105792, 105795, 105816, 105828,
            105829, 106426, 106561, 106569, 106593, 106841, 106862, 107507, 107662, 107881, 107889, 107899, 108238, 108389, 108520,
            108536, 108543, 108550, 108556, 108560, 109122, 109123, 110959, 111243, 111338, 111345, 111372, 111530, 111535, 111566,
            113765, 120950, 121595, 121904, 121908, 121919, 121920, 121960, 122281, 122305, 123587, 123910, 123911, 123912, 123913,
            131202, 131504, 131640, 135016, 146534, 171491, 171501, 171502, 175354, 179772, 179776, 179778, 179779, 179780, 181767,
            181773, 181818, 181842, 181867, 181871, 181880, 182042, 182050, 182056, 182095, 182102, 182119, 182136, 182153, 182574,
            182576, 182603, 182608, 182610, 182612, 183977, 184445, 184585, 184968, 184999, 185025, 186469, 188447, 188484, 191204,
            193964, 193965, 193966, 193967, 193968, 193969, 193970, 193971, 193972, 193973, 194308, 194359, 216050, 251637, 348308,
            354714, 356987, 356991, 357125, 357569, 357846, 358018, 358120, 358358, 358653, 359900, 361799, 366173, 376693, 395431,
            404291, 404352, 404785, 404802, 405092, 405095, 405096, 405097, 405098, 405104, 426125, 426135, 432690, 432691, 432692,
            432693, 432694, 432695, 6483, 6487, 6494, 69308, 71643, 74042, 74056, 101441, 105501, 105502, 105812, 105953, 105955,
            105956, 105957, 105958, 105969, 105977, 106385, 106502, 106504, 106584, 106731, 106749, 107011, 107030, 107031, 107035,
            107067, 107107, 107112, 107114, 107162, 107223, 107265, 107705, 110714, 112311, 112327, 112338, 112345, 112347, 117574,
            134115, 134121, 134123, 134125, 134127, 134128, 140874, 144001, 182271, 182276, 182278, 182283, 186485, 193295, 206229,
            206230, 215811, 215813, 215814, 215815, 215816, 215817, 215818, 215819, 215820, 215822, 215835, 215841, 215844, 215847,
            215852, 218915, 218916, 251609, 251610, 314817, 362958, 362959, 3919, 4699, 4700, 4701, 4716, 4719, 4724, 4725, 4730,
            4731, 51327, 59822, 71909, 97399, 97458, 97558, 98829, 98835, 98836, 98871, 98883, 98940, 99063, 99096, 99694, 101550,
            110525, 110526, 110546, 110549, 117329, 120216, 120239, 120365, 121145, 122566, 123865, 123885, 128993, 129183, 129197,
            129329, 129333, 136022, 136893, 136925, 137408, 137413, 137491, 137525, 137527, 137528, 137567, 137572, 137656, 137675,
            137752, 137761, 137767, 137779, 137780, 137781, 137848, 137859, 137928, 137943, 137964, 137968, 137976, 138261, 139403,
            139419, 139431, 139455, 139780, 139869, 139889, 140271, 140312, 140779, 140878, 141074, 141081, 141143, 141175, 141186,
            141192, 141198, 141341, 141354, 141376, 141577, 141581, 141622, 141630, 141700, 141736, 141773, 141938, 142048, 142433,
            142478, 142503, 142514, 142719, 142737, 142788, 142826, 142845, 142851, 143216, 143225, 143273, 143445, 143504, 143509,
            143513, 143598, 143601, 143759, 143770, 143773, 143776, 143797, 143799, 143800, 143806, 143814, 143818, 143996, 144045,
            144046, 144100, 144199, 144209, 144214, 144218, 144234, 144461, 144765, 144770, 144773, 144774, 144775, 144776, 144785,
            144786, 144788, 144789, 144790, 144791, 144832, 144837, 144838, 144840, 144841, 144842, 144843, 144891, 144949, 144950,
            144952, 144953, 144954, 144955, 144974, 145010, 145025, 145028, 145089, 145195, 145196, 145295, 145310, 145461, 145485,
            145503, 145541, 145659, 145685, 145709, 145715, 146041, 146048, 146502, 146593, 146596, 147043, 147136, 147227, 147239,
            147257, 147934, 147935, 147936, 147937, 147938, 149848, 149849, 149851, 150410, 150411, 150412, 150413, 150414, 150415,
            150416, 150417, 150418, 150419, 168878, 169077, 169123, 169890, 169891, 169904, 169905, 169906, 169907, 169908, 169909,
            170972, 171008, 172170, 172187, 172191, 172193, 172489, 176921, 176924, 176955, 181336, 181431, 181568, 182353, 182360,
            182365, 182370, 182384, 191350, 192095, 192103, 197887, 202172, 213766, 215087, 215205, 215635, 218913, 218914, 223650,
            224033, 224150, 224172, 247403, 247405, 247407, 249990, 249994, 305732, 305733, 305734, 305830, 305831, 305832, 317351,
            317352, 317353, 317354, 317355, 317356, 319337, 319583, 319776, 320135, 320136, 323124, 357875, 357880, 357881, 358124,
            358125, 358126, 358128, 358129, 362956, 362957, 363236, 363237, 366341, 367774, 374071, 374080, 374084, 376290, 391711,
            391762, 392434, 392477, 392611, 392620, 395888, 395892, 396441, 396442, 396443, 396444, 396445, 396650, 409250, 409362,
            409385, 409486, 409487, 409488, 409528, 409533, 409641, 409688, 409705, 409708, 409710, 409745, 409748, 409852, 409853,
            409858, 409861, 409930, 410107, 410215, 426074, 426080, 426081, 426083, 426091, 426092, 426095, 426103, 426106, 426107,
            426110, 426121, 426123,
        };

        #region Methods

        /// <summary>
        /// Add an ActorSNO to the blacklist. Returns false if the blacklist already contains the ActorSNO
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public static bool AddToBlacklist(int actorId)
        {
            if (!blacklistIds.Contains(actorId))
            {
                blacklistIds.Add(actorId);
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
