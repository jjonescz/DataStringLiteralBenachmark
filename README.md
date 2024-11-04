# DataStringLiteral benchmark

| Method       | Categories | Mean        | Error      | StdDev    | Ratio | RatioSD | Code Size |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|----------:|
| Init_short   | init       |  16.1501 ns |  3.3894 ns | 0.1858 ns |     ? |       ? |     200 B |
| Init_long    | init       | 125.3370 ns | 11.7162 ns | 0.6422 ns |     ? |       ? |     203 B |
|              |            |             |            |           |       |         |           |
| Ldstr_long   | long       |   0.5799 ns |  0.0600 ns | 0.0033 ns |  1.00 |    0.01 |      16 B |
| Ldsfld_long  | long       |   0.5776 ns |  0.0450 ns | 0.0025 ns |  1.00 |    0.01 |      19 B |
|              |            |             |            |           |       |         |           |
| Ldstr_short  | short      |   0.5807 ns |  0.0706 ns | 0.0039 ns |  1.00 |    0.01 |      16 B |
| Ldsfld_short | short      |   0.5735 ns |  0.0319 ns | 0.0017 ns |  0.99 |    0.01 |      19 B |

## .NET 9.0.0 (9.0.24.47305), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```assembly
; Tests.Ldstr_short()
       mov       rcx,1908030B068
       jmp       qword ptr [7FF9711A40A8]; Tests.TakeString(System.String)
; Total bytes of code 16
```

## .NET 9.0.0 (9.0.24.47305), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```assembly
; Tests.Ldsfld_short()
       mov       rcx,21C97801470
       mov       rcx,[rcx]
       jmp       qword ptr [7FF9710EFFC0]; Tests.TakeString(System.String)
; Total bytes of code 19
```
