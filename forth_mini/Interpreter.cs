﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forth_mini {
    public partial class Interpreter {
        private ForthStack stack = new ForthStack();
        private Stack<int> loopStack = new Stack<int>(); // インデックススタック
        private Dictionary<string, Action> words = new Dictionary<string, Action>();
        private Queue<string> tokenQueue = new Queue<string>();
        private Dictionary<string, List<string>> userWords = new Dictionary<string, List<string>>();
        private bool isDefiningWord = false;
        private string currentWordName;
        private List<string> currentWordTokens;

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
                } else if (userWords.ContainsKey(token)) {
                    ExecuteTokens(userWords[token]);
                } else {
                    throw new InvalidOperationException($"Unknown word: {token}");
                }
            }
        }
        public void Execute(string command) {
            var tokens = Tokenize(command);
            foreach (var token in tokens) {
                tokenQueue.Enqueue(token);
            }

            while (tokenQueue.Count > 0) {
                var token = tokenQueue.Dequeue();
                if (isDefiningWord) {
                    if (token == ";") {
                        words[";"]();
                    } else {
                        currentWordTokens.Add(token);
                    }
                } else if (double.TryParse(token, out double number)) {
                    stack.Push(number);
                } else if (token.StartsWith("\"") && token.EndsWith("\"")) {
                    // 文字列リテラルとして処理
                    stack.Push(token.Trim('"'));
                } else if (words.ContainsKey(token)) {
                    words[token]();
                } else if (userWords.ContainsKey(token)) {
                    ExecuteTokens(userWords[token]);
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
