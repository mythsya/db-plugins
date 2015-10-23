using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using Trinity.Helpers;
using Trinity.Objects;
using Trinity.Reference;
using Trinity.Settings.Loot;
using Trinity.Technicals;
using Trinity.UIComponents;
using Zeta.Game;
using Zeta.Game.Internals.Actors;

namespace Trinity.Items
{
    public class ItemList
    {
        internal static bool ShouldStashItem(CachedACDItem cItem, bool test = false)
        {
            if (cItem.AcdItem != null && cItem.AcdItem.IsValid)
            {
                var item = Legendary.GetItemByACD(cItem.AcdItem);
                if (item == null)
                {
                    Logger.Log(TrinityLogLevel.Info, "  >>  Unknown Item {0} {1} - Auto-keeping", cItem.RealName, cItem.ActorSNO);
                    return true;
                }
                        
                return ShouldStashItem(item, cItem, test); ;
            }
            return false;
        }

        internal static bool ShouldStashItem(Item referenceItem, CachedACDItem cItem, bool test = false)
        {
            var id = referenceItem.Id;

            var logLevel = test ? TrinityLogLevel.Info : TrinityLogLevel.Debug;

            if (cItem.AcdItem.IsCrafted)
            {
                Logger.Log(logLevel, "  >>  Crafted Item {0} {1} - Auto-keeping", cItem.RealName, id);
                return true;
            }

            if (test)
            {
                var props = ItemDataUtils.GetPropertiesForItem(referenceItem);
                
                Logger.LogVerbose("------- Starting Test of {0} supported properties for {1} against max value", props.Count, cItem.RealName);
              
                foreach (var prop in props)
                {
                    var range = ItemDataUtils.GetItemStatRange(referenceItem, prop);
                    EvaluateRule(cItem, prop, range.AncientMax, 0);
                }

                Logger.LogVerbose("------- Finished Test for {0} against max value", cItem.RealName);
            }

            var itemSetting = Trinity.Settings.Loot.ItemList.SelectedItems.FirstOrDefault(i => referenceItem.Id == i.Id);
            if (itemSetting != null)
            {
                Logger.Log(logLevel, "  >>  {0} ({2}) is a Selected ListItem with {1} rules.", cItem.RealName, itemSetting.Rules.Count, id);

                if (itemSetting.RequiredRules.Any())
                    Logger.Log(logLevel, "  >>  {0} required rules:", itemSetting.RequiredRules.Count);

                // If any of the required rules are false, trash.
                foreach (var itemRule in itemSetting.RequiredRules)
                {
                    if (!EvaluateRule(itemRule, cItem))
                        return false;
                }

                if (!itemSetting.OptionalRules.Any())
                    return true;

                Logger.Log(logLevel, "  >>  {1}/{0} optional rules:", itemSetting.OptionalRules.Count, itemSetting.Ops);

                // X optional rules must be true. in test mode evaluate all rules
                var trueOptionals = 0;
                foreach (var itemRule in itemSetting.OptionalRules)
                {
                    if (EvaluateRule(itemRule, cItem))
                        trueOptionals++;

                    if (trueOptionals >= itemSetting.Ops && !test)
                        return true;
                }

                if (trueOptionals >= itemSetting.Ops && test)
                    return true;

                return false;
            }



            Logger.Log(logLevel, "  >>  Unselected ListItem {0} {1}", cItem.RealName, id);
            return false;
        }



        internal static bool EvaluateRule(LRule itemRule, CachedACDItem cItem)
        {
            return EvaluateRule(cItem, itemRule.ItemProperty, itemRule.Value, itemRule.Variant, itemRule.RuleType);
        }

