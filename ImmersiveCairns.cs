using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Text;

namespace ImmersiveCairns {
	internal class ImmersiveCairns {

		private const string PREFIX = "CAIRN_Quote";
		private const string SOME_LOC_KEY = PREFIX + "001";
		private const string DEFAULT_MESSAGE = "\"Its story is not yet written...\"";
		private const string CAIRN_FILE_NAME = "ImmersiveCairns.json";

		public static void OnLoad() {
			CairnStrings cairnStrings = LoadCairnStrings();
			ReplaceCairnMessages(cairnStrings);
			Debug.Log("[ImmersiveCairns] Replaced cairn messages.");
		}

		private static CairnStrings LoadCairnStrings() {
			string modsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string stringsFilePath = Path.Combine(modsDir, CAIRN_FILE_NAME);
			if (!File.Exists(stringsFilePath))
				throw new Exception("[ImmersiveCairns] Missing cairn messages file. Copy both the DLL and the JSON file into the mods folder.");

			string stringsJson = File.ReadAllText(stringsFilePath, Encoding.UTF8);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<CairnStrings>(stringsJson);
		}

		private static void ReplaceCairnMessages(CairnStrings cairnStrings) {
			Dictionary<string, string[]> dictionary = Localization.dictionary;
			int arrayLength = dictionary[SOME_LOC_KEY].Length;

			foreach (KeyValuePair<string, string> entry in cairnStrings.Replace) {
				dictionary[PREFIX + entry.Key] = MakeArray(arrayLength, entry.Value);
			}
			foreach (KeyValuePair<string, string> entry in cairnStrings.Add) {
				string[] previous = dictionary[PREFIX + entry.Key];
				if (previous[0] != DEFAULT_MESSAGE)
					continue;

				dictionary[PREFIX + entry.Key] = MakeArray(arrayLength, entry.Value);
			}
		}

		private static string[] MakeArray(int size, string firstValue) {
			string[] array = new string[size];
			array[0] = firstValue;
			return array;
		}

		private class CairnStrings {
			public Dictionary<string, string> Replace = new Dictionary<string, string>();
			public Dictionary<string, string> Add = new Dictionary<string, string>();
		}
	}
}
