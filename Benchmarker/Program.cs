using BenchmarkDotNet.Running;
using Benchmarker.Benchmarks;

var summary = BenchmarkRunner.Run(typeof(IkSolverRunner).Assembly);