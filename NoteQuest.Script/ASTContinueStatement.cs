namespace NoteQuest.Script;

public class ASTContinueStatement : ASTStatement
{
    public override object Evaluate(ScriptEnvironment env)
    {
        throw new ContinueException();
    }
    
    public override string ToString()
    {
        return "continue;";
    }
}