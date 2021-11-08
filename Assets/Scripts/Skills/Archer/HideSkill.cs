using UnityEngine;

public class HideSkill : UpgradeableSkill
{

    [SerializeField] ParticleSystem hideEffect;

    public override int level
    {
        set
        {
            base.level = value;
            castTime = level < 10 ? 10 - level : 1;
        }
    }

    protected override void OnUse()
    {
        if (isServer)
        {
            unit.RemoveFocus();
            unit.hasInteract = false;
        }
        else hideEffect.Play();
        base.OnUse();
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            unit.hasInteract = true;
        }
        else hideEffect.Stop();
        base.OnCastComplete();
    }
}
