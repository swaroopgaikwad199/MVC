if (!com)
	var com = {};
if (!com.logicpartners)
	com.logicpartners = {};
if (!com.logicpartners.designerTools)
	com.logicpartners.designerTools = {};

com.logicpartners.designerTools.image = function() {
	var self = this;
 var stored_fields = ['x', 'y', 'type', 'name', 'data', 'rotated', 'timestamp', 'width', 'height'];
 
	this.counter = 1;
	this.data = null;
	this.width = null;
	this.height = null;
	this.orientation = null;
	this.timestamp = null;
 this.rotated = null; // Stores the 90 deg rotated image. Will be used when generating landscape ZPL.
 
	var tmpimgData = "";

	this.button = $("<div></div>").addClass("designerToolbarImage designerToolbarButton").attr("title", "Image").append($("<div></div>"));
	this.activate = function(toolbar) {
		self.data = null;

		// Open up a dialog to get the image
		var dialog = $("<div></div>").prop("title", "Add Image");
		var imageFile = $("<input type=\"file\" />").css({ width : 400 })

		.on("change", function() {
			if (typeof window.FileReader !== 'function') {
				alert('This page requires the file API that is included in modern browsers such as Google Chrome. Please try again in an up to date web browser.');
			}
			timestamp = new Date().getUTCMilliseconds();
			this.timestamp = timestamp;

			var input = imageFile[0];
			if (!input.files[0]) {
				alert('Please select a file to insert.');
			}
			else {
				var file = input.files[0];
				var reader = new FileReader();
				var insertImg = imageLeft;
				var canvasResult = imageRight;
				reader.onloadend = function() {
					var canvas = canvasResult;
					var imgSelf = insertImg;
					insertImg.css( { "width" : "auto", "height" : "auto", "max-width" : 200, "max-height" : 200 });
					canvas.css( { "width" : "auto", "height" : "auto" });
					insertImg[0].onload = function() {

						var tCanvas = $("<canvas />");

						var elementID = 'canvas_' + timestamp;
						$('<canvas>').attr({id: elementID }).css({
						    width: '100 px',
						    height: '100 px',
						}).appendTo('#canvasArea');

						$("#"+elementID).hide();

						var tmpCanvas = document.getElementById(elementID);

						tmpCanvas.width = imgSelf[0].height;
						tmpCanvas.height = imgSelf[0].width;

						var tmpCtx = tmpCanvas.getContext("2d");


						//tmpCtx.translate(tmpCanvas.height / 2, tmpCanvas.width / 2);
						tmpCtx.rotate(Math.PI / 2);
						tmpCtx.drawImage(imgSelf[0], 0, -(imgSelf[0].height), imgSelf[0].width, imgSelf[0].height);


						tCanvas[0].width = imgSelf[0].width;
						tCanvas[0].height = imgSelf[0].height;
						canvas[0].width = imgSelf[0].width;
						canvas[0].height = imgSelf[0].height;
						var tctx = tCanvas[0].getContext("2d");
						var ctx = canvas[0].getContext("2d");

						tctx.drawImage(imgSelf[0], 0, 0, tCanvas[0].width, tCanvas[0].height);


						var tImgData = tctx.getImageData(0, 0, tCanvas[0].width, tCanvas[0].height);
						var imgData = ctx.getImageData(0, 0, tCanvas[0].width, tCanvas[0].height);

						var tmptImgData = tmpCtx.getImageData(0, 0, tmpCanvas.width, tmpCanvas.height);
						tmpimgData = tmpCtx.getImageData(0, 0, tmpCanvas.width, tmpCanvas.height);

						// Convert the canvas data to GRF.
						for (var y = 0; y < tCanvas[0].height; y++) {
							for (x = 0; x < tCanvas[0].width; x++) {
								var pixelStart = 4 * (tCanvas[0].width * y + x);
								var luminance = tImgData.data[pixelStart] * 0.299 + tImgData.data[pixelStart + 1] * 0.587 + tImgData.data[pixelStart + 2] * 0.114;

								if (luminance > 127) {
									imgData.data[pixelStart] = 255;
									imgData.data[pixelStart + 1] = 255;
									imgData.data[pixelStart + 2] = 255;
									imgData.data[pixelStart + 3] = 255;
								}
								else {
									imgData.data[pixelStart] = 0;
									imgData.data[pixelStart + 1] = 0;
									imgData.data[pixelStart + 2] = 0;
									imgData.data[pixelStart + 3] = 255;
								}
							}
						}

						// Convert the canvas data to GRF.
						for (var y = 0; y < tmpCanvas.height; y++) {
							for (x = 0; x < tmpCanvas.width; x++) {
								var pixelStart = 4 * (tmpCanvas.width * y + x);
								var luminance = tmptImgData.data[pixelStart] * 0.299 + tmptImgData.data[pixelStart + 1] * 0.587 + tmptImgData.data[pixelStart + 2] * 0.114;

								if (luminance > 127) {
									tmpimgData.data[pixelStart] = 255;
									tmpimgData.data[pixelStart + 1] = 255;
									tmpimgData.data[pixelStart + 2] = 255;
									tmpimgData.data[pixelStart + 3] = 255;
								}
								else {
									tmpimgData.data[pixelStart] = 0;
									tmpimgData.data[pixelStart + 1] = 0;
									tmpimgData.data[pixelStart + 2] = 0;
									tmpimgData.data[pixelStart + 3] = 255;
								}
							}
						}
						self.width = canvas[0].width;
						self.height = canvas[0].height;
						self.data = imgData.data;
      self.rotated =  tmpimgData.data;

						ctx.putImageData(imgData, 0, 0);
					}
					insertImg[0].src = reader.result;
				}
				reader.readAsDataURL(file);
			}
		}).appendTo(dialog);
		var imageContainer = $("<div></div>").css({ "padding-top" : "5px" });
		var imageLeft = $("<img />").prop("src", "../Content/designer/images/blank.gif").prop("border", "none").css({ float: "left", width: 200, height: 200, border: "1px solid #DDDDDD" }).appendTo(imageContainer);
		var imageRight = $("<canvas />").css({ float: "right", width: 200, height: 200, border: "1px solid #DDDDDD"}).appendTo(imageContainer);

		imageContainer.appendTo(dialog);

		var Toolbar = toolbar;
		dialog.dialog({
			modal : true,
			width : 470,
			height : 400,
			buttons : {
				"Insert" : function() {
					// Insert the image onto the screen.
					Toolbar.labelDesigner.addObject(new self.object(0, 0, self.width, self.height, self.data, self.rotated, timestamp));

					$(this).dialog("close");
				},
				"Cancel" : function() {
					self.data = null;
					$(this).dialog("close");
				}
			}
		})
		.on("dialogclose", { toolbar : toolbar }, function(event) {
			self.button.removeClass("designerToolbarButtonActive");
			event.data.toolbar.setTool(null);
		});
	};

	this.object =  function(x, y, width, height, data, rotated, timestamp) {

		this.name = "Image " + self.counter++;
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.data = data;
  this.rotated = rotated;
  this.type = 'image';
  
		this.readonly = [ "width", "height", "data" ];
		this.hidden = [ "data", "uniqueID" ];
  
  this.save = function()
  {
    var store;
    
    store = {};
    for(var i = 0; i < stored_fields.length; i++)
      if(['data','rotated'].indexOf(stored_fields[i]) != -1)
        store[stored_fields[i]] = this[stored_fields[i]].toString();
      else
        store[stored_fields[i]] = this[stored_fields[i]];
    
    return store;
  }
  
		this.getZPLData = function() {
			var GRFVal = function(nibble) {
				var nibbleMap = {
					"0" : "0000",
					"1" : "0001",
					"2" : "0010",
					"3" : "0011",
					"4" : "0100",
					"5" : "0101",
					"6" : "0110",
					"7" : "0111",
					"8" : "1000",
					"9" : "1001",
					"A" : "1010",
					"B" : "1011",
					"C" : "1100",
					"D" : "1101",
					"E" : "1110",
					"F" : "1111",
				};

				for (key in nibbleMap) {
					if (nibbleMap[key] == nibble) {
						return key;
					}
				}

				return "";
			}
			debugger;
			var imgData = "";
			var PageOrientation = $(".orientationCls").val();

			if(PageOrientation == 'landscape'){

							var bytesPerLine = Math.ceil(this.height / 8);
							//console.log(bytesPerLine);
							//console.log(this.width);
							//console.log(bytesPerLine);
							for (var y = 0; y < this.width; y++) {
								var nibble = "";
								var bytes = 0;
								for (var x = 0; x < this.height; x++) {
									var point = 4 * (this.height * y + x);
									if (this.rotated[point+1] == 0) {
										nibble += "1";
									}
									else nibble += "0";

									if (nibble.length > 7) {
										imgData += GRFVal(nibble.substring(0, 4)) + GRFVal(nibble.substring(4, 8));
										nibble = "";
										bytes++;
									}
								}

								if (nibble.length > 0) {
									while (nibble.length < 8) nibble += "0";
									imgData += GRFVal(nibble.substring(0, 4)) + GRFVal(nibble.substring(4, 8));
									nibble = "";
									bytes++;
								}

								while (bytes < bytesPerLine) {
									imgData += GRFVal("0000") + GRFVal("0000");
									bytes++;
								}

								imgData += "\n";
							}

							return "~DGIMG" + timestamp + "," + bytesPerLine * width + "," + bytesPerLine + "," + imgData;
		}else {

							var bytesPerLine = Math.ceil(this.width / 8);
							//console.log(bytesPerLine);
							//console.log(this.width);
							//console.log(bytesPerLine);
							for (var y = 0; y < this.height; y++) {
								var nibble = "";
								var bytes = 0;
								for (var x = 0; x < this.width; x++) {
									var point = 4 * (this.width * y + x);
									if (this.data[point+1] == 0) {
										nibble += "1";
									}
									else nibble += "0";

									if (nibble.length > 7) {
										imgData += GRFVal(nibble.substring(0, 4)) + GRFVal(nibble.substring(4, 8));
										nibble = "";
										bytes++;
									}
								}

								if (nibble.length > 0) {
									while (nibble.length < 8) nibble += "0";
									imgData += GRFVal(nibble.substring(0, 4)) + GRFVal(nibble.substring(4, 8));
									nibble = "";
									bytes++;
								}

								while (bytes < bytesPerLine) {
									imgData += GRFVal("0000") + GRFVal("0000");
									bytes++;
								}

								imgData += "\n";
							}

							return "~DGIMG" + timestamp + "," + bytesPerLine * height + "," + bytesPerLine + "," + imgData;
		}


		},



		this.toZPL = function(labelx, labely, labelwidth, labelheight) {
			var selDPI = $(".dpiCls").val();
			var actualDPI = Math.round(parseInt(selDPI)/100);
			var PaperOrientation = $(".orientationCls").val();
			debugger;
			if(PaperOrientation == "portrait"){
					return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^XGR:IMG" + timestamp + ",1,1^FS";
			}else if(PaperOrientation == 'landscape'){
			    paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
			    paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);

				return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) -height) + "," + (this.x - labelx) * actualDPI + "^XGR:IMG" + timestamp + ",1,1^FS";
			}


		},

		this.draw = function(context, width, height) {
			var ctxData = context.getImageData(0, 0, width, height);
			for (var y = 0; y < this.height; y++) {
				for (var x = 0; x < this.width; x++) {
					if (this.x + x >= 0 && this.x + x < width
						&& this.y + y >= 0 && this.y + y < height) {
						var drawPoint = 4 * (width * (this.y + y) + this.x + x);
						var drawFromPoint = 4 * (this.width * y + x);
						ctxData.data[drawPoint] = this.data[drawFromPoint];
						ctxData.data[drawPoint + 1] = this.data[drawFromPoint + 1];
						ctxData.data[drawPoint + 2] = this.data[drawFromPoint + 2];
						ctxData.data[drawPoint + 3] = this.data[drawFromPoint + 3];
					}
				}
			}

			context.putImageData(ctxData, 0, 0);
		}

		this.setWidth = function(width) {
			//this.width = width;
		}

		this.getWidth = function() {
			return this.width;
		}

		this.setHeight = function(height) {
			//height = height;
		}

		this.getHeight = function() {
			return this.height;
		}

		this.setHandle = function(coords) {
			this.handle = this.resizeZone(coords);
		}

		this.getHandle = function() {
			return this.handle;
		}

		this.drawActive = function(context) {
			context.dashedStroke(parseInt(this.x + 1), parseInt(this.y + 1), parseInt(this.x) + parseInt(this.width) - 1, parseInt(this.y) + parseInt(this.height) - 1, [2, 2]);
		}

		this.hitTest = function(coords) {
			return (coords.x >= parseInt(this.x) && coords.x <= parseInt(this.x) + parseInt(this.width) && coords.y >= parseInt(this.y) && coords.y <= parseInt(this.y) + parseInt(this.height));
		}
	}
 
 com.logicpartners.designerTools.image.load = function(store)
 {
   var text, chars;
   
   text = new self.object(store.x, store.y, store.width, store.height);
   for(var i = 0; i < stored_fields.length; i++)
     if(['data','rotated'].indexOf(stored_fields[i]) != -1)
     { 
       chars = store[stored_fields[i]].split(',');
       for(var j = 0; j < chars.length; j++)
         chars[j] *= 1;
        
       text[stored_fields[i]] = new Uint8ClampedArray(chars);
     }
     else
       text[stored_fields[i]] = store[stored_fields[i]];

   return text;
 }
}
