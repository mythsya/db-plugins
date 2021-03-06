﻿using System;

namespace Trinity
{
    /// <summary>
    /// Primary "lowest level" item type (eg EXACTLY what kind of item it is)
    /// </summary>
    public enum TrinityItemType
    {
        Unknown = 0,
        Amethyst,
        Amulet,
        Axe,
        Belt,
        Boots,
        Bracer,
        CeremonialKnife,
        Chest,
        Cloak,
        ConsumableAddSockets,
        CraftTome,
        CraftingMaterial,
        CraftingPlan,
        CrusaderShield,
        Dagger,
        Diamond,
        Dye,
        Emerald,
        FistWeapon,
        FollowerEnchantress,
        FollowerScoundrel,
        FollowerTemplar,
        Flail,
        Gloves,
        HandCrossbow,
        HealthGlobe,
        HealthPotion,
        Helm,
        HoradricRelic,
        HoradricCache,
        InfernalKey,
        Legs,
        LootRunKey,
        Mace,
        MightyBelt,
        MightyWeapon,
        Mojo,
        Orb,
        PowerGlobe,
        ProgressionGlobe,
        Quiver,
        Ring,
        Ruby,
        Shield,
        Shoulder,
        Spear,
        SpecialItem,
        SpiritStone,
        StaffOfHerding,
        Sword,
        TieredLootrunKey,
        Topaz,
        TwoHandAxe,
        TwoHandBow,
        TwoHandCrossbow,
        TwoHandDaibo,
        TwoHandFlail,
        TwoHandMace,
        TwoHandMighty,
        TwoHandPolearm,
        TwoHandStaff,
        TwoHandSword,
        VoodooMask,
        Wand,
        WizardHat,
    }

    [Flags]
    public enum ItemSelectionType : ulong
    {
        Unknown = 0,
        Amulet = 0x00000001,
        Axe = 0x00000002,
        Belt = 0x00000004,
        Boots = 0x00000008,
        Bracer = 0x00000010,
        CeremonialKnife = 0x00000020,
        Chest = 0x00000040,
        Cloak = 0x00000080,
        CrusaderShield = 0x00000100,
        Dagger = 0x00000200,
        FistWeapon = 0x00000400,
        Flail = 0x00000800,
        Gloves = 0x00001000,
        HandCrossbow = 0x00002000,
        Helm = 0x00004000,
        Legs = 0x00008000,
        Mace = 0x00010000,
        MightyBelt = 0x00020000,
        MightyWeapon = 0x00040000,
        Mojo = 0x00080000,
        Orb = 0x00100000,
        Quiver = 0x00200000,
        Ring = 0x00400000,
        Shield = 0x00800000,
        Shoulder = 0x01000000,
        Spear = 0x02000000,
        SpiritStone = 0x04000000,
        Sword = 0x08000000,
        TwoHandAxe = 0x10000000,
        TwoHandBow = 0x20000000,
        TwoHandCrossbow = 0x40000000,
        TwoHandDaibo = 0x80000000,
        TwoHandFlail = 0x100000000,
        TwoHandMace = 0x200000000,
        TwoHandMighty = 0x400000000,
        TwoHandPolearm = 0x800000000,
        TwoHandStaff = 0x1000000000,
        TwoHandSword = 0x2000000000,
        VoodooMask = 0x4000000000,
        Wand = 0x8000000000,
        WizardHat = 0x10000000000,
    }

    public enum ItemQualityColor
    {
        Unknown = 0,
        Grey,
        White,
        Blue,
        Yellow,
        Orange,
        Green,
        Other
    }

    public enum MysteryItemType
    {
        MysteryWeapon_1H,
        MysteryWeapon_2H,
        MysteryAmulet,
        MysteryBelt,
        MysteryBoots,
        MysteryBracers,
        MysteryChestArmor,
        MysteryGloves,
        MysteryHelm,
        MysteryMojo,
        MysteryOrb,
        MysteryPants,
        MysteryQuiver,
        MysteryRing,
        MysteryShield,
        MysteryShoulders,
    }

    //public enum KeepDistanceMode
    //{
    //    Never = 0,
    //    Everything,
    //    BossesOnly,
    //    BossesAndElites
    //}

    /// <summary>
    /// Base types, eg "one handed weapons" "armors" etc.
    /// </summary>
    public enum TrinityItemBaseType
    {
        Unknown,
        WeaponOneHand,
        WeaponTwoHand,
        WeaponRange,
        Offhand,
        Armor,
        Jewelry,
        FollowerItem,
        Misc,
        Gem,
        HealthGlobe,
        PowerGlobe,
        ProgressionGlobe,
    }

