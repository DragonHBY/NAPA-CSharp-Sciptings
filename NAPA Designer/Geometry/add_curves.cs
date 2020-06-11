using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.ComponentModel;
using Napa.Core.Geometry;
using Napa.Gui;
using Napa.Graphics;
using Napa.Legacy.NativeAccess;
using Napa.Scripting;

public class Script : ScriptBase {
    public override void Run() {
         // Input from the user by a popup window
        // -------------------------------------
        var input = new Input1();
        input.hullorg = "HULLA";
        input.curvetype = "X";
        input.curveloc1 = "0"; 
        input.curveloc2 = " "; 
        UI.GetInput("Create New Curves", input, "OK");
        
        //define the prefix of curve name
        string curnamepref1 ="";
        if (input.curvetype =="X") curnamepref1 = "FR";
        if (input.curvetype =="Y") curnamepref1 = "BT";
        if (input.curvetype =="Z") curnamepref1 = "WL"; 
        string curnamepref2 ="";
        if (input.hullorg.Contains("F"))  curnamepref2 = "F";
        if (input.hullorg.Contains("A"))  curnamepref2 = "A";
        if (input.hullorg == "HULL")  curnamepref2 = "";
		string curnamepref = curnamepref1 + curnamepref2;
        
        //get the curve positon
        string[] curlocarr1 = input.curveloc1.Split(new Char[] {','});
        string[] curlocarr2 = input.curveloc2.Split(new Char[] {','});
            
       if (input.curveloc2 != " ")  {
          double sv1 = double.Parse(curlocarr2[0]);      
          double fv1 = double.Parse(curlocarr2[1]);     
          double stepv1 = double.Parse(curlocarr2[2]);  
          double dnr = (fv1-sv1)/stepv1;
          int nr = (int) dnr;
          Array.Resize(ref curlocarr1,nr+1);
         for (var i = 0; i<nr+1; i++) {
             double xloc = sv1+stepv1*i;
             curlocarr1[i] = xloc.ToString(); 
              }  
         }
        
        string hulldefstr=" ";
        ISurface hullsur = Geometry.Manager.GetSurface(input.hullorg);
        var hulldefstr1 =hullsur.ToDefinitionBuilder().DefinitionText.ToString(); 
        
         //create curves
        foreach(string cur1 in curlocarr1) {
        string  curname1=curnamepref+cur1;
        hulldefstr1 = hulldefstr1 + "," + curname1;
        DefineCur(curname1,input.curvetype,double.Parse(cur1),input.hullorg); 
        } 
        Geometry.RunDefinition(hulldefstr1.ToString()); 
         
    }
    
 // Input popup properties
// ----------------------
public class Input1 {
    [CategoryAttribute("Create New Curves")]
    [DisplayName("Set surface to be intersected (HULLF or HULLA)")]
    [Tooltip("Currently support hullf and hulla only")] 
    public string hullorg { get; set; }
    
    [CategoryAttribute("Create New Curves")]
    [DisplayName("Set Curve location (X, Y, Z)")]
    [Tooltip("X=frame, Y=buttock, Z=waterline")] 
    public string curvetype { get; set; }
    
    [CategoryAttribute("Create New Curves")]
    [DisplayName("Input Coordinate Value:1,2,3:")]
    [Tooltip("1,2,3")]
    public string curveloc1 { get; set; }
    
    [CategoryAttribute("Create New Curves")]
    [DisplayName("Input Coordinate Value: min,max,step")]
    [Tooltip("10,20,5")]
    public string curveloc2 { get; set; }
}

public void DefineCur(string curname,string curtype, double xloc1,string hullsur) {
   var definition = new StringBuilder();
  definition.AppendLine("CUR " + curname);
  definition.AppendLine(curtype + " " + xloc1);
  if (curtype =="X") definition.AppendLine("ZY *" + hullsur);
  if (curtype =="Y") definition.AppendLine("ZX *" + hullsur);
  if (curtype =="Z") definition.AppendLine("XY *" + hullsur);
  definition.AppendLine("OK");
  Geometry.RunDefinition(definition.ToString());
 definition.Clear();  
}

}