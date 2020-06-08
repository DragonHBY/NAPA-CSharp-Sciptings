using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Napa.Core.Geometry;
using Napa.Core.Geometry.Definitions;
using Napa.Core.Steel;
using Napa.Scripting;

///<summary>
/// This script is an example of editing existing NAPA geometry. The main
/// purpose is to show how to access the definition builder of geometric object. 
/// Definition builders are classes to help constructing or modifying 
/// the definition text of the geometry. In this example the object name used as a limit 
/// is replaced with another object name. The operation is done for the surface objects 
/// which are selected on the graphics.
///</summary>
public class EditGeometry : ScriptBase {

    public override void Run() {
        var oldLimit = "SSTG_1P";
        var newLimit = "SSTG_3P";
    
        // Get surface objects selected on the graphics
        var selectedSurfaceObjects = Graphics.GetSelectedGeometricObjects<ISurfaceObject>();
        
        // Loop through the selected surface objects and replace the old limit with new limit and run the definition
        foreach (var surfaceObject in selectedSurfaceObjects) {
            // Get definition builder from surface object and cast it to correct type
            var definitionBuilder = surfaceObject.ToDefinitionBuilder() as SurfaceObjectDefinitionBuilder;
            if (definitionBuilder == null) continue;
            
            // Loop through all the definition parts (i.e. LIM, RED, ADD)
            foreach (var definitionPart in definitionBuilder.Parts) {
                // Loop through each limit in one part (e.g. <+HULL or Z<MDECK etc.)
                foreach (var definitionLimit in definitionPart.Limits) {
                    if (definitionLimit.Value.Contains(oldLimit)) {
                        definitionLimit.Value = definitionLimit.Value.Replace(oldLimit, newLimit);
                        Geometry.RunDefinition(definitionBuilder.DefinitionText);
                    }
                }
            }
        }
    }
}