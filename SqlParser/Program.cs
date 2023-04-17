var lexer = new SqlParser.Lexer("SELECT A, B FROM C WHERE D = 2");

while (lexer.NextToken() is var token
    && token.Kind != SqlParser.SyntaxKind.EndOfFileToken)
{
    System.Console.WriteLine(token);
}