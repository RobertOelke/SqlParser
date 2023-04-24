namespace SqlParser.Tokenization;

using System.Text;

public sealed class Lexer
{
    private int _position;
    private readonly string _text;
    private readonly StringBuilder _sb;

    public Lexer(string text)
    {
        _sb = new StringBuilder();
        _text = text;
    }

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
                return '\0';
            
            return _text[_position];
        }
    }

    private void Next() => _position++;

    public SyntaxToken NextToken()
    {
        if (Current == '\0')
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, 0);

        return
            WhitespaceToken()
            ?? QuotedTextToken()
            ?? CharToken()
            ?? NumberToken()
            ?? TextToken()
            ?? BadToken();
    }

    private SyntaxToken? WhitespaceToken() => CheckStringToken(char.IsWhiteSpace, _ => SyntaxKind.WhitespaceToken);

    private SyntaxToken? QuotedTextToken()
    {
        var start = _position;
        if (Current == '\'')
        {
            Next();
            while(Current != '\'')
            {
                Next();
            }

            Next();
            var end = _position;

            return new SyntaxToken(SyntaxKind.QuotedTextToken, start, end - start);
        }
        else
        {
            return null;
        }
    }

    // [-]123[.456]
    private SyntaxToken? NumberToken()
    {
        int startAnchor = _position;

        var hasDigit = false;
        var start = _position;

        if (Current == '-')
            Next();

        while (char.IsDigit(Current))
        {
            Next();
            hasDigit = true;
        }

        if (Current == '.')
        {
            Next();
            hasDigit = false;
        }

        while (char.IsDigit(Current))
        {
            Next();
            hasDigit = true;
        }

        var end = _position;

        if (hasDigit)
        {
            return new SyntaxToken(SyntaxKind.NumberToken, start, end - start);
        }
        else
        {
            _position = startAnchor;
            return null;
        }
    }

    private SyntaxToken? CharToken()
    {
        SyntaxKind? kind =
            Current switch
            {
                '=' => SyntaxKind.EqualsToken,
                ',' => SyntaxKind.CommaToken,
                _ => null,
            };

        if (kind.HasValue)
        {
            Next();
            return new SyntaxToken(kind.Value, _position - 1, 1);
        }

        return null;
    }

    private SyntaxToken? TextToken() => CheckStringToken(char.IsLetterOrDigit, MatchKind);

    private SyntaxToken BadToken()
    {
        var start = _position;
        Next();

        return new SyntaxToken(SyntaxKind.BadIdentifierToken, start, 1);
    }

    private SyntaxKind MatchKind(string text)
    {
        switch (text.ToUpperInvariant())
        {
            case "SELECT": return SyntaxKind.SelectToken;
            case "FROM": return SyntaxKind.FromToken;
            case "WHERE": return SyntaxKind.WhereToken;
            case "AS": return SyntaxKind.AsToken;
            default: return SyntaxKind.LiteralToken;
        }
    }
    
    private SyntaxToken? CheckStringToken(Predicate<char> predicate, Func<string, SyntaxKind> matchKind)
    {
        if (predicate(Current))
        {
            var start = _position;
            _sb.Length = 0;
            while (predicate(Current))
            {
                _sb.Append(Current);
                Next();
            }

            var end = _position;
            var text = _sb.ToString();

            return new SyntaxToken(matchKind(text), start, end - start);
        }

        return null;
    }
}