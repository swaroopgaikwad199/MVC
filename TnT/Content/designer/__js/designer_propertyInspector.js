if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};

com.logicpartners.propertyInspector = function (designer, canvas) {
    this.canvas = canvas;
    this.canvasElement = $(canvas);
    this.labelDesigner = designer;
    this.activeElement = null;
    this.propertyNodes = {};
    this.boundingBox = null;
    var self = this;
    var typeBarcodeVar = "";
    var type2DVar = "";
    var lblInsWidth = $(".designerUtilityLabelInspector").width();

    // Create the property window.
    this.propertyInspector = $('<div></div>')
			.addClass("designerUtilityWindow")
			.css({
			    "left": "89%",
			    "top": this.canvas.getBoundingClientRect().top
			})
			//.draggable({handle: "div.designerPropertyTitle"})
			.insertAfter(this.canvasElement);

    this.updatePosition = function (xchange) {
        this.propertyInspector.css("left", parseInt(this.propertyInspector.css("left")) + xchange);
        this.boundingBox = this.propertyInspector[0].getBoundingClientRect();
    }


    this.propertyViewContainer = $('<div></div>')
			.addClass("designerPropertyContainer")
			/* .resizable({
				resize: function(event, ui) {
					ui.size.width = ui.originalSize.width;
				}
			}) */
			.appendTo(this.propertyInspector);

    this.titleBar = $('<div>Property Inspector</div>')
			.addClass("designerPropertyTitle")
			.prependTo(this.propertyInspector)
			.on("dblclick", function () {
			    self.propertyViewContainer.toggle();
			});

    this.propertyView = $('<div></div>')
			.addClass("designerPropertyContent")
			.appendTo(this.propertyViewContainer);

    this.update = function (activeElement) {
        var self = this;
        var getType = {};
        var keys = [];

        if (this.activeElement == activeElement) {

            for (var key in activeElement) {

                // skip keys that should be hidden
                if ($.inArray(key, activeElement.hidden) != -1)
                    continue;

                if (!activeElement.readonly || key != "readonly" && $.inArray(key, activeElement.readonly) == -1) {
                    if (getType.toString.call(activeElement[key]) != '[object Function]') {

                        if (key == 'type') {
                            if (activeElement[key] == 'Barcode') {
                                var barCodeType = this.propertyNodes[key].val();
                                this.propertyNodes[key].val(barCodeType);
                            } else if (activeElement[key] == 'twoD') {
                                var TwoDCodeType = this.propertyNodes[key].val();
                                this.propertyNodes[key].val(TwoDCodeType);
                            } else if (activeElement[key] == 'label_text') {
                                var labelTextType = this.propertyNodes[key].val();
                                this.propertyNodes[key].val(labelTextType);
                                this.propertyNodes[key].val(labelTextType).attr("selected", "selected");
                            }

                        } else {
                            this.propertyNodes[key].val(activeElement[key]);
                        }
                    }
                }
            }
        }
        else {
            this.activeElement = activeElement;
            this.propertyView.html('');

            for (var key in activeElement) {

                // skip keys that should be hidden
                if ($.inArray(key, activeElement.hidden) != -1)
                    continue;

                if (!keys[key]) {

                    keys[key] = true;

                    if (key != "readonly" && getType.toString.call(activeElement[key]) != '[object Function]') {

                        var elementKey = $('<div>' + key.charAt(0).toUpperCase() + key.substr(1) + '</div>')
								.css({
								    "width": "65px",
								    "height": "20px",
								    "border": "1px solid #AAAAAA",
								    "float": "left",
								    "font-size": "12px",
								    "line-height": "20px",
								    "border-right": "none",
								    "text-align": "right",
								    "padding-right": "5px",
								    "margin-left": "5px"
								});
                        if (key != 'type' && key != 'mil') {

                            var elementValue = $('<input type="text" name="' + key + '" value="' + activeElement[key] + '">')
                                    .css({
                                        "width": "120px",
                                        "float": "left",
                                        "height": "22px",
                                        "line-height": "20px",
                                        "padding-left": "5px"
                                    });

                        } else {
                            if (activeElement[key] == "Barcode") {

                                var codeTypes = "";
                                var options = [{ value: 'Product_Barcode', title: 'Product Barcode' }
                                             , { value: 'SSCC_Barcode', title: 'SSCC Barcode' }
                                             , { value: 'UID_BARCODE', title: 'UID Barcode' }
                                             , { value: 'Secondary_Barcode', title: 'Secondary Barcode' }
                                             , { value: 'Serialized_Barcode', title: 'Serialized Barcode' }
                                             , { value: 'NonSerialized_Barcode', title: 'NonSerialized Barcode' }
                                             , { value: 'PPN_Barcode', title: 'PPN Barcode' }
                                             , { value: 'PPN_BARCODE2', title: 'PPN BARCODE2' }
                                             , { value: 'NDC_BARCODE', title: 'NDC BARCODE' }
                                ];

                                for (var i = 0; i < options.length; i++)
                                    if (options[i].value == activeElement['barcodeType'])
                                        codeTypes += '<option value="' + options[i].value + '" selected>' + options[i].title + '</option>';
                                    else
                                        codeTypes += '<option value="' + options[i].value + '">' + options[i].title + '</option>';

                                var elementValue = $('<select class=\"barcodeTypeCLS\" id=\"barcodeType\">' + codeTypes + '</select>')
                                                        .css({
                                                            "width": "120px",
                                                            "float": "left",
                                                            "height": "22px",
                                                            "line-height": "20px",
                                                            "padding-left": "5px"
                                                        }).change(function () {
                                                            self.activeElement['barcodeType'] = this.value;
                                                        });
                            } else if (activeElement[key] == "twoD") {

                                var matrixTypes = "";
                                var options = [{ value: 'GS1_DataMatrix', title: 'GS1 DataMatrix' }
                                             , { value: 'Non_GS1', title: 'Non GS1' }
                                             , { value: 'PPN_DataMatrix', title: 'PPN DataMatrix' }
                                ];

                                for (var i = 0; i < options.length; i++)
                                    if (options[i].value == activeElement['matrixType'])
                                        matrixTypes += '<option value="' + options[i].value + '" selected>' + options[i].title + '</option>';
                                    else
                                        matrixTypes += '<option value="' + options[i].value + '">' + options[i].title + '</option>';

                                var elementValue = $('<select class=\"twoDTypeCLS\" id=\"twoDTypeCLS\">' + matrixTypes + '</select>')
														.css({
														    "width": "120px",
														    "float": "left",
														    "height": "22px",
														    "line-height": "20px",
														    "padding-left": "5px"
														}).change(function () {
														    self.activeElement['matrixType'] = this.value;
														});

                            } else if (activeElement[key] == "label_text") {
                                var lblTxtOptions = "";
                                var options = [{ value: 'LabelText', title: 'Label Text' }
                                             , { value: 'BATCHNO', title: 'Batch Number' }
                                             , { value: 'MFGDATE', title: 'MFG Date' }
                                             , { value: 'EXPDATE', title: 'EXP Date' }
                                             , { value: 'QTYINCASENO', title: 'Qty in Case No' }
                                             , { value: 'CNNO', title: 'CNNO' }
                                             , { value: 'PRODUCTNAME', title: 'Product Name' }
                                             , { value: 'GTIN', title: 'GTIN' }
                                             , { value: 'UID', title: 'UID' }
                                             , { value: 'COUNTRYCODE', title: 'Country Code' }
                                             , { value: 'POSTALCODE', title: 'Postal Code' }
                                             , { value: 'PPN', title: 'PPN' }
                                             , { value: 'COMPANYNAME', title: 'Company Name' }
                                             , { value: 'COMPANYADDRESS', title: 'Company Address' }
                                             , { value: 'COMPANYCODE', title: 'Company Code' }
                                             , { value: 'NETWEIGHT', title: 'Net Weight' }
                                             , { value: 'GROSSWEIGHT', title: 'Gross Weight' }
                                             , { value: 'TAREWEIGHT', title: 'Tare Weight' }
                                             , { value: 'TENDERTEXT', title: 'Tender Text' }
                                             , { value: 'PRODUCTBARCODETEXT', title: 'Product Barcode Text' }
                                             , { value: 'NDCBARCODETEXT', title: 'NDC Barcode Text' }
                                             , { value: 'SSCCBARCODETEXT', title: 'SSCC Barcode Text' }
                                             , { value: 'UIDBARCODETEXT', title: 'UID Barcode Text' }
                                             , { value: 'SECONDARYBARCODETEXT', title: 'Secondary Barcode Text' }
                                             , { value: 'SERIALIZEDBARCODETEXT', title: 'Serialized Barcode Text' }
                                             , { value: 'NONSERIALIZEDBARCODETEXT', title: 'NonSerialized Barcode Text' }
                                             , { value: 'PPNBARCODETEXT', title: 'PPN Barcode Text' }
                                             , { value: 'PPNBARCODE2TEXT', title: 'PPN Barcode2 Text' }
                                             , { value: 'PONUMBER', title: 'PO Number' }
                                              , { value: 'EXTRA_EXPIRY', title: 'Extra Expiry' }
                                             , { value: 'NDC', title: 'NDC' }
                                ];

                                for (var i = 0; i < options.length; i++)
                                    if (options[i].value == activeElement['textType'])
                                        lblTxtOptions += '<option value="' + options[i].value + '" selected>' + options[i].title + '</option>';
                                    else
                                        lblTxtOptions += '<option value="' + options[i].value + '">' + options[i].title + '</option>';

                                var elementValue = $('<select class=\"lblTxtTypeCLS\" id=\"lblTxtTypeCLS\">' + lblTxtOptions + '</select>')
														.css({
														    "width": "120px",
														    "float": "left",
														    "height": "22px",
														    "line-height": "20px",
														    "padding-left": "5px"
														}).change(function () {
														    self.activeElement['textType'] = this.value;
														});
                            } else if (key == 'mil') {

                                var milSizes = "";
                                var milOptions = [{ value: '1', title: '3 Mil' }
                                                , { value: '2', title: '7 Mil' }
                                                , { value: '3', title: '10 Mil' }
                                                , { value: '4', title: '13 Mil' }
                                                , { value: '5', title: '17 Mil' }
                                                , { value: '6', title: '20 Mil' }
                                                , { value: '7', title: '23 Mil' }
                                                , { value: '8', title: '27 Mil' }
                                                , { value: '9', title: '30 Mil' }
                                                , { value: '10', title: '33 Mil' }
                                                , { value: '11', title: '37 Mil' }
                                                , { value: '12', title: '40 Mil' }
                                ];

                                for (var i = 0; i < milOptions.length; i++)
                                    if (milOptions[i].value == activeElement['mil'])
                                        milSizes += '<option value="' + milOptions[i].value + '" selected>' + milOptions[i].title + '</option>';
                                    else
                                        milSizes += '<option value="' + milOptions[i].value + '">' + milOptions[i].title + '</option>';

                                var elementValue = $('<select class=\"barcodeTypeCLS\" id=\"mil\">' + milSizes + '</select>')
                                                        .css({
                                                            "width": "120px",
                                                            "float": "left",
                                                            "height": "22px",
                                                            "line-height": "20px",
                                                            "padding-left": "5px"
                                                        }).change(function () {
                                                            self.activeElement['mil'] = this.value;
                                                        });
                            } else {

                                var elementValue = $('<input type="text" name="' + key + '" value="' + activeElement[key] + '">')
                                                            .css({
                                                                "width": "120px",
                                                                "float": "left",
                                                                "height": "22px",
                                                                "line-height": "20px",
                                                                "padding-left": "5px"
                                                            });

                            }



                        }

                        if (!activeElement.readonly || $.inArray(key, activeElement.readonly) == -1) {
                            elementValue.on("keyup", { "objectProperty": key }, function (event) {
                                var data = self.activeElement[event.data.objectProperty];
                                self.activeElement[event.data.objectProperty] = (data === parseInt(data, 10)) ? parseInt($(this).val()) : $(this).val();

                                self.labelDesigner.updateCanvas();
                            });
                        }
                        else {
                            // Draw readonly textbox.
                            elementValue.prop("readonly", true).css({ "background-color": "#DDDDDD", border: "1px solid #AAAAAA" });
                        }

                        this.propertyNodes[key] = elementValue;

                        var elementContainer = $('<div></div>')
								.css({
								    "clear": "both",
								    "padding-top": "2px"
								})
								.append(elementKey).append(elementValue);
                        this.propertyView.append(elementContainer);
                    }
                }
            }
        }
    }

    this.updatePosition(0);
}