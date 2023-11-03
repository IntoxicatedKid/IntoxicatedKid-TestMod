using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Attributes;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActionRecord;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Dialogs;
using LBoL.Core.GapOptions;
using LBoL.Core.Helpers;
using LBoL.Core.Intentions;
using LBoL.Core.JadeBoxes;
using LBoL.Core.PlatformHandlers;
using LBoL.Core.Randoms;
using LBoL.Core.SaveData;
using LBoL.Core.Stations;
using LBoL.Core.Stats;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.Common;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Adventures.Shared12;
using LBoL.EntityLib.Adventures.Shared23;
using LBoL.EntityLib.Adventures.Stage1;
using LBoL.EntityLib.Adventures.Stage2;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Koishi;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoL.EntityLib.Cards.Neutral.Green;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Cards.Neutral.Red;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.Cards.Neutral.White;
using LBoL.EntityLib.Cards.Other.Adventure;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.EntityLib.Cards.Other.Misfortune;
using LBoL.EntityLib.Cards.Other.Tool;
using LBoL.EntityLib.Dolls;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Character.DreamServants;
using LBoL.EntityLib.EnemyUnits.Lore;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.EnemyUnits.Normal.Bats;
using LBoL.EntityLib.EnemyUnits.Normal.Drones;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.EntityLib.EnemyUnits.Normal.Maoyus;
using LBoL.EntityLib.EnemyUnits.Normal.Ravens;
using LBoL.EntityLib.EnemyUnits.Opponent;
using LBoL.EntityLib.Exhibits;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Mythic;
using LBoL.EntityLib.Exhibits.Seija;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.Mixins;
using LBoL.EntityLib.PlayerUnits;
using LBoL.EntityLib.Stages;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Enemy.SeijaItems;
using LBoL.EntityLib.StatusEffects.Marisa;
using LBoL.EntityLib.StatusEffects.Neutral;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoL.EntityLib.StatusEffects.Neutral.Green;
using LBoL.EntityLib.StatusEffects.Neutral.Red;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Neutral.White;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoL.EntityLib.UltimateSkills;
using LBoL.Presentation;
using LBoL.Presentation.Animations;
using LBoL.Presentation.Bullet;
using LBoL.Presentation.Effect;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Dialogs;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Transitions;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.ReflectionHelpers;
using LBoLEntitySideloader.Resource;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using test.Cards;
using test.Exhibits;
using test.Stages;
using UnityEngine;
using Untitled;
using Untitled.ConfigDataBuilder;
using Untitled.ConfigDataBuilder.Base;
using Debug = UnityEngine.Debug;

