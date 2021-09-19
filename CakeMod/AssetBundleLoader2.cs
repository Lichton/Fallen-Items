using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CakeMod
{
	class AssetBundleLoader
	{

		public static AssetBundle LoadAssetBundleFromLiterallyAnywhere(string name)
		{

			AssetBundle result = null;
			bool flag = File.Exists(ItemsMod.metadataARCHIVE);
			if (flag)
			{
				ZipFile zipFile = ZipFile.Read(ItemsMod.metadataARCHIVE);
				bool flag2 = zipFile != null && zipFile.Entries.Count > 0;
				if (flag2)
				{
					foreach (ZipEntry zipEntry in zipFile.Entries)
					{
						bool flag3 = zipEntry.FileName == name;
						if (flag3)
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								zipEntry.Extract(memoryStream);
								memoryStream.Seek(0L, SeekOrigin.Begin);
								result = AssetBundle.LoadFromStream(memoryStream);
								Debug.Log("Successfully loaded assetbundle!");
								break;
							}
						}
					}
				}
			}
			else
			{
				bool flag4 = File.Exists(ItemsMod.metadata + "/" + name);
				if (flag4)
				{
					try
					{
						result = AssetBundle.LoadFromFile(Path.Combine(ItemsMod.metadata, name));
						Debug.Log("Successfully loaded assetbundle!");
					}
					catch (Exception ex)
					{
						Debug.LogError("Failed loading asset bundle from file.");
						Debug.LogError(ex.ToString());
					}
				}
				else
				{
					Debug.LogError("AssetBundle NOT FOUND!");
				}
			}

			return result;
		}
	}
}
