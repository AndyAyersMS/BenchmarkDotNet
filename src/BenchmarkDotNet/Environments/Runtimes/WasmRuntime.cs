﻿using System;
using System.ComponentModel;
using System.IO;
using BenchmarkDotNet.Jobs;

namespace BenchmarkDotNet.Environments
{
    public class WasmRuntime : Runtime, IEquatable<WasmRuntime>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static readonly WasmRuntime Default = new WasmRuntime();

        public string JavaScriptEngine { get; }

        public string JavaScriptEngineArguments { get;  }

        public bool Aot { get;  }

        public DirectoryInfo RuntimeSrcDir { get;  }

        /// <summary>
        /// creates new instance of WasmRuntime
        /// </summary>
        /// <param name="javaScriptEngine">Full path to a java script engine used to run the benchmarks. "v8" by default</param>
        /// <param name="javaScriptEngineArguments">Arguments for the javascript engine. "--expose_wasm" by default</param>
        /// <param name="msBuildMoniker">moniker, default: "net5.0"</param>
        /// <param name="displayName">default: "Wasm"</param>
        /// <param name="aot">Specifies whether AOT or Interpreter (default) project should be generated.</param>
        /// <param name="runtimeSrcDir">The path to runtime source directory.</param>
        public WasmRuntime(string msBuildMoniker = "net5.0", string displayName = "Wasm", string javaScriptEngine = "v8", string javaScriptEngineArguments = "--expose_wasm", bool aot = false, DirectoryInfo runtimeSrcDir = null, RuntimeMoniker moniker = RuntimeMoniker.Wasm) : base(moniker, msBuildMoniker, displayName)
        {
            if (!string.IsNullOrEmpty(javaScriptEngine) && javaScriptEngine != "v8" && !File.Exists(javaScriptEngine))
                throw new FileNotFoundException($"Provided {nameof(javaScriptEngine)} file: \"{javaScriptEngine}\" doest NOT exist");

            JavaScriptEngine = javaScriptEngine;
            JavaScriptEngineArguments = javaScriptEngineArguments;
            Aot = aot;
            RuntimeSrcDir = runtimeSrcDir;
        }

        public override bool Equals(object obj)
            => obj is WasmRuntime other && Equals(other);

        public bool Equals(WasmRuntime other)
            => other != null && base.Equals(other) && other.JavaScriptEngine == JavaScriptEngine && other.JavaScriptEngineArguments == JavaScriptEngineArguments && other.Aot == Aot && other.RuntimeSrcDir == RuntimeSrcDir;

        public override int GetHashCode()
            => base.GetHashCode() ^ (JavaScriptEngine?.GetHashCode() ?? 0) ^ (JavaScriptEngineArguments?.GetHashCode() ?? 0 ^ Aot.GetHashCode() ^ (RuntimeSrcDir?.GetHashCode() ?? 0));
    }
}
