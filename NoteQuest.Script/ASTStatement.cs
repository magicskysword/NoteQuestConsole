using System.Collections.Generic;

namespace NoteQuest.Script;

public abstract class ASTStatement : ASTList
{
    public ASTStatement()
    {
        
    }
    
    public ASTStatement(IEnumerable<ASTNode> children) : base(children)
    {
        
    }
}