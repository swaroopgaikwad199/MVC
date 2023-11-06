if (!com)
	var com = {};
if (!com.logicpartners)
	com.logicpartners = {};
if (!com.logicpartners.labelControl)
	com.logicpartners.labelControl = {};
	
com.logicpartners.labelControl.size = function(designer) {
	var self = this;
	this.designer = designer;
	this.workspace = $("<div></div>").addClass("designerLabelControl").attr("title", "Label Size");
	
	this.widthContainer = $("<div>Width: </div>").addClass("designerLabelControlContainer").appendTo(this.workspace);
	this.widthController = $("<input class=\"clsWidth\" type=\"text\" />")
		.addClass("designerLabelControlElement")
		.css({
			width : "50px",
		    height: '20px'	
		})
		.val(this.designer.labelWidth / this.designer.dpi)
		.appendTo(this.widthContainer)
		.on("blur", function() {
				self.updateDesigner();
		})
		.on("keypress", function(e) {
			if (e.which == 13) {
				e.preventDefault();
				self.updateDesigner();
			}
		});
		
	this.heightContainer = $("<div>Height: </div>").addClass("designerLabelControlContainer").appendTo(this.workspace);
	this.heightController = $("<input class=\"clsHeight\" type=\"text\" />")
		.addClass("designerLabelControlElement")
		.css({
			width : "50px",
			height: '20px'
		})
		.val(this.designer.labelHeight / this.designer.dpi)
		.appendTo(this.heightContainer)
		.on("blur", function() {
			
				self.updateDesigner();
		})
		.on("keypress", function(e) {
			if (e.which == 13) {
				e.preventDefault();
				self.updateDesigner();
			}
		});
		
	this.dpiContainer = $("<div>DPI: </div>").addClass("designerLabelControlContainer").appendTo(this.workspace);
	this.dpiController = $("<select class=\"dpiCls\"><option  value=\"203\">203</option><option value=\"305\">305</option></select>")
		.addClass("designerLabelControlElement")
		.css({
			width : "50px"
		})
		.val(this.designer.dpi)
		.appendTo(this.dpiContainer)
		.on("blur", function() {
			
				//self.updateDesigner();
		})
		.on("keypress", function(e) {
			if (e.which == 13) {
				e.preventDefault();
				//self.updateDesigner();
			}
		});
		
		this.orientationContainer = $("<div>Orientation: </div>").addClass("designerLabelControlContainer").appendTo(this.workspace);
		this.orientationController = $("<select class=\"orientationCls\"><option  value=\"portrait\">Portrait</option><option value=\"landscape\">Landscape</option></select>")
		.addClass("designerLabelControlElement")
		.css({
			width : "100px"
		})
		.val(this.designer.orientation)
		.appendTo(this.orientationContainer)
		.on("change", function() {
			
				//self.updateDesigner();
		})
		.on("keypress", function(e) {
			if (e.which == 13) {
				e.preventDefault();
				//self.updateDesigner();
			}
		});

	
	this.ZPLlabelContainer = $("<div>Labels: </div>").addClass("designerLabelControlContainer").appendTo(this.workspace);
	this.ZPLlabelController = $("<select class=\"ZPLlblCls\"></select>")
		.addClass("designerLabelControlElement")
		.css({
		    width: "100px"
		})
		.val(this.designer.ZPLLabel)
		.appendTo(this.ZPLlabelContainer)
		.on("change", function () {

		    //self.updateDesigner();
		})
		.on("keypress", function (e) {
		    if (e.which == 13) {
		        e.preventDefault();
		        //self.updateDesigner();
		    }
		});
		
	this.updateDesigner = function() {
		var dpi = this.designer.dpi;
		
		if (!isNaN(this.dpiController.val())) dpi = this.dpiController.val();
		this.designer.dpi = dpi;
		
		var width = this.designer.labelWidth / this.designer.dpi;
		var height = this.designer.labelHeight / this.designer.dpi;
		
		if (!isNaN(this.widthController.val())) width = this.widthController.val();
		if (!isNaN(this.heightController.val())) height = this.heightController.val();
		
		this.designer.updateLabelSize(width, height);
		this.widthController.val(width);
		this.heightController.val(height);
	}
		
	this.update = function() {
		this.widthController.val(this.designer.labelWidth / this.designer.dpi);
		this.heightController.val(this.designer.labelHeight / this.designer.dpi);
	}
}