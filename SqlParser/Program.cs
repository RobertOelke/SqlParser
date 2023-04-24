var lexer = new SqlParser.Tokenization.Lexer("SELECT A, B FROM C WHERE D = 2");

while (lexer.NextToken() is var token
    && token.Kind != SqlParser.Tokenization.SyntaxKind.EndOfFileToken)
{
    Console.WriteLine(token);
}