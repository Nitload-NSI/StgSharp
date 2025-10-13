//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Global.cs"
// Project: StepVisualizer
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

global using StgSharp.Internal;
global using StgSharp.Internal.Intrinsic;
global using StgSharp.Internal.OpenGL;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StgSharp.Mathematics")]

[assembly: SuppressMessage(
        "Design",
        "CA1008:枚举应具有零值",
        Justification = "<挂起>",
        Scope = "type",
        Target = "~T:StgSharp.Graphics.OpenGL.ShaderStatus")]
[assembly: SuppressMessage(
        "Naming",
        "CA1715:标识符应具有正确的前缀",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~M:StgSharp.Graphics.OpenGL.ShaderProgram.GetUniform``2(System.String)~StgSharp.Graphics.OpenGL.Uniform{``0,``1}")]
[assembly: SuppressMessage(
        "Naming",
        "CA1715:标识符应具有正确的前缀",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~M:StgSharp.Graphics.OpenGL.ShaderProgram.GetUniform``3(System.String)~StgSharp.Graphics.OpenGL.Uniform{``0,``1,``2}")]
[assembly: SuppressMessage(
        "Naming",
        "CA1715:标识符应具有正确的前缀",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~M:StgSharp.Graphics.OpenGL.ShaderProgram.GetUniform``4(System.String)~StgSharp.Graphics.OpenGL.Uniform{``0,``1,``2,``3}")]
[assembly: SuppressMessage(
        "Naming",
        "CA1715:标识符应具有正确的前缀",
        Justification = "<挂起>",
        Scope = "type",
        Target = "~T:StgSharp.Graphics.OpenGL.Uniform`4")]
[assembly: SuppressMessage(
        "Design",
        "CA1043:将整型或字符串参数用于索引器",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~P:StgSharp.MVVM.ViewBase.IViewResponder`1.Item(StgSharp.Controlling.UsrActivity.ITrigger)")]
[assembly: SuppressMessage(
        "Design",
        "CA1043:将整型或字符串参数用于索引器",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~P:StgSharp.MVVM.ViewBase.ViewResponder`1.Item(StgSharp.Controlling.UsrActivity.ITrigger)")]
[assembly: SuppressMessage(
        "Performance",
        "CA1864:首选 \"IDictionary.TryAdd(TKey, TValue)\" 方法",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~P:StgSharp.MVVM.ViewBase.ViewRender`1.Item(System.String)")]
[assembly: SuppressMessage(
        "Design",
        "CA1033:接口方法应可由子类型调用",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~P:StgSharp.Collections.IBidirectionalDictionary`2.System#Collections#Generic#IDictionary<TFirst,TSecond>#Keys")]
[assembly: SuppressMessage(
        "Design",
        "CA1033:接口方法应可由子类型调用",
        Justification = "<挂起>",
        Scope = "member",
        Target = "~P:StgSharp.Collections.IBidirectionalDictionary`2.System#Collections#Generic#IDictionary<TFirst,TSecond>#Values")]
