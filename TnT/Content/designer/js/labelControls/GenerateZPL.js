if (!com)
    var com = {};
if (!com.logicpartners)
    com.logicpartners = {};
if (!com.logicpartners.labelControl)
    com.logicpartners.labelControl = {};

com.logicpartners.labelControl.generatezpl = function (designer) {
    var self = this;
    this.designer = designer;
    this.workspace = $("<div></div>").addClass("designerLabelControl").attr("title", "Label Size").css({ float: "right" });
    this.filename = 'label';
    this.buttonContainer = $("<div></div>").appendTo(this.workspace);
    $("<button>New</button>").css({ "line-height": "30px", "margin-right": "5px" }).appendTo(this.buttonContainer)
        .on('click', function () {

            $("#CRefresh").html("The Changes You Have Made Will Be Discarded");
            $("#showProgressBar").trigger("click");
        });
    $("<button>Save</button>").css({ "line-height": "30px" }).appendTo(this.buttonContainer)
        .on('click', function () {
            var DPIVal = $('.dpiCls').val();
            var OrientationVal = $('.orientationCls').val();
            if (DPIVal == null) {
                toastr.warning("Please Select DPI.");
                return false;
            }

            if (OrientationVal == null) {
                toastr.warning("Please Select Orientation.");
                return false;
            }

            showSaveDialog("Save Design File", self.filename, function (filename) {
                filename = filename.replace(/^\s+|\s+$|\s+(?=\s)/g, "");
                if (filename == "") {
                    toastr.warning("To Save Label, Please Enter Label Name.");
                    return false;
                }



                var zpl, data, png;

                self.filename = filename;

                zpl = self.designer.generateZPL();
                data = self.designer.saveDesign();
                png = self.designer.generatePNG();

                console.log(data);
                var zplstring = btoa(zpl.data + zpl.zpl);
                var jsonstring = btoa(data);

                var dataObject = {
                    'png': png,
                    'zpl': zplstring,
                    'jsonstring': jsonstring,
                    'lblName': filename
                };

                // var d = JSON.parse('{"png":"' + png + '","zpl":"' + zplstring + '","jsonstring":"' + jsonstring + '","lblName":"'+filename+'"}');
                var formData = new FormData();
                formData.append('png', png);
                formData.append('zpl', zplstring);
                formData.append('jsonstring', jsonstring);
                formData.append('lblName', filename);

                $.ajax({
                    url: '../LblLytDsg/getZPLImage', //
                    dataType: "json",
                    data: formData,
                    type: "POST",
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        //

                        if (result === "Success") {
                            toastr.success("Label Saved Successfully.");

                        }
                        $('.ZPLlblCls').html('');
                        loadLabels();
                    },
                    error: function (result) {
                        toastr.warning("An Error Occurred While Saving File. Please Check Directory Permissions.");
                    }
                });
                //saveFile(filename + '.zpl', 'data:;base64,' + btoa(zpl.data + zpl.zpl));
                //saveFile(filename + '.json', 'data:;base64,' + btoa(data));
                //saveFile(filename + '.png', png);
            });
        });

    $("<button>Print</button>").css({ "line-height": "30px", 'margin-left': '5px' }).appendTo(this.buttonContainer)
        .on('click', function () {
            var DPIVal = $('.dpiCls').val();
            var OrientationVal = $('.orientationCls').val();
            if (DPIVal == null) {
                toastr.warning("Please Select DPI.");
                return false;
            }

            if (OrientationVal == null) {
                toastr.warning("Please Select Orientation.");
                return false;
            }

            showPrintDialog("Print Sample Label", function () {

                zpl = self.designer.getZPL();
                var ip = $('#PrinterIP').val();
                var pport = $('#PrinterPort').val();

                var formData = new FormData();
                formData.append('IPAddress', ip);
                formData.append('PortNumber', pport);
                //formData.append('Zpl', zpl.zpl);
                formData.append('Zpl', zpl.data + zpl.zpl);


                $.ajax({
                    url: '../LblLytDsg/SendDataToPrinter', //
                    dataType: "json",
                    data: formData,
                    type: "POST",
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        //

                        if (result === "Success") {
                            toastr.success("Connected");
                        }
                        if (result === "Requied  PrinterIp or PortNumber") {
                            toastr.warning("Please Provide PrinterIP or PortNumber");
                        }
                        if (result === "Printer Not Connected") {
                            toastr.warning("Printer Is Not Connected.");
                        }
                    },
                    error: function (result) {
                        console.log(result);
                        toastr.warning("An Error Occurred While Printing Label.");
                    }
                });
                //saveFile(filename + '.zpl', 'data:;base64,' + btoa(zpl.data + zpl.zpl));
                //saveFile(filename + '.json', 'data:;base64,' + btoa(data));
                //saveFile(filename + '.png', png);
            });
        });

    $("<button>Load</button>").css({ "line-height": "30px", 'margin-left': '5px' }).appendTo(this.buttonContainer).on('click', function () {
        loadFile(function (file) {

            var reader;

            reader = new FileReader()
            reader.onload = function (event) {
                console.log(reader.result)
                designer.loadDesign(reader.result);
            };

            reader.onerror = function (event) {
                toastr.warning("An Error Occurred: " + reader.error);

            };

            reader.readAsText(file);
        });
    });

    // create file input element

    this.buttonContainer.append("<input type='file' style='position:absolute; left:0; top:0; visibility: hidden'>");

    var filedlg = {
        node: this.buttonContainer[0].lastElementChild
        , callback: null
        , open: function (func) {
            this.callback = func;
            this.node.click();
        }
    };

    filedlg.node.addEventListener('change', function (event) {
        filedlg.callback(this.files[0]);
        this.value = null;
    });

    function loadFile(onload) {
        var fileName = $('.ZPLlblCls').val();
        if (fileName == "") {
            toastr.warning("Please Select Label.");
            return false;
        }
        self.filename = $('.ZPLlblCls').val().replace(".txt", "");

        if (confirm("Unsaved Label Changes Will Be Discard. Do You Want To Continue ?")) {
            //jQuery.get(, function (data) {
            var datapost = { 'fileName': fileName.toString() };
            //});
            $.ajax({
                url: '../LblLytDsg/LoadLabel',
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'POST',
                data: JSON.stringify(datapost),

                success: function (result) {

                    console.log(result);
                    designer.loadDesign(result);
                },
                error: function (data) {

                    toastr.warning("Error Occured While Loading Data.");

                }
            });
        }
        else {
            return false;
        }




        //filedlg.open(onload);
    }

    function showSaveDialog(title, filename, onsave) {
        var dialog = $(document.createElement('div'));
        var input = $(document.createElement('input'));

        dialog.append("<span>File Name: </span>");
        dialog.append(input);

        input.val(filename);

        input.change(function () {
            filename = this.value;
        });

        input.keydown(function (event) {
            if (event.keyCode === 13) {
                onsave(this.value)
                dialog.dialog('close');
            }
        });

        dialog.dialog({
            modal: true,
            width: 470,
            height: 400,
            title: title,
            buttons: {
                'Save': function () {
                    onsave(filename)
                    $(this).dialog('close');
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });
    }

    function showPrintDialog(title, onsave) {
        var dialog = $(document.createElement('div'));
        var input = $(document.createElement('input'));
        var input1 = $(document.createElement('input'));

        dialog.append("<span>Printer IP: </span>");
        dialog.append(input);
        input.attr("id", 'PrinterIP');

        dialog.append(input1);
        input1.attr("id", 'PrinterPort');
        input1.val('9100')
        input1.attr("placeholder", 'Port');
        input1.css("width", '150px');
        input1.css("margin", '9px');

        input.keydown(function (event) {
            if (event.keyCode === 13) {
                onsave(this.value)
                dialog.dialog('close');
            }
        });

        dialog.dialog({
            modal: true,
            width: 470,
            height: 400,
            title: title,
            beforeClose: function (event, ui) { $(this).empty(); },
            buttons: {
                'Print': function () {
                    onsave()
                    $(this).dialog('close');
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });
    }

    function saveFile(filename, dataurl) {
        var dom;

        dom = document.createElement('a');

        dom.href = dataurl;
        dom.download = filename;

        dom.click();
    }

    this.update = function () {
        this.widthController.val(this.designer.labelWidth / this.designer.dpi);
        this.heightController.val(this.designer.labelHeight / this.designer.dpi);
    }
}