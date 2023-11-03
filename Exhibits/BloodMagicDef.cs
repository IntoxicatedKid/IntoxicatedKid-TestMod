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
using Mono.Cecil;
using LBoL.Core.StatusEffects;
using System.Linq;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Misfortune;
using static UnityEngine.TouchScreenKeyboard;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using LBoL.EntityLib.Exhibits;
using JetBrains.Annotations;
using LBoL.Core.Stations;

namespace test.Exhibits
{
    public sealed class BloodMagicDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BloodMagic);
        }
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.ExhibitsEn.yaml");
            return locFiles;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: sequenceTable.Next(typeof(ExhibitConfig)),
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 2,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Philosophy = 5 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(BloodMagicDef))]
        [UsedImplicitly]
        public sealed class BloodMagic : ShiningExhibit
        {
            private bool Free = false;
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnStarted));
                ReactBattleEvent(Battle.ManaConsumed, new EventSequencedReactor<ManaEventArgs>(OnManaDecreased));
                ReactBattleEvent(Battle.ManaLost, new EventSequencedReactor<ManaEventArgs>(OnManaDecreased));
                ReactBattleEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
            }
            private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
            {
                Active = true;
                Free = false;
                if (Active && !Free && !Battle.BattleShouldEnd && Battle.BattleMana == ManaGroup.Empty)
                {
                    NotifyActivating();
                    Free = true;
                    foreach (Card card in Battle.EnumerateAllCards())
                    {
                        card.FreeCost = true;
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnManaDecreased(ManaEventArgs args)
            {
                if (Active && !Free && !Battle.BattleShouldEnd && Battle.BattleMana == ManaGroup.Empty)
                {
                    NotifyActivating();
                    Free = true;
                    foreach (Card card in Battle.EnumerateAllCards())
                    {
                        card.FreeCost = true;
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (Free && args.Card.BaseCost.Amount > 0 && !args.Card.IsXCost)
                {
                    NotifyActivating();
                    Active = false;
                    Free = false;
                    foreach (Card card in Battle.EnumerateAllCards())
                    {
                        card.FreeCost = false;
                    }
                    yield return new DamageAction(Owner, Owner, DamageInfo.HpLose(args.Card.BaseCost.Amount * Value1), "Instant", GunType.Single);
                }
            }
            protected override void OnLeaveBattle()
            {
                Active = false;
                Free = false;
            }
        }
    }
}