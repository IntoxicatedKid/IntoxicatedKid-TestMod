using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using UnityEngine;
using LBoL.Core;

namespace test.AllyUnits
{
    // Token: 0x02000073 RID: 115
    public class AllyGroup : IEnumerable<AllyUnit>, IEnumerable
    {
        // Token: 0x17000194 RID: 404
        // (get) Token: 0x0600054C RID: 1356 RVA: 0x00010E80 File Offset: 0x0000F080
        public string Id { get; }

        // Token: 0x0600054D RID: 1357 RVA: 0x00010E88 File Offset: 0x0000F088
        internal AllyGroup(string id, IEnumerable<AllyGroupEntry.EntrySource> entries, EnemyType enemyType, string formationName, Vector2 playerRootV2, string preBattleDialogName, string postBattleDialogName, float debutTime)
        {
            Id = id;
            _enemies = new List<AllyUnit>();
            foreach (AllyGroupEntry.EntrySource entrySource in entries)
            {
                AllyUnit AllyUnit = TypeFactory<AllyUnit>.CreateInstance(entrySource.Type);
                AllyUnit AllyUnit2 = AllyUnit;
                int num = _index + 1;
                _index = num;
                AllyUnit2.Index = num;
                AllyUnit.RootIndex = entrySource.RootIndex;
                _enemies.Add(AllyUnit);
            }
            EnemyType = enemyType;
            FormationName = formationName;
            PlayerRootV2 = playerRootV2;
            PreBattleDialogName = preBattleDialogName;
            PostBattleDialogName = postBattleDialogName;
            DebutTime = debutTime;
        }

        // Token: 0x17000195 RID: 405
        // (get) Token: 0x0600054E RID: 1358 RVA: 0x00010F4C File Offset: 0x0000F14C
        public EnemyType EnemyType { get; }

        // Token: 0x17000196 RID: 406
        // (get) Token: 0x0600054F RID: 1359 RVA: 0x00010F54 File Offset: 0x0000F154
        public string FormationName { get; }

        // Token: 0x17000197 RID: 407
        // (get) Token: 0x06000550 RID: 1360 RVA: 0x00010F5C File Offset: 0x0000F15C
        public Vector2 PlayerRootV2 { get; }

        // Token: 0x17000198 RID: 408
        // (get) Token: 0x06000551 RID: 1361 RVA: 0x00010F64 File Offset: 0x0000F164
        public string PreBattleDialogName { get; }

        // Token: 0x17000199 RID: 409
        // (get) Token: 0x06000552 RID: 1362 RVA: 0x00010F6C File Offset: 0x0000F16C
        public string PostBattleDialogName { get; }

        // Token: 0x1700019A RID: 410
        // (get) Token: 0x06000553 RID: 1363 RVA: 0x00010F74 File Offset: 0x0000F174
        public float DebutTime { get; }

        // Token: 0x1700019B RID: 411
        // (get) Token: 0x06000554 RID: 1364 RVA: 0x00010F7C File Offset: 0x0000F17C
        public int Count
        {
            get
            {
                return _enemies.Count;
            }
        }

        // Token: 0x1700019C RID: 412
        // (get) Token: 0x06000555 RID: 1365 RVA: 0x00010F89 File Offset: 0x0000F189
        public IEnumerable<AllyUnit> Alives
        {
            get
            {
                return _enemies.Where((e) => e.IsAlive);
            }
        }

        // Token: 0x1700019D RID: 413
        // (get) Token: 0x06000556 RID: 1366 RVA: 0x00010FB5 File Offset: 0x0000F1B5
        public IEnumerable<AllyUnit> Deads
        {
            get
            {
                return _enemies.Where((e) => e.IsDead);
            }
        }

        // Token: 0x06000557 RID: 1367 RVA: 0x00010FE1 File Offset: 0x0000F1E1
        public IEnumerator<AllyUnit> GetEnumerator()
        {
            return _enemies.GetEnumerator();
        }

        // Token: 0x06000558 RID: 1368 RVA: 0x00010FF3 File Offset: 0x0000F1F3
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Token: 0x06000559 RID: 1369 RVA: 0x00010FFC File Offset: 0x0000F1FC
        internal void Add(AllyUnit enemy)
        {
            int num = _index + 1;
            _index = num;
            enemy.Index = num;
            _enemies.Add(enemy);
        }

        // Token: 0x0400029C RID: 668
        private readonly List<AllyUnit> _enemies;

        // Token: 0x0400029D RID: 669
        private int _index;
    }
}
