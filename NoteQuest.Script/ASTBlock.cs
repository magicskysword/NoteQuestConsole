using System.Collections.Generic;

namespace NoteQuest.Script;

public class ASTBlock : ASTStatement
{
    public ASTBlock(IEnumerable<ASTNode> children) : base(children)
    {
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        foreach (ASTNode child in Children)
        {
            child.Evaluate(env);
        }

        return null;
    }
    
    public override string ToString()
    {
        return "{ " + string.Join(" ", Children) + " }";
    }
}