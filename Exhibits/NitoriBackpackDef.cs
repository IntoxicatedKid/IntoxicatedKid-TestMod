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
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.Core.Stations;
using LBoL.EntityLib.Exhibits.Common;
using HarmonyLib;
using LBoL.EntityLib.Cards.Character.Cirno;
using test.Cards;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Widgets;

namespace test.Exhibits
{
    public sealed class NitoriBackpackDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(NitoriBackpack);
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
                IsDebug: true,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 1,
                Value2: 3,
                Value3: null,
                Mana: new ManaGroup() { Green = 1 },
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
        [EntityLogic(typeof(NitoriBackpackDef))]
        [UsedImplicitly]
        public sealed class NitoriBackpack : ShiningExhibit
        {
            [HarmonyPatch(typeof(ExhibitWidget), nameof(ExhibitWidget.OnExhibitClicked))]
            class ExhibitWidget_OnExhibitClicked_Patch
            {
                static void Postfix(ExhibitWidget __instance)
                {
                    if (__instance.Exhibit is NitoriBackpack && GameMaster.Instance.CurrentGameRun.Player.HasExhibit<NitoriBackpack>())
                    {
                        GameMaster.Instance.StartCoroutine(Proceed());
                    }
                }
                static private IEnumerator Proceed()
                {
                    List<Card> list = new List<Card>
                        {
                            Library.CreateCard<NitoriCraftSimple>(),
                            Library.CreateCard<NitoriCraftAdvanced>(),
                            Library.CreateCard<NitoriInventory>(),
                            Library.CreateCard<NitoriBlueprint>()
                        };
                    MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(list, false, false, true)
                    {
                        CanCancel = true,
                        Source = GameMaster.Instance.CurrentGameRun.Player.GetExhibit<NitoriBackpack>(),
                        Description = "Choose an option"
                    };
                    yield return GameMaster.Instance.CurrentGameRun.InteractionViewer.View(interaction);
                    if (interaction.SelectedCard == list[0])
                    {
                        Debug.LogError("UNIMPLEMENTED");
                    }
                    else if (interaction.SelectedCard == list[1])
                    {
                        Debug.LogError("UNIMPLEMENTED");
                    }
                    else if (interaction.SelectedCard == list[2])
                    {
                        ShowCardsPayload showCardsPayload = new ShowCardsPayload
                        {
                            Name = "Game.Deck".Localize(true),
                            Description = "Cards.Show".Localize(true),
                            Cards = NitoriInventory.defaultMaterials
                        };
                        UiManager.GetPanel<ShowCardsPanel>().Show(showCardsPayload);
                    }
                    else if (interaction.SelectedCard == list[3])
                    {
                        Debug.LogError("UNIMPLEMENTED");
                    }
                }
            }
        }
    }
}