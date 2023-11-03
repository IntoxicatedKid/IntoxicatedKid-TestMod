using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using static test.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base.Extensions;
using System.Collections;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Neutral.Blue;
using HarmonyLib;
using LBoL.Core.StatusEffects;
using UnityEngine.Rendering;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits.Shining;
using Mono.Cecil;
using JetBrains.Annotations;
using System.Linq;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Enemy;
using test.StatusEffects;
using LBoL.Core.Randoms;

namespace test.Cards
{
    public sealed class AyaArsenalBirdDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArsenalBird);
        }
        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.CardsEn.yaml");
            return locFiles;
        }
        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "Simple1",
               GunNameBurst: "Simple1",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Blue, ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 3, Blue = 1, Red = 1 },
               UpgradedCost: new ManaGroup() { Any = 3, Blue = 1, Red = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: 20,
               UpgradedShield: 30,
               Value1: 2,
               UpgradedValue1: 3,
               Value2: null,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Any = 1 },
               UpgradedMana: new ManaGroup() { Any = 1 },
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.Shield,
               UpgradedKeywords: Keyword.Shield,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "AyaAccelerationSe" },
               UpgradedRelativeEffects: new List<string>() { "AyaAccelerationSe" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "igneous25",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaArsenalBirdDef))]
    public sealed class AyaArsenalBird : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.Player.HasStatusEffect<AyaAccelerationSeDef.AyaAccelerationSe>())
            {
                yield return new RemoveStatusEffectAction(Battle.Player.GetStatusEffect<AyaAccelerationSeDef.AyaAccelerationSe>(), false);
            }
            yield return PerformAction.Sfx("DroneSummon", 0f);
            yield return DefenseAction(false);
            yield return BuffAction<AyaArsenalBirdSeDef.AyaArsenalBirdSe>(0, 0, IsUpgraded ? 1 : 0, 0, 0.2f);
            yield break;
        }
    }
    public sealed class AyaArsenalBirdSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArsenalBirdSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaArsenalBirdSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 11,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Max,
                ShowPlusByLimit: true,
                Keywords: Keyword.Exile | Keyword.Ethereal | Keyword.TempMorph,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaArsenalBirdSeDef))]
        public sealed class AyaArsenalBirdSe : StatusEffect
        {
            [UsedImplicitly]
            public ManaGroup Mana
            {
                get
                {
                    //return ManaGroup.Empty;
                    return ManaGroup.Anys(1);
                }
            }
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnOwnerTurnStarted));
            }
            private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                NotifyActivating();
                List<Card> list = new List<Card>();
                Card[] attack = Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Attack);
                list.AddRange(attack);
                Card[] defense = Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Defense);
                list.AddRange(defense);
                foreach (Card card in list)
                {
                    /*if (Limit == 1)
                    {
                        card.Upgrade();
                    }*/
                    card.SetTurnCost(Mana);
                    card.IsExile = true;
                    card.IsEthereal = true;
                }
                Battle.MaxHand += 2;
                yield return new AddCardsToHandAction(list);
                Battle.MaxHand -= 2;
            }
        }
    }
}