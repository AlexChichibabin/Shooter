
public class EntityContextAction : EntityAnimationAction
{
    public bool IsCanStart;
    public bool IsCanEnd;

    public override void StartAction()
    {
        if (IsCanStart == false) return;

        IsCanStart = false;

        base.StartAction();
    }
    public override void EndAction()
    {
        if(IsCanEnd == false) return;

        IsCanEnd = false;

        base.EndAction();
    }
}
