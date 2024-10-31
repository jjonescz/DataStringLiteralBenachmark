// dotnet run -c Release --filter "*"

using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[ShortRunJob]
[DisassemblyDiagnoser(maxDepth: 0)]
public class Tests
{
    [Benchmark(Baseline = true)]
    public void Ldstr() => TakeString("test");

    [Benchmark]
    public void Ldsfld() => TakeString(Generated.s_test);

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void TakeString(string s) { }
}

static class Generated
{
    public static readonly string s_test = Encoding.UTF8.GetString("test"u8);
}