    /// <summary>
    /// Generic object types - eg a monster, an item to pickup, a shrine to click etc.
    /// </summary>
    public enum TrinityObjectType
    {
        Unknown,
        Avoidance,
        Backtrack,
        Barricade,
        Checkpoint,
        Container,
        Destructible,
        Door,
        HealthGlobe,
        Gold,
        HealthWell,
        HotSpot,
        Interactable,
        Item,
        MarkerLocation,
        Player,
        PowerGlobe,
        ProgressionGlobe,
        Proxy,
        SavePoint,
        ServerProp,
        Shrine,
        StartLocation,
        Trigger,
        Unit,
        CursedChest,
        CursedShrine,
        Portal,
        Banner,
        Waypoint,
    }

    [Flags]
    public enum Element
    {
        Unknown = 0,
        Arcane = 1 << 1,
        Cold = 1 << 2,
        Fire = 1 << 3,
        Holy = 1 << 4,
        Lightning = 1 << 5,
        Physical = 1 << 6,
        Poison = 1 << 7,   
        Any = Arcane | Cold | Fire | Holy | Lightning | Physical | Poison
    }

    public enum SpellCategory
    {
        Unknown = 0,
        Primary,
        Secondary,
        Defensive,
        Might,
        Tactics,
        Rage,
        Techniques,
        Focus,
        Mantras,
        Utility,
        Laws,
        Conviction,
        Voodoo,
        Decay,
        Terror,
        Hunting,
        Archery,
        Devices,
        Conjuration,
        Mastery,
        Force,
    }

    public enum Resource
    {
        Unknown = 0,
        None,
        Fury,
        Arcane,
        Wrath,
        Mana,
        Discipline,
        Hatred,
        Spirit
    }

    public enum ItemQualityName
    {
        Inferior,
        Normal,
        Magic,
        Rare,
        Legendary,
        Set,
        Special
    }

    public enum ResourceEffectType
    {
        None = 0,
        Generator,
        Spender
    }

    public enum AreaEffectShapeType
    {
        None = 0,
        Cone,                
        Circle,
        Beam        
    }

    [Flags]
    public enum EffectTypeFlags
    {        
        None            = 0,
        Stun            = 1 << 0,
        Knockback       = 1 << 1,
        Immobilize      = 1 << 2,
        Chill           = 1 << 3,
        Blind           = 1 << 4,
        Charm           = 1 << 5,
        Slow            = 1 << 6,
        Freeze          = 1 << 7, 

        Any = Freeze | Stun | Knockback | Immobilize | Chill | Blind | Charm | Slow
    }

    public enum IgnoreReason
    {        
        None = 0,
        Invalid,
        ObjectType,
        PlayerSummon,
        Environment,
        ClientEffect,
        Blacklist,
        ZDiff,
        IgnoreUnknown,
        IgnoreLoS,
        IgnoreName,
        Distance,
        Invulnerable,
        Invisible,
        PlayerHeadstone,
        NotInRange,
        DisabledByScript,
        NoDamage,
        HasBeenOperated,
        IsLocked,
        DoorAnimating,
        GizmoState1,
        Untargetable,
        EndAnimation,
        Settings,
        AlreadyOpen
    }
    

    public enum MonsterQuality
    {
        Normal = 0,
        Champion = 1,
        Rare = 2,
        Minion = 3,
        Unique = 4,
        Hireling = 5,
        Boss = 7
    }

    public enum TrinityPetType
    {
        Unknown = 0,
        Sentry,
        Hydra,
        ZombieDog,
        MysticAlly,
        Gargantuan,
        Companion,
        Hireling,
    }

    
    /// <summary>
    /// MonsterAffixes indexed by GameBalanceId
    /// </summary>
    public enum TrinityMonsterAffix
    {
        // Direct Cast from DiaUnit.Affixes is 5x faster than the MonsterAffixEntries/MonsterAffixes properties
        None = -1,
        ArcaneEnchanted = -1669589516,
        Avenger = 1165197192,
        Desecrator = -121983956,
        Electrified = -1752429632,
        Fast = 3775118,
        FireChains = -439707236,
        Frozen = -163836908,
        FrozenPulse = 1886876669,
        Frenzy = -164208642,
        HealthLink = 1799201764,
        Horde = 127452338,
        Illusionist = 394214687,
        Jailer = -27686857,
        Knockback = -2088540441,
        MissileDampening = -1412750743,
        Molten = 106438735,
        Mortar = 106654229,
        Nightmarish = -1245918914,
        Orbiter = 1905614711,
        Plagued = -1333953694,
        PoisonEnchanted = 1929212066,
        ReflectsDamage = -1374592233,
        Shielding = -725865705,
        Teleporter = -507706394,
        Thunderstorm = -50556465,
        Vortex = 458872904,
        Waller = 481181063,
        Wormhole = 1156956365,
        //ExtraHealth = 3, // Disabled in Patch 2.1.2
        //Vampiric = 14, // Disabled in Patch 2.1.2
        //Rare = 4206314,
        //ChampionBase = 924743082, 
        //Elite = 16, // Can't seem to find this one
        //Minion = 99383434, 
        //Unique = 418225399,
    }

}

