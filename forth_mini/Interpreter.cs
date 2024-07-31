using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forth_mini {
    public class Interpreter {
        private ForthStack stack = new ForthStack();
        private Stack<int> loopStack = new Stack<int>(); // インデックススタック
        private Dictionary<string, Action> words = new Dictionary<string, Action>();
        private Queue<string> tokenQueue = new Queue<string>();

        public Interpreter() {
            RegisterBasicWords();
        }

        // 階乗を計算する関数
        static double Factorial(double number) {
            double result = 1;
            for (int i = 1; i <= number; i++) {
                result *= i;
            }
            return result;
        }

        private void RegisterBasicWords() {
            words["."] = () => Console.WriteLine(stack.Pop());
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
            words["drop"] = () => stack.Pop();
            words["swap"] = () => {
                var a = stack.Pop();
                var b = stack.Pop();
                stack.Push(a);
                stack.Push(b);
            };
            words["over"] = () => {
                var a = stack.Pop();
                var b = stack.Peek();
                stack.Push(a);
                stack.Push(b);
            };
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

            // 文字列操作の基本的な単語を追加
            words["concat"] = () => {
                var a = stack.Pop().ToString();
                var b = stack.Pop().ToString();
                stack.Push(b + a);
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
                var loopTokens = CaptureLoopTokens("loop");

                for (var i = loopStart; i < loopEnd; i++) {
                    loopStack.Push(i);
                    ExecuteTokens(loopTokens);
                    loopStack.Pop();
                }
            };
            words["loop"] = () => { };

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
        }

        private void SkipTo(params string[] targets) {
            int nestedIfs = 0;
            while (true) {
                var token = GetNextToken();
                if (token == null) throw new InvalidOperationException("Unmatched if/else/then");

                if (token == "if") nestedIfs++;
                else if (token == "then") {
                    if (nestedIfs == 0) break;
                    nestedIfs--;
                } else if (token == "else" && nestedIfs == 0) break;
            }
        }

        private string GetNextToken() {
            if (tokenQueue.Count == 0) return null;
            return tokenQueue.Dequeue();
        }

        private List<string> CaptureLoopTokens(string endToken) {
            var tokens = new List<string>();
            int nestedLoops = 0;
            while (true) {
                var token = GetNextToken();
                if (token == null) throw new InvalidOperationException($"Unmatched loop token: {endToken}");

                if (token == "do" || token == "begin" || token == "while") nestedLoops++;
                else if (token == endToken) {
                    if (nestedLoops == 0) break;
                    nestedLoops--;
                }

                tokens.Add(token);
            }
            return tokens;
        }

        private void ExecuteTokens(List<string> tokens) {

            foreach (var token in tokens) {
                if (double.TryParse(token, out double number)) {
                    stack.Push(number);
                } else if (token.StartsWith("\"") && token.EndsWith("\"")) {
                    // 文字列リテラルとして処理
                    stack.Push(token.Trim('"'));
                } else if (words.ContainsKey(token)) {
                    words[token]();
                } else {
                    throw new InvalidOperationException($"Unknown word: {token}");
                }
            }
        }

        public void Execute(string command) {
            string _command = Regex.Replace(command, @"\s+", " ");
            var tokens = Tokenize(_command);
            foreach (var token in tokens) {
                tokenQueue.Enqueue(token);
            }

            while (tokenQueue.Count > 0) {
                var token = tokenQueue.Dequeue();
                if (double.TryParse(token, out double number)) {
                    stack.Push(number);
                } else if (token.StartsWith("\"") && token.EndsWith("\"")) {
                    // 文字列リテラルとして処理
                    stack.Push(token.Trim('"'));
                } else if (words.ContainsKey(token)) {
                    words[token]();
                } else {
                    throw new InvalidOperationException($"Unknown word: {token}");
                }
            }
        }

        private IEnumerable<string> Tokenize(string command) {
            var tokens = new List<string>();
            var sb = new StringBuilder();
            bool inString = false;

            foreach (var c in command) {
                if (c == '\"') {
                    if (inString) {
                        sb.Append(c);
                        tokens.Add(sb.ToString());
                        sb.Clear();
                    } else {
                        if (sb.Length > 0) {
                            tokens.Add(sb.ToString());
                            sb.Clear();
                        }
                        sb.Append(c);
                    }
                    inString = !inString;
                } else if (c == ' ' && !inString) {
                    if (sb.Length > 0) {
                        tokens.Add(sb.ToString());
                        sb.Clear();
                    }
                } else {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0) {
                tokens.Add(sb.ToString());
            }

            return tokens;
        }
    }
}

