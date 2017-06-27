using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceBuilding.Core
{
    public class ScriptBuilder
    {
        public object Build(string @namespace, string contextName, string outFilePath, string propertyName)
        {
            var tree = CompilationUnit()
                .WithUsings(
                    SingletonList(
                        UsingDirective(
                                IdentifierName(@namespace))
                            .WithUsingKeyword(
                                Token(
                                    TriviaList(
                                        Trivia(
                                            ReferenceDirectiveTrivia(
                                                Literal("nuget:NetStandard.Library,1.6.1"),
                                                true)),
                                        Trivia(
                                            ReferenceDirectiveTrivia(
                                                Literal("nuget:Microsoft.EntityFrameworkCore.SqlServer,1.1.2"),
                                                true)),
                                        Trivia(
                                            ReferenceDirectiveTrivia(
                                                Literal(
                                                    $@"""{outFilePath}""",
                                                    outFilePath),
                                                true))),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList()))))
                .WithMembers(
                    List(
                        new MemberDeclarationSyntax[]{
                            FieldDeclaration(
                                VariableDeclaration(
                                        IdentifierName("var"))
                                    .WithVariables(
                                        SingletonSeparatedList(
                                            VariableDeclarator(
                                                    Identifier("context"))
                                                .WithInitializer(
                                                    EqualsValueClause(
                                                        ObjectCreationExpression(
                                                                IdentifierName(contextName))
                                                            .WithArgumentList(
                                                                ArgumentList())))))),
                            FieldDeclaration(
                                VariableDeclaration(
                                        IdentifierName("var"))
                                    .WithVariables(
                                        SingletonSeparatedList(
                                            VariableDeclarator(
                                                    Identifier("query"))
                                                .WithInitializer(
                                                    EqualsValueClause(
                                                        InvocationExpression(
                                                                MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("context"),
                                                                        IdentifierName(propertyName)),
                                                                    IdentifierName("Take")))
                                                            .WithArgumentList(
                                                                ArgumentList(
                                                                    SingletonSeparatedList(
                                                                        Argument(
                                                                            LiteralExpression(
                                                                                SyntaxKind.NumericLiteralExpression,
                                                                                Literal(1)))))))))))}))
                .NormalizeWhitespace();


            var descendants = tree.DescendantNodesAndTokens();
            var triviaDesc = tree.DescendantTrivia();
            var scriptText = tree.GetText().ToString();
            return scriptText;
        }

        public string GetPropertyName(string sourceText)
        {
            var root = CSharpSyntaxTree.ParseText(sourceText).GetCompilationUnitRoot();
            var dbsetPropertyNode = root.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(n => (n.Type as GenericNameSyntax)?.Identifier.Text == "DbSet");
            var propertyName = dbsetPropertyNode.Identifier.Text;
            return propertyName;
        }
    }
}