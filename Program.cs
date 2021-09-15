using System;
using System.Collections.Generic;
using System.Linq;

namespace BitScript
{
    class Program
    {
        static Logger _logger;
        
        static void Main(string[] args)
        {
            _logger = new Logger(
                bool.TryParse(args.Length >= 2 ? args[1] : "false", out var r1) ? r1 : false,
                bool.TryParse(args.Length >= 3 ? args[2] : "false", out var r2) ? r2 : false);
            
            var s = Run(args[0]);
            
            Console.WriteLine($"\n{string.Join('\n', s.Select(l => Convert.ToString(l, 2).PadLeft(8, '0')))}");
        }
        
        static Stack<byte> Run(string code)
        {
            code = code.Replace(" ", string.Empty).Replace("\n", string.Empty);
            
            Stack<byte> MainStack = new Stack<byte>();
            
            Stack<char> CodeStack = new Stack<char>(code.Reverse());
            
            for (int i = 0; CodeStack.TryPop(out char c1); i++)
            {
                _logger.LogLine($"Command : {c1}");
                
                switch (c1)
                {
                    case '$':
                        if (CodeStack.TryPop(out char c2))
                        {
                            _logger.LogLine($"Sub-command : {c2}");
                            if (c2 == '<')
                            {
                                _logger.LogLine("Action : push");
                                if (GetValue(out var r))
                                    MainStack.Push(r);
                            }
                            else if (c2 == '>')
                            {
                                MainStack.TryPop(out var _);
                                _logger.LogLine("Pop");
                            }
                            else
                            {
                                _logger.LogLine("Unknown Sub-command");
                                CodeStack.Push(c2);
                            }
                        }
                        break;
                        
                    case '|':
                        if (MainStack.Count < 2)
                            break;
                            
                        var ora = (int)MainStack.Pop();
                        var orb = (int)MainStack.Pop();
                        
                        var orr = (byte)(ora ^ orb);
                        
                        _logger.LogLine($"Push value {ora} {c1} {orb} = {orr}");
                        
                        MainStack.Push(orr);
                        break;
                        
                    case '&':
                        if (MainStack.Count < 2)
                            break;
                            
                        var anda = (int)MainStack.Pop();
                        var andb = (int)MainStack.Pop();
                        
                        var andr = (byte)(anda & andb);
                        
                        _logger.LogLine($"Push value {anda} {c1} {andb} = {andr}");
                        
                        MainStack.Push(andr);
                        break;
                        
                    case '^':
                        if (MainStack.Count < 2)
                            break;
                            
                        var xora = (int)MainStack.Pop();
                        var xorb = (int)MainStack.Pop();
                        
                        var xorr = (byte)(xora ^ xorb);
                        
                        _logger.LogLine($"Push value {xora} {c1} {xorb} = {xorr}");
                        
                        MainStack.Push(xorr);
                        break;
                        
                    case '~':
                        if (MainStack.TryPop(out var invr))
                        {
                            _logger.LogLine($"Push value {c1} {invr} = {(byte)(~invr)}");
                            MainStack.Push((byte)(~invr));
                        }
                        else
                            _logger.LogLine("Stack is empty.");
                        break;
                        
                    case '%':
                        if (MainStack.TryPop(out var revr))
                        {
                            MainStack.Push(revr.Reverse());
                        }
                        else
                            _logger.LogLine("Stack is empty.");
                        break;
                        
                    case '<':
                        if (CodeStack.TryPop(out var lshf))
                        {
                            if (lshf != '<')
                            {
                                _logger.LogLine("Syntax Error");
                                break;
                            }
                            
                            if (GetValue(out var lshfb))
                            {
                                var lshfp = MainStack.Pop();
                                
                                var shfr = (byte)(lshfp << lshfb);
                                
                                _logger.LogLine($"Push value {lshfp} << {lshfb} = {shfr}");
                                
                                MainStack.Push(shfr);
                            }
                        }
                        break;
                        
                    case '>':
                        if (CodeStack.TryPop(out var rshf))
                        {
                            if (rshf != '>')
                            {
                                _logger.LogLine("Syntax Error");
                                break;
                            }
                            
                            if (GetValue(out var rshfb))
                            {
                                var rshfp = MainStack.Pop();
                                
                                var shfr = (byte)(rshfp >> rshfb);
                                
                                _logger.LogLine($"Push value {rshfp} >> {rshfb} = {shfr}");
                                
                                MainStack.Push(shfr);
                            }
                        }
                        break;
                        
                    case '@':
                        if (CodeStack.TryPop(out var atc))
                        {
                            _logger.LogLine($"Sub-command : {atc}");
                            
                            if (atc == '<')
                            {
                                var k = Console.ReadKey(true).KeyChar;
                                
                                _logger.LogLine($"Input Value : {k}");
                                
                                MainStack.Push((byte)k);
                            }
                            else if (atc == '>')
                            {
                                char k = (char)(MainStack.TryPop(out var kk) ? kk : 0);
                                
                                _logger.LogLine($"Output Value : {k}");
                                
                                if (_logger.IsDebug)
                                    _logger.AddOutput(k);
                                else
                                    Console.Write(k);
                            }
                        }
                        break;
                        
                    case ';':
                        if (MainStack.TryPop(out var semiv))
                        {
                            _logger.LogLine($"Push value : {semiv}");
                            MainStack.Push(semiv);
                            MainStack.Push(semiv);
                        }
                        else
                            _logger.LogLine("Stack is empty");
                        break;
                        
                    default:
                        _logger.LogLine("Unknown Command");
                        break;
                }
            }
            
            bool GetValue(out byte b)
            {
                int a = 0;
                char c4;
                
                if (CodeStack.TryPop(out c4))
                {
                    if (c4 != '[')
                    {
                        _logger.LogLine($"Syntax Error");
                        b = 0;
                        return false;
                    }
                }
                
                int j;
                
                for (j = 0; j < 3; j++)
                {
                    if (!CodeStack.TryPop(out char c3))
                        break;
                    
                    _logger.Log($"Pop value : {c3} | ");
                    
                    if (c3 == '+')
                        a += 1;
                    else if (c3 == '-')
                        a += 0;
                    else
                    {
                        _logger.LogLine("Unknown value");
                        CodeStack.Push(c3);
                        break;
                    }
                    
                    a <<= 1;
                    
                    _logger.LogLine($"Current value : {a}");
                }
                                
                if (j != 3)
                {
                    b = 0;
                    return false;
                }
                
                a >>= 1;
                
                _logger.LogLine($"Final value : {a}");
                
                if (CodeStack.TryPop(out c4))
                {
                    if (c4 == ']')
                    {
                        _logger.LogLine("Success to get final value.");
                        b = (byte)a;
                        return true;
                    }
                    else
                    {
                        CodeStack.Push(c4);
                        _logger.LogLine("Check final Failed.");
                   }
                }
                
                b = 0;
                return false;
            }
            
            if (_logger.IsDebug)
                Console.Write(_logger.DebugOutput);
            
            return MainStack;
        }
    }
}
