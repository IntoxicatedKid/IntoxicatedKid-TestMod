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

namespace test
{
    public sealed class AyaBunbunmaruPrinterDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBunbunmaruPrinter);
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
               Rarity: Rarity.Uncommon,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Green = 2 },
               UpgradedCost: new ManaGroup() { Green = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
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
               RelativeCards: new List<string>() { "AyaNews", "HatateNews" },
               UpgradedRelativeCards: new List<string>() { "AyaNews", "HatateNews" },
               Owner: null,
               Unfinished: false,
               Illustrator: "",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaBunbunmaruPrinterDef))]
    public sealed class AyaBunbunmaruPrinter : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<AyaBunbunmaruPrinterSeDef.AyaBunbunmaruPrinterSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
    public sealed class AyaBunbunmaruPrinterSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBunbunmaruPrinterSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.DayuuAbilitySe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Add,
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
        [EntityLogic(typeof(AyaBunbunmaruPrinterSeDef))]
        public sealed class AyaBunbunmaruPrinterSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            }
            private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd)
                {
                    List<Card> list = base.Battle.HandZone.Where((Card card) => (card.CardType == CardType.Friend) && !(card is DayuuFriend)).ToList<Card>();
                    List<Card> list2 = base.Battle.HandZone.Where((Card card) => card is DayuuFriend).ToList<Card>();
                    if (list.Count > 0)
                    {
                        base.NotifyActivating();
                        ManaGroup manaGroup = ManaGroup.Empty;
                        for (int i = 0; i < base.Count * list.Count; i++)
                        {
                            manaGroup += ManaGroup.Single(ManaColors.Colors.Sample(base.GameRun.BattleRng));
                        }
                        yield return new GainManaAction(manaGroup);
                    }
                    if (list2.Count > 0)
                    {
                        base.NotifyActivating();
                        ManaGroup manaGroup2 = ManaGroup.Empty;
                        for (int i = 0; i < base.Level * list2.Count; i++)
                        {
                            manaGroup2 += ManaGroup.Single(ManaColors.Colors.Sample(base.GameRun.BattleRng));
                        }
                        yield return new GainManaAction(manaGroup2);
                    }
                }
                yield break;
            }
        }
    }
}