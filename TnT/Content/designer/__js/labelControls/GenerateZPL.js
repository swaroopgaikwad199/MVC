if (!com)
	var com = {};
if (!com.logicpartners)
	com.logicpartners = {};
if (!com.logicpartners.labelControl)
	com.logicpartners.labelControl = {};
	
com.logicpartners.labelControl.generatezpl = function(designer) {
	var self       = this;
	this.designer  = designer;
	this.workspace = $("<div></div>").addClass("designerLabelControl").attr("title", "Label Size").css({ float : "right" });
	this.filename  = 'label';
	this.buttonContainer = $("<div></div>").appendTo(this.workspace);
	$("<button>New Design</button>").css({ "line-height": "30px","margin-right": "5px" }).appendTo(this.buttonContainer)
  .on('click', function () {

      $("#CRefresh").html("The Changes You Have Made Will Be Discarded");
      $("#showProgressBar").trigger("click");
  });
 	$("<button>Save Design</button>").css({ "line-height" : "30px"}).appendTo(this.buttonContainer)
  .on('click', function()
  {
      var DPIVal = $('.dpiCls').val();
      var OrientationVal = $('.orientationCls').val();
      if (DPIVal == null) {
          toastr.warning("Please select DPI");
          return false;
      }

      if (OrientationVal == null) {
          toastr.warning("Please select orientation");
          return false;
      }

    showSaveDialog("Save Design File", self.filename, function(filename)
    {
        debugger;

      var zpl, data, png;

      self.filename = filename;
      
      zpl  = self.designer.generateZPL();
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
          data:  formData ,
          type: "POST",
          cache: false,
          contentType: false,
          processData: false,
          success: function (result) {
          //

              if(result==="Success")
              {
                  toastr.success("Label Saved Successfully");
              }
              $('.ZPLlblCls').html('');
              loadLabels();
          },
          error: function (result) {
              toastr.warning("An error occurred while saveing file. Please check directory permissions.");
          }
      });
      //saveFile(filename + '.zpl', 'data:;base64,' + btoa(zpl.data + zpl.zpl));
      //saveFile(filename + '.json', 'data:;base64,' + btoa(data));
      //saveFile(filename + '.png', png);
    });
  });
  
 	$("<button>Load Design</button>").css({ "line-height" : "30px", 'margin-left' : '5px'}).appendTo(this.buttonContainer).on('click', function()
  {
    loadFile(function(file)
    {
      
      var reader;
      
      reader        = new FileReader()
      reader.onload = function(event)
      {
          console.log(reader.result)
        designer.loadDesign(reader.result);
      };
      
      reader.onerror = function(event)
      {
          toastr.warning("An error occurred: " + reader.error);

      };
      
      reader.readAsText(file);
    });
  });
  
   // create file input element
   
   this.buttonContainer.append("<input type='file' style='position:absolute; left:0; top:0; visibility: hidden'>");

   var filedlg = {node    : this.buttonContainer[0].lastElementChild
                , callback: null
                , open    : function(func)
                            {
                              this.callback = func;
                              this.node.click();
                            }
                };
  
  filedlg.node.addEventListener('change', function(event)
  {
    filedlg.callback(this.files[0]);
    this.value = null;
  });
  
  function loadFile(onload)
  {
      debugger;
      var fileName = $('.ZPLlblCls').val();
      if (fileName == "") {
          toastr.warning("Please select label.");
          return false;
      }
      
      if (confirm("Unsaved label changes will be discard. Do you want to continue ?")) {
          //jQuery.get(, function (data) {
          var datapost={ 'fileName': fileName.toString() };
          //});
          $.ajax({
              url: '../LblLytDsg/LoadLabel',
              dataType: 'json',
              contentType: "application/json;charset=utf-8",
              type: 'POST',
              data:JSON.stringify(datapost) ,
            
              success: function (result) {

                  console.log(result);
                  designer.loadDesign(result);
              },
              error: function (data) {

                  toastr.warning("Error occured while loading data");

              }
          });
      }
      else {
          return false;
      }

      
      
      
    //filedlg.open(onload);
  }
  
  function showSaveDialog(title, filename, onsave)
  {
    var dialog = $(document.createElement('div'));
    var input  = $(document.createElement('input'));
    
    dialog.append("<span>File Name: </span>");
    dialog.append(input);
    
    input.val(filename);
    
    input.change(function()
    { 
      filename = this.value;
    });
     
    input.keydown(function(event)
    {
      if(event.keyCode === 13)
      {
        onsave(this.value)
        dialog.dialog('close');
      }
    });
    
    dialog.dialog({modal   : true,
                   width   : 470,
                   height  : 400,
                   title   : title,
                   buttons : {'Save'   : function()
                              {
                                onsave(filename)
                                $(this).dialog('close');
                              },
                              'Cancel' : function()
                              {
                                $(this).dialog('close');
                              }
                             }
                 });
  }
 
  function saveFile(filename, dataurl)
  {
    var dom;
    
    dom = document.createElement('a');

    dom.href     = dataurl;
    dom.download = filename;
    
    dom.click();
  }
   
	this.update = function() {
		this.widthController.val(this.designer.labelWidth / this.designer.dpi);
		this.heightController.val(this.designer.labelHeight / this.designer.dpi);
	}
}