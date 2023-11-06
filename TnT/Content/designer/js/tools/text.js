if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};
if (!com.logicpartners.designerTools)
    com.logicpartners.designerTools = {};

com.logicpartners.designerTools.text = function () {
    var self = this;
    var stored_fields = ['x', 'y', 'type', 'name', 'text', 'textType', 'fontSize', 'fontType', 'width', 'height', 'lineSpacing'];

    this.counter = 1;
    this.button = $("<div></div>").addClass("designerToolbarText designerToolbarButton").attr("title", "Text").append($("<div></div>"));
    this.object = function (x, y, width, height) {
        this.type = "label_text";
        this.name = "Textbox " + self.counter++;
        this.textType = "LabelText"; // Used to store the type of text that this is.
        this.text = this.name;
        this.x = x;
        this.y = y;
        this.fontSize = 14;
        this.fontType = "Arial";
        this.width = 100;
        this.height = 0;
        this.lineSpacing = 24;

        this.readonly = ["height"];
        this.hidden = ["textType"];

        this.getFontHeight = function () {
            var textMeasure = $("<div></div>").css({
                "font-size": this.fontSize + "px",
                "font-family": this.fontType,
                "opacity": 0,
            }).text("M").appendTo($("body"));

            var height = textMeasure.outerHeight();
            textMeasure.remove();
            return height;
        }

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
            var PaperOrientation = $(".orientationCls").val();
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            var txtData = "";

            if (this.textType == "LabelText") {
                txtData = this.textType + "||" + this.text;
            } else if (this.textType == "EXTRA_EXPIRY") {
                txtData = this.textType + "||" + this.text;
            } else {
                txtData = this.textType;
            }
            var fontSizeForDPI = 2.16;
            if (selDPI == "305") {
                fontSizeForDPI = fontSizeForDPI + 2.5;
            }

            fontWidth = Math.round(this.fontSize * fontSizeForDPI).toFixed();
            fontHeight = Math.round(this.fontSize * fontSizeForDPI).toFixed();

            fontWidth = Math.round(this.fontSize * 4.16).toFixed();
            fontHeight = Math.round(this.fontSize * 4.06).toFixed();

            if (PaperOrientation == "portrait") {
                paperWidth = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsHeight").val()) * parseInt(selDPI);

                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^A0N," + fontWidth + "," + fontHeight + "^FH^FD{{" + txtData + "}}^FS";
             } else if (PaperOrientation == 'landscape') {
                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) - this.height * actualDPI) + "," + (this.x - labelx) * actualDPI + "^A0R," + fontWidth + "," + fontHeight + "^FH^FD{{" + txtData + "}}^FS";
            }
        }

        this.toZPLPrint = function (labelx, labely, labelwidth, labelheight) {
            var PaperOrientation = $(".orientationCls").val();
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);
            var txtData = "";

            if (this.textType == "LabelText") {
                txtData = this.textType + "||" + this.text;
            } else if (this.textType == "EXTRA_EXPIRY") {
                txtData = this.textType + "||" + this.text;
            } else {
                txtData = this.textType;
            }

            var fontSizeForDPI = 2.16;
            if (selDPI == "305") {
                fontSizeForDPI = fontSizeForDPI + 2.5;
            }
            fontWidth = Math.round(this.fontSize * fontSizeForDPI).toFixed();
            fontHeight = Math.round(this.fontSize * fontSizeForDPI).toFixed();

            fontWidth = Math.round(this.fontSize * 4.16).toFixed();
            fontHeight = Math.round(this.fontSize * 4.06).toFixed();

            if (PaperOrientation == "portrait") {
                paperWidth = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsHeight").val()) * parseInt(selDPI);

                //return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^A0N," + fontWidth + "," + fontHeight + "^FH^FB" + this.width * actualDPI + ",10," + this.lineSpacing + "^FD" + this.text + "^FS";
                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^A0N," + fontWidth + "," + fontHeight + "^FH^FD" + this.text + "^FS";

            } else if (PaperOrientation == 'landscape') {
                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                return "^FO" + ((paperWidth - (this.y - labely) * actualDPI) - this.height * actualDPI) + "," + (this.x - labelx) * actualDPI + "^A0R," + fontWidth + "," + fontHeight + "^FH^FD" + this.text + "^FS";
            }
        }

        this.draw = function (context) {


            context.font = this.fontSize + 1 + "px " + this.fontType;
            var oColor = context.fillStyle;
            context.fillStyle = "white";
            this.height = this.getFontHeight();
            var measuredText = context.measureText(this.text);
            //this.width = measuredText.width;
            context.globalCompositeOperation = "difference";
            //context.fillText(this.text, this.x, this.y + (this.height * 0.75));
            context.globalCompositeOperation = "source-over";
            context.fillStyle = oColor;

            var maxWidth = this.width;
            var lineHeight = this.lineSpacing;

            if (lineHeight === undefined) {
                lineHeight = 24;
            }

            //var x1 = this.x;
            //var y1 = this.y;

            var yPara = parseInt(this.y) + (parseInt(this.height) * 0.75);

            this.wrapText(context, this.text, this.x, yPara, maxWidth, lineHeight);
        }

        this.wrapText = function (context, text, x, y, maxWidth, lineHeight) {

            var words = text.split(' ');
            var line = '';
            var k = 0;
            for (var n = 0; n < words.length; n++) {
                var testLine = line + words[n] + ' ';
                var metrics = context.measureText(testLine);
                var testWidth = metrics.width;
                if (testWidth > maxWidth && n > 0) {
                    context.fillText(line, x, y);
                    line = words[n] + ' ';
                    y += lineHeight;
                    k = parseInt(k) + parseInt(lineHeight);
                }
                else {
                    line = testLine;

                }
            }
            console.log(y + ' - ' + k);
            if (k != 0) {
                this.height = parseInt(y) - 24;
            }
            context.fillText(line, x, y);
        }
        this.setWidth = function (width) {
            //this.width = width;
        }

        this.getWidth = function () {
            return this.width;
        }

        this.setHeight = function (height) {
            //height = height;
        }

        this.getHeight = function () {
            return this.height * 0.75;
        }

        this.setHandle = function (coords) {
            this.handle = this.resizeZone(coords);
        }

        this.getHandle = function () {
            return this.handle;
        }

        this.drawActive = function (context) {
            context.dashedStroke(parseInt(this.x + 1), parseInt(this.y + 1), parseInt(this.x) + parseInt(this.width) - 1, parseInt(this.y) + parseInt(this.height * 0.9) - 1, [2, 2]);
        }

        this.hitTest = function (coords) {
            return (coords.x >= parseInt(this.x) && coords.x <= parseInt(this.x) + parseInt(this.width) && coords.y >= parseInt(this.y) && coords.y <= parseInt(this.y) + parseInt(this.height) * 0.75);
        }
    }

    com.logicpartners.designerTools.text.load = function (store) {
        var text;

        text = new self.object(store.x, store.y, store.width, store.height);
        for (var i = 0; i < stored_fields.length; i++)
            text[stored_fields[i]] = store[stored_fields[i]];

        return text;
    }
}