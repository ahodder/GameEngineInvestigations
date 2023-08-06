// See https://aka.ms/new-console-template for more information


using BenchmarkDotNet.Running;
using Cyberstar.Benchmarking.UI;

// BenchmarkRunner.Run<ReflectionBenchmarks>();


new ReflectionBenchmarks().BenchmarkFloatComponent();