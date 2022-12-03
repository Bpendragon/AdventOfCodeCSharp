using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day18 : ASolution
    {
        readonly List<string> Expressions = new();
        readonly List<string> PartTwoExpressions = new();
        public Day18() : base(18, 2020, "Operation Order")
        {
            foreach(var l in Input.SplitByNewline())
            {
                Expressions.Add(l);
                PartTwoExpressions.Add(ConvertForPart2(l));
            }
        }

        protected override object SolvePartOne()
        {
            long totalSum = 0;
            foreach (var e in Expressions)
            {
                totalSum += EvaluateExpression(e);
            }

            return totalSum;
        }

        protected override object SolvePartTwo()
        {
            long totalSum = 0;
            foreach (var e in PartTwoExpressions)
            {
                totalSum += EvaluateExpression(e);
            }

            return totalSum;
        }


        /*
         * So this is a bit of fun stack based manipulation similar to Reverse Polish Notation https://en.wikipedia.org/wiki/Reverse_Polish_notation
         * RPN is inherently stack based, push operands onto the stack till you hit an operator, pop two, do operation, and push the result.
         * This is similar to that, and about halfway between there and the Shunting Yard Algorithm which can convert infix (that is a+b*d) notation into RPN
         * 
         * How this algorithm works:
         * 
         * Create two stacks, one for operators and one for numerics (you can do it with one "Object" stack but that's harder to implement)
         * Step through the list and at each character (in this instance operators are '(', ')', '*', and '+'):
         * if numeric:
         *     if Operator stack has items, and the top of the stack is not an open paren:
         *          Pop the top item off the numeric stack
         *          Pop the top item off the Operator stack
         *          Do the appropriate operation on the two items
         *          Push the result onto the numeric stack
         *      if the Operator stack is empty, or Peek reveals an open-paren:
         *          Push the item on the numeric stack
         * if an Operator:
         *      If NOT ')':
         *          Push the item
         *      Else:
         *          Peek the Operator stack, if '(', pop it into oblivion
         *          
         *          While OperatorStack.Peek != '('
         *            Pop the operator stack to get operation
         *            pop the numeric stack twice to get operands
         *            Do the Appropriate Operation and push the result to the numeric stack
         * 
         * After you've stepped through the Expression:
         * While OperatorStack has items:
         *      Pop the operator stack to get operation
         *      pop the numeric stack twice to get operands
         *      Do the Appropriate Operation and push the result to the numeric stack
         * Result is sitting as the only item in the numeric stack, pop and return it
         * 
         */

        private static long EvaluateExpression(string expression)
        {
            Stack<long> nums = new();
            Stack<char> operations = new();

            long opVal;
            foreach (var c in expression)
            {
                if (c == ' ') continue;
                if (long.TryParse(c.ToString(), out long val))
                {
                    if (operations.Count > 0 && operations.Peek() != '(')
                    {
                        char op = operations.Pop();
                        opVal = nums.Pop();
                        switch (op)
                        {
                            case '+': nums.Push(opVal + val); break;
                            case '*': nums.Push(opVal * val); break;
                        }
                    }
                    else
                    {
                        nums.Push(val);
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '(':
                        case '+':
                        case '*': operations.Push(c); break;
                        case ')':
                            if (operations.Peek() == '(') operations.Pop();
                            while (operations.Count > 0 && operations.Peek() != '(')
                            {
                                char op = operations.Pop();
                                val = nums.Pop();
                                opVal = nums.Pop();
                                switch (op)
                                {
                                    case '+': nums.Push(opVal + val); break;
                                    case '*': nums.Push(opVal * val); break;
                                }
                            }
                            break;
                    }
                }
            }

            while (operations.Count > 0)
            {
                char op = operations.Pop();
                opVal = nums.Pop();
                long val = nums.Pop();
                switch (op)
                {
                    case '+': nums.Push(opVal + val); break;
                    case '*': nums.Push(opVal * val); break;
                }
            }

            return nums.Pop();
        }


        /*
         * Converts the given expression to work with '+' having precedence
         * 
         * Steps taken:
         * 
         * Strip whitespace and convert string to List<char>
         * 
         * Find Index of first '+' operand
         * 
         * if the character directly after '+' is a open paren (it should never be an close paren):
         *      Walk forwards through list to find the matching close paren
         *      Insert close paren at that location
         * Else insert close paren at index + 2 (after the next number)
         * 
         * if the character directly before '+' is a clsoe paren (it should never be an open paren):
         *      Walk forwards through list to find the matching open paren
         *      Insert open paren at that location
         * Else insert open paren at index - 1  (before the next number)
         * Find the next index of '+' from your current location
         * Repeat until reached end of list
         * Convext List<char> to string and return
         */

        private static string ConvertForPart2(string expression)
        {
            var e2 = expression.Replace(" ", "").ToList();

            int index = e2.IndexOf('+');
            while (index > 0)
            {
                int rearIndex;
                int frontIndex;
                if (e2[index + 1] == '(')
                {
                    int i = 1;
                    int parenCount = 0;
                    while ((index + i) < e2.Count)
                    {
                        if (e2[index + i] == '(') parenCount++;
                        if (e2[index + i] == ')') parenCount--;
                        if (parenCount == 0) break;
                        i++;
                    }
                    rearIndex = index + i;

                }
                else rearIndex = index + 2;
                e2.Insert(rearIndex, ')');

                if (e2[index - 1] == ')')
                {
                    int i = 1;
                    int parenCount = 0;
                    while ((index + i) >= 0)
                    {
                        if (e2[index - i] == '(') parenCount++;
                        if (e2[index - i] == ')') parenCount--;
                        if (parenCount == 0) break;
                        i++;
                    }
                    frontIndex = index - i;
                }
                else frontIndex = index - 1;
                e2.Insert(frontIndex, '(');

                index = e2.IndexOf('+', index + 2);
            }

            return e2.JoinAsStrings();
        }
    }
}
