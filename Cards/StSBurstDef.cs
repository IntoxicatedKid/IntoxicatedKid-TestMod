﻿using LBoL.ConfigData;
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
using test;
using LBoL.EntityLib.PlayerUnits;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.StatusEffects.Reimu;

namespace test
{
    public sealed class StSBurstDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSBurst);
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
                Type: CardType.Skill,
                TargetType: TargetType.Self,
                Colors: new List<ManaColor>() { ManaColor.Blue },
                IsXCost: false,
                Cost: new ManaGroup() { Blue = 1 },
                UpgradedCost: new ManaGroup() { Blue = 1 },
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 1,
                UpgradedValue1: 2,
                Value2: null,
                UpgradedValue2: null,
                Mana: null,
                UpgradedMana: null,
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

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.None,
                EmptyDescription: false,
                RelativeKeyword: Keyword.None,
                UpgradedRelativeKeyword: Keyword.None,

                RelativeEffects: new List<string>() { },
                UpgradedRelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: null,
                Unfinished: false,
                Illustrator: "Mega Crit",
                SubIllustrator: new List<string>() { }
             );

            return cardConfig;
        }
        [EntityLogic(typeof(StSBurstDef))]
        public sealed class StSBurst : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return base.BuffAction<StSBurstSeDef.StSBurstSe>(base.Value1, 0, 0, 0, 0.2f);
                yield break;
            }
        }

    }
    public sealed class StSBurstSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSBurstSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.StSBurstSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 20,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(StSBurstSeDef))]
        public sealed class StSBurstSe : StatusEffect
        {
            private bool Again;
            private Card card;
            private Card card2;
            private UnitSelector unitSelector;
            protected override void OnAdded(Unit unit)
            {
                this.Again = false;
                card = null;
                card2 = null;
                unitSelector = null;
                base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
                base.ReactOwnerEvent<CardMovingEventArgs>(base.Battle.CardMoving, new EventSequencedReactor<CardMovingEventArgs>(this.OnCardMoving));
                base.ReactOwnerEvent<CardEventArgs>(base.Battle.CardExiling, new EventSequencedReactor<CardEventArgs>(this.OnCardExiling));
                base.ReactOwnerEvent<CardEventArgs>(base.Battle.CardRemoving, new EventSequencedReactor<CardEventArgs>(this.OnCardRemoving));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if ((args.Card.CardType == CardType.Defense || args.Card.CardType == CardType.Skill) && args.Card != card && args.Card != card2)
                {
                    this.Again = true;
                    card = args.Card;
                    unitSelector = args.Selector;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardMoving(CardMovingEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card && !(args.SourceZone == CardZone.PlayArea && args.DestinationZone == CardZone.Hand))
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    int num = base.Level - 1;
                    base.Level = num;
                    if (base.Level <= 0)
                    {
                        yield return new RemoveStatusEffectAction(this, true);
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardExiling(CardEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card)
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    int num = base.Level - 1;
                    base.Level = num;
                    if (base.Level <= 0)
                    {
                        yield return new RemoveStatusEffectAction(this, true);
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardRemoving(CardEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card)
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    int num = base.Level - 1;
                    base.Level = num;
                    if (base.Level <= 0)
                    {
                        yield return new RemoveStatusEffectAction(this, true);
                    }
                }
                yield break;
            }
        }
    }
}