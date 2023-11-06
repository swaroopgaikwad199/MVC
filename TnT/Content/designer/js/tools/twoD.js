if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};
if (!com.logicpartners.designerTools)
    com.logicpartners.designerTools = {};

com.logicpartners.designerTools.twoD = function () {
    var self = this;
    var stored_fields = ['x', 'y', 'type', 'name', 'matrixType', 'text', 'width', 'height', 'mil'];

    this.counter = 1;
    this.button = $("<div></div>").addClass("designerToolbar2D designerToolbarButton").attr("title", "GS1 DataMatrix").append($("<div></div>"));
    this.object = function (x, y, width, height) {
        var width = 26;
        var canvasHolder = $("<canvas></canvas>").prop("width", width).prop("height", width);
        this.type = "twoD";
        this.matrixType = 'GS1_DataMatrix';  // Used to store the type of matrix that this is.
        this.name = "GS1 DataMatrix " + self.counter++;
        //this.text = "(01)03453120000011(17)120508(10)ABCD1234(410)9501101020917";
        this.text = "01-17-10-21";
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        this.hidden = ["matrixType"];
        this.mil = 10;
        this.save = function () {
            var store;

            store = {};
            for (var i = 0; i < stored_fields.length; i++)
                store[stored_fields[i]] = this[stored_fields[i]];

            return store;
        }

        this.getZPLData = function () {
            return "";
        }

        this.toZPL = function (labelx, labely, labelwidth, labelheight) {
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            var PaperOrientation = $(".orientationCls").val();
            var GSS = "";
            console.log(this.matrixType);
            if (this.matrixType == "GS1_DataMatrix") {
                GSS = "\\";
            } else if (this.matrixType == "Non_GS1") {
                GSS = "";
            } else if (this.matrixType == "PPN_DataMatrix") {
                GSS = "";
            }
            if (PaperOrientation == "portrait") {

                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^BXN," + this.mil + ",200," + this.width + "," + this.height + ",,_^FH" + GSS + "^FD{{" + this.matrixType + " || " + this.text + "}}^FS";

                //return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^BXN,5,200," + this.width*actualDPI + "," + this.height*actualDPI + ",,_^FH"+GSS+"^FD{{" + this.matrixType + " || " + this.text +"}}^FS";
            } else if (PaperOrientation == 'landscape') {

                var DotsPerMM = parseInt($(".clsHeight").val()) * parseInt(actualDPI);
                var DMHeight = DotsPerMM * 5 * 2;

                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);

                return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) - this.height * actualDPI) + "," + ((this.x - labelx) * actualDPI) + "^BXR," + this.mil + ",200," + this.width + "," + this.height + ",,_^FH" + GSS + "^FD{{" + this.matrixType + " || " + this.text + "}}^FS";
            }
        }

        this.toZPLPrint = function (labelx, labely, labelwidth, labelheight) {
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            var PaperOrientation = $(".orientationCls").val();
            var GSS = "";
            console.log(this.matrixType);
            if (this.matrixType == "GS1_DataMatrix") {
                GSS = "\\";
            } else if (this.matrixType == "Non_GS1") {
                GSS = "";
            } else if (this.matrixType == "PPN_DataMatrix") {
                GSS = "";
            }
            if (PaperOrientation == "portrait") {

                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^BXN," + this.mil + ",200," + this.width + "," + this.height + ",,_^FH" + GSS + "^FD" + this.text + "^FS";

                //return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^BXN,5,200," + this.width*actualDPI + "," + this.height*actualDPI + ",,_^FH"+GSS+"^FD{{" + this.matrixType + " || " + this.text +"}}^FS";
            } else if (PaperOrientation == 'landscape') {

                var DotsPerMM = parseInt($(".clsHeight").val()) * parseInt(actualDPI);
                var DMHeight = DotsPerMM * 5 * 2;

                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);

                return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) - this.height * actualDPI) + "," + ((this.x - labelx) * actualDPI) + "^BXR," + this.mil + ",200," + this.width + "," + this.height + ",,_^FH" + GSS + "^FD" + this.text + "^FS";
            }
        }

        this.draw = function (context) {
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            var cwidth = canvasHolder[0].width;
            var cheight = canvasHolder[0].height;
            var ctx = $("canvas")[0].getContext("2d")

            img = new Image();
            img.src = "../Content/designer/images/sample2D.png";

            if (img.complete) // image data has already been loaded before
            {
                if (Number.isNaN(this.x)) {
                    this.x = "0";
                }
                if (Number.isNaN(this.y)) {
                    this.y = "0";
                }
                context.drawImage(img, this.x, this.y, this.width * 1.76, this.height * 1.76);
            }
            else // image must be allowed to load asynchronously
            {
                var parent;

                parent = this;
                img.onload = function () {
                    if (Number.isNaN(parent.x)) {
                        this.x = "0";
                    }
                    if (Number.isNaN(parent.y)) {
                        this.y = "0";
                    }
                    context.drawImage(img, parent.x, parent.y, parent.width * 1.76, parent.height * 1.76);
                };
            }

            //var cData = context.getImageData(0, 0, cwidth, cheight);

            //	for (var i = 0; i < cwidth; i++) {
            //if (cData.data[i * 4 + 3] == 255) { // Black (barcode = black or white)
            // Draw a black rectangle at this point.
            //		context.drawImage(img, this.x, this.y, this.width, this.height);
            //}
            //}
        }

        this.setWidth = function (width) {
            this.width = width;
        }

        this.getWidth = function () {
            return this.width;
        }

        this.setHeight = function (height) {
            this.height = height;
        }

        this.getHeight = function () {
            return this.height;
        }

        this.setHandle = function (coords) {
            this.handle = this.resizeZone(coords);
        }

        this.getHandle = function () {
            return this.handle;
        }

        this.drawActive = function (context) {
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);

            if (Number.isNaN(this.x)) {
                this.x = "0";
            }
            if (Number.isNaN(this.y)) {
                this.y = "0";
            }
            context.dashedStroke(parseInt(this.x) + 1, parseInt(this.y) + 1, (parseInt(this.x) + parseInt(this.width * 1.76) - 1), (parseInt(this.y) + parseInt(this.height * 1.76) - 1), [2, 2]);
        }

        this.hitTest = function (coords) {
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            if (Number.isNaN(this.x)) {
                this.x = "0";
            }
            if (Number.isNaN(this.y)) {
                this.y = "0";
            }
            return (coords.x >= parseInt(this.x) && coords.x <= parseInt(this.x) + parseInt(this.width * 1.76) && coords.y >= parseInt(this.y) && coords.y <= parseInt(this.y) + parseInt(this.height * 1.76));
        }
    }

    com.logicpartners.designerTools.twoD.load = function (store) {
        var text;

        text = new self.object(store.x, store.y, store.width, store.height);
        for (var i = 0; i < stored_fields.length; i++)
            text[stored_fields[i]] = store[stored_fields[i]];

        return text;
    }
};