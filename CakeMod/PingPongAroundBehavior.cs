using System;
using UnityEngine;

// Token: 0x02000DEE RID: 3566
namespace CakeMod
{
	public class PingPongAroundBehavior2 : MovementBehaviorBase
	{
		// Token: 0x06004B8F RID: 19343 RVA: 0x00191EFC File Offset: 0x001900FC
		public PingPongAroundBehavior2()
		{
			this.startingAngles = new float[]
			{
			45f,
			135f,
			225f,
			315f
			};
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004B90 RID: 19344 RVA: 0x00191F23 File Offset: 0x00190123


		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06004B91 RID: 19345 RVA: 0x00191F3F File Offset: 0x0019013F

		// Token: 0x06004B92 RID: 19346 RVA: 0x00191F5B File Offset: 0x0019015B
		public override void Start()
		{
			base.Start();
			SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x00191F90 File Offset: 0x00190190
		public override BehaviorResult Update()
		{
			this.m_startingAngle = BraveMathCollege.ClampAngle360(BraveUtility.RandomElement<float>(this.startingAngles));
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startingAngle, this.m_aiActor.MovementSpeed);
			this.m_isBouncing = true;
			return BehaviorResult.RunContinuousInClass;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00191FE8 File Offset: 0x001901E8
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			base.ContinuousUpdate();
			return this.m_aiActor.BehaviorOverridesVelocity ? ContinuousBehaviorResult.Continue : ContinuousBehaviorResult.Finished;
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x00192008 File Offset: 0x00190208
		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			this.m_isBouncing = false;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00192018 File Offset: 0x00190218
		protected virtual void OnCollision(CollisionData collision)
		{
			if (!this.m_isBouncing)
			{
				return;
			}
			if (collision.OtherRigidbody && collision.OtherRigidbody.projectile)
			{
				return;
			}
			if (collision.CollidedX || collision.CollidedY)
			{
				Vector2 vector = new Vector2(collision.MyRigidbody.Velocity.x, collision.MyRigidbody.Velocity.y);
				if (collision.CollidedX && this.m_aiActor.BehaviorVelocity.x < 0)
				{
					vector.x = 1.5f;
				}
				if (collision.CollidedX && this.m_aiActor.BehaviorVelocity.x > 0)
				{
					vector.x = -1.5f;
				}
				if (collision.CollidedY && this.m_aiActor.BehaviorVelocity.y < 0)
				{
					vector.y = 5f;
				}
				if (collision.CollidedY && this.m_aiActor.BehaviorVelocity.y > 0)
				{
					vector.y = -5f;
				}
				vector = vector.normalized * this.m_aiActor.MovementSpeed;
				PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
				this.m_aiActor.BehaviorVelocity = new Vector2(vector.x, vector.y);
			}
		}

		// Token: 0x04004146 RID: 16710
		public float[] startingAngles;

		// Token: 0x04004147 RID: 16711

		// Token: 0x04004148 RID: 16712
		private bool m_isBouncing;

		// Token: 0x04004149 RID: 16713
		private float m_startingAngle;

		// Token: 0x02000DEF RID: 356
	}
}