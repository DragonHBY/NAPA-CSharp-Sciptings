using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Napa.Scripting;
using Napa.Graphics;

public class Script : ScriptBase {
    public override void Run() {
        Graphics.Erase();
        Graphics.UpdateView();
    }
}