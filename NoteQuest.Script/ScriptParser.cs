using System;
using System.Collections.Generic;
using Sprache;

namespace NoteQuest.Script;

public static class ScriptParser
{
    public static readonly Parser<ASTLiteral> Literal =
        (from nullValue in Lexer.Null select new ASTNullLiteral())
        .Or<ASTLiteral>(from booleanValue in Lexer.Boolean select new ASTBooleanLiteral(booleanValue))
        .Or(from doubleValue in Lexer.DoubleFloat select new ASTDoubleFloatLiteral(doubleValue))
        .Or(from longValue in Lexer.LongInteger select new ASTLongIntegerLiteral(longValue))
        .Or(from stringValue in Lexer.String select new ASTStringLiteral(stringValue));

    public static readonly Parser<ASTIdentifier> Identifier =
        from id in Lexer.Identifier select new ASTIdentifier(id);
    
    public delegate ASTNode CreateNode(ASTNode left,string op,ASTNode right);
    
    /// <summary>
    /// 左递归符号解析器生成
    /// </summary>
    /// <param name="leftExpr"></param>
    /// <param name="symbol"></param>
    /// <param name="apply"></param>
    /// <returns></returns>
    public static Parser<ASTNode> LeftOperator(Parser<ASTNode> leftExpr, Parser<string> symbol,
        CreateNode apply)
    {
        ASTNode CreateNode(ASTNode left, IEnumerable<Tuple<string,ASTNode>> rights)
        {
            foreach(var right in rights)
            {
                left = apply(left, right.Item1, right.Item2);
            }

            return left;
        }
        
        Parser<Tuple<string,ASTNode>> innerOperatorExpr = (
            from getSymbol in symbol
            from right in leftExpr
            select new Tuple<string, ASTNode>(getSymbol, right)
        );
        
        Parser<ASTNode> operatorExpr = (
                from left in leftExpr
                from rights in innerOperatorExpr.Many()
                select CreateNode(left, rights)
            );
        
        return operatorExpr;
    }
    
    public static readonly Parser<ASTNode> Expr = Parse.Ref(() => Expr9);

    public static readonly Parser<ASTNode> Primary = (
            from left in Lexer.LeftBracket
            from expr in Parse.Ref(() => Expr)
            from right in Lexer.RightBracket
            select expr)
        .Or(Literal)
        .Or(Identifier);

    public static readonly Parser<ASTNode> Expr1 =(
            from symbol in Lexer.Negate.Or(Lexer.Not)
            from expr in Primary
            select (ASTNode)(symbol == "-" ? new ASTUnaryExprNegative(expr) : new ASTUnaryExprNot(expr)))
        .Or(Primary);
    
    public static readonly Parser<ASTNode> Expr2 = LeftOperator(Expr1, Lexer.Dice.Or(Lexer.DiceUpper),
        (left, op, right) => new ASTBinaryExprDice(left, right));
    
    public static readonly Parser<ASTNode> Expr3 =(
            from left in Expr2
            from symbol in Lexer.Power
            from right in Expr3
            select new ASTBinaryExprPower(left, right))
        .Or(Expr2);

    public static readonly Parser<ASTNode> Expr4 = LeftOperator(Expr3, Lexer.Multiply.Or(Lexer.Divide).Or(Lexer.Modulo),
        (left, op, right) =>
        {
            switch (op)
            {
                case "*":
                    return new ASTBinaryExprMultiply(left, right);
                case "/":
                    return new ASTBinaryExprDivide(left, right);
                case "%":
                    return new ASTBinaryExprModulo(left, right);
                default:
                    throw new ArgumentException($"Unknown operator '{op}'");
            }
        });

    public static readonly Parser<ASTNode> Expr5 = LeftOperator(Expr4, Lexer.Plus.Or(Lexer.Minus),
        (left, op, right) =>
        {
            switch (op)
            {
                case "+":
                    return new ASTBinaryExprPlus(left, right);
                case "-":
                    return new ASTBinaryExprMinus(left, right);
                default:
                    throw new ArgumentException($"Unknown operator '{op}'");
            }
        });
    
