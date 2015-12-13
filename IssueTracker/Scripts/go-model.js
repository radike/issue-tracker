function gojs_init() {
    var $ = go.GraphObject.make;

    myDiagram = $(go.Diagram, "swVisualization",
                  {
                      initialContentAlignment: go.Spot.Center,
                      "undoManager.isEnabled": true,
                      allowHorizontalScroll: false,
                      allowVerticalScroll: false
                  });

    // define a simple Node template
    myDiagram.nodeTemplate =
      $(go.Node, "Auto",  // the Shape will go around the TextBlock
        $(go.Shape, "RoundedRectangle",
          // Shape.fill is bound to Node.data.color
          new go.Binding("fill", "color")),
        $(go.TextBlock,
          { margin: 5, stroke: "white", font: "bold 14px Helvetica" },
          // TextBlock.text is bound to Node.data.key
          new go.Binding("text", "key"))
      );

    window.gojs_create_flow();
}

function load(jsondata) {
    // create the model from the data in the JavaScript object parsed from JSON text
    myDiagram.model = new go.GraphLinksModel(jsondata["nodes"], jsondata["links"]);
}