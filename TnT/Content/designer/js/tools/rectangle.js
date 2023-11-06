if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};
if (!com.logicpartners.designerTools)
    com.logicpartners.designerTools = {};

com.logicpartners.designerTools.rectangle = function () {
    var self = this;
    var stored_fields = ['x', 'y', 'type', 'name', 'width', 'height'];

    this.counter = 1;

    this.button = $("<div></div>").addClass("designerToolbarRectangle designerToolbarButton").attr("title", "Rectangle").append($("<div></div>"));
    this.object = function (x, y, width, height) {
        this.name = "Rectangle " + self.counter++;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.type = 'rectangle';

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

            if (PaperOrientation == "portrait") {
                paperWidth = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsHeight").val()) * parseInt(selDPI);


                if (this.width > this.height) {
                    return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^GB" + this.width * actualDPI + ",0," + this.height * actualDPI + "^FS";
                }
                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^GB" + "0," + this.height * actualDPI + "," + this.width * actualDPI + "^FS";
            } else if (PaperOrientation == 'landscape') {
                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                if (this.width > this.height) {
                    return "^FO" + (paperWidth - ((this.y - labely) * actualDPI) - this.height * actualDPI) + "," +
                        (this.x - labelx) * actualDPI + "^GB" + "0," + this.width * actualDPI + "," + this.height * actualDPI + "^FS";
                }
                else if (this.width < this.height) {
                    return "^FO" + (paperWidth - ((this.y - labely) * actualDPI) - this.height * actualDPI) + "," + (this.x - labelx) * actualDPI + "," + "^GB" + this.height * actualDPI + ",0," + this.width * actualDPI + "^FS";
                }
                //return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^GB" + "0," + //this.height*actualDPI + "," + this.width*actualDPI + "^FS";
            }
        }

        this.toZPLPrint = function (labelx, labely, labelwidth, labelheight) {
            var PaperOrientation = $(".orientationCls").val();
            var selDPI = $(".dpiCls").val();
            var actualDPI = Math.round(parseInt(selDPI) / 100);

            if (PaperOrientation == "portrait") {
                paperWidth = parseInt($(".clsWidth").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsHeight").val()) * parseInt(selDPI);

                if (this.width > this.height) {
                    return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^GB" + this.width * actualDPI + ",0," + this.height * actualDPI + "^FS";
                }

                return "^FO" + (this.x - labelx) * actualDPI + "," + (this.y - labely) * actualDPI + "^GB" + "0," + this.height * actualDPI + "," + this.width * actualDPI + "^FS";

            } else if (PaperOrientation == 'landscape') {
                paperWidth = parseInt($(".clsHeight").val()) * parseInt(selDPI);
                paperHeight = parseInt($(".clsWidth").val()) * parseInt(selDPI);

                if (this.width > this.height) {
                    return "^FO" + (paperWidth - ((this.y - labely) * actualDPI) - this.height * actualDPI) + "," +
                        (this.x - labelx) * actualDPI + "^GB" + "0," + this.width * actualDPI + "," + this.height * actualDPI + "^FS";
                }
                else if (this.width < this.height) {
                    return "^FO" + (paperWidth - ((this.y - labely) * actualDPI) - this.height * actualDPI) + "," + (this.x - labelx) * actualDPI + "," + "^GB" + this.height * actualDPI + ",0," + this.width * actualDPI + "^FS";
                }
                //return "^FO" + (this.x - labelx)*actualDPI + "," + (this.y - labely)*actualDPI + "^GB" + "0," + //this.height*actualDPI + "," + this.width*actualDPI + "^FS";
            }
        }

        this.draw = function (context) {
            context.fillRect(this.x, this.y, this.width, this.height);
        }

        this.setWidth = function (width) {
            this.width = parseInt(width);
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
            context.dashedStroke(parseInt(this.x + 1), parseInt(this.y + 1), parseInt(this.x) + parseInt(this.width) - 1, parseInt(this.y) + parseInt(this.height) - 1, [2, 2]);
        }

        this.hitTest = function (coords) {
            return (coords.x >= parseInt(this.x) && coords.x <= parseInt(this.x) + parseInt(this.width) && coords.y >= parseInt(this.y) && coords.y <= parseInt(this.y) + parseInt(this.height));
        }
    }

    com.logicpartners.designerTools.rectangle.load = function (store) {
        var text;

        text = new self.object(store.x, store.y, store.width, store.height);
        for (var i = 0; i < stored_fields.length; i++)
            text[stored_fields[i]] = store[stored_fields[i]];

        return text;
    }
}