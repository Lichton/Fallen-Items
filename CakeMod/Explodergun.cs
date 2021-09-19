using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace CakeMod
{
	// Token: 0x0200006A RID: 106
	public class Explodergun : GunBehaviour
	{
		// Token: 0x0600028C RID: 652 RVA: 0x000162F8 File Offset: 0x000144F8
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Explodergun", "ExplodeCharge");
			Game.Items.Rename("outdated_gun_mods:explodergun", "cak:explodergun");
			GunExt.SetShortDescription(gun, "Crowbot");
			GunExt.SetLongDescription(gun, "A covering that can be applied to the arm. it shoots grenades at rapid pace, but has an obnoxious purple colouring.");
			GunExt.SetupSprite(gun, null, "ExplodeCharge_idle_001", 8);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 16);
			GunExt.SetAnimationFPS(gun, gun.chargeAnimation, 6);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(19) as Gun, true, false);



			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				projectileModule.ammoCost = 1;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.Charged;
				projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				projectileModule.cooldownTime = 1f;
				projectileModule.angleVariance = 20f;
				projectileModule.numberOfShotsInClip = 1;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
				projectileModule.projectiles[0] = projectile;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				projectile.baseData.damage *= 2f;
				projectile.AdditionalScaleMultiplier *= 1.2f;
				projectile.baseData.range *= 0.5f;
				GoopModifier goop = projectile.gameObject.AddComponent<GoopModifier>();
				bool flag = projectileModule != gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 0;
				}
				projectile.transform.parent = gun.barrelOffset;
				ProjectileModule.ChargeProjectile item = new ProjectileModule.ChargeProjectile
				{
					Projectile = projectile,
					ChargeTime = 1f
				};
				PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
				orAddComponent.penetratesBreakables = true;
				orAddComponent.penetration++;
				projectileModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
				{
					item
				};
			}
			gun.reloadTime = 1f;
			gun.SetBaseMaxAmmo(100);
			gun.DefaultModule.angleVariance = 0.2f;
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 3;
			gun.encounterTrackable.EncounterGuid = "goboomlolololol";
			gun.gunHandedness = GunHandedness.HiddenOneHanded;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.barrelOffset.transform.localPosition = new Vector3(1.37f, 0.37f, 0f);
			Explodergun.ExplodergunID = gun.PickupObjectId;
			CakeIDs.Explodergun = gun.PickupObjectId;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001666C File Offset: 0x0001486C
		public override void PostProcessProjectile(Projectile projectile)
		{
			base.PostProcessProjectile(projectile);
		}


		public override void OnPostFired(PlayerController player, Gun gun)
		{
			//This determines what sound you want to play when you fire a gun.
			//Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_BOSS_tank_grenade_01	", gameObject);
		}
		private bool HasReloaded;
		//This block of code allows us to change the reload sounds.
		protected void Update()
		{
			if (gun.CurrentOwner)
			{

				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_face_melter_shot_01", base.gameObject);
			}
		}

		// Token: 0x040000DB RID: 219
		public static int ExplodergunID;

		protected Vector2? m_lastGoopPosition;



	}
}