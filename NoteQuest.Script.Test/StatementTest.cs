using NUnit.Framework;
using Sprache;
using NoteQuest.Script;

namespace Tests;

public class StatementTest
{
    [Test]
    public void AssigStatementTest()
    {
        Assert.AreEqual("a = 1;", ScriptParser.AssignStatement.Parse(" a = 1 ").ToString());
        Assert.AreEqual("a = (1 + 1);", ScriptParser.AssignStatement.Parse(" a = 1 + 1; ").ToString());
        Assert.AreEqual("a = (2 == b);", ScriptParser.AssignStatement.Parse(" a = 2 == b ").ToString());
        Assert.Catch<ParseException>(() => ScriptParser.AssignStatement.Parse(" a + 1 = 1 "));
    }

    [Test]
    public void AssignStatementEvaluateTest()
    {
        var env = new ScriptEnvironment();
        ScriptParser.AssignStatement.Parse(" a = 1 ").Evaluate(env);
        Assert.AreEqual(1, env.GetVariable("a"));

        ScriptParser.AssignStatement.Parse(" a = 1 + 1 ").Evaluate(env);
        Assert.AreEqual(2, env.GetVariable("a"));

        Assert.AreEqual(null, env.GetVariable("b"));

        ScriptParser.AssignStatement.Parse(" b = 2 ").Evaluate(env);
        Assert.AreEqual(2, env.GetVariable("b"));

        ScriptParser.AssignStatement.Parse(" c = a == b ").Evaluate(env);
        Assert.AreEqual(true, env.GetVariable("c"));
    }

    [Test]
    public void IfStatementTest()
    {
        Assert.AreEqual("if ((a == 1)) b = 1;", ScriptParser.IfStatement.Parse(" if (a == 1) \n b = 1 ").ToString());
        Assert.AreEqual("if ((a == 1)) b = 1; else c = 2;",
            ScriptParser.IfStatement.Parse(" if (a == 1) \n b = 1 \n else \n c = 2 ").ToString());
        Assert.AreEqual("if (a) { b = 0; } else { b = -100; }", ScriptParser.IfStatement.Parse(@"
            if(a)
            {
                b = 0;
            }
            else
            {
                b = -100;
            }
            ").ToString());
    }

    [Test]
    public void IfStatementEvaluateTest()
    {
        var env = new ScriptEnvironment();
        env.SetVariable("a", 1);
        ScriptParser.IfStatement.Parse(" if (a == 1) \n b = 1 ").Evaluate(env);
        Assert.AreEqual(1, env.GetVariable("b"));

        ScriptParser.IfStatement.Parse(@"
            if(a == 2)
            {
                c = 100;
            }
            ").Evaluate(env);
        Assert.AreEqual(null, env.GetVariable("c"));

        ScriptParser.IfStatement.Parse(@"
            if(c != null)
            {
                d = 100;
            }
            else
            {
                d = -100;
            }
            ").Evaluate(env);
        Assert.AreEqual(-100, env.GetVariable("d"));
    }

    [Test]
    public void WhileStatementTest()
    {
        Assert.AreEqual("while ((a == 1)) a = 1;", ScriptParser.WhileStatement.Parse(" while (a == 1) \n a = 1 ").ToString());
        Assert.AreEqual("while ((a == 1)) { a = 1; }", ScriptParser.WhileStatement.Parse(@"
            while(a == 1)
            {
                a = 1;
            }
            ").ToString());
        
        Assert.AreEqual("while ((a == 1)) { a = 1; b = 2; }", ScriptParser.WhileStatement.Parse(@"
            while(a == 1)
            {
                a = 1;
                b = 2;
            }
            ").ToString());
    }

    [Test]
    public void WhileStatementEvaluateTest()
    {
        var env = new ScriptEnvironment();
        env.SetVariable("a", 1);
        ScriptParser.WhileStatement.Parse(" while (a == 1) \n a = 2 ").Evaluate(env);
        Assert.AreEqual(2, env.GetVariable("a"));
        
        env.SetVariable("a", 1);
        ScriptParser.WhileStatement.Parse(@"
            while(a < 10)
            {
                a = a + 1;
            }").Evaluate(env);
        Assert.AreEqual(10, env.GetVariable("a"));
        
        env.SetVariable("a", 1);
        env.SetVariable("b", 0);
        ScriptParser.WhileStatement.Parse(@"
            while(a < 10)
            {
                b = b + a;
                a = a + 1;
            }").Evaluate(env);
        Assert.AreEqual(45, env.GetVariable("b"));
    }
    
    [Test]
    public void DoWhileStatementTest()
    {
        Assert.AreEqual("do b = 1; while ((a == 1))", ScriptParser.DoWhileStatement.Parse(" do \n b = 1 \n while (a == 1) ").ToString());
        Assert.AreEqual("do { a = 2; } while ((a == 1))", ScriptParser.DoWhileStatement.Parse(@"
            do
            {
                a = 2;
            }
            while(a == 1)
            ").ToString());
        
        Assert.AreEqual("do { a = 2; b = 4; } while ((a == 1))", ScriptParser.DoWhileStatement.Parse(@"
            do
            {
                a = 2;
                b = 4;
            }
            while(a == 1)
            ").ToString());
    }

    [Test]
    public void DoWhileStatementEvaluate()
    {
        var env = new ScriptEnvironment();
        env.SetVariable("a", 1);
        ScriptParser.DoWhileStatement.Parse(" do \n a = 2 \n while (a == 1) ").Evaluate(env);
        Assert.AreEqual(2, env.GetVariable("a"));
        
        env.SetVariable("a", 1);
        ScriptParser.DoWhileStatement.Parse(@"
            do
            {
                a = 2;
            }
            while(a == 1)
            ").Evaluate(env);
        
        env.SetVariable("a", 1);
        env.SetVariable("b", 0);
        ScriptParser.DoWhileStatement.Parse(@"
            do
            {
                b = b + a;
                a = a + 1;
            }
            while(a < 10)
            ").Evaluate(env);
        Assert.AreEqual(45, env.GetVariable("b"));
    }
}