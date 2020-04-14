using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OSFEModding
{
    public class SelicyZone : ICustomZone
    {
        public SelicyZone()
        {
        }

        public void GenerateZone(SpawnCtrl spawnCtrl, ZoneType zoneType)
        {
            if (zoneType == (ZoneType) 17)
            {
                spawnCtrl.ti.mainBattleGrid.currentEnemies.Clear();
                spawnCtrl.ctrl.GameState = GState.Battle;


                List<Tile> tileList = spawnCtrl.ti.mainBattleGrid.Get(new TileApp(Location.RandEnemyUnique, Shape.Default, Pattern.Unoccupied, 1, (Tile) null, Pattern.All), 0,
                    (Being) null, (ItemObject) null, true);
                int index = Random.Range(0, tileList.Count);
                spawnCtrl.PlaceBeing("BossSelicy", spawnCtrl.ti.mainBattleGrid.grid[tileList[index].x, tileList[index].y], 0, true, (BattleGrid) null);
            }
        }

        public void SetupZone(WorldBar worldBar)
        {
            if (!worldBar.zoneSprites.ContainsKey("17"))
                worldBar.zoneSprites.Add("17", worldBar.zoneSprites["Shop"]);

            foreach (ZoneDot currentZoneDot in worldBar.currentZoneDots)
            {
                if(currentZoneDot.type == ZoneType.Shop && Random.Range(0,10) == 0)
                    currentZoneDot.SetType((ZoneType) 17);
            }
        }
    }
}