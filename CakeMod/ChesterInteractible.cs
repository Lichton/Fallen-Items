using ItemAPI;
using System;
using System.Collections;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200001F RID: 31
	public class ChesterInteractible : SimpleInteractable, IPlayerInteractable
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public void Start()
		{
			this.talkPoint = base.transform.Find("talkpoint");
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			this.m_canUse = true;
			base.spriteAnimator.Play("idle");
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009B18 File Offset: 0x00007D18
		public void Interact(PlayerController interactor)
		{
			
			bool flag = TextBoxManager.HasTextBox(this.talkPoint);
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				this.m_canUse = ((this.CanUse != null) ? this.CanUse(interactor, base.gameObject) : this.m_canUse);
				bool flag4 = !this.m_canUse;
				bool flag5 = flag4;
				bool flag6 = flag5;
				if (flag6)
				{
					base.spriteAnimator.PlayForDuration("talk", 2f, "idle", false);
					TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "No taksies backsies, pal!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				}
				else
				{
					base.StartCoroutine(this.HandleConversation(interactor));
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009BD9 File Offset: 0x00007DD9
		private IEnumerator HandleConversation(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			base.spriteAnimator.PlayForDuration("talk_start", 1f, "talk", false);
			interactor.SetInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return null;
			int num;
			for (int conversationIndex = this.m_allowMeToIntroduceMyself ? 0 : (this.conversation.Count - 1); conversationIndex < this.conversation.Count - 1; conversationIndex = num + 1)
			{
				Tools.Print<string>(string.Format("Index: {0}", conversationIndex), "FFFFFF", false);
				TextBoxManager.ClearTextBox(this.talkPoint);
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, this.conversation[conversationIndex], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
				float timer = 0f;
				while (!BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
				{
					timer += BraveTime.DeltaTime;
					yield return null;
				}
				num = conversationIndex;
			}
			this.m_allowMeToIntroduceMyself = false;
			TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, this.conversation[this.conversation.Count - 1], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.acceptText, this.declineText);
			int selectedResponse = -1;
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			bool flag = selectedResponse == 0;
			bool flag2 = flag;
			if (flag2)
			{
				TextBoxManager.ClearTextBox(this.talkPoint);
				base.spriteAnimator.PlayForDuration("do_effect", -1f, "talk", false);
				Action<PlayerController, GameObject> onAccept = this.OnAccept;
				bool flag3 = onAccept != null;
				if (flag3)
				{
					onAccept(interactor, base.gameObject);
					
				}
				base.spriteAnimator.Play("talk");
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "The deal is on, kid!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				yield return new WaitForSeconds(1f);
				onAccept = null;
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				if (onDecline != null)
				{
					onDecline(interactor, base.gameObject);
				}
				TextBoxManager.ClearTextBox(this.talkPoint);
			}
			interactor.ClearInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
			base.spriteAnimator.Play("idle");
			yield break;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009BEF File Offset: 0x00007DEF
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009C1A File Offset: 0x00007E1A
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009C3C File Offset: 0x00007E3C
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00009C58 File Offset: 0x00007E58
		public float GetDistanceToPoint(Vector2 point)
		{
			bool flag = base.sprite == null;
			float result;
			if (flag)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00009CB8 File Offset: 0x00007EB8
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		// Token: 0x0400005B RID: 91
		private bool m_allowMeToIntroduceMyself = true;
	}
}
