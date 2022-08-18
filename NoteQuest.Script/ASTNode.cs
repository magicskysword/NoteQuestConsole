namespace NoteQuest.Script;

public abstract class ASTNode
{
    public abstract object Evaluate(ScriptEnvironment env);
}