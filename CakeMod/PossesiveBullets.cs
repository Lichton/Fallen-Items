using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using SaveAPI;
using Dungeonator;
using System.Collections;

namespace CakeMod
{
    class PBullets : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Scapegoat";
            string resourceName = "CakeMod/Resources/substitute";

            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Accept The Substitute";
            string longDesc = "A lifelike automaton of the well-known bullet kin, it uses quantum entanglement to swap itself with dying gungeoneers.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cak");


            item.quality = PickupObject.ItemQuality.C;

        }

        public override void Pickup(PlayerController player)
        {
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage += this.ModifyIncomingDamage;
            player.OnNewFloorLoaded += this.Gdagadgdagad;
            base.Pickup(player);
        }

        private void Gdagadgdagad(PlayerController obj)
        {
            DoIReplace = true;
        }

        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            PlayableCharacters characterIdentity = Owner.characterIdentity;

            if (characterIdentity != PlayableCharacters.Robot)
            {
                if (DoIReplace == true)
                {
                    if (args.ModifiedDamage >= source.GetCurrentHealth())
                    {
                        PlayerController player = GameManager.Instance.PrimaryPlayer;
                        AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                        args.ModifiedDamage = 0f;
                        float CurrentHealth = Owner.stats.GetStatValue(PlayerStats.StatType.Health);
                        this.ImprovedMindControl(player);

                    }
                }
                else
                {
                    if (Owner.healthHaver.Armor == 1)
                    {
                        if (DoIReplace == true)
                        {
                                PlayerController player = GameManager.Instance.PrimaryPlayer;
                                AkSoundEngine.PostEvent("Play_VO_lichA_cackle_01", base.gameObject);
                            args.ModifiedDamage = 0f;
                                this.ImprovedMindControl(player);
                        }
                    }
                }
            }
        }

        private const string SynergyNameToCheck = "Mind, Control, Delete";
        public bool DoIReplace = true;
        private void ImprovedMindControl(PlayerController player)
        {
            var Enemy = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
            if (player.PlayerHasActiveSynergy("Skilled & Killed"))
            {
                Enemy = EnemyDatabase.GetOrLoadByGuid("70216cae6c1346309d86d4a0b4603045");
            }
            if (player.PlayerHasActiveSynergy("Darkened Soul"))
            {
                Enemy = EnemyDatabase.GetOrLoadByGuid("39e6f47a16ab4c86bec4b12984aece4c");
            }
            if (player.PlayerHasActiveSynergy("Ridin' Shotgun"))
            {
                Enemy = EnemyDatabase.GetOrLoadByGuid("128db2f0781141bcb505d8f00f9e4d47");
            }
            AIActor aiactor = AIActor.Spawn(Enemy.aiActor, player.CenterPosition, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Spawn, true);
            MindControlNotGamer THEBRAINWAVES = aiactor.gameObject.GetOrAddComponent<MindControlNotGamer>();
            THEBRAINWAVES.owner = player;
            GameManager.Instance.MainCameraController.StopTrackingPlayer();
            GameManager.Instance.MainCameraController.SetManualControl(true, false);
            aiactor.IgnoreForRoomClear = true;
            aiactor.CanTargetEnemies = true;
            aiactor.HitByEnemyBullets = true;
            aiactor.MovementSpeed = player.stats.MovementSpeed;
            aiactor.gameObject.AddComponent<KillOnRoomClear>();
            player.healthHaver.IsVulnerable = false;
            player.ToggleRenderer(false, "arbitrary teleporter.");
            player.ToggleGunRenderers(false, "arbitrary teleporter.");
            player.ToggleHandRenderers(false, "arbitrary teleporter.");
            aiactor.healthHaver.OnDeath += this.SummonPlayer;
            if (player.PlayerHasActiveSynergy(SynergyNameToCheck))
            {
                BraveTime.ClearMultiplier(base.gameObject);
                Gamer = true;
                player.RemovePassiveItem(279);
                aiactor.healthHaver.OnDamaged += this.SwapPositions;
                BraveTime.ClearMultiplier(base.gameObject);
            }
            daguy = aiactor;
            player.IsVisible = false;
            player.specRigidbody.CollideWithOthers = false;
           
            player.MovementModifiers += this.NoMotionModifier;
            player.IsStationary = true;
            player.CurrentStoneGunTimer = 9999999999999999999999f;
            StartCoroutine("jaja");
           
        }
        public AIActor aiactor5;
        public AIActor aiactor4;
        public AIActor aiactor3;
        public AIActor aiactor2;
        public int attempts = 0;
        private void SwapPositions(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            if (resultValue > 0)
            {
                attempts++;
                RoomHandler absoluteRoom = daguy.CenterPosition.GetAbsoluteRoom();
                AIActor randomActiveEnemy2 = absoluteRoom.GetRandomActiveEnemy(false);
                while(randomActiveEnemy2 == daguy && !(attempts >= 100))
                {
                    attempts = 0;
                    randomActiveEnemy2 = absoluteRoom.GetRandomActiveEnemy(false);
                }
                if (randomActiveEnemy2 != daguy)
                {
                    Vector2 vector = daguy.CenterPosition;
                    daguy.transform.position = randomActiveEnemy2.CenterPosition;
                    randomActiveEnemy2.transform.position = vector;
                    daguy.specRigidbody.Reinitialize();
                    randomActiveEnemy2.specRigidbody.Reinitialize();
                }

            }
        }

        public SpeculativeRigidbody specrigidbody;
        private void SummonPlayer(Vector2 obj)
        {
            DoIReplace = false;
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            if(Gamer == true)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(279).gameObject, player);
                Gamer = false;
            }
            player.healthHaver.IsVulnerable = true;
            player.IsVisible = true;
            player.ToggleRenderer(true, "arbitrary teleporter.");
            player.ToggleGunRenderers(true, "arbitrary teleporter.");
            player.ToggleHandRenderers(true, "arbitrary teleporter.");
            player.MovementModifiers -= this.NoMotionModifier;
            player.IsStationary = false;
            player.CurrentStoneGunTimer = 0f;
            GameManager.Instance.MainCameraController.StartTrackingPlayer();
            GameManager.Instance.MainCameraController.SetManualControl(false, true);
            player.specRigidbody.CollideWithOthers = true;
            RoomHandler absoluteRoom = player.CenterPosition.GetAbsoluteRoom();
            foreach (AIActor enemy in absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                if (enemy != daguy || enemy != aiactor2 || enemy != aiactor3 || enemy != aiactor4 || enemy != aiactor5)
                {

                    if (daguy.healthHaver.IsDead == true)
                    {

                        enemy.CanTargetPlayers = true;
                        enemy.CanTargetEnemies = false;
                        enemy.OverrideTarget = player.specRigidbody;

                    }
                }
            }
        }

        private IEnumerator jaja()
        {

            yield return new WaitForSeconds(10f);
            daguy.healthHaver.ApplyDamage(100000f, Vector2.zero, "da powa of da scapegoat", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
            yield break;
        }

        private void NoMotionModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
        {
            voluntaryVel = Vector2.zero;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            HealthHaver healthHaver = player.healthHaver;
            healthHaver.ModifyDamage -= this.ModifyIncomingDamage;
            return base.Drop(player);
        }
        int i = 0;
        protected override void Update()
        {
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            RoomHandler absoluteRoom = player.CenterPosition.GetAbsoluteRoom();
            foreach (AIActor enemy in absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                if (enemy != daguy || enemy != aiactor2 || enemy != aiactor3 || enemy != aiactor4 || enemy != aiactor5)
                {

                    if (daguy.healthHaver.IsAlive == true)
                    {

                        enemy.CanTargetPlayers = false;
                        enemy.CanTargetEnemies = true;
                        enemy.HitByEnemyBullets = true;
                        enemy.OverrideTarget = daguy.specRigidbody;

                    }

                }
            }
            if (daguy != null)
            {
                GameManager.Instance.MainCameraController.OverridePosition = daguy.Position;

            }
            if (daguy.healthHaver.IsDead != true)
            {
                
                player.specRigidbody.Position = new Position(daguy.Position);
                player.transform.position = (daguy.Position);
                player.gameActor.PlacedPosition = daguy.Position.IntXY();
            }
            base.Update();
        }
       
        public AIActor daguy;
public bool Gamer = false;
    }
}
