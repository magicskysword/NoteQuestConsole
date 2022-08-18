namespace NoteQuest.Script;

public class ASTDoWhileStatement : ASTStatement
{
    public ASTDoWhileStatement(ASTNode condition, ASTNode statement) : base(new[] { condition, statement })
    {
    }
    
    public ASTNode Condition => this[0];
    public ASTNode Statement => this[1];

    public override object Evaluate(ScriptEnvironment env)
    {
        bool b;
        do
        {
            try
            {
                Statement.Evaluate(env);
            }
            catch (BreakException)
            {
                break;
            }
            catch (ContinueException)
            {
                // ignore
            }
            
            var result = Condition.Evaluate(env);
            if (result is not bool b2)
            {
                throw new EvaluateException($"Condition '{Condition}' must return boolean");
            }

            b = b2;
        } while (b);
        
        return null;
    }
    
    public override string ToString()
    {
        return $"do {Statement} while ({Condition})";
    }
}