using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathZone : MonoBehaviour
{
    Destructible dest;

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out dest);

        if (dest != null)
        {
            dest.KillSelf();
            if (dest is Vehicle && (dest as Vehicle).Driver.gameObject == Player.Instance.gameObject)
            {
                (dest as Vehicle).Driver.GetComponent<Destructible>().KillSelf();
            }
            //Player.Instance.RestartScene();
        }
    }
    
}
