using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace forth_mini {

    public class ForthStack {
        private Stack<object> stack = new Stack<object>();

        public void Push(object value) {
            stack.Push(value);
        }

        public object Pop() {
            if (stack.Count == 0) throw new InvalidOperationException("Stack is empty");
            return stack.Pop();
        }

        public object Peek() {
            if (stack.Count == 0) throw new InvalidOperationException("Stack is empty");
            return stack.Peek();
        }

        public int Count => stack.Count;

        public object Pick(int n) {
            if (n < 0 || n >= stack.Count) throw new InvalidOperationException("Invalid pick index");
            var tempStack = new Stack<object>();
            for (int i = 0; i < n; i++) {
                tempStack.Push(stack.Pop());
            }
            var value = stack.Peek();
            while (tempStack.Count > 0) {
                stack.Push(tempStack.Pop());
            }
            return value;
        }

        public void Roll(int n) {
            if (n < 0 || n >= stack.Count) throw new InvalidOperationException("Invalid roll index");
            var tempStack = new Stack<object>();
            for (int i = 0; i < n; i++) {
                tempStack.Push(stack.Pop());
            }
            var value = stack.Pop();
            while (tempStack.Count > 0) {
                stack.Push(tempStack.Pop());
            }
            stack.Push(value);
        }

        public object[] ToArray() {
            return stack.ToArray();
        }
    }

    public class Compiler {
        private Interpreter interpreter;

        public Compiler(Interpreter interpreter) {
            this.interpreter = interpreter;
        }

        public void Compile(string sourceCode) {
            var lines = sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines) {
                interpreter.Execute(line);
            }
        }
    }

    class Program {
        static void Main(string[] args) {
            var interpreter = new Interpreter();
            var compiler = new Compiler(interpreter);

            // ユーザー入力を受け付けてインタープリターを実行
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("Forth mini Ver." + version + " by Spumoni");
            Console.WriteLine("Enter Forth commands (type 'exit' to quit):");
            string input;
            while ((input = Console.ReadLine()) != "exit") {
                try {
                    compiler.Compile(input);
                    if (Console.CursorLeft != 0) {
                        Console.WriteLine();
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