namespace test
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.version)]
    [BepInDependency(LBoLEntitySideloader.PluginInfo.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AddWatermark.API.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("LBoL.exe")]
    public class BepinexPlugin : BaseUnityPlugin
    {

        private static readonly Harmony harmony = PluginInfo.harmony;

        internal static BepInEx.Logging.ManualLogSource log;

        internal static TemplateSequenceTable sequenceTable = new TemplateSequenceTable();

        internal static IResourceSource embeddedSource = new EmbeddedSource(Assembly.GetExecutingAssembly());

        internal static DirectorySource directorySource = new DirectorySource(PluginInfo.GUID, "");

        static KeyboardShortcut TestF1Key = new KeyboardShortcut(KeyCode.F1);

        static KeyboardShortcut TestF2Key = new KeyboardShortcut(KeyCode.F2);

        private void Awake()
        {
            log = Logger;

            // very important. Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            EntityManager.RegisterSelf();

            harmony.PatchAll();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(AddWatermark.API.GUID))
                WatermarkWrapper.ActivateWatermark();

            NewBackgrounds.AddNewBackgrounds();
        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }
        private void Update()
        {
            if (TestF1Key.IsDown())
            {
                if (GameMaster.Instance?.CurrentGameRun != null)
                {
                    GameMaster.Instance.StartCoroutine(TestF1());
                }
                else
                {
                    log.LogInfo("Run needs to be started.");
                }
            }
            if (TestF2Key.IsDown())
            {
                if (GameMaster.Instance?.CurrentGameRun != null)
                {
                    GameMaster.Instance.StartCoroutine(TestF2());
                }
                else
                {
                    log.LogInfo("Run needs to be started.");
                }
            }
        }
        private IEnumerator TestF1()
        {
            yield return new WaitForSeconds(.1f);
            List<Exhibit> list = new List<Exhibit>
            {
                Library.CreateExhibit<TestExhibitDef.TestExhibit>()

            };
            if (!GameMaster.Instance.CurrentGameRun.Player.HasExhibit<TestExhibitDef.TestExhibit>())
            {
                foreach (Exhibit exhibit in list)
                {
                    yield return GameMaster.Instance.CurrentGameRun.GainExhibitRunner(exhibit, true, new VisualSourceData
                    {
                        SourceType = VisualSourceType.Vn,
                        Index = -1
                    });
                }
            }
            else
            {
                foreach (Exhibit exhibit in list)
                {
                    GameMaster.Instance.CurrentGameRun.LoseExhibit(GameMaster.Instance.CurrentGameRun.Player.GetExhibit<TestExhibitDef.TestExhibit>(), false, true);
                }
            }
        }
        private IEnumerator TestF2()
        {
            yield return new WaitForSeconds(.1f);
            List<Exhibit> list = new List<Exhibit>
            {
                Library.CreateExhibit<TASBotDef.TASBot>()
            };
            if (!GameMaster.Instance.CurrentGameRun.Player.HasExhibit<TASBotDef.TASBot>())
            {
                foreach (Exhibit exhibit in list)
                {
                    yield return GameMaster.Instance.CurrentGameRun.GainExhibitRunner(exhibit, true, new VisualSourceData
                    {
                        SourceType = VisualSourceType.Vn,
                        Index = -1
                    });
                }
            }
            else
            {
                foreach (Exhibit exhibit in list)
                {
                    GameMaster.Instance.CurrentGameRun.LoseExhibit(GameMaster.Instance.CurrentGameRun.Player.GetExhibit<TASBotDef.TASBot>(), false, true);
                }
            }
        }
        public static class Helpers
        {
            public static void FakeQueueConsumingMana(ManaGroup manaGroup)
            {
                // should not be changed
                var cost = manaGroup;
                var manaPanel = UiManager.GetPanel<BattleManaPanel>();
                manaPanel._consumingDeque.Insert(0, new BattleManaPanel.ConsumingManaWidgets(new ConsumingMana(cost, new ManaGroup() { Any = 0 }), new List<BattleManaWidget>() { }, new List<BattleManaWidget>() { }));

            }
        }
        /*[HarmonyPatch(typeof(BattleManaPanel), nameof(BattleManaPanel.ConsumeAndReleaseManaWidget))]
        class BattleManaPanel_ConsumeAndReleaseManaWidget_Patch
        {
            static bool Prefix(BattleManaPanel __instance)
            {
                if (StSAmplifySeDef.StSAmplifySe.card != null || StSBurstSeDef.StSBurstSe.card != null || StSDoubleTapSeDef.StSDoubleTapSe.card != null || StSEchoFormSeDef.StSEchoFormSe.card != null)
                {
                    return false;
                }
                return true;
            }
        }*/
        /*[HarmonyPatch(typeof(BossStation), nameof(BossStation.GenerateBossRewards))]
        class BossStation_GenerateBossRewards_Patch
        {
            static void Postfix(BossStation __instance)
            {
                List<Exhibit> list = new List<Exhibit>
                    {
                        Library.CreateExhibit<HuiyeBaoxiang>(),
                        Library.CreateExhibit<HuiyeBaoxiang>(),
                        Library.CreateExhibit<HuiyeBaoxiang>()
                    };
                __instance.BossRewards = list.ToArray();
            }
        }*/
        /*[HarmonyPatch]
        class _Allowduplicateexhibit1_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return ExtraAccess.InnerMoveNext(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner));
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                CodeInstruction prevprevCi = null;
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Call && prevCi.opcode == OpCodes.Call && prevprevCi.opcode == OpCodes.Callvirt)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Ret && prevCi.opcode == OpCodes.Ldc_I4_0 && prevprevCi.opcode == OpCodes.Call)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Newobj && prevCi.opcode == OpCodes.Ldstr && prevprevCi.opcode == OpCodes.Ldstr)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Throw && prevCi.opcode == OpCodes.Newobj && prevprevCi.opcode == OpCodes.Ldstr)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevprevCi = prevCi;
                    prevCi = ci;
                }
            }
        }
        [HarmonyPatch(typeof(PlayerUnit), nameof(PlayerUnit.AddExhibit))]
        class _Allowduplicateexhibit2_Patch
        {

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeInstruction prevprevCi = null;
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Throw && prevCi.opcode == OpCodes.Newobj && prevprevCi.opcode == OpCodes.Ldstr)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else if (prevprevCi != null && prevCi != null && ci.opcode == OpCodes.Newobj && prevCi.opcode == OpCodes.Ldstr && prevprevCi.opcode == OpCodes.Ldstr)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevprevCi = prevCi;
                    prevCi = ci;
                }
            }
        }*/
        /*[HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SetTurboMode))]
        class GameMaster_SetTurboMode_Patch
        {
            static bool Prefix(bool turboMode)
            {
                Time.timeScale = (turboMode ? 2.4f : 1.5f);
                return false;
            }
        }*/
        /*[HarmonyPatch(typeof(CardUi), nameof(CardUi.Awake))]
        class _13thCard_Patch
        {

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_S && prevCi.opcode == OpCodes.Ldloc_0)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, 99);
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevCi = ci;
                }
            }
        }*/
        /*[HarmonyPatch]
        class ViewConsumeMana_ErrorMessage_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return ExtraAccess.InnerMoveNext(typeof(BattleManaPanel), nameof(BattleManaPanel.ViewConsumeMana));
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (prevCi != null && ci.opcode == OpCodes.Call && prevCi.opcode == OpCodes.Ldstr && prevCi.operand.ToString() == "Cannot dequeue consuming mana, resetting all.")
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else if (prevCi != null && ci.opcode == OpCodes.Call && prevCi.opcode == OpCodes.Call)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevCi = ci;
                }
            }
        }*/
        /*[HarmonyPatch]
        class ViewConsumeMana_ErrorMessage2_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return ExtraAccess.InnerMoveNext(typeof(BattleManaPanel), nameof(BattleManaPanel.ViewConsumeMana));
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var finish = false;
                int start = -1, end = -1;

                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldstr)
                    {
                        if (finish)
                        {
                            Debug.LogError("endline " + i);

                            end = i;
                            break;
                        }
                        else
                        {
                            Debug.LogError("startline " + (i + 1));

                            start = i + 1;

                            for (int j = start; j < codes.Count; j++)
                            {
                                if (codes[j].opcode == OpCodes.Call && codes[j - 1].opcode == OpCodes.Call)
                                {
                                    finish = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (start > -1 && end > -1)
                {
                    codes[start].opcode = OpCodes.Pop;
                    codes.RemoveRange(start + 1, end - start - 1);
                }
                return codes.AsEnumerable();
            }
        }*/
    }
}
