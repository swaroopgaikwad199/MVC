if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};
if (!com.logicpartners.designerTools)
    com.logicpartners.designerTools = {};

com.logicpartners.designerTools.barcode = function () {
    var self = this;
    var stored_fields = ['x', 'y', 'type', 'name', 'text', 'barcodeType', 'width', 'height', 'data', 'mil'];

    this.data = null;
    self.type = null;
    this.counter = 1;
    this.button = $("<div></div>").addClass("designerToolbarBarcode designerToolbarButton").attr("title", "Barcode").append($("<div></div>"));
    self.data = null;
    this.object = function (x, y, width, height) {
        var width = 100;
        var canvasHolder = $("<canvas></canvas>").prop("width", "100").prop("height", "1");
        this.type = "Barcode";
        this.name = "Barcode " + self.counter++;
        this.text = "BARCODE";
        this.barcodeType = 'Product_Barcode'; // Used to store the type of barcode that this is.
        this.x = x;
        this.y = y;

        if (Number.isNaN(this.x)) {
            this.x = "0";
        }
        if (Number.isNaN(this.y)) {
            this.y = "0";
        }
        //this.width = width;
        this.height = height;
        this.data = "123456798";

        this.hidden = ["barcodeType"];
        this.mil = 5;
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

            if (PaperOrientation == "portrait") {
                paperWidth = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsHeight").val()) * parseInt(selDPI);

                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^BY" + this.mil + "^BCN," + this.height * actualDPI + ",N,N,N,D^FD{{" + this.barcodeType.toUpperCase() + " || " + this.data + "}}^FS";

            } else if (PaperOrientation == 'landscape') {
                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);

                return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) - this.height * actualDPI) + "," + (this.x - labelx) * actualDPI + "^BY" + this.mil + "^BCR," + this.height * actualDPI + ",N,N,N,D^FD{{" + this.barcodeType.toUpperCase() + " || " + this.data + "}}^FS";
            }


            //return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^BY3^BCN," + this.height*actualDPI + ",N,N,N^FD" + this.text + "^FS";
        }

        this.draw = function (context) {

            canvasHolder.JsBarcode(this.text, { width: 1, height: 1 });
            var cwidth = canvasHolder[0].width;
            var cheight = canvasHolder[0].height;
            var ctx = canvasHolder[0].getContext('2d');
            width = cwidth;

            var cData = ctx.getImageData(0, 0, cwidth, cheight);

            for (var i = 0; i < cwidth; i++) {
                if (cData.data[i * 4 + 3] == 255) { // Black (barcode = black or white)
                    // Draw a black rectangle at this point.
                    context.fillRect(this.x + i, this.y, 1, this.height);
                }
            }
        }

        this.setWidth = function (width) {
            //this.width = width;
        }

        this.getWidth = function () {
            return width;
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
            debugger


            context.dashedStroke(parseInt(this.x) + 1, parseInt(this.y) + 1, parseInt(this.x) + parseInt(width) - 1, parseInt(this.y) + parseInt(this.height) - 1, [2, 2]);
        }

        this.hitTest = function (coords) {
            return (coords.x >= parseInt(this.x) && coords.x <= parseInt(this.x) + parseInt(width) && coords.y >= parseInt(this.y) && coords.y <= parseInt(this.y) + parseInt(this.height));
        }
    }

    com.logicpartners.designerTools.barcode.load = function (store) {
        var text;

        text = new self.object(store.x, store.y, store.width, store.height);
        for (var i = 0; i < stored_fields.length; i++)
            text[stored_fields[i]] = store[stored_fields[i]];

        return text;
    }
};