using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Napa.Scripting;

public class Script : ScriptBase {
    public override void Run() {
        //Output "Hello, NAPA" in below Console window
        //在下方console界面输出字符串Hello,NAPA
        Console.WriteLine("Hello,NAPA");
    }
}