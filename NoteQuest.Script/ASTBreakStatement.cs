namespace NoteQuest.Script;

public class ASTBreakStatement : ASTStatement
{
    public override object Evaluate(ScriptEnvironment env)
    {
        throw new BreakException();
    }
    
    public override string ToString()
    {
        return "break;";
    }
}