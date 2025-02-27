﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forth_mini {
    public partial class Interpreter {
        private void RegisterBasicWords() {
            words["."] = () =>
                Console.Write(stack.Pop() + " ");
            // .S コマンドの追加（スタックの内容を表示）
            words[".s"] = () => {
                Console.Write($"<{stack.Count}> ");
                Array _ = stack.ToArray();
                Array.Reverse(_);
                foreach (var item in _) {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
            };

            words["+"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(a + b);
            };
            words["-"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b - a);
            };
            words["*"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(a * b);
            };
            words["/"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b / a);
            };
            words["mod"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b % a);
            };
            words["/mod"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b % a);
                stack.Push(Math.Floor(b / a));
            };
            words["pi"] = () => {
                stack.Push(Math.PI);
            };
            words["e"] = () => {
                stack.Push(Math.E);
            };
            words["abs"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Abs(a));
            };
            // 最大の整数値
            words["floor"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Floor(a));
            };
            // 最小の整数値
            words["ceiling"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Ceiling(a));
            };
            // 最も近い整数
            words["round"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Round(a));
            };
            // 整数部
            words["truncate"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Truncate(a));
            };
            // 指数関数
            words["exp"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Exp(a));
            };
            // 立方根
            words["cbrt"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Cbrt(a));
            };
            // 平方根
            words["sqrt"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Sqrt(a));
            };
            // 累乗
            words["pow"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Pow(b, a));
            };
            words["**"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Pow(b, a));
            };
            // 符号を変える
            words["minus"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(-(a));
            };
            // nPr
            words["npr"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Factorial(b) / Factorial(b - a));
            };
            // nCr
            words["ncr"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Factorial(b) / (Factorial(a) * Factorial(b - a)));
            };
            // 自然対数
            words["log"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Log(a, b));
                //stack.Push(Math.Log(a));
            };
            words["log2"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Log2(a));
            };
            words["log10"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Log10(a));
            };
            words["ln"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Log(a, Math.E));
            };

            // 三角関数
            words["sin"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Sin(a * (Math.PI / 180)));
            };
            words["cos"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Cos(a * (Math.PI / 180)));
            };
            words["tan"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Tan(a * (Math.PI / 180)));
            };
            words["asin"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Asin(a) / (Math.PI / 180));
            };
            words["acos"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Acos(a) / (Math.PI / 180));
            };
            words["atan"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Atan(a) / (Math.PI / 180));
            };
            words["atan2"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Atan2(b, a) / (Math.PI / 180));
            };
            words["sinh"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Sinh(a));
            };
            words["cosh"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Cosh(a));
            };
            words["tanh"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.Tanh(a));
            };

            // 逆数
            words["rcp"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                stack.Push(Math.ReciprocalEstimate(a));
            };
            // 0.0から1.0の間の乱数
            words["random"] = () => {
                int seed = Environment.TickCount;
                Random rnd = new Random(seed);
                stack.Push(rnd.NextDouble());
            };
            words["dup"] = () => {
                var a = stack.Peek();
                stack.Push(a);
            };
            words["-dup"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                if (a != 0)
                    stack.Push(a);
            };
            words["?dup"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                if (a != 0)
                    stack.Push(a);
            };
            words["rot"] = () => {
                var a = stack.Pop();
                var b = stack.Pop();
                var c = stack.Pop();
                stack.Push(b);
                stack.Push(a);
                stack.Push(c);
            };
            words["drop"] = () => stack.Pop();
            words["swap"] = () => {
                var a = stack.Pop();
                var b = stack.Pop();
                stack.Push(a);
                stack.Push(b);
            };
            words["over"] = () => {
                var a = stack.Pick(1);
                stack.Push(a);
            };
            words["tuck"] = () => {
                var a = stack.Pop();
                var b = stack.Pop();
                stack.Push(a);
                stack.Push(b);
                stack.Push(a);
            };
            // pick と roll の追加
            words["pick"] = () => {
                var n = Convert.ToInt32(stack.Pop());
                stack.Push(stack.Pick(n));
            };
            words["roll"] = () => {
                var n = Convert.ToInt32(stack.Pop());
                stack.Roll(n);
            };
            // 比較演算
            words["="] = () => {
                var a = stack.Pop();
                var b = stack.Pop();
                stack.Push(a.Equals(b) ? -1 : 0);
            };
            words["<"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b < a ? -1 : 0);
            };
            words[">"] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b > a ? -1 : 0);
            };
            words["<="] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b <= a ? -1 : 0);
            };
            words[">="] = () => {
                var a = Convert.ToDouble(stack.Pop());
                var b = Convert.ToDouble(stack.Pop());
                stack.Push(b >= a ? -1 : 0);
            };


            // 文字列操作の基本的な単語を追加
            words["concat"] = () => {
                var a = stack.Pop().ToString();
                var b = stack.Pop().ToString();
                stack.Push(b + a);
            };

            // ユーザー定義の単語の開始と終了を追加
            words[":"] = () => {
                isDefiningWord = true;
                currentWordName = GetNextToken();
                currentWordTokens = new List<string>();
            };
            words[";"] = () => {
                if (!isDefiningWord) {
                    throw new InvalidOperationException("No word is being defined");
                }
                isDefiningWord = false;
                userWords[currentWordName] = new List<string>(currentWordTokens);
            };

            // 条件分岐の実装
            words["if"] = () => {
                var condition = Convert.ToDouble(stack.Pop());
                if (condition == 0) {
                    SkipTo("else", "then");
                }
            };
            words["else"] = () => SkipTo("then");
            words["then"] = () => { };

            // ループの実装
            words["do"] = () => {
                var loopEnd = Convert.ToInt32(stack.Pop());
                var loopStart = Convert.ToInt32(stack.Pop());
                var loopTokens = CaptureLoopTokens("loop", "+loop");

                var i = loopStart;
                while (i < loopEnd) {
                    loopStack.Push(i);
                    ExecuteTokens(loopTokens);
                    loopStack.Pop();
                    if (loopTokens.Last() == "+loop") 
                        i += Convert.ToInt32(stack.Pop());
                    else
                        i++;
                }
            };
            words["loop"] = () => { };

            words["+loop"] = () => { };

            words["begin"] = () => {
                var loopTokens = CaptureLoopTokens("until");
                bool condition = false;
                do {
                    ExecuteTokens(loopTokens);
                    condition = Convert.ToDouble(stack.Pop()) != 0;
                } while (!condition);
            };
            words["until"] = () => { };

            words["while"] = () => {
                var conditionTokens = CaptureLoopTokens("repeat");
                var loopTokens = CaptureLoopTokens("repeat");
                while (true) {
                    ExecuteTokens(conditionTokens);
                    if (Convert.ToDouble(stack.Pop()) == 0) break;
                    ExecuteTokens(loopTokens);
                }
            };
            words["repeat"] = () => { };

            // インデックスを返す単語 "i" の追加
            words["i"] = () => {
                if (loopStack.Count > 0) {
                    stack.Push(loopStack.Peek());
                } else {
                    throw new InvalidOperationException("No loop index available");
                }
            };
            // words コマンドの追加
            words["words"] = () => {
                var allWords = new List<string>(words.Keys);
                allWords.AddRange(userWords.Keys);
                allWords.Sort();

                foreach (var word in allWords) {
                    Console.WriteLine(word);
                }
            };
        }
    }
}