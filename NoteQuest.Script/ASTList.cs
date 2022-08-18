using System.Collections;
using System.Collections.Generic;

namespace NoteQuest.Script;

public abstract class ASTList : ASTNode, IEnumerable<ASTNode>
{
    public ASTList()
    {
        
    }
    
    public ASTList(IEnumerable<ASTNode> list)
    {
        _children.AddRange(list);
    }
    
    private List<ASTNode> _children = new List<ASTNode>();
    public IReadOnlyList<ASTNode> Children => _children;
    public ASTNode this[int i] => _children[i];
    public IEnumerator<ASTNode> GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}