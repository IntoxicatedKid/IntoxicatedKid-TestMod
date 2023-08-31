﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using LBoL.Core.StatusEffects;
using LBoL.Core.Randoms;
using LBoL.Core.Units;
using LBoL.Base.Extensions;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.StatusEffects.Others;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using LBoL.EntityLib.JadeBoxes;
using static test.BepinexPlugin;
using static test.DayuuAbilityDef;
using UnityEngine.Playables;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using LBoL.Core.GapOptions;
using Mono.Cecil;
using test;

namespace test
{
    public sealed class DayuuRealeDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuReale);
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
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.Black },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 3, Black = 2 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 2,
               UpgradedValue1: 2,
               Value2: 1,
               UpgradedValue2: 1,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: 1,
               UpgradedLoyalty: 1,
               PassiveCost: 2,
               UpgradedPassiveCost: 2,
               ActiveCost: 2,
               UpgradedActiveCost: 2,
               UltimateCost: -6,
               UpgradedUltimateCost: -6,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: true,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Reale雷耀",
               SubIllustrator: new List<string>() { "Cruezor" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuRealeDef))]
    public sealed class DayuuReale : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && base.Battle.Player.IsInTurn && this.Zone == CardZone.Hand && this.Summoned)
            {
                List<Card> DayuuA = base.Battle.HandZone.Where((Card card) => (card is DayuuAttack)).ToList<Card>();
                List<Card> DayuuD = base.Battle.HandZone.Where((Card card) => (card is DayuuDefense)).ToList<Card>();
                List<Card> DayuuS = base.Battle.HandZone.Where((Card card) => (card is DayuuSkill)).ToList<Card>();
                List<Card> DayuuP = base.Battle.HandZone.Where((Card card) => (card is DayuuAbility)).ToList<Card>();
                if (DayuuA.Count > 0 && DayuuD.Count > 0 && DayuuS.Count > 0 && DayuuP.Count > 0)
                {
                    List<Card> Dayuu = base.Battle.HandZone.Where((Card card) => (card is DayuuAttack) || (card is DayuuDefense) || (card is DayuuSkill) || (card is DayuuAbility) || (card is DayuuFriend)).ToList<Card>();
                    foreach (Card card in Dayuu)
                    {
                        yield return new RemoveCardAction(card);
                    }
                    Card Exodia = Library.CreateCard<DayuuExodia>();
                    Exodia.Summon();
                    yield return new AddCardsToHandAction(new Card[] { Exodia });
                }
            }
        }
        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return this.GetPassiveActions();
        }
        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            int num;
            for (int i = 0; i < base.Battle.FriendPassiveTimes; i = num + 1)
            {
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                foreach (BattleAction battleAction in base.DebuffAction<TempFirepowerNegative>(base.Battle.AllAliveEnemies, base.Value1, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                if (base.Loyalty <= 0)
                {
                    yield return new RemoveCardAction(this);
                    yield break;
                }
                num = i;
            }
            yield break;
        }
        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            this.IsEthereal = false;
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition))
            {
                yield return battleAction;
            }
            yield break;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.ActiveCost;
                yield return PerformAction.Effect(base.Battle.Player, "Wave1s", 0f, "BirdSing", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                foreach (BattleAction battleAction in base.DebuffAction<FirepowerNegative>(base.Battle.AllAliveEnemies, base.Value2, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                foreach (BattleAction battleAction2 in base.DebuffAction<Weak>(base.Battle.AllAliveEnemies, 0, base.Value2, 0, 0, true, 0.2f))
                {
                    yield return battleAction2;
                }
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return PerformAction.Effect(base.Battle.Player, "Wave1s", 0f, "BirdSing", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
                {
                    if (enemyUnit.Hp <= (enemyUnit.MaxHp + 1) / 4)
                    {
                        yield return new ForceKillAction(base.Battle.Player, enemyUnit);
                    }
                }
                foreach (BattleAction battleAction in base.DebuffAction<FirepowerNegative>(base.Battle.AllAliveEnemies, 3, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                foreach (BattleAction battleAction2 in base.DebuffAction<Weak>(base.Battle.AllAliveEnemies, 0, 3, 0, 0, true, 0.2f))
                {
                    yield return battleAction2;
                }
                foreach (BattleAction battleAction3 in base.DebuffAction<Vulnerable>(base.Battle.AllAliveEnemies, 0, 3, 0, 0, true, 0.2f))
                {
                    yield return battleAction3;
                }
            }
            yield break;
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (base.Loyalty <= 0 || base.UltimateUsed == true)
            {
                yield return new RemoveCardAction(this);
                yield break;
            }
            yield return new MoveCardAction(this, CardZone.Hand);
            yield break;
        }
    }
    public sealed class DayuuRealeSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuRealeSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.DayuuExodiaSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: false,
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



        [EntityLogic(typeof(DayuuRealeSeDef))]
        public sealed class DayuuRealeSe : StatusEffect
        {
        }
    }
}