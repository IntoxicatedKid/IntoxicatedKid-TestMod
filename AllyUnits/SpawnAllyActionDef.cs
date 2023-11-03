using System;
using System.Collections.Generic;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation;

namespace test.AllyUnits
{
    public class SpawnAllyAction : SimpleEventBattleAction<UnitEventArgs>
    {
        public float OccupationTime { get; }
        public float FadeInDelay { get; }
        public SpawnAllyAction(PlayerUnit spawner, Type allyType, int rootIndex, float occupationTime = 0f, float fadeInDelay = 0.3f, bool isServant = true)
        {
            Args = new UnitEventArgs();
            _spawner = spawner;
            _allyType = allyType;
            _rootIndex = rootIndex;
            OccupationTime = occupationTime;
            FadeInDelay = fadeInDelay;
            _isServant = isServant;
        }

        // Token: 0x06000DAC RID: 3500 RVA: 0x00024474 File Offset: 0x00022674
        public override void PreEventPhase()
        {
            Trigger(Battle.EnemySpawning);
        }

        // Token: 0x06000DAD RID: 3501 RVA: 0x00024487 File Offset: 0x00022687
        public void MainPhase(BattleControllerAlly battleControllerAlly)
        {
            Args.Unit = battleControllerAlly.SpawnAlly(_spawner, _allyType, _rootIndex, _isServant);
            Args.IsModified = true;
        }

        // Token: 0x06000DAE RID: 3502 RVA: 0x000244C3 File Offset: 0x000226C3
        public override void PostEventPhase()
        {
            Trigger(Battle.EnemySpawned);
        }

        // Token: 0x0400060D RID: 1549
        private readonly PlayerUnit _spawner;

        // Token: 0x0400060E RID: 1550
        private readonly Type _allyType;

        // Token: 0x0400060F RID: 1551
        private readonly int _rootIndex;

        // Token: 0x04000610 RID: 1552
        private readonly bool _isServant;
    }
    [ActionViewerType(typeof(SpawnAllyAction))]
    public sealed class SpawnAllyAction<TAllyUnit> : SpawnAllyAction where TAllyUnit : AllyUnit
    {
        public SpawnAllyAction(PlayerUnit spawner, int rootIndex, float occupationTime = 0f, float fadeInDelay = 0.3f, bool isServant = true)
            : base(spawner, typeof(TAllyUnit), rootIndex, occupationTime, fadeInDelay, isServant)
        {
        }
    }
    public class BattleControllerAlly : BattleController
    {
        public BattleControllerAlly(GameRunController gameRun, EnemyGroup enemyGroup, IEnumerable<Card> deck) : base(gameRun, enemyGroup, deck)
        {
        }
        internal AllyUnit SpawnAlly(PlayerUnit spawner, Type type, int rootIndex, bool isServant)
        {
            return Spawn(spawner, CreateAllyUnit(type), rootIndex, isServant);
        }
        private AllyUnit Spawn(PlayerUnit spawner, AllyUnit allyUnit, int rootIndex, bool isServant)
        {
            allyUnit.EnterGameRun(GameMaster.Instance.CurrentGameRun);
            allyUnit.RootIndex = rootIndex;
            AllyGroup.Add(allyUnit);
            allyUnit.EnterBattle(this);
            if (isServant)
            {
                React(new ApplyStatusEffectAction<Servant>(allyUnit, null, null, null, null, 0f, true), null, ActionCause.None);
            }
            allyUnit.OnSpawn(spawner);
            return allyUnit;
        }
        public AllyGroup AllyGroup { get; }
        public static AllyUnit CreateAllyUnit(Type allyUnitType)
        {
            return TypeFactory<AllyUnit>.CreateInstance(allyUnitType);
        }
    }
}
