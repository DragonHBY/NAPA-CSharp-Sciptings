using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Napa.Core.Geometry;
using Napa.Scripting;

///<summary>
/// This script is an example of creating NAPA geometry.
/// At first, new principal planes are created on the given frames.
/// Then new surface objects are created on the planes with given limits
/// and finally new rooms are created inside the given limits 
/// splitted by the planes created earlier.
///</summary>
public class CreateGeometry : ScriptBase {

    public override void Run() {
        // Frame numbers and limits to be used to create new transverse bulkheads + rooms in between
        var frameNumbers = new[] { 54, 62, 69, 76, 83, 91, 99 };
        var limits = new[] { "<+S.INNHULL_P", "Z<S.MDECK", "Z>S.INNBTM" };
        
        // Loop through the frame numbers and create new reference planes and surface objects
        foreach (var i in frameNumbers) {
            var name = "TEST_#" + i;
            Geometry.CreatePlane("S." + name, Axis.X, "#" + i);   
            Geometry.CreateSurfaceObject("SO." + name, "S." + name, limits);
        }
        
        // Create new rooms between the transverse bulkheads
        // NOTE: This runs raw text based definition of NAPA geometry, i.e. any NAPA geometry 
        // can be created by building the text definition of geometry
        for (int i = 1; i < frameNumbers.Length; i++) {
            var definition = new StringBuilder();
            definition.AppendLine("ROOM R.TEST_" + i);
            definition.AppendLine("LIM " + string.Join(" ", limits) + " X>S.TEST_#" + frameNumbers[i-1] + " X<S.TEST_#" + frameNumbers[i] );
            Geometry.RunDefinition(definition.ToString());
        }
    }
}