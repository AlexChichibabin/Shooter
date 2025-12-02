using UnityEngine;

public class Projectile : Entity
{
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_Velocity;
    [SerializeField] private float m_LifeTime;
    [SerializeField] private float m_DirSensity;
    [SerializeField] private bool IsSelfDirected;
    [SerializeField] private bool isPlayer;
    [SerializeField] private CubeArea m_Area;
    [SerializeField] private SO_ImpactMaterials m_ImpactEffectPrefab;

    private Destructible m_Parent;
    private GameObject m_RocketTarget;
    private float m_Timer;

    public int Damage => m_Damage;

    private void Update()
    {
        float stepLenght = m_Velocity * Time.deltaTime;
        Vector3 step = transform.forward * stepLenght;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, stepLenght) == true
            && hit.collider.isTrigger != true)
        {
            Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

            if (dest != null && dest != m_Parent)
            {
                dest.ApplyDamage(m_Damage, m_Parent);
            }
            OnProjectileLifeEnd(hit);
        }

        m_Timer += Time.deltaTime;
        if (m_Timer > m_LifeTime) Destroy(gameObject);

        transform.position += new Vector3(step.x, step.y, step.z);

        ControllRocket();
    }

    public void SetParentShooter(Destructible parent) => m_Parent = parent;


    private void OnProjectileLifeEnd(RaycastHit hit)
    {
        Collider col = hit.collider;
        Vector3 pos = hit.point;
        Vector3 normal = hit.normal;

        ImpactEffect impact;
        AudioClip clip;

        if (m_ImpactEffectPrefab != null)
        {
            if (m_ImpactEffectPrefab.GetImpactPrefabByMaterial(col.material, out impact, out clip) == true)
            {
                impact = Instantiate(impact, pos, Quaternion.LookRotation(normal));
                AudioSource sourse = impact.GetComponent<AudioSource>();
                sourse.clip = clip;
                impact.transform.SetParent(col.transform);
                sourse.Play();
                if (col.GetComponent<Surface>() != null)
                {
                    impact.GetComponent<ImpactEffect>().UpdateType(col.GetComponent<Surface>().Type);
                }
            }
        }
        Destroy(gameObject);
    }











    private void ControllRocket()
    {
        if (IsSelfDirected == true)
        {
            if (m_RocketTarget == null)
            {
                GetTarget();
            }
            if (m_RocketTarget != null)
            {
                CorrectDirection();
            }
        }
    }
    private void GetTarget()
    {
        /*Collider targetHit = Physics.OverlapCircle(new Vector2(transform.position.x, transform.position.y), m_Area.Radius);
        if (targetHit != null && targetHit.transform.root.GetComponent<Destructible>() != m_Parent)
        {
            m_RocketTarget = targetHit.transform.gameObject;
        }
        if (m_RocketTarget == null) return;*/
    }
    private void CorrectDirection()
    {
        Vector3 dir = (m_RocketTarget.transform.position - transform.position).normalized;
        transform.up = Vector3.Slerp(transform.up, dir, Time.deltaTime * m_DirSensity);
    }

}