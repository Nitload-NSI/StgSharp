//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="KeyWord.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public static partial class ExpressCompile
    {

        private static HashSet<string> _keywordSet;

        private static void PoolKeywords()
        {
            _keywordSet = new();

            FieldInfo[] fields = typeof( Keyword ).GetFields( BindingFlags.Public );
            ParallelLoopResult result = Parallel.ForEach(
                fields, static( field ) => {
                    if( field.FieldType == typeof( string ) && field.GetValue( null ) is string str )
                    {
                        _compileStringCache.GetOrAdd( str );
                        _keywordSet.Add( str );
                    }
                } );
            while( !result.IsCompleted ) { }
        }

        public static class Keyword
        {

            public static HashSet<string> ScopeBlock = new HashSet<string> {
                EndAlias, EndCase, EndConstant, EndEntity, EndFunction, EndIf, EndLocal, EndProcedure, EndRepeat, EndRule, EndSchema, EndSubtypeConstraint, EndType
            };

            public static HashSet<string> Symbols = [
                Dot, Comma, Semicolon, Colon, Percent, Apostrophe,
                Backslash, IndexOf, RightBracket, LeftBrace,
                RightBrace, Pipe, LeftParen, RightParen, LeftAsterisk,
                Assignment, DoublePipe, ExpSymbol, CommentSingleLine,
                CommentStart, CommentEnd, InstanceEqual, InstanceNotEqual
            ];

            public static HashSet<string> DataTypes { get; } = [
                Array, Bag, Binary, Boolean, Enumeration, Integer, List, Logical, Number, Of, Real, Set, String
            ];

            public static HashSet<string> InternalProcess { get; } = [
                Insert, Remove
            ];

            public static HashSet<string> BuiltinFunctions { get; } = [
                Abs, Acos, Asin, Atan, BLength, Cos, Exists, Exp,
                Format, HiBound, HiIndex, Length, LoBound, Log,
                Log2, Log10, LoIndex, Nvl, Odd, Rolesof, Sin,
                Sizeof, Sqrt, Tan, Typeof, Usedin, Value,
                ValueIn, ValueUnique
            ];

            public static HashSet<string> Literals { get; } = [
                QuestionMark, Self, Const, E, Pi, False, True, Unknown
            ];

            public static HashSet<string> Operators { get; } = [
                And, AndOr, Div, In, Like, Mod, Not, Or, Xor,
                Add, Sub, Mul, Slash, Equal, NotEqual,
                LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual
            ];

            public static HashSet<string> Aliasing { get; } = [
                Alias, Renamed
            ];

            public static HashSet<string> ModifiersAttributes { get; } = [
                Abstract, Aggregate, As, BasedOn, By, Escape, Fixed, From, Use, With
            ];

            public static HashSet<string> QueryReference { get; } = [
                Query, Reference, Select
            ];

            public static HashSet<string> ConstraintsInheritance { get; } = [
                Oneof, Optional, Subtype, SubtypeConstraint, Supertype, TotalOver, Unique, Where
            ];

            public static HashSet<string> Declarations { get; } = [
                Constant, Derive, Entity, Extensible, Fixed, Function, Generic, 
                GenericEntity, Inverse, Local, Procedure, Rule, Schema, Type, Var
            ];

            public static HashSet<string> FlowControl { get; } = [
                Begin, Case, Else, End, For, If, Otherwise, Repeat, Return, Skip, Then, To, Until, While
            ];

            #region Basic Data Type

            public const string Array = "ARRAY";
            public const string Bag = "BAG";
            public const string Binary = "BINARY";
            public const string Boolean = "BOOLEAN";
            public const string Enumeration = "ENUMERATION";
            public const string Integer = "INTEGER";
            public const string List = "LIST";
            public const string Logical = "LOGICAL";
            public const string Number = "NUMBER";
            public const string Of = "OF";
            public const string Real = "REAL";
            public const string Set = "SET";
            public const string String = "STRING";

            #endregion

            #region Flow Control

            public const string Begin = "BEGIN";
            public const string Case = "CASE";
            public const string Else = "ELSE";
            public const string End = "END";
            public const string For = "FOR";
            public const string If = "IF";
            public const string Otherwise = "OTHERWISE";
            public const string Repeat = "REPEAT";
            public const string Return = "RETURN";
            public const string Skip = "SKIP";
            public const string Then = "THEN";
            public const string To = "TO";
            public const string Until = "UNTIL";
            public const string While = "WHILE";

            #endregion

            #region Declarations

            public const string Constant = "CONSTANT";
            public const string Derive = "DERIVE";
            public const string Entity = "ENTITY";
            public const string Extensible = "EXTENSIBLE";
            public const string Fixed = "FIXED";
            public const string Function = "FUNCTION";
            public const string Generic = "GENERIC";
            public const string GenericEntity = "GENERIC_ENTITY";
            public const string Inverse = "INVERSE";
            public const string Local = "LOCAL";
            public const string Procedure = "PROCEDURE";
            public const string Rule = "RULE";
            public const string Schema = "SCHEMA";
            public const string Type = "TYPE";
            public const string Var = "VAR";

            #endregion

            #region Constraints Inheritance

            public const string Oneof = "ONEOF";
            public const string Optional = "OPTIONAL";
            public const string Subtype = "SUBTYPE";
            public const string SubtypeConstraint = "SUBTYPE CONSTRAINT";
            public const string Supertype = "SUPERTYPE";
            public const string TotalOver = "TOTAL OVER";
            public const string Unique = "UNIQUE";
            public const string Where = "WHERE";

            #endregion

            #region Scope

            public const string EndAlias = "END_ALIAS";
            public const string EndCase = "END_CASE";
            public const string EndConstant = "END_CONSTANT";
            public const string EndEntity = "END_ENTITY";
            public const string EndFunction = "END_FUNCTION";
            public const string EndIf = "END_IF";
            public const string EndLocal = "END_LOCAL";
            public const string EndProcedure = "END_PROCEDURE";
            public const string EndRepeat = "END_REPEAT";
            public const string EndRule = "END_RULE";
            public const string EndSchema = "END_SCHEMA";
            public const string EndSubtypeConstraint = "END_SUBTYPE CONSTRAINT";
            public const string EndType = "END_TYPE";

            #endregion

            #region Index and Ref

            public const string Query = "QUERY";
            public const string Reference = "REFERENCE";
            public const string Select = "SELECT";

            #endregion

            #region Modifiers Attributes

            public const string Abstract = "ABSTRACT";
            public const string Aggregate = "AGGREGATE";
            public const string As = "AS";
            public const string BasedOn = "BASED ON";
            public const string By = "BY";
            public const string Escape = "ESCAPE";
            public const string From = "FROM";
            public const string Use = "USE";
            public const string With = "WITH";

            #endregion

            #region Aliasing

            public const string Alias = "ALIAS";
            public const string Renamed = "RENAMED";

            #endregion

            #region Operator

            public const string And = "AND";
            public const string AndOr = "ANDOR";
            public const string Div = "DIV";
            public const string In = "IN";
            public const string Like = "LIKE";
            public const string Mod = "MOD";
            public const string Not = "NOT";
            public const string Or = "OR";
            public const string Xor = "XOR";

            #endregion

            #region Constants

            public const string QuestionMark = "?";
            public const string Self = "SELF";
            public const string Const = "CONST";
            public const string E = "E";
            public const string Pi = "PI";
            public const string False = "FALSE";
            public const string True = "TRUE";
            public const string Unknown = "UNKNOWN";

            #endregion

            #region Functions

            public const string Abs = "ABS";
            public const string Acos = "ACOS";
            public const string Asin = "ASIN";
            public const string Atan = "ATAN";
            public const string BLength = "BLENGTH";
            public const string Cos = "COS";
            public const string Exists = "EXISTS";
            public const string Exp = "EXP";
            public const string Format = "FORMAT";
            public const string HiBound = "HIBOUND";
            public const string HiIndex = "HIINDEX";
            public const string Length = "LENGTH";
            public const string LoBound = "LObound";
            public const string Log = "LOG";
            public const string Log2 = "LOG2";
            public const string Log10 = "LOG10";
            public const string LoIndex = "LOINDEX";
            public const string Nvl = "NVL";
            public const string Odd = "ODD";
            public const string Rolesof = "ROLESOF";
            public const string Sin = "SIN";
            public const string Sizeof = "SIZEOF";
            public const string Sqrt = "SQRT";
            public const string Tan = "TAN";
            public const string Typeof = "TYPEOF";
            public const string Usedin = "USEDIN";
            public const string Value = "VALUE";
            public const string ValueIn = "VALUE IN";
            public const string ValueUnique = "VALUE UNIQUE";

            #endregion

            #region Procedure

            public const string Insert = "INSERT";
            public const string Remove = "REMOVE";

            #endregion

            #region Symbols

            public const string Dot = ".";
            public const string Comma = ",";
            public const string Semicolon = ";";
            public const string Colon = ":";
            public const string Mul = "*";
            public const string Add = "+";
            public const string UnaryPlus = "+";
            public const string Sub = "-";
            public const string UnaryMinus = "-";
            public const string Equal = "=";
            public const string Percent = "%";
            public const string Apostrophe = "'";
            public const string Backslash = "\\";
            public const string Slash = "/";
            public const string LessThan = "<";
            public const string GreaterThan = ">";
            public const string IndexOf = "[";
            public const string LeftBracket = "[";
            public const string RightBracket = "]";
            public const string LeftBrace = "{";
            public const string RightBrace = "}";
            public const string Pipe = "|";
            public const string EConstant = "E";
            public const string LeftParen = "(";
            public const string RightParen = ")";
            public const string LessThanOrEqual = "<=";
            public const string NotEqual = "<>";
            public const string GreaterThanOrEqual = ">=";
            public const string LeftAsterisk = "<*";
            public const string Assignment = ":=";
            public const string DoublePipe = "||";
            public const string ExpSymbol = "**";
            public const string CommentSingleLine = "--";
            public const string CommentStart = "(*";
            public const string CommentEnd = "*)";
            public const string InstanceEqual = ":=:";
            public const string InstanceNotEqual = ":<>:";

        #endregion
        }

    }
}
