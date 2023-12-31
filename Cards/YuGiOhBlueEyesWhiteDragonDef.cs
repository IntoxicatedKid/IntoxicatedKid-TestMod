﻿/*using LBoL.Base;
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
using JetBrains.Annotations;

namespace test
{
    public sealed class YuGiOhBlueEyesWhiteDragonDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(YuGiOhBlueEyesWhiteDragon);
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
               IsPooled: false,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.White },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, White = 3 },
               UpgradedCost: new ManaGroup() { Any = 1, White = 2 },
               MoneyCost: null,
               Damage: 30,
               UpgradedDamage: null,
               Block: 25,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 30,
               UpgradedValue1: null,
               Value2: 25,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: 1,
               UpgradedLoyalty: null,
               PassiveCost: 8,
               UpgradedPassiveCost: 8,
               ActiveCost: -8,
               UpgradedActiveCost: -8,
               UltimateCost: -8,
               UpgradedUltimateCost: -8,

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
               Illustrator: "",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(YuGiOhBlueEyesWhiteDragonDef))]
    public sealed class YuGiOhBlueEyesWhiteDragon : Card
    {
        [UsedImplicitly]
        public override FriendCostInfo FriendU
        {
            get
            {
                return new FriendCostInfo(base.UltimateCost, FriendCostType.Active);
            }
        }
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
}*/
