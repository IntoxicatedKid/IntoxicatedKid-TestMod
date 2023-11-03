using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.Core;
using UnityEngine;

namespace test.AllyUnits
{
    // Token: 0x02000074 RID: 116
    public class AllyGroupEntry : IEnumerable<AllyGroupEntry.EntrySource>, IEnumerable
    {
        // Token: 0x1700019E RID: 414
        // (get) Token: 0x0600055A RID: 1370 RVA: 0x0001102C File Offset: 0x0000F22C
        public AllyGroupConfig Config { get; }

        // Token: 0x1700019F RID: 415
        // (get) Token: 0x0600055B RID: 1371 RVA: 0x00011034 File Offset: 0x0000F234
        public string Id
        {
            get
            {
                return Config.Id;
            }
        }

        // Token: 0x170001A0 RID: 416
        // (get) Token: 0x0600055C RID: 1372 RVA: 0x00011041 File Offset: 0x0000F241
        public EnemyType EnemyType
        {
            get
            {
                return Config.EnemyType;
            }
        }

        // Token: 0x170001A1 RID: 417
        // (get) Token: 0x0600055D RID: 1373 RVA: 0x0001104E File Offset: 0x0000F24E
        public bool RollBossExhibit
        {
            get
            {
                return Config.RollBossExhibit;
            }
        }

        // Token: 0x170001A2 RID: 418
        // (get) Token: 0x0600055E RID: 1374 RVA: 0x0001105B File Offset: 0x0000F25B
        public string FormationName
        {
            get
            {
                return Config.FormationName;
            }
        }

        // Token: 0x170001A3 RID: 419
        // (get) Token: 0x0600055F RID: 1375 RVA: 0x00011068 File Offset: 0x0000F268
        public Vector2 PlayerRootV2
        {
            get
            {
                return Config.PlayerRoot;
            }
        }

        // Token: 0x170001A4 RID: 420
        // (get) Token: 0x06000560 RID: 1376 RVA: 0x00011075 File Offset: 0x0000F275
        public string PreBattleDialogName
        {
            get
            {
                return Config.PreBattleDialogName;
            }
        }

        // Token: 0x170001A5 RID: 421
        // (get) Token: 0x06000561 RID: 1377 RVA: 0x00011082 File Offset: 0x0000F282
        public string PostBattleDialogName
        {
            get
            {
                return Config.PostBattleDialogName;
            }
        }

        // Token: 0x170001A6 RID: 422
        // (get) Token: 0x06000562 RID: 1378 RVA: 0x0001108F File Offset: 0x0000F28F
        public float DebutTime
        {
            get
            {
                return Config.DebutTime;
            }
        }

        // Token: 0x06000563 RID: 1379 RVA: 0x0001109C File Offset: 0x0000F29C
        public AllyGroupEntry(AllyGroupConfig config)
        {
            Config = config;
        }

        // Token: 0x06000564 RID: 1380 RVA: 0x000110B6 File Offset: 0x0000F2B6
        public void Add(Type type)
        {
            _entries.Add(new EntrySource(type, _entries.Count));
        }

        // Token: 0x06000565 RID: 1381 RVA: 0x000110D4 File Offset: 0x0000F2D4
        public void Add(Type type, int rootIndex)
        {
            _entries.Add(new EntrySource(type, rootIndex));
        }

        // Token: 0x06000566 RID: 1382 RVA: 0x000110E8 File Offset: 0x0000F2E8
        internal AllyGroup Generate(GameRunController gameRun)
        {
            AllyGroup AllyGroup = new AllyGroup(Id, _entries, EnemyType, FormationName, PlayerRootV2, PreBattleDialogName, PostBattleDialogName, DebutTime);
            foreach (AllyUnit enemyUnit in AllyGroup)
            {
                enemyUnit.EnterGameRun(gameRun);
            }
            return AllyGroup;
        }

        // Token: 0x06000567 RID: 1383 RVA: 0x00011168 File Offset: 0x0000F368
        public IEnumerator<EntrySource> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        // Token: 0x06000568 RID: 1384 RVA: 0x0001117A File Offset: 0x0000F37A
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Token: 0x06000569 RID: 1385 RVA: 0x00011184 File Offset: 0x0000F384
        public List<EntrySource> ToList()
        {
            if (_list != null)
            {
                return _list;
            }
            _list = Enumerable.Repeat<EntrySource>(null, 5).ToList();
            foreach (EntrySource entrySource in _entries)
            {
                if (_list[entrySource.RootIndex] != null)
                {
                    throw new InvalidOperationException("'" + Id + "' has duplicated root index");
                }
                _list[entrySource.RootIndex] = entrySource;
            }
            return _list;
        }

        // Token: 0x040002A5 RID: 677
        private readonly List<EntrySource> _entries = new List<EntrySource>();

        // Token: 0x040002A6 RID: 678
        private List<EntrySource> _list;

        // Token: 0x020001FE RID: 510
        public class EntrySource
        {
            // Token: 0x170004DF RID: 1247
            // (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0002A84B File Offset: 0x00028A4B
            public Type Type { get; }

            // Token: 0x170004E0 RID: 1248
            // (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0002A853 File Offset: 0x00028A53
            public int RootIndex { get; }

            // Token: 0x06000FE8 RID: 4072 RVA: 0x0002A85B File Offset: 0x00028A5B
            public EntrySource(Type type, int rootIndex)
            {
                Type = type;
                RootIndex = rootIndex;
            }
        }
    }
}
