using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceBuilding.Core
{
    public class ScriptBuilder
    {
        public object Build(string @namespace, string contextName, string outFilePath)
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
                    SingletonList<MemberDeclarationSyntax>(
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
                                                            ArgumentList()))))))))
                .NormalizeWhitespace();


            var descendants = tree.DescendantNodesAndTokens();
            var triviaDesc = tree.DescendantTrivia();
            var scriptText = tree.GetText().ToString();
            return scriptText;
        }
    }
}