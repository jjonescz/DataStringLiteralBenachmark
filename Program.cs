// dotnet run -c Release --filter "*"

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
    [BenchmarkCategory("short"), Benchmark(Baseline = true)]
    public void Ldstr_short() => TakeString("test");

    [BenchmarkCategory("short"), Benchmark]
    public void Ldsfld_short() => TakeString(Generated.s_short);

    [BenchmarkCategory("init"), Benchmark]
    public string Init_short() => Encoding.UTF8.GetString("""
        test
        """u8);

    [BenchmarkCategory("long"), Benchmark(Baseline = true)]
    public void Ldstr_long() => TakeString("test2");

    [BenchmarkCategory("long"), Benchmark]
    public void Ldsfld_long() => TakeString(Generated.s_long);
    
    [BenchmarkCategory("init"), Benchmark]
    public string Init_long() => Encoding.UTF8.GetString("""
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
        """u8);

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void TakeString(string s) { }
}

static class Generated
{
    public static readonly string s_short = Encoding.UTF8.GetString("test"u8);
    public static readonly string s_long = Encoding.UTF8.GetString("""
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
        """u8);
}