        private static bool EvaluateRule(CachedACDItem cItem, ItemProperty prop, double value, int variant, RuleType type = RuleType.Test)
        {
            var result = false;
            string friendlyVariant = string.Empty;
            double itemValue = 0;
            double ruleValue = 0;

            switch (prop)
            {
                case ItemProperty.Ancient:
                    itemValue = cItem.IsAncient ? 1 : 0;
                    ruleValue = value;
                    result = cItem.IsAncient == (value == 1);
                    break;

                case ItemProperty.PrimaryStat:
                    itemValue = ItemDataUtils.GetMainStatValue(cItem.AcdItem);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.CriticalHitChance:
                    itemValue = cItem.CritPercent;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.CriticalHitDamage:
                    itemValue = cItem.CritDamagePercent;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.AttackSpeed:
                    itemValue = ItemDataUtils.GetAttackSpeed(cItem.AcdItem);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ResourceCost:
                    itemValue = Math.Round(cItem.AcdItem.Stats.ResourceCostReductionPercent, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.Cooldown:
                    itemValue = Math.Round(cItem.AcdItem.Stats.PowerCooldownReductionPercent, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ResistAll:
                    itemValue = cItem.AcdItem.Stats.ResistAll;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.Sockets:
                    itemValue = cItem.AcdItem.Stats.Sockets;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.Vitality:
                    itemValue = cItem.AcdItem.Stats.Vitality;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.AreaDamage:
                    itemValue = cItem.AcdItem.Stats.OnHitAreaDamageProcChance;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.FireSkills:
                    itemValue = cItem.AcdItem.Stats.FireSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ColdSkills:
                    itemValue = cItem.AcdItem.Stats.ColdSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LightningSkills:
                    itemValue = cItem.AcdItem.Stats.LightningSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ArcaneSkills:
                    itemValue = cItem.AcdItem.Stats.ArcaneSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.HolySkills:
                    itemValue = cItem.AcdItem.Stats.HolySkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.PoisonSkills:
                    itemValue = cItem.AcdItem.Stats.PosionSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.PhysicalSkills:
                    itemValue = cItem.AcdItem.Stats.PhysicalSkillDamagePercentBonus;
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.DamageAgainstElites:
                    itemValue = Math.Round(cItem.AcdItem.Stats.DamagePercentBonusVsElites, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.DamageFromElites:
                    itemValue = Math.Round(cItem.AcdItem.Stats.DamagePercentReductionFromElites, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.BaseMaxDamage:
                    itemValue = ItemDataUtils.GetMaxBaseDamage(cItem.AcdItem);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.SkillDamage:

                    var skillId = variant;
                    var skill = ItemDataUtils.GetSkillsForItemType(cItem.TrinityItemType, Trinity.Player.ActorClass).FirstOrDefault(s => s.Id == skillId);
                    if (skill != null)
                    {
                        friendlyVariant = skill.Name;
                        itemValue = cItem.AcdItem.SkillDamagePercent(skill.SNOPower);
                    }

                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ElementalDamage:

                    var elementId = variant;
                    var element = (Element)elementId;
                    if (element != Element.Unknown)
                    {
                        friendlyVariant = ((EnumValue<Element>)element).Name;
                        itemValue = Math.Round(ItemDataUtils.GetElementalDamage(cItem.AcdItem, element), MidpointRounding.AwayFromZero);
                    }

                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.PercentDamage:
                    itemValue = Math.Round(cItem.AcdItem.WeaponDamagePercent(), MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.CriticalHitsGrantArcane:
                    itemValue = Math.Round(cItem.ArcaneOnCrit, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.Armor:
                    itemValue = Math.Round(cItem.Armor, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ChanceToBlock:
                    itemValue = Math.Round(cItem.BlockChance, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.HatredRegen:
                    itemValue = Math.Round(cItem.HatredRegen, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LifePercent:
                    itemValue = Math.Round(cItem.LifePercent, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LifePerHit:
                    itemValue = Math.Round(cItem.LifeOnHit, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.RegenerateLifePerSecond:
                    itemValue = Math.Round(cItem.AcdItem.Stats.HealthPerSecond, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.ManaRegen:
                    itemValue = Math.Round(cItem.AcdItem.Stats.ManaRegen, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.MovementSpeed:
                    itemValue = Math.Round(cItem.AcdItem.Stats.MovementSpeed, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.SpiritRegen:
                    itemValue = Math.Round(cItem.AcdItem.Stats.SpiritRegen, MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.WrathRegen:
                    itemValue = Math.Round((double)cItem.AcdItem.GetAttribute<float>(ActorAttributeType.ResourceRegenPerSecondFaith), MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LifePerFury:
                    itemValue = Math.Round((double)cItem.AcdItem.GetAttribute<float>(ActorAttributeType.SpendingResourceHealsPercentFury), MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LifePerSpirit:
                    itemValue = Math.Round((double)cItem.AcdItem.GetAttribute<float>(ActorAttributeType.SpendingResourceHealsPercentSpirit), MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;

                case ItemProperty.LifePerWrath:
                    itemValue = Math.Round((double)cItem.AcdItem.GetAttribute<float>(ActorAttributeType.SpendingResourceHealsPercentFaith), MidpointRounding.AwayFromZero);
                    ruleValue = value;
                    result = itemValue >= ruleValue;
                    break;
            }

            Logger.LogVerbose("  >>  Evaluated {0} -- {1} {5} Type={6} (Item: {2} -v- Rule: {3}) = {4}",
                cItem.RealName,
                prop.ToString().AddSpacesToSentence(),
                itemValue,
                ruleValue,
                result,
                friendlyVariant,
                type);

            return result;
        }

    }
}

