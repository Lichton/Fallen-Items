using System;
using System.Collections.Generic;
using ItemAPI;

namespace CakeMod
{

	internal class SyngergraceThisMod
	{

		public class HatSynergy : AdvancedSynergyEntry
		{

			public HatSynergy()
			{
				this.NameKey = "Hat's Off to You";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Top Hat Effigy"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Hat Shells"].PickupObjectId,
					ETGMod.Databases.Items["Concealed Treasure"].PickupObjectId
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}

			
		}
		public class IceSynergy : AdvancedSynergyEntry
		{

			public IceSynergy()
			{
				this.NameKey = "Ice Cold";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Ice Ogre Head"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					387,
					130
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class OldSynergy : AdvancedSynergyEntry
		{

			public OldSynergy()
			{
				this.NameKey = "Ye Olden Ages";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Drawn47"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					484,
					638,
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class RainbowSynergy : AdvancedSynergyEntry
		{
			public RainbowSynergy()
			{
				this.NameKey = "Painbows & Gunshine";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Rainbowllets"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Bowler Hat"].PickupObjectId,
					ETGMod.Databases.Items["Bowler's Rainbow Dust"].PickupObjectId,
					100
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class LoveSynergy : AdvancedSynergyEntry
		{

			public LoveSynergy()
			{
				this.NameKey = "Bubble Love";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Baby Good Lovebulon"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Blob Heart"].PickupObjectId,
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class ChaosSynergy : AdvancedSynergyEntry
		{
			public ChaosSynergy()
			{
				this.NameKey = "Agent of Chaos";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Elixir"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					484,
					638
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		

		public class VoodooSynergy : AdvancedSynergyEntry
		{

			public VoodooSynergy()
			{
				this.NameKey = "Terrible Person";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Voodoo Heart"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Sewing Kit"].PickupObjectId,
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class SoundSynergy : AdvancedSynergyEntry
		{

			public SoundSynergy()
			{
				this.NameKey = "Micro-Phone";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Megaphone"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					443
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public class ArrowLSynergy : AdvancedSynergyEntry
		{

			public ArrowLSynergy()
			{
				this.NameKey = "Yhade ahdahOldedahgadn Agegdasgdahgas";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Pirate"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					484,
					638,
				};
				this.IgnoreLichEyeBullets = false;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
		public class TimeSynergy : AdvancedSynergyEntry
		{

			public TimeSynergy()
			{
				this.NameKey = "Time Paradox";
				this.MandatoryItemIDs = new List<int>
				{
					ETGMod.Databases.Items["Time Erasure Gun"].PickupObjectId,

				};
				this.OptionalItemIDs = new List<int>
				{
					169
				};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}

		public static void Synergies()
		{
			List<string> mandatoryConsoleIDtimeparadox = new List<string> {
			"cak:time_erasure_gun"
			};
			List<string> optionalConsoleIDtimeparadox = new List<string> {
			"black_hole_gun",
			};
			CustomSynergies.Add("Time Paradox", mandatoryConsoleIDtimeparadox, optionalConsoleIDtimeparadox);

			List<string> mandatoryConsoleIDtimeparadox2 = new List<string> {
			"cak:pirate"
			};
			List<string> optionalConsoleIDtimeparadox2 = new List<string> {
			"devolver",
			"devolver_rounds",
			};
			CustomSynergies.Add("Ye Olden Ages", mandatoryConsoleIDtimeparadox2, optionalConsoleIDtimeparadox2);
			List<string> mandatoryConsoleIDtimeparadox23 = new List<string> {
			"cak:sydney_sleeper"
			};
			List<string> optionalConsoleIDtimeparadox23 = new List<string> {
			"awp",
			};
			CustomSynergies.Add("A. W. Piss", mandatoryConsoleIDtimeparadox23, optionalConsoleIDtimeparadox23);

			List<string> mandatoryConsoleIDtimeparadox222 = new List<string> {
			"cak:scapegoat",
			"old_knights_flask"
			};
			
			CustomSynergies.Add("Darkened Soul", mandatoryConsoleIDtimeparadox222);

			List<string> mandatoryConsoleIDtimeparadox2222 = new List<string> {
			"cak:scapegoat",
			"super_hot_watch"
			};

			CustomSynergies.Add("Mind, Control, Delete", mandatoryConsoleIDtimeparadox2222);

			List<string> mandatoryConsoleIDtimeparadox22222 = new List<string> {
			"cak:scapegoat",
			"shell"
			};

			CustomSynergies.Add("Ridin' Shotgun", mandatoryConsoleIDtimeparadox22222);

			List<string> mandatoryConsoleIDtimeparadox222222 = new List<string> {
			"cak:scapegoat",
			"bullet"
			};

			CustomSynergies.Add("Skilled & Killed", mandatoryConsoleIDtimeparadox222222);

			List<string> mandatoryConsoleIDtimeparadox22224 = new List<string> {
			"cak:resourceful_round",
			"chamber_gun"
			};

			CustomSynergies.Add("Rat Mastery", mandatoryConsoleIDtimeparadox22224);
		}
	}
}