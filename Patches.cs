using Harmony;
using UnityEngine;

namespace ImmersiveCairns {
	internal class Patches {
		[HarmonyPatch(typeof(Panel_HUD), "Start")]
		private class CairnSetupPatch {
			private static void Postfix(Panel_HUD __instance) {
				UILabel cairnNumberLabel = __instance.m_CairnBackerLabel.GetComponent<UILabel>();
				cairnNumberLabel.SetAnchor((Transform) null);
				cairnNumberLabel.transform.localPosition = new Vector3(0, -42, 0);
			}
		}

		[HarmonyPatch(typeof(Panel_HUD), "ShowCairnNotification")]
		private class CairnDisplayPatch {
			private static bool Prefix(Panel_HUD __instance, Cairn cairn) {
				__instance.m_CairnNameLabel.text = Localization.Get("GAMEPLAY_Cairn");
				__instance.m_CairnQuoteLabel.text = Localization.Get("CAIRN_Quote" + cairn.m_BackerLookupNum);

				string cairnNumber = Localization.Get("GAMEPLAY_CollectionCounter");
				cairnNumber = cairnNumber.Replace("{num-value}", cairn.m_JournalEntryNumber.ToString());
				cairnNumber = cairnNumber.Replace("{max-value}", InterfaceManager.m_Panel_Log.m_TotalNumCairnCollectibles.ToString());
				__instance.m_CairnBackerLabel.text = cairnNumber;
				__instance.m_CairnThanksLabel.text = string.Empty;

				__instance.m_CairnNotificationTween.gameObject.SetActive(true);
				__instance.m_CairnNotificationTween.ResetToBeginning();
				__instance.m_CairnNotificationTween.PlayForward();

				AccessTools.Field(typeof(Panel_HUD), "m_IsShowingCairnNotification").SetValue(__instance, true);

				return false;
			}
		}
	}
}
