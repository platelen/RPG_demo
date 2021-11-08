using UnityEngine;

public class HealSkill : UpgradeableSkill
{

    [SerializeField] int healAmount = 10;
    [SerializeField] ParticleSystem particle;

    public override int level
    {
        set
        {
            base.level = value;
            healAmount = 10 + level;
        }
    }

    protected override void OnCastComplete()
    {
        if (isServer) unit.stats.AddHealth(healAmount);
        else particle.Play();
        base.OnCastComplete();
    }
}