    public static readonly Parser<ASTNode> Expr6 = LeftOperator(Expr5, 
        Lexer.GreaterThanOrEqual.Or(Lexer.GreaterThan)
        .Or(Lexer.LessThanOrEqual).Or(Lexer.LessThan),
        (left, op, right) =>
        {
            switch (op)
            {
                case ">":
                    return new ASTBinaryExprGreaterThan(left, right);
                case ">=":
                    return new ASTBinaryExprGreaterThanOrEqual(left, right);
                case "<":
                    return new ASTBinaryExprLessThan(left, right);
                case "<=":
                    return new ASTBinaryExprLessThanOrEqual(left, right);
                default:
                    throw new ArgumentException($"Unknown operator '{op}'");
            }
        });
        
    public static readonly Parser<ASTNode> Expr7 = LeftOperator(Expr6, Lexer.Equal.Or(Lexer.NotEqual),
        (left, op, right) =>
        {
            switch (op)
            {
                case "==":
                    return new ASTBinaryExprEqual(left, right);
                case "!=":
                    return new ASTBinaryExprNotEqual(left, right);
                default:
                    throw new ArgumentException($"Unknown operator '{op}'");
            }
        });
    
    public static readonly Parser<ASTNode> Expr8 = LeftOperator(Expr7, Lexer.And,
        (left, op, right) => new ASTBinaryExprAnd(left, right));
    
    public static readonly Parser<ASTNode> Expr9 = LeftOperator(Expr8, Lexer.Or,
        (left, op, right) => new ASTBinaryExprOr(left, right));

    public static readonly Parser<ASTStatement> Statement = Parse.Ref(() => Block)
        .Or(Parse.Ref(() => AssignStatement))
        .Or(Parse.Ref(() => IfStatement))
        .Or(Parse.Ref(() => WhileStatement))
        .Or(Parse.Ref(() => DoWhileStatement))
        .Or(Parse.Ref(() => ContinueStatement))
        .Or(Parse.Ref(() => BreakStatement));

    public static readonly Parser<ASTStatement> Block = 
        from left in Lexer.LeftBrace
        from statements in Statement.Many()
        from right in Lexer.RightBrace
        select new ASTBlock(statements);

    public static readonly Parser<ASTStatement> AssignStatement =
        from left in Identifier
        from assign in Lexer.Assign
        from right in Expr
        from _ in Lexer.Semicolon.Optional()
        select new ASTAssignStatement(left, right);
    
    public static readonly Parser<ASTNode> Condition =
        from left in Lexer.LeftBracket
        from condition in Expr
        from right in Lexer.RightBracket
        select condition;
    
    public static readonly Parser<ASTStatement> ElseStatement =
        from left in Lexer.Else
        from statement in Statement
        select statement;

    public static readonly Parser<ASTStatement> IfStatement =
        from keywordIf in Lexer.If
        from condition in Condition
        from statement in Statement
        from elseStatement in ElseStatement.Optional()
        select elseStatement.IsEmpty 
            ? new ASTIfStatement(condition, statement) 
            : new ASTIfStatement(condition, statement, elseStatement.Get());
    
    public static readonly Parser<ASTStatement> BreakStatement =
        from keywordBreak in Lexer.Break
        from _ in Lexer.Semicolon.Optional()
        select new ASTBreakStatement();
    
    public static readonly Parser<ASTStatement> ContinueStatement =
        from keywordContinue in Lexer.Continue
        from _ in Lexer.Semicolon.Optional()
        select new ASTContinueStatement();

    public static readonly Parser<ASTStatement> WhileStatement =
        from keywordWhile in Lexer.While
        from condition in Condition
        from statement in Statement
        select new ASTWhileStatement(condition, statement);
    
    public static readonly Parser<ASTStatement> DoWhileStatement =
        from keywordDo in Lexer.Do
        from statement in Statement
        from keywordWhile in Lexer.While
        from condition in Condition
        select new ASTDoWhileStatement(condition, statement);
        
}