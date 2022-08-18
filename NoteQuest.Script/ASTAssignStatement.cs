namespace NoteQuest.Script;

public class ASTAssignStatement : ASTStatement
{
    public ASTAssignStatement(ASTNode left, ASTNode right) : base(new []{left, right})
    {
        
    }

    public ASTNode Left => this[0];
    
    public ASTNode Right => this[1];
    
    public override string ToString()
    {
        return $"{Left} = {Right};";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        string variableName = ((ASTIdentifier)Left).Name;
        env.SetVariable(variableName, Right.Evaluate(env));
        
        return null;
    }
}