﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoveringTooltip : MonoBehaviour
{
    TextMeshProUGUI text;
    RectTransform rect;
    int remainingFrames = 0;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (remainingFrames > 0)
            remainingFrames--;
        else
            gameObject.SetActive(false);
    }

    public void UpdateInformationTraitsOnly(string[] words)
    {
        string description = GetTraitDescription(words);
        if (description == "")
        {
            gameObject.SetActive(false);
            return;
        }
        rect.sizeDelta = new Vector2(500, 200);
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    public void UpdateInformationSpellsOnly(string[] words)
    {
        string description = GetSpellDescription(words);
        if (description == "")
        {
            gameObject.SetActive(false);
            return;
        }
        rect.sizeDelta = new Vector2(500, 200);
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    public void UpdateInformationAIOnly(string[] words)
    {
        string description = GetAIDescription(words);
        if (description == "")
        {
            gameObject.SetActive(false);
            return;
        }
        rect.sizeDelta = new Vector2(500, 200);
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    public void UpdateInformation(string[] words, Unit unit, Actor_Unit actor)
    {
        string description = GetDescription(words, unit, actor);
        if (description == "")
        {
            gameObject.SetActive(false);
            return;
        }
        rect.sizeDelta = new Vector2(500, 200);
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    public void UpdateInformation(Slider slider)
    {
        rect.sizeDelta = new Vector2(350, 80);
        string description = $"Slider Value: {Math.Round(slider.value, 3)}\nRight Click to type in the number.";
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    public void HoveringValidName()
    {
        string description = "Click to show the information for this consumed unit";
        gameObject.SetActive(true);
        remainingFrames = 3;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }

    string GetTraitDescription(string[] words)
    {
        if (Enum.TryParse(words[2], out Traits trait))
        {
            return GetTraitData(trait);
        }
        return "";
    }

    string GetSpellDescription(string[] words)
    {
        if (Enum.TryParse(words[2], out SpellTypes spell))
        {
            List<Spell> AllSpells = SpellList.SpellDict.Select(s => s.Value).ToList();
            string complete = $"{words[0]} {words[1]} {words[2]} {words[3]} {words[4]}";
            for (int i = 0; i < AllSpells.Count; i++)
            {
                if (words[2] == AllSpells[i].SpellType.ToString() || (complete.Contains(AllSpells[i].SpellType.ToString()) && AllSpells[i].SpellType.ToString().Contains(words[2]))) //Ensures that the phrase contains the highlighed word
                {
                    return $"{AllSpells[i].Description}\nRange: {AllSpells[i].Range.Min}-{AllSpells[i].Range.Max}\nMana Cost: {AllSpells[i].ManaCost}\nTargets: {string.Join(", ", AllSpells[i].AcceptibleTargets)}";
                }
            }
        }
        return "";
    }

    string GetAIDescription(string[] words)
    {
        if (Enum.TryParse(words[2], out RaceAI ai))
        {
            return GetAIData(ai);
        }
        return "";
    }

    string GetDescription(string[] words, Unit unit, Actor_Unit actor)
    {
        if (int.TryParse(words[2], out int temp))
        {
            return "";
        }
        if (Enum.TryParse(words[2], out Stat stat) && unit != null)
        {
            switch (stat)
            {
                case Stat.Strength:
                    return $"Affects melee accuracy and damage, also has a lesser impact on health, has minor effects on vore defense and vore escape\n{StatData(Stat.Strength)}";
                case Stat.Dexterity:
                    return $"Affects ranged accuracy and damage, has minor effect on vore escape\n{StatData(Stat.Dexterity)}";
                case Stat.Voracity:
                    return $"Affects vore odds, also has a minor effect on keeping prey down, also affects digestion damage to a minor degree\n{StatData(Stat.Voracity)}";
                case Stat.Agility:
                    return $"Affects melee and ranged evasion and movement speed\n{StatData(Stat.Agility)}\nMovement: {actor?.MaxMovement() ?? Mathf.Max(3 + ((int)Mathf.Pow(unit.GetStat(Stat.Agility) / 4, .8f)), 1)} tiles";
                case Stat.Will:
                    return $"Affects vore defense, escape rate, mana capacity, and magic defense\n{StatData(Stat.Will)}";
                case Stat.Mind:
                    return $"Affects spell damage, success odds, and duration with a minor amount of mana capacity\n{StatData(Stat.Mind)}";
                case Stat.Endurance:
                    return $"Affects total health, also reduces damage from acid, has a minor role in escape chance.\n{StatData(Stat.Endurance)}";
                case Stat.Stomach:
                    return $"Affects stomach capacity and digestion rate.  Also helps keep prey from escaping.\n{StatData(Stat.Stomach)}\n" +
                        $"{(actor?.PredatorComponent != null ? $"Used Capacity: {Math.Round(actor.PredatorComponent.GetBulkOfPrey(), 2) }\n" : "")}Max Capacity: {Math.Round(State.RaceSettings.GetStomachSize(unit.Race) * (unit.GetStat(Stat.Stomach) / 12f * unit.TraitBoosts.CapacityMult), 1)}";
                case Stat.Leadership:
                    return $"Provides a stat boost for all friendly units\nStat value: {unit.GetStatBase(Stat.Leadership)}";
            }
        }
        if (Enum.TryParse(words[2], out Traits trait))
        {
            return GetTraitData(trait);
        }
        if (Enum.TryParse(words[2], out UnitType unitType))
        {
            switch (unitType)
            {
                case UnitType.Soldier:
                    return "A generic soldier, does anything and everything they are tasked with";
                case UnitType.Leader:
                    return "The leader of an empire, inspires friendly troops through leadership";
                case UnitType.Mercenary:
                    return "A hired mercenary from the mercenary camp";
                case UnitType.Summon:
                    return "A summoned unit.  It will vanish at the end of the battle";
                case UnitType.SpecialMercenary:
                    return "A unique mercenary, only one of each can exist in the world at once, can not retreat and will return to the merc camp if dismissed";
                case UnitType.Adventurer:
                    return "An adventurer, recruited not from the village population, but from an inn";
            }
        }



        if (Enum.TryParse(words[2], out StatusEffectType effectType))
        {
            var effect = unit.GetLongestStatusEffect(effectType);
            if (effect != null)
            {
                switch (effectType)
                {
                    case StatusEffectType.Shielded:
                        return $"(Spell) Unit has resistance against incoming damage\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Mending:
                        return $"(Spell) Unit heals a medium amount every turn.\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Fast:
                        return $"(Spell) Unit moves faster\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Valor:
                        return $"(Spell) Unit does additional damage\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Predation:
                        return $"(Spell) Unit has increased voracity and stomach\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Poisoned:
                        return $"(Spell) Unit is taking damage over time\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.WillingPrey:
                        return $"(Spell) Unit wants to be prey (is easier to eat, and less likely to escape)\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Diminished:
                        return $"(Spell) Unit is tiny, with decreased stats and easy to eat\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Enlarged:
                        return $"(Spell) Unit has increased size and stats\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Clumsiness:
                        return $"Unit is easier to hit\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Empowered:
                        return $"Unit's stats are temporarily boosted\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Shaken:
                        return $"Unit's stats are temporarily lowered\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Webbed:
                        return $"Unit only gets 1 AP per turn, and stats are temporarily lowered\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Petrify:
                        return $"Unit cannot perform any actions, but is easy to hit, takes half damage from attacks and is bulky to eat.\nTurns Remaining: {effect.Duration}";
                    case StatusEffectType.Berserk:
                        return $"Unit is berserk, its strength and voracity are greatly increased for a brief period\nTurns Remaining: {effect.Duration}";

                }
            }
        }


        if (State.World?.ItemRepository != null)
        {
            List<Item> AllItems = State.World.ItemRepository.GetAllItems();
            string complete = $"{words[0]} {words[1]} {words[2]} {words[3]} {words[4]}";
            for (int i = 0; i < AllItems.Count; i++)
            {
                if (words[2] == AllItems[i].Name || (complete.Contains(AllItems[i].Name) && AllItems[i].Name.Contains(words[2]))) //Ensures that the phrase contains the highlighed word
                {
                    if (AllItems[i] is Weapon weapon)
                    {
                        return $"{weapon.Description}\nDamage:{weapon.Damage}\nRange:{weapon.Range}\nAccuracy:{weapon.AccuracyModifier}";
                    }
                    if (AllItems[i] is Accessory accessory)
                    {
                        return $"{accessory.Description}"; // \n+{accessory.StatBonus} to {(Stat)accessory.ChangedStat}";
                    }
                    if (AllItems[i] is SpellBook book)
                    {
                        return $"{book.Description}\n{book.DetailedDescription()}";
                    }
                }
            }
        }

        if (Enum.TryParse(words[2], out Race race))
        {
            if (unit == null) //Protector for the add a race screen
                return "";
            var racePar = RaceParameters.GetTraitData(unit);
            var bodySize = State.RaceSettings.GetBodySize(race);
            var stomachSize = State.RaceSettings.GetStomachSize(race);
            //return $"{race}\n{racePar.RaceDescription}\nBody Size: {State.RaceSettings.GetBodySize(race)}\nBase Stomach Size: {State.RaceSettings.GetStomachSize(race)}\nFavored Stat: {racePar.FavoredStat}\nDefault Traits:\n{State.RaceSettings.ListTraits(race)}";
            return $"{race}\n{racePar.RaceDescription}\nRace Body Size: {bodySize}\nCurrent Bulk: {actor?.Bulk()}\nBase Stomach Size: {stomachSize}\nFavored Stat: {State.RaceSettings.GetFavoredStat(race)}";
        }

        if (unit != null && words[2] == InfoPanel.RaceSingular(unit))
        {
            race = unit.Race;
            var racePar = RaceParameters.GetTraitData(unit);
            var bodySize = State.RaceSettings.GetBodySize(race);
            var stomachSize = State.RaceSettings.GetStomachSize(race);
            //return $"{race}\n{racePar.RaceDescription}\nBody Size: {State.RaceSettings.GetBodySize(race)}\nBase Stomach Size: {State.RaceSettings.GetStomachSize(race)}\nFavored Stat: {racePar.FavoredStat}\nDefault Traits:\n{State.RaceSettings.ListTraits(race)}";
            return $"{race}\n{racePar.RaceDescription}\nRace Body Size: {bodySize}\nCurrent Bulk: {actor?.Bulk()}\nBase Stomach Size: {stomachSize}\nFavored Stat: {State.RaceSettings.GetFavoredStat(race)}";
        }


        {
            List<Spell> AllSpells = SpellList.SpellDict.Select(s => s.Value).ToList();
            string complete = $"{words[0]} {words[1]} {words[2]} {words[3]} {words[4]}";
            for (int i = 0; i < AllSpells.Count; i++)
            {
                if (words[2] == AllSpells[i].Name || (complete.Contains(AllSpells[i].Name) && AllSpells[i].Name.Contains(words[2]))) //Ensures that the phrase contains the highlighed word
                {
                    return $"{AllSpells[i].Description}\nRange: {AllSpells[i].Range.Min}-{AllSpells[i].Range.Max}\nMana Cost: {AllSpells[i].ManaCost}\nTargets: {string.Join(", ", AllSpells[i].AcceptibleTargets)}";
                }
            }
        }




        switch (words[2])
        {
            case "surrendered":
                return "This unit has surrendered, all units have a 100% chance to eat it, and it only costs 2 mp to eat it.";

            case "Imprinted":
                return $"This unit is imprinted in the village of {unit.SavedVillage.Name}, at level {unit.SavedCopy?.Level ?? 0} with {Math.Round(unit.SavedCopy?.Experience ?? 0)} exp.  " +
                    $"Unit will automatically resurrect there at that power, assuming the village is controlled by friendlies when the unit dies";


            default:
                return "";
        }



        string StatData(Stat Stat)
        {

            string leader = "";
            int leaderBonus = unit.GetLeaderBonus();
            if (leaderBonus > 0) leader = $"+{leaderBonus} from leader\n";
            string traits = "";
            int traitBonus = unit.GetTraitBonus(Stat);
            if (traitBonus > 0) traits = $"+{traitBonus} from traits\n";
            string effects = "";
            int effectBonus = unit.GetEffectBonus(Stat);
            if (effectBonus > 0) effects = $"+{effectBonus} from effects\n";
            else if (effectBonus < 0) effects = $"{effectBonus} from effects\n";
            return $"{unit.GetStatBase(Stat)} base {Stat}\n{leader}{traits}{effects}Final Stat: {unit.GetStat(Stat)}";
        }


    }

    public static string GetTraitData(Traits trait)
    {
        Trait traitClass = TraitList.GetTrait(trait);
        if (traitClass != null)
            return traitClass.Description;
        switch (trait)
        {
            case Traits.Resilient:
                return "Takes less damage from attacks";
            case Traits.FastDigestion:
                return "Unit digests prey faster than normal";
            case Traits.SlowDigestion:
                return "Unit digests prey slower than normal";
            case Traits.Intimidating:
                return "Enemies within 1 tile get a penalty to accuracy against all targets";
            case Traits.AdeptLearner:
                return "All stats are favored, randomly get 1 point in 2 different stats with level up";
            case Traits.SlowBreeder:
                return "Race produces new population at a slower rate than normal";
            case Traits.ProlificBreeder:
                return "Race produces new population at a faster rate than normal";
            case Traits.Flight:
                return "Unit can pass through obstacles and other units in tactical mode.\nMust end turn on solid ground\nIf you try to take an action or end your turn in an invalid place, it will automatically undo your movement";
            case Traits.Pounce:
                return "Unit spends a minimum of 2 mp to jump next to a target that is within 2-4 tiles (if there is space) and melee attack or vore it";
            case Traits.Biter:
                return "A failed vore attack will result in an attack attempt with a weak melee weapon";
            case Traits.Pathfinder:
                return "Passes through all terrain at a movement cost of 1.\nMore than half of the army has to have this trait to have an effect";
            case Traits.AstralCall:
                return "Unit has a chance at the beginning of battle to summon a weaker unit from its race\nThey return to their own dimension after the battle";
            case Traits.TentacleHarassment:
                return "Shifting tentacles distract and harass opponents within 1 tile.\nLowers enemy stats by a small amount";
            case Traits.BornToMove:
                return "Experienced at carrying extra weight.\nUnit suffers no defense penalty and no movement penalty from units it is carrying.";
            case Traits.Resourceful:
                return "Unit has an additional item slot";
            case Traits.ForcefulBlow:
                return "Unit will knock enemy units back in melee (straight back or one diagonal to the side).\nIf a unit is blocked in those directions, it will take extra damage";
            case Traits.NimbleClimber:
                return "Unit is a strong climber and can pass through trees unhindered";
            case Traits.Dazzle:
                return "Units attacking this unit have to run a check based on the comparison of the will values, units failing the check will simply end their turn without attacking and suffer a stat penalty until their next turn (chance caps at 20% at 5x will, and is reflected in shown hit odds)";
            case Traits.Charge:
                return "Unit has a signficant boost to movement speed for the first two turns of every battle";
            case Traits.Feral:
                return "Unit can't use weapons or books, but does a considerable amount of melee damage (6 base) (AI will still try to buy stuff for them)";
            case Traits.DualStomach:
                return "Unit has two stomachs, the second digests faster than the first and is harder to escape from (units slowly migrate to the second)";
            case Traits.MadScience:
                return "Allows casting of a random spell for normal mana cost once per battle";
            case Traits.ShunGokuSatsu:
                return "Allows usage of a powerful ability that does attacks and vore.  Can only be used every 3 turns";
            case Traits.Eternal:
                return "(Cheat Trait) - This unit can never die.  If it is killed during a battle, it will be set to full hp and act as though it fled (will rejoin if the army wins, otherwise sets off for the closest town)";
            case Traits.Reanimator:
                return "(Cheat Trait) - This unit can never die from weapons or spells, though digestion can kill it permanently.  If it is 'killed' during a battle, it will be set to full hp and act as though it fled (will rejoin if the army wins, otherwise sets off for the closest town) Note that these don't give immunity to digestion conversion unlike the pure eternal trait";
            case Traits.Reformer:
                return "(Cheat Trait) - This unit can never die from being digested, but spells and weapons can kill it permanently.  If it is killed during a battle, it will be set to full hp and act as though it fled (will rejoin if the army wins, otherwise sets off for the closest town) Note that these don't give immunity to digestion conversion unlike the pure eternal trait";
            case Traits.LuckySurvival:
                return "Unit has an 80% chance of acting like an eternal unit on death (coming back to life after the battle), with a 20% chance of dying normally.";
            case Traits.Replaceable:
                return "If the unit dies and its side wins, the unit will be replaced by a rather similar unit from the same race";
            case Traits.Greedy:
                return "The unit will avoid giving up prey at all costs -- will not auto regurgitate friendlies regardless of settings, and the regurgitate command is disabled";
            case Traits.RangedVore:
                return "Unit can perform vore actions at a range of up to 4 tiles, but chance of success drops per tile, but doesn't drop against flying units.";
            case Traits.HeavyPounce:
                return "Pounce does extra melee damage based on the weight of prey, but defense is also lowered for one turn after pouncing based on the weight of prey (requires the pounce trait to be useable)";
            case Traits.Cruel:
                return "Unit can attempt to eat non-surrendered allied units at normal odds";
            case Traits.MetabolicSurge:
                return "Unit gains a burst of power after digesting a victim";
            case Traits.EnthrallingDepths:
                return "Prey is afflicted with the Prey's Curse effect";
            case Traits.FearsomeAppetite:
                return "Consuming a victim frightens nearby allies of the prey, temporarily reducing their stats";
            case Traits.Endosoma:
                return "Can vore friendly units, friendly units that are vored take no digestion damage \nThey do not try to escape, but can be regurgitated or are freed at the end of battle\nHas 100% chance to eat allies, and only costs 2 mp, like eating surrendered units.  May cause battles to not automatically end if used with TheGreatEscape";
            case Traits.TailStrike:
                return "An attack that does less damage, but attacks the tile and the 2 tiles adjacent to it that are within reach";
            case Traits.HealingBelly:
                return "An accessory trait to endosoma that makes friendly prey receive healing each turn.  (Does nothing without the endosoma trait)\n(Cheat Trait)";
            case Traits.Assimilate:
                return "If the unit has less than 5 traits, upon finishing absorption of an enemy unit, will take a random trait from them that the unit doesn't currently have.  If the unit has 5 traits, the random trait will replace this trait.\n(Cheat Trait)";
            case Traits.AdaptiveBiology:
                return "Upon finishing absorption of an enemy unit, will take a random trait from them that the unit doesn't currently have and add it to a list of 3 rotating traits.  If the list already has 3 rotating traits, the oldest trait is removed.  This can't trigger on the same kill as Assimilate.\n(Cheat Trait)";
            case Traits.KillerKnowledge:
                return "Every four weapon / spell kills (but not vore kills), the unit will get a permanent +1 to all stats\n(Cheat Trait)";
            case Traits.PollenProjector:
                return "Allows using the pollen cloud ability once per battle, which attempts to inflict a few status effects in a 3x3 area for a small mana cost.  This trait also makes the unit immune to most of the statuses from this ability.";
            case Traits.Webber:
                return "Allows using the web ability once per battle, which attempts to inflict the webbed status for 3 turns, which lowers AP to 1, and reduces stats.";
            case Traits.Camaraderie:
                return "Prevents the unit from defecting to rejoin its race if that option is enabled.";
            case Traits.RaceLoyal:
                return "Unit will defect to rejoin its race at every opportunity if that option is enabled.";
            case Traits.WillingRace:
                return "Gives the whole race the willing prey spell effect permanently, which makes them easier to eat, and changes some of the dialogue.";
            case Traits.InfiniteAssimilation:
                return "Upon finishing absorption of an enemy unit, will take a random trait from them that the unit doesn't currently have.  This version has no cap, so it can be a little bit of a text mess.\n(Cheat Trait)";
            case Traits.GlueBomb:
                return "Gives access to a single use ability that applies the glued status effect to a 3x3 group.  Glued units are very slow, and it takes a while to get it off.";
            case Traits.TasteForBlood:
                return "After digesting or killing someone, the unit will get a random positive buff for 5 turns.";
            case Traits.PleasurableTouch:
                return "This unit's belly rub actions are more effective on others (doubled effect).";
            case Traits.PoisonSpit:
                return "Allows using the poison spit ability once per battle, which does damage in a 3x3 and attempts to apply a strong short term poison as well.  This trait also makes the unit immune to poison damage.";
            case Traits.DigestionConversion:
                return "When a unit finishes digesting someone, there's a 50% chance they will then convert to the predator's side, and be healed to half of their max life.  Can't convert leaders, summons, or units with saved copies.\n(Cheat Trait)"; 
            case Traits.DigestionRebirth:
                return "When a unit finishes digesting someone, there's a 50% chance they will then convert to the predator's side and change race to the predator's race, and be healed to half of their max life.  Can't convert leaders, summons, or units with saved copies.\n(Cheat Trait)";
            case Traits.SenseWeakness:
                return "Unit does more melee/ranged damage against targets with less health, and also increases for every negative status effect the target has."; 
            case Traits.BladeDance:
                return "Unit gains a stack of blade dance every time they successfully hit their opponent with melee, and lose a stack every time they are hit with melee.  Each stack gives +2 strength and +1 agility.";
            case Traits.EssenceAbsorption:
                return "Every four vore digestions, the unit will get a permanent +1 to all stats\n(Cheat Trait)";
            case Traits.AntPheromones:
                return "Unit will summon some friendly ants, depending on how many units have this trait.";
            case Traits.Fearless:
                return "Unit can not flee nor surrender (also applies to auto-surrender).  If something does happen to make the unit surrender, it will automatically recover on the next turn.";
            case Traits.Inedible:
                return "Unit can not be vored by other units.  (It makes their effective size so big that no one has the capacity to eat them)\n(Cheat Trait)";
            case Traits.AllOutFirstStrike:
                return "Unit starts battle in a protected state, with high dodge rate.  On their first attack or vore attempt of the battle, they get a significant bonus to damage or vore chance.  After that they become vulnerable, and move slower and have a dodge penalty.";
            case Traits.VenomousBite:
                return "A missed bite from the biter trait will also poison an enemy, and give them the shaken debuff.";
            case Traits.Petrifier:
                return "Gives access to a single use ability that applies the petrified status effect to a target.  It prevents the target from acting, but also makes them resistant to damage and bulky to swallow.";
            case Traits.VenomShock:
                return "Gives this unit significantly increased melee damage and vore odds against targets who are poisoned.";
            case Traits.Tenacious:
                return "Unit gains a stack of tenacity every time they are hit or miss an attack, and lose five stacks every time they hit or vore an enemy.  Each stack gives +10% str/agi/vor.";
            case Traits.PredConverter:
                return "This unit will always convert unbirthed prey to their side upon full digestion regardless of KuroTenko settings, putting this together with PredRebirther or PredGusher on same unit is not recommended";
            case Traits.PredRebirther:
                return "This unit will always rebirth unbirthed prey as their race upon full absorption regardless of KuroTenko settings, putting this together with PredConverter or PredGusher on same unit is not recommended.";
            case Traits.PredGusher:
                return "This unit will always turn unbirthed units into a sticky puddle, they will not be converted/rebirthed. (Basically cancels out certain game settings or traits)";
            case Traits.SeductiveTouch:
                return "Unit's belly rub action can make enemies pause for a turn, or even switch sides, as long as they haven't taken damage for two turns.\n(Cheat Trait)";
            case Traits.TheGreatEscape:
                return "This unit cannot be digested, but the battle will end if only units with this remain and they're all eaten.  The prey are assumed to escape sometime later, and are count as fled units. (Note that you'd need end of battle review checked to see the escape messages as they happen at the very end of battle)";
            case Traits.Growth:
                return "Each absorbtion makes this unit grow in size, but the effect slowly degrades outside battle.\n(Cheat Trait)";
            case Traits.PermanentGrowth:
                return "An accessory trait to Growth that makes growth gained permanent.  (Does nothing without the Growth trait)\n(Cheat Trait)";
            case Traits.Berserk:
                return "If the unit is reduced below half health by an attack, will go berserk, greating increasing its strength and voracity for 3 turns.\nCan only occur once per battle.";
            case Traits.SynchronizedEvolution:
                return "Any trait this unit assimilates is received by all members of their race. (requires Assimilate or InfiniteAssimilation)\n(Cheat Trait)";
            case Traits.Charmer:
                return "Allows the casting of the Charm spell once per battle";
        }
        return "<b>This trait needs a tooltip!</b>";
    }

    public static string GetAIData(RaceAI ai)
    {
        switch (ai)
        {
            case RaceAI.Standard:
                return "Straightforward battlers";
            case RaceAI.Hedonist:
                return "Will try to find the time for massaging any prey-filled parts on their comrades or their own body.\nDon't be fooled – this is deceptively efficient.";
            case RaceAI.ServantRace:
                return "Acts Subservient towards units of the most powerful race on their side, flocking to rub those individuals.\n" +
                    "Racial superiority is based on eminence.";
        }
        return "<b>This AI needs a tooltip!</b>";
    }

    internal void UpdateInformationDefaultTooltip(int value)
    {
       
        string description = DefaultTooltips.Tooltip(value);
        if (description == "")
        {
            gameObject.SetActive(false);
            return;
        }
        rect.sizeDelta = new Vector2(500, 200);
        gameObject.SetActive(true);
        remainingFrames = 999;
        text.text = description;
        float xAdjust = 10;
        float exceeded = Input.mousePosition.x + (rect.rect.width * Screen.width / 1920) - Screen.width;
        if (exceeded > 0)
            xAdjust = -exceeded;
        transform.position = Input.mousePosition + new Vector3(xAdjust, 0, 0);
    }
}
