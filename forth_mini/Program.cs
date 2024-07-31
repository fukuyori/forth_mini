using System;
using System.Collections.Generic;
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
            Console.WriteLine("Enter Forth commands (type 'exit' to quit):");
            string input;
            while ((input = Console.ReadLine()) != "exit") {
                try {
                    compiler.Compile(input);
                } catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
