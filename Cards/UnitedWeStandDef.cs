using LBoL.Base;
using LBoL.ConfigData;
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
using static test.BepinexPlugin;
using System.Linq;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using JetBrains.Annotations;
using LBoL.Base.Extensions;
using static UnityEngine.GraphicsBuffer;
using LBoL.Presentation.UI.Widgets;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Others;
using UnityEngine;

namespace test.Cards
{
    public sealed class UnitedWeStandDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UnitedWeStand);
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
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: null,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Any = 0 },
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

               Keywords: Keyword.Exile,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.Friend,
               UpgradedRelativeKeyword: Keyword.Friend,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Utarion",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(UnitedWeStandDef))]
    public sealed class UnitedWeStand : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //_maxhand = Battle.MaxHand;
            //Battle.MaxHand = 10;
            if (!Battle.Player.HasStatusEffect<UnitedWeStandSeDef.UnitedWeStandSe>())
            {
                yield return new ApplyStatusEffectAction<UnitedWeStandSeDef.UnitedWeStandSe>(Battle.Player, null, null, null, null, 0.0f, true);
            }
            int num = Battle.MaxHand - Battle.HandZone.Count;
            if (num > 0)
            {
                yield return new DrawManyCardAction(num);
            }
            GameRun.SynergyAdditionalCount += 1;
            EnemyUnit target = selector.SelectedEnemy;
            List<Card> list = Battle.HandZone.ToList();
            foreach (Card card in list)
            {
                if (!target.IsAlive)
                {
                    if (Battle.BattleShouldEnd)
                    {
                        GameRun.SynergyAdditionalCount -= 1;
                        yield break;
                    }
                    target = Battle.AllAliveEnemies.Sample(GameRun.BattleRng);
                }
                if (card.CardType == CardType.Misfortune || card.CardType == CardType.Status)
                {
                    yield return new ExileCardAction(card);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Nobody && !card.IsForbidden)
                {
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, UnitSelector.Nobody, Mana);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.SingleEnemy && !card.IsForbidden)
                {
                    UnitSelector unitSelector = new UnitSelector(target);
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, unitSelector, Mana);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.AllEnemies && !card.IsForbidden)
                {
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, UnitSelector.AllEnemies, Mana);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.RandomEnemy && !card.IsForbidden)
                {
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, UnitSelector.RandomEnemy, Mana);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Self && !card.IsForbidden)
                {
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, UnitSelector.Self, Mana);
                }
                else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.All && !card.IsForbidden)
                {
                    Helpers.FakeQueueConsumingMana(Mana);
                    yield return new UseCardAction(card, UnitSelector.All, Mana);
                }
                if (card.CardType == CardType.Friend && card.Zone == CardZone.PlayArea && card.Loyalty <= 0)
                {
                    yield return new RemoveCardAction(card);
                }
                //Battle.MaxHand = 10;
            }
            if (Battle.Player.HasStatusEffect<UnitedWeStandSeDef.UnitedWeStandSe>())
            {
                yield return new RemoveStatusEffectAction(Battle.Player.GetStatusEffect<UnitedWeStandSeDef.UnitedWeStandSe>(), true);
            }
            //Battle.MaxHand = _maxhand;
            GameRun.SynergyAdditionalCount -= 1;
            yield break;
        }
        //private int _maxhand;
    }
    public sealed class UnitedWeStandSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UnitedWeStandSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.UnitedWeStandSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Special,
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
        [EntityLogic(typeof(UnitedWeStandSeDef))]
        public sealed class UnitedWeStandSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(unit.DamageTaking, new GameEventHandler<DamageEventArgs>(OnDamageTaking));
                ReactOwnerEvent(Owner.TurnEnded, new EventSequencedReactor<UnitEventArgs>(OnTurnEnded));
            }
            private void OnDamageTaking(DamageEventArgs args)
            {
                int num = args.DamageInfo.Damage.RoundToInt();
                if (num > 0)
                {
                    NotifyActivating();
                    args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
                    args.AddModifier(this);
                }
            }
            private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
            {
                yield return new RemoveStatusEffectAction(this, true);
                yield break;
            }
            public override string UnitEffectName
            {
                get
                {
                    return "InvincibleLoop";
                }
            }
        }
    }
}
