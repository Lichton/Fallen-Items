using UnityEngine;

public class ColliderComponent : MonoBehaviour
{
    private AIActor aiactor;
    private bool used;

    private void Start()
    {
        aiactor = base.GetComponent<AIActor>();
        for (int i = 0; i < aiactor.specRigidbody.PixelColliders.Count; i++)
        {
            aiactor.specRigidbody.PixelColliders[i].RegenerateFromManual(aiactor.transform, IntVector2.Zero, new IntVector2(10, 80));
        }
        PhysicsEngine.Instance.Register(aiactor.specRigidbody);
        PhysicsEngine.UpdatePosition(aiactor.specRigidbody);
    }
}