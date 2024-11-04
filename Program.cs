// dotnet run -c Release --filter "*"

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

Dictionary<string, string> strings = new()
{
    { "short", "test" },
    { "long", """
        Let me not to the marriage of true minds
        Admit impediments; love is not love
        Which alters when it alteration finds,
        Or bends with the remover to remove.
        O no, it is an ever-fixed mark
        That looks on tempests and is never shaken;
        It is the star to every wand'ring bark
        Whose worth's unknown, although his height be taken.
        Love's not time's fool, though rosy lips and cheeks
        Within his bending sickle's compass come.
        Love alters not with his brief hours and weeks,
        But bears it out even to the edge of doom:
        If this be error and upon me proved,
        I never writ, nor no man ever loved.
        """ },
};

var methods = string.Join(Environment.NewLine, strings.Select(static (p) => $$""""

    [BenchmarkCategory("{{p.Key}}"), Benchmark(Baseline = true)]
    public void Ldstr_{{p.Key}}() => TakeString("""
    {{p.Value}}
    """);

    [BenchmarkCategory("{{p.Key}}"), Benchmark]
    public void Ldsfld_{{p.Key}}() => TakeString(Generated.s_{{p.Key}});

    [BenchmarkCategory("{{p.Key}}-init"), Benchmark]
    public string Init_{{p.Key}}() => Encoding.UTF8.GetString("""
    {{p.Value}}
    """u8);

    """"));

var fields = string.Join(Environment.NewLine, strings.Select(static (p) => $$""""

    public static readonly string s_{{p.Key}} = Encoding.UTF8.GetString("""
    {{p.Value}}
    """u8);

    """"));

var source = $$"""
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;

    BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

    [ShortRunJob]
    [DisassemblyDiagnoser(maxDepth: 0)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class Tests
    {
        {{methods}}

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void TakeString(string s) { }
    }

    static class Generated
    {
        {{fields}}
    }
    """;

Directory.CreateDirectory("Tests");
File.WriteAllText(path: "Tests/Program.cs", contents: source);

IEnumerable<Assembly> assemblies = [
    typeof(BenchmarkAttribute).Assembly,
    typeof(ShortRunJobAttribute).Assembly,
];

var outputPath = Path.GetFullPath("bin/Tests.dll");

var result = CSharpCompilation.Create(
    assemblyName: "Tests",
    syntaxTrees: [CSharpSyntaxTree.ParseText(source)],
    references: [
        ..Basic.Reference.Assemblies.Net90.References.All,
        ..assemblies.Select(static a => MetadataReference.CreateFromFile(a.Location)),
    ],
    options: new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Release))
    .Emit(outputPath);

Console.WriteLine(string.Join(Environment.NewLine, result.Diagnostics));

BenchmarkSwitcher.FromAssembly(Assembly.LoadFile(outputPath)).Run(args);

// BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[ShortRunJob]
[DisassemblyDiagnoser(maxDepth: 0)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class Tests
{
    [BenchmarkCategory("test"), Benchmark(Baseline = true)]
    public void Ldstr() => TakeString("test");

    [BenchmarkCategory("test"), Benchmark]
    public void Ldsfld() => TakeString(Generated.s_test);

    [BenchmarkCategory("test2"), Benchmark(Baseline = true)]
    public void Ldstr2() => TakeString("test2");

    [BenchmarkCategory("test2"), Benchmark]
    public void Ldsfld2() => TakeString(Generated.s_test2);

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void TakeString(string s) { }
}

static class Generated
{
    public static readonly string s_test = Encoding.UTF8.GetString("test"u8);
    public static readonly string s_test2 = Encoding.UTF8.GetString("test2"u8);
}
