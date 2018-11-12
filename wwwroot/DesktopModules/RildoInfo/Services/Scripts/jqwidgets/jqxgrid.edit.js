/*
jQWidgets v3.0.2 (2013-August-26)
Copyright (c) 2011-2013 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a){a.extend(a.jqx._jqxGrid.prototype,{_handledblclick:function(t,n){if(t.target==null){return}if(n.disabled){return}if(a(t.target).ischildof(this.columnsheader)){return}var w;if(t.which){w=(t.which==3)}else{if(t.button){w=(t.button==2)}}if(w){return}var B;if(t.which){B=(t.which==2)}else{if(t.button){B=(t.button==1)}}if(B){return}var v=this.showheader?this.columnsheader.height()+2:0;var o=this._groupsheader()?this.groupsheader.height():0;var A=this.showtoolbar?this.toolbarheight:0;o+=A;var e=this.host.offset();var m=t.pageX-e.left;var l=t.pageY-v-e.top-o;var b=this._hittestrow(m,l);var h=b.row;var j=b.index;var q=t.target.className;var p=this.table[0].rows[j];if(p==null){return}n.mousecaptured=true;n.mousecaptureposition={left:t.pageX,top:t.pageY-o};var r=this.hScrollInstance;var s=r.value;var d=0;var k=this.groupable?this.groups.length:0;for(var u=0;u<p.cells.length;u++){var f=parseInt(a(this.columnsrow[0].cells[u]).css("left"));var g=f-s;if(n.columns.records[u].pinned){g=f}var c=this._getcolumnat(u);if(c!=null&&c.hidden){continue}var z=g+a(this.columnsrow[0].cells[u]).width();if(z>=m&&m>=g){d=u;break}}if(h!=null){var c=this._getcolumnat(d);if(!(q.indexOf("jqx-grid-group-expand")!=-1||q.indexOf("jqx-grid-group-collapse")!=-1)){if(h.boundindex!=-1){n.begincelledit(n.getboundindex(h),c.datafield,c.defaulteditorvalue)}}}},_getpreveditablecolumn:function(c){var b=this;while(c>0){c--;var d=b.getcolumnat(c);if(!d){return null}if(!d.editable){continue}if(!d.hidden){return d}}return null},_getnexteditablecolumn:function(c){var b=this;while(c<this.columns.records.length){c++;var d=b.getcolumnat(c);if(!d){return null}if(!d.editable){continue}if(!d.hidden){return d}}return null},_handleeditkeydown:function(A,u){var E=A.charCode?A.charCode:A.keyCode?A.keyCode:0;if(u.showfilterrow&&u.filterable){if(this.filterrow){if(a(A.target).ischildof(this.filterrow)){return true}}}if(u.pageable){if(a(A.target).ischildof(this.pager)){return true}}if(this.showtoolbar){if(a(A.target).ischildof(this.toolbar)){return true}}if(this.showstatusbar){if(a(A.target).ischildof(this.statusbar)){return true}}if(this.rowdetails){if(a(A.target).ischildof(this.content.find("[role='rowgroup']"))){return true}}if(this.editcell){if(this.editcell.columntype==null||this.editcell.columntype=="textbox"||this.editcell.columntype=="numberinput"||this.editcell.columntype=="combobox"||this.editcell.columntype=="datetimeinput"){if(E>=33&&E<=40&&u.selectionmode=="multiplecellsadvanced"){var h=this.editcell.columntype=="textbox"||this.editcell.columntype==null?this.editcell.editor:this.editcell.editor.find("input");var F=u._selection(h);var v=h.val().length;if(F.length>0&&this.editcell.columntype!="datetimeinput"){u._cancelkeydown=true}if(F.start>0&&E==37){u._cancelkeydown=true}if(F.start<v&&E==39&&this.editcell.columntype!="datetimeinput"){u._cancelkeydown=true}if(this.editcell.columntype=="datetimeinput"&&E==39){if(F.start+F.length<v){u._cancelkeydown=true}}}}else{if(this.editcell.columntype=="dropdownlist"){if(E==37||E==39&&u.selectionmode=="multiplecellsadvanced"){u._cancelkeydown=false}}else{if(this.selectionmode=="multiplecellsadvanced"&&this.editcell.columntype!="textbox"&&this.editcell.columntype!="numberinput"){u._cancelkeydown=true}}}if(E==32){if(u.editcell.columntype=="checkbox"){var e=u.getcolumn(u.editcell.datafield);if(e.editable){var l=!u.getcellvalue(u.editcell.row,u.editcell.column);if(e.cellbeginedit){var b=e.cellbeginedit(u.editcell.row,e.datafield,e.columntype,!l);if(b==false){return false}}u.setcellvalue(u.editcell.row,u.editcell.column,l,true);u._raiseEvent(18,{rowindex:u.editcell.row,datafield:u.editcell.column,oldvalue:!l,value:l,columntype:"checkbox"});return false}}}if(E==9){var g=this.editcell.row;var s=this.editcell.column;var k=s;var x=u._getcolumnindex(s);var r=false;var d=u.getrowvisibleindex(g);this.editchar="";if(this.editcell.validated!=false){if(A.shiftKey){var e=u._getpreveditablecolumn(x);if(e){s=e.datafield;r=true;if(u.selectionmode.indexOf("cell")!=-1){u.selectprevcell(g,k);setTimeout(function(){u.ensurecellvisible(d,s)},10)}}}else{var e=u._getnexteditablecolumn(x);if(e){s=e.datafield;r=true;if(u.selectionmode.indexOf("cell")!=-1){u.selectnextcell(g,k);u._oldselectedcell=u.selectedcell;setTimeout(function(){u.ensurecellvisible(d,s)},10)}}}if(r){u.begincelledit(g,s);if(this.editcell!=null&&this.editcell.columntype=="checkbox"){this._renderrows(this.virtualsizeinfo)}}else{if(this.editcell!=null){u.endcelledit(g,s,false);this._renderrows(this.virtualsizeinfo)}return true}}return false}else{if(E==13){var q=this.selectedcell;if(q){var t=this.getrowvisibleindex(q.rowindex)}this.endcelledit(this.editcell.row,this.editcell.column,false,true);if(this.selectionmode=="multiplecellsadvanced"){var c=u.getselectedcell();if(c!=null){if(u.selectcell){if(this.editcell==null){if(c.rowindex+1<this.dataview.totalrecords){if(this.sortcolumn!=c.datafield){var d=this.getrowvisibleindex(c.rowindex);var D=this.dataview.loadedrecords[d+1];if(D){if(!this.pageable||(this.pageable&&d+1<(this.dataview.pagenum+1)*this.pagesize)){this.clearselection(false);this.selectcell(this.getboundindex(D),c.datafield);var c=this.getselectedcell();this.ensurecellvisible(D.visibleindex,c.datafield)}}}else{if(q!=null){var C=this.dataview.loadedrecords[t+1];if(C){if(!this.pageable||(this.pageable&&t+1<this.pagesize)){this.clearselection(false);this.selectcell(this.getboundindex(C),c.datafield)}else{if(this.pageable&&t+1>=this.pagesize){this.clearselection(false);var C=this.dataview.loadedrecords[t];this.selectcell(this.getboundindex(C),c.datafield)}}}}}}}}}}return false}else{if(E==27){this.endcelledit(this.editcell.row,this.editcell.column,true,true);return false}}}}else{var w=false;if(E==113){w=true}if(!A.ctrlKey&&!A.altKey){if(E>=48&&E<=57){this.editchar=String.fromCharCode(E);w=true}if(E>=65&&E<=90){this.editchar=String.fromCharCode(E);var p=false;if(A.shiftKey){p=A.shiftKey}else{if(A.modifiers){p=!!(A.modifiers&4)}}if(!p){this.editchar=this.editchar.toLowerCase()}w=true}else{if(E>=96&&E<=105){this.editchar=E-96;this.editchar=this.editchar.toString();w=true}}var o=a(".jqx-grid").length;w=w&&(o==1||(o>1&&u.focused));var j=a.data(document.body,"jqxgrid.edit");if(j!==undefined&&j!==""){if(E===13||w){if(j!=u.element.id){return true}}}}if(E==13||w){if(u.getselectedrowindex){var g=u.getselectedrowindex();switch(u.selectionmode){case"singlerow":case"multiplerows":case"multiplerowsextended":if(g>=0){var s="";for(var y=0;y<u.columns.records.length;y++){var e=u.getcolumnat(y);if(e.editable){s=e.datafield;break}}u.begincelledit(g,s)}break;case"singlecell":case"multiplecells":case"multiplecellsextended":var c=u.getselectedcell();if(c!=null){var e=u._getcolumnbydatafield(c.datafield);if(e.columntype!="checkbox"){u.begincelledit(c.rowindex,c.datafield)}}break;case"multiplecellsadvanced":var c=u.getselectedcell();if(c!=null){if(E==13){if(u.selectcell){if(c.rowindex+1<u.dataview.totalrecords){var d=this.getrowvisibleindex(c.rowindex);var D=this.dataview.loadedrecords[d+1];if(D){this.clearselection(false);this.selectcell(this.getboundindex(D),c.datafield);var c=this.getselectedcell();this.ensurecellvisible(D.visibleindex,c.datafield)}}}}else{u.begincelledit(c.rowindex,c.datafield)}}break}return false}}if(E==46){var f=u.getselectedcells();if(u.selectionmode.indexOf("cell")==-1){if(u._getcellsforcopypaste){f=u._getcellsforcopypaste()}}if(f!=null&&f.length>0){for(var n=0;n<f.length;n++){var c=f[n];if(!c.datafield){continue}var e=u.getcolumn(c.datafield);var B=u.getcellvalue(c.rowindex,c.datafield);if(B!==""&&e.editable&&u.enablekeyboarddelete){var i=null;if(e.columntype=="checkbox"){if(!e.threestatecheckbox){i=false}}if(e.cellbeginedit){var b=e.cellbeginedit(c.rowindex,e.datafield,e.columntype,i);if(b==false){return false}}u._raiseEvent(17,{rowindex:c.rowindex,datafield:c.datafield,value:B});if(n==f.length-1){u.setcellvalue(c.rowindex,c.datafield,i,true);if(e.displayfield!=e.datafield){u.setcellvalue(c.rowindex,e.displayfield,i,true)}}else{u.setcellvalue(c.rowindex,c.datafield,i,false);if(e.displayfield!=e.datafield){u.setcellvalue(c.rowindex,e.displayfield,i,true)}}if(e.cellendedit){var z=e.cellendedit(c.rowindex,e.datafield,e.columntype,i)}u._raiseEvent(18,{rowindex:c.rowindex,datafield:c.datafield,oldvalue:B,value:i})}}this.dataview.updateview();this._renderrows(this.virtualsizeinfo);return false}}if(E==32){var c=u.getselectedcell();if(c!=null){var e=u.getcolumn(c.datafield);if(e.columntype=="checkbox"&&e.editable){var l=!u.getcellvalue(c.rowindex,c.datafield);if(e.cellbeginedit){var b=e.cellbeginedit(c.rowindex,e.datafield,e.columntype,!l);if(b==false){return false}}u._raiseEvent(17,{rowindex:c.rowindex,datafield:c.datafield,value:!l,columntype:"checkbox"});u.setcellvalue(c.rowindex,c.datafield,l,true);u._raiseEvent(18,{rowindex:c.rowindex,datafield:c.datafield,oldvalue:!l,value:l,columntype:"checkbox"});return false}}}}return true},begincelledit:function(k,d,i){var e=this.getcolumn(d);if(d==null){return}if(e.columntype=="number"||e.columntype=="button"){return}if(this.groupable){if(this.groups.indexOf(d)>=0){return}if(this.groups.indexOf(e.displayfield)>=0){return}}if(this.editrow!=undefined){return}if(this.editcell){if(this.editcell.row==k&&this.editcell.column==d){return true}var c=this.endcelledit(this.editcell.row,this.editcell.column,false,true,false);if(false==c){return}}var f=e.columntype=="checkbox"||e.columntype=="button";this.host.removeClass("jqx-disableselect");this.content.removeClass("jqx-disableselect");if(e.editable){if(e.cellbeginedit){var h=this.getcell(k,d);var j=e.cellbeginedit(k,d,e.columntype,h!=null?h.value:null);if(j==false){return}}var g=this.getrowvisibleindex(k);this.editcell=this.getcell(k,d);this.editcell.visiblerowindex=g;if(!this.editcell.editing){if(!f){this.editcell.editing=true}this.editcell.columntype=e.columntype;this.editcell.defaultvalue=i;if(e.defaultvalue!=undefined){this.editcell.defaultvalue=e.defaultvalue}this.editcell.init=true;if(e.columntype!="checkbox"){this._raiseEvent(17,{rowindex:k,datafield:e.datafield,value:this.editcell.value,columntype:e.columntype})}a.data(document.body,"jqxgrid.edit",this.element.id);if(!f){var b=this.getrowvisibleindex(k);this.ensurecellvisible(b,e.datafield);this._renderrows(this.virtualsizeinfo)}if(this.editcell){this.editcell.init=false;return true}}}else{if(!this.editcell){return}this.editcell.editor=null;this.editcell.editing=false;this._renderrows(this.virtualsizeinfo);this.editcell=null}},getScrollTop:function(){if(this._py){return pageYOffset}this._py=typeof pageYOffset!="undefined";if(this._py){return pageYOffset}else{var c=document.body;var b=document.documentElement;b=(b.clientHeight)?b:c;return b.scrollTop}},getScrollLeft:function(){if(typeof pageXOffset!="undefined"){return pageXOffset}else{var c=document.body;var b=document.documentElement;b=(b.clientHeight)?b:c;return b.scrollLeft}},endcelledit:function(g,m,i,e,n){if(g==undefined||m==undefined){if(this.editcell){g=this.editcell.row;m=this.editcell.column}if(i==undefined){i=true}}if(!this.editcell){return}var d=this.getcolumn(m);var t=this;if(t.editrow!=undefined){return}var s=function(){if(n!=false){if(!t.isNestedGrid){var u=t.getScrollTop();var w=t.getScrollLeft();try{t.element.focus();t.content.focus();if(u!=t.getScrollTop()){window.scrollTo(w,u)}setTimeout(function(){t.element.focus();t.content.focus();if(u!=t.getScrollTop()){window.scrollTo(w,u)}},10)}catch(v){}}}};if(d.columntype=="checkbox"||d.columntype=="button"){if(this.editcell){this.editcell.editor=null;this.editcell.editing=false;this.editcell=null}return true}var h=this._geteditorvalue(d);var f=function(v){v._hidecelleditor();if(d.cellendedit){d.cellendedit(g,m,d.columntype,v.editcell.value,h)}v.editchar=null;if(d.displayfield!=d.datafield){var u=v.getcellvalue(v.editcell.row,d.displayfield);var w=v.editcell.value;oldvalue={value:w,text:u}}else{oldvalue=v.editcell.value}v._raiseEvent(18,{rowindex:g,datafield:m,displayfield:d.displayfield,oldvalue:h,value:h,columntype:d.columntype});v.editcell.editor=null;v.editcell.editing=false;v.editcell=null;if(e||e==undefined){v._renderrows(v.virtualsizeinfo)}s();if(!v.enablebrowserselection){v.host.addClass("jqx-disableselect");v.content.addClass("jqx-disableselect")}};if(i){f(this);return false}if(this.validationpopup){this.validationpopup.hide();this.validationpopuparrow.hide()}if(d.cellvaluechanging){var b=d.cellvaluechanging(g,m,d.columntype,this.editcell.value,h);if(b!=undefined){h=b}}if(d.validation){var c=this.getcell(g,m);try{var o=d.validation(c,h);var k=this.gridlocalization.validationstring;if(o.message!=undefined){k=o.message}var l=typeof o=="boolean"?o:o.result;if(!l){if(o.showmessage==undefined||o.showmessage==true){this._showvalidationpopup(g,m,k)}this.editcell.validated=false;return false}}catch(q){this._showvalidationpopup(g,m,this.gridlocalization.validationstring);this.editcell.validated=false;return false}}if(d.displayfield!=d.datafield){var j=this.getcellvalue(this.editcell.row,d.displayfield);var p=this.editcell.value;oldvalue={value:p,text:j}}else{oldvalue=this.editcell.value}if(d.cellendedit){var r=d.cellendedit(g,m,d.columntype,this.editcell.value,h);if(r==false){this._raiseEvent(18,{rowindex:g,datafield:m,displayfield:d.displayfield,oldvalue:oldvalue,value:oldvalue,columntype:d.columntype});f(this);return false}}this._raiseEvent(18,{rowindex:g,datafield:m,displayfield:d.displayfield,oldvalue:oldvalue,value:h,columntype:d.columntype});this._hidecelleditor(false);if(this.editcell!=undefined){this.editcell.editor=null;this.editcell.editing=false}this.editcell=null;this.editchar=null;this.setcellvalue(g,m,h,e);if(!this.enablebrowserselection){this.host.addClass("jqx-disableselect");this.content.addClass("jqx-disableselect")}s();a.data(document.body,"jqxgrid.edit","");return true},beginrowedit:function(d){if(!this.editcells){this.editcells=new Array()}if(this.editcells.length>0){if(this.editcells[0].row==d){return}var c=this.endrowedit(this.editcells[0].row,false,true);if(false==c){return}}this.host.removeClass("jqx-disableselect");this.content.removeClass("jqx-disableselect");var b=this;this.editcells=new Array();a.each(this.columns.records,function(){if(b.editable){var e=b.getcell(d,this.datafield);e.editing=true;if(this.defaultvalue!=undefined){e.defaultvalue=column.defaultvalue}e.init=true;b.editcells[this.datafield]=e}});b.editrow=d;b._renderrows(this.virtualsizeinfo);a.each(this.columns.records,function(){b.editcells[this.datafield].init=false})},endrowedit:function(b){if(this.editcell.editor==undefined){return false}return true},_selection:function(b){if("selectionStart" in b[0]){var g=b[0];var h=g.selectionEnd-g.selectionStart;return{start:g.selectionStart,end:g.selectionEnd,length:h,text:g.value}}else{var d=document.selection.createRange();if(d==null){return{start:0,end:g.value.length,length:0}}var c=b[0].createTextRange();var f=c.duplicate();c.moveToBookmark(d.getBookmark());f.setEndPoint("EndToStart",c);var h=d.text.length;return{start:f.text.length,end:f.text.length+d.text.length,length:h,text:d.text}}},_setSelection:function(e,b,d){if("selectionStart" in d[0]){d[0].focus();d[0].setSelectionRange(e,b)}else{var c=d[0].createTextRange();c.collapse(true);c.moveEnd("character",b);c.moveStart("character",e);c.select()}},findRecordIndex:function(g,c,b){var b=b;if(c){var e=b.length;for(var h=0;h<e;h++){var f=b[h];var d=f.text;if(g==d){return h}}}return -1},_destroyeditors:function(){var b=this;a.each(this.columns.records,function(f,h){var c=a.trim(this.datafield).split(" ").join("");switch(this.columntype){case"dropdownlist":var g=b.editors["dropdownlist_"+c];if(g){g.jqxDropDownList("destroy");b.editors["dropdownlist_"+c]=null}break;case"combobox":var j=b.editors["combobox_"+c];if(j){j.jqxComboBox("destroy");b.editors["combobox_"+c]=null}break;case"datetimeinput":var d=b.editors["datetimeinput_"+this.datafield];if(d){d.jqxDateTimeInput("destroy");b.editors["datetimeinput_"+c]=null}break;case"numberinput":var e=b.editors["numberinput_"+c];if(e){e.jqxNumberInput("destroy");b.editors["numberinput_"+c]=null}break;case"custom":case"template":if(this.destroycustomeditor){this.destroycustomeditor(b.editors["customeditor_"+c]);b.editors["customeditor_"+c]=null}if(this.destrotemplateeditor){var l=b.getrows.length();for(var k=0;k<l;k++){this.destrotemplateeditor(b.editors["templateeditor_"+c+"_"+k]);b.editors["templateeditor_"+c+"_"+k]=null}}break}});b.editors=new Array()},_showcelleditor:function(q,G,n,I,w){if(this.editrow!=undefined){this.editcell=this.editcells[G.datafield]}if(n==undefined){return}if(this.editcell==null){return}if(G.columntype=="checkbox"&&G.editable){return}if(w==undefined){w=true}var E=G.datafield;var g=a(n);var s=this;var d=this.editcell.editor;var H=this.getcellvalue(q,E);var C=this.getcelltext(q,E);var j=this.hScrollInstance;var t=j.value;var i=parseInt(t);this.editcell.element=n;if(this.editcell.validated==false){this._showvalidationpopup()}var l=function(N){if(s.hScrollInstance.isScrolling()||s.vScrollInstance.isScrolling()){return}if(!w){return}if(!s.isNestedGrid){N.focus()}if(s.gridcontent[0].scrollTop!=0){s.scrolltop(Math.abs(s.gridcontent[0].scrollTop));s.gridcontent[0].scrollTop=0}if(s.gridcontent[0].scrollLeft!=0){s.gridcontent[0].scrollLeft=0}};switch(G.columntype){case"dropdownlist":if(this.host.jqxDropDownList){n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");var A=a.trim(G.displayfield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}if(A.indexOf(".")!=-1){A=A.replace(".","")}var k=this.editors["dropdownlist_"+D];d=k==undefined?a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='dropdownlisteditor'></div>"):k;d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(k==undefined){d.prependTo(this.table);d[0].id="dropdownlisteditor"+this.element.id+D;var f=this.source._source?true:false;var x=null;if(!f){x=new a.jqx.dataAdapter(this.source,{autoBind:false,uniqueDataFields:[A],async:false,autoSort:true,autoSortField:A})}else{var o={localdata:this.source.records,datatype:this.source.datatype,async:false};x=new a.jqx.dataAdapter(o,{autoBind:false,async:false,uniqueDataFields:[A],autoSort:true,autoSortField:A})}var u=!G.createeditor?true:false;d.jqxDropDownList({enableBrowserBoundsDetection:true,keyboardSelection:false,source:x,rtl:this.rtl,autoDropDownHeight:u,theme:this.theme,width:g.width()-2,height:g.height()-2,displayMember:A,valueMember:E});this.editors["dropdownlist_"+D]=d;if(G.createeditor){G.createeditor(q,H,d)}}if(G._requirewidthupdate){d.jqxDropDownList({width:g.width()-2})}var c=d.jqxDropDownList("listBox").visibleItems;if(!G.createeditor){if(c.length<8){d.jqxDropDownList("autoDropDownHeight",true)}else{d.jqxDropDownList("autoDropDownHeight",false)}}var H=this.getcellvalue(q,A);var z=this.findRecordIndex(H,A,c);if(I){if(H!=""){d.jqxDropDownList("selectIndex",z,true)}else{d.jqxDropDownList("selectIndex",-1)}}if(this.editcell.defaultvalue!=undefined){d.jqxDropDownList("selectIndex",this.editcell.defaultvalue,true)}if(w){d.jqxDropDownList("focus")}}break;case"combobox":if(this.host.jqxComboBox){n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");var A=a.trim(G.displayfield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}if(A.indexOf(".")!=-1){A=A.replace(".","")}var r=this.editors["combobox_"+D];d=r==undefined?a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='comboboxeditor'></div>"):r;d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(r==undefined){d.prependTo(this.table);d[0].id="comboboxeditor"+this.element.id+D;var f=this.source._source?true:false;var x=null;if(!f){x=new a.jqx.dataAdapter(this.source,{autoBind:false,uniqueDataFields:[A],async:false,autoSort:true,autoSortField:A})}else{var o={localdata:this.source.records,datatype:this.source.datatype,async:false};x=new a.jqx.dataAdapter(o,{autoBind:false,async:false,uniqueDataFields:[A],autoSort:true,autoSortField:A})}var u=!G.createeditor?true:false;d.jqxComboBox({enableBrowserBoundsDetection:true,keyboardSelection:false,source:x,rtl:this.rtl,autoDropDownHeight:u,theme:this.theme,width:g.width()-2,height:g.height()-2,displayMember:A,valueMember:E});this.editors["combobox_"+D]=d;if(G.createeditor){G.createeditor(q,H,d)}}if(G._requirewidthupdate){d.jqxComboBox({width:g.width()-2})}var c=d.jqxComboBox("listBox").visibleItems;if(!G.createeditor){if(c.length<8){d.jqxComboBox("autoDropDownHeight",true)}else{d.jqxComboBox("autoDropDownHeight",false)}}var H=this.getcellvalue(q,A);var z=this.findRecordIndex(H,A,c);if(I){if(H!=""){d.jqxComboBox("selectIndex",z,true);d.jqxComboBox("val",H)}else{d.jqxComboBox("selectIndex",-1);d.jqxComboBox("val",H)}}if(this.editcell.defaultvalue!=undefined){d.jqxComboBox("selectIndex",this.editcell.defaultvalue,true)}if(this.editchar&&this.editchar.length>0){d.jqxComboBox("input").val(this.editchar)}if(w){setTimeout(function(){l(d.jqxComboBox("input"));d.jqxComboBox("_setSelection",0,0);if(s.editchar){d.jqxComboBox("_setSelection",1,1);s.editchar=null}else{var N=d.jqxComboBox("input").val();d.jqxComboBox("_setSelection",0,N.length)}},10)}}break;case"datetimeinput":if(this.host.jqxDateTimeInput){n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}var v=this.editors["datetimeinput_"+D];d=v==undefined?a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='datetimeeditor'></div>"):v;d.show();d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(v==undefined){d.prependTo(this.table);d[0].id="datetimeeditor"+this.element.id+D;var F={calendar:this.gridlocalization};d.jqxDateTimeInput({enableBrowserBoundsDetection:true,localization:F,_editor:true,theme:this.theme,rtl:this.rtl,width:g.width(),height:g.height(),formatString:G.cellsformat});this.editors["datetimeinput_"+D]=d;if(G.createeditor){G.createeditor(q,H,d)}}if(G._requirewidthupdate){d.jqxDateTimeInput({width:g.width()-2})}if(I){if(H!=""&&H!=null){var J=new Date(H);if(J=="Invalid Date"){if(this.source.getvaluebytype){J=this.source.getvaluebytype(H,{name:G.datafield,type:"date"})}}d.jqxDateTimeInput("setDate",J)}else{d.jqxDateTimeInput("setDate",null)}if(this.editcell.defaultvalue!=undefined){d.jqxDateTimeInput("setDate",this.editcell.defaultvalue)}}if(w){setTimeout(function(){l(d.jqxDateTimeInput("dateTimeInput"))},10)}}break;case"numberinput":if(this.host.jqxNumberInput){n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}var L=this.editors["numberinput_"+D];d=L==undefined?a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='numbereditor'></div>"):L;d.show();d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(L==undefined){d.prependTo(this.table);d[0].id="numbereditor"+this.element.id+D;var m="";var y="left";var K=2;if(G.cellsformat){if(G.cellsformat.indexOf("c")!=-1){m=this.gridlocalization.currencysymbol;y=this.gridlocalization.currencysymbolposition;if(y=="before"){y="left"}else{y="right"}if(G.cellsformat.length>1){K=parseInt(G.cellsformat.substring(1),10)}}else{if(G.cellsformat.indexOf("p")!=-1){m=this.gridlocalization.percentsymbol;y="right";if(G.cellsformat.length>1){K=parseInt(G.cellsformat.substring(1),10)}}}}else{K=0}d.jqxNumberInput({decimalSeparator:this.gridlocalization.decimalseparator,decimalDigits:K,inputMode:"simple",theme:this.theme,rtl:this.rtl,width:g.width()-1,height:g.height()-1,spinButtons:true,symbol:m,symbolPosition:y});this.editors["numberinput_"+D]=d;if(G.createeditor){G.createeditor(q,H,d)}}if(G._requirewidthupdate){d.jqxNumberInput({width:g.width()-2})}if(I){if(H!=""&&H!=null){var M=H;d.jqxNumberInput("setDecimal",M)}else{d.jqxNumberInput("setDecimal",0)}if(this.editcell.defaultvalue!=undefined){d.jqxNumberInput("setDecimal",this.editcell.defaultvalue)}if(this.editchar&&this.editchar.length>0){var p=parseInt(this.editchar);if(!isNaN(p)){d.jqxNumberInput("setDecimal",p)}}if(w){setTimeout(function(){l(d.jqxNumberInput("numberInput"));d.jqxNumberInput("_setSelectionStart",0);if(s.editchar){if(G.cellsformat.length>0){d.jqxNumberInput("_setSelectionStart",2)}else{d.jqxNumberInput("_setSelectionStart",1)}s.editchar=null}else{var N=d.jqxNumberInput("spinButtons");if(N){var O=d.jqxNumberInput("numberInput").val();s._setSelection(d.jqxNumberInput("numberInput")[0],O.length,O.length)}else{var O=d.jqxNumberInput("numberInput").val();s._setSelection(d.jqxNumberInput("numberInput")[0],0,O.length)}}},10)}}}break;case"custom":n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}var B=this.editors["customeditor_"+D+"_"+q];d=B==undefined?a("<div style='overflow: hidden; border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='customeditor'></div>"):B;d.show();d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(B==undefined){d.prependTo(this.table);d[0].id="customeditor"+this.element.id+D+"_"+q;this.editors["customeditor_"+D+"_"+q]=d;var b=g.width()-1;var e=g.height()-1;d.width(b);d.height(e);if(G.createeditor){G.createeditor(q,H,d,C,b,e,this.editchar)}}if(G._requirewidthupdate){d.width(g.width()-2)}break;case"template":n.innerHTML="";var D=a.trim(G.datafield).split(" ").join("");if(D.indexOf(".")!=-1){D=D.replace(".","")}var h=this.editors["templateeditor_"+D];d=h==undefined?a("<div style='overflow: hidden; border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='templateeditor'></div>"):h;d.show();d.css("top",a(n).parent().position().top);if(this.oldhscroll){d.css("left",-i+parseInt(a(n).position().left))}else{d.css("left",parseInt(a(n).position().left))}if(G.pinned){d.css("left",i+parseInt(a(n).position().left))}if(h==undefined){d.prependTo(this.table);d[0].id="templateeditor"+this.element.id+D;this.editors["templateeditor_"+D]=d;var b=g.width()-1;var e=g.height()-1;d.width(b);d.height(e);if(G.createeditor){G.createeditor(q,H,d,C,b,e,this.editchar)}}if(G._requirewidthupdate){d.width(g.width()-2)}break;case"textbox":default:n.innerHTML="";d=this.editors["textboxeditor_"+G.datafield]||a("<input 'type='textbox' id='textboxeditor'/>");d[0].id="textboxeditor"+this.element.id+G.datafield;d.appendTo(g);if(this.rtl){d.css("direction","rtl")}if(I){d.addClass(this.toThemeProperty("jqx-input"));d.addClass(this.toThemeProperty("jqx-widget-content"));if(this.editchar&&this.editchar.length>0){d.val(this.editchar)}else{if(G.cellsformat!=""){H=this.getcelltext(q,E)}d.val(H)}if(this.editcell.defaultvalue!=undefined){d.val(this.editcell.defaultvalue)}d.width(g.width()+1);d.height(g.height()+1);if(G.createeditor){G.createeditor(q,H,d)}if(G.cellsformat!=""){if(G.cellsformat.indexOf("p")!=-1||G.cellsformat.indexOf("c")!=-1||G.cellsformat.indexOf("n")!=-1||G.cellsformat.indexOf("f")!=-1){if(!this.editors["textboxeditor_"+G.datafield]){d.keydown(function(O){var U=O.charCode?O.charCode:O.keyCode?O.keyCode:0;var R=String.fromCharCode(U);var S=parseInt(R);if(isNaN(S)){return true}if(s._selection(d).length>0){return true}var Q="";var P=d.val();if(G.cellsformat.length>1){var T=parseInt(G.cellsformat.substring(1));if(isNaN(T)){T=0}}else{var T=0}if(T>0){if(P.indexOf(s.gridlocalization.decimalseparator)!=-1){if(s._selection(d).start>P.indexOf(s.gridlocalization.decimalseparator)){return true}}}for(var V=0;V<P.length-T;V++){var N=P.substring(V,V+1);if(N.match(/^[0-9]+$/)!=null){Q+=N}}if(Q.length>=11){return false}})}}}}this.editors["textboxeditor_"+G.datafield]=d;if(I){if(w){setTimeout(function(){l(d);if(s.editchar){s._setSelection(d[0],1,1);s.editchar=null}else{s._setSelection(d[0],0,d.val().length)}},10)}}break}if(I){if(G.initeditor){G.initeditor(q,H,d,C,this.editchar)}}if(d){d[0].style.zIndex=1+n.style.zIndex;if(a.jqx.browser.msie&&a.jqx.browser.version<8){d[0].style.zIndex=1+this.columns.records.length+n.style.zIndex}d.css("display","block");this.editcell.editor=d}setTimeout(function(){if(s.content){s.content[0].scrollTop=0;s.content[0].scrollLeft=0}if(s.gridcontent){s.gridcontent[0].scrollLeft=0;s.gridcontent[0].scrollTop=0}},10)},_setSelection:function(d,g,b){try{if("selectionStart" in d){d.setSelectionRange(g,b)}else{var c=d.createTextRange();c.collapse(true);c.moveEnd("character",b);c.moveStart("character",g);c.select()}}catch(e){var f=e}},_hideeditors:function(){if(this.editcells!=null){var b=this;for(var c in this.editcells){b.editcell=b.editcells[c];b._hidecelleditor()}}},_hidecelleditor:function(b){if(!this.editcell){return}if(this.editcell.columntype=="checkbox"){return}if(this.editcell.editor){this.editcell.editor.hide();switch(this.editcell.columntype){case"dropdownlist":this.editcell.editor.jqxDropDownList({closeDelay:0});this.editcell.editor.jqxDropDownList("hideListBox");this.editcell.editor.jqxDropDownList({closeDelay:300});break;case"combobox":this.editcell.editor.jqxComboBox({closeDelay:0});this.editcell.editor.jqxComboBox("hideListBox");this.editcell.editor.jqxComboBox({closeDelay:300});break;case"datetimeinput":var c=this.editcell.editor;if(c.jqxDateTimeInput("isOpened")){c.jqxDateTimeInput({closeDelay:0});c.jqxDateTimeInput("hideCalendar");c.jqxDateTimeInput({closeDelay:300})}break}}if(this.validationpopup){this.validationpopup.hide();this.validationpopuparrow.hide()}if(!this.isNestedGrid){if(b!=false){this.element.focus()}}},_geteditorvalue:function(h){var l=new String();if(this.editcell.editor){switch(h.columntype){case"textbox":default:l=this.editcell.editor.val();if(h.cellsformat!=""){var k="string";var e=this.source.datafields||((this.source._source)?this.source._source.datafields:null);if(e){var m="";a.each(e,function(){if(this.name==h.displayfield){if(this.type){m=this.type}return false}});if(m){k=m}}var i=k==="number"||k==="float"||k==="int"||k==="integer";var f=k==="date"||k==="time";if(i||(k==="string"&&(h.cellsformat.indexOf("p")!=-1||h.cellsformat.indexOf("c")!=-1||h.cellsformat.indexOf("n")!=-1||h.cellsformat.indexOf("f")!=-1))){if(l===""&&h.nullable){return""}if(l.indexOf(this.gridlocalization.currencysymbol)>-1){l=l.replace(this.gridlocalization.currencysymbol,"")}l=l.replace(this.gridlocalization.decimalseparator,".");if(l.indexOf(this.gridlocalization.percentsymbol)>-1){l=l.replace(this.gridlocalization.percentsymbol,"")}var d="";for(var o=0;o<l.length;o++){var b=l.substring(o,o+1);if(b==="-"){d+="-"}if(b==="."){d+="."}if(b.match(/^[0-9]+$/)!=null){d+=b}}l=d;l=l.replace(/ /g,"");l=new Number(l);if(isNaN(l)){l=""}}if(f||(k==="string"&&(h.cellsformat.indexOf("H")!=-1||h.cellsformat.indexOf("m")!=-1||h.cellsformat.indexOf("M")!=-1||h.cellsformat.indexOf("y")!=-1||h.cellsformat.indexOf("h")!=-1||h.cellsformat.indexOf("d")!=-1))){if(l===""&&h.nullable){return""}var c=l;l=new Date(l);if(l=="Invalid Date"||l==null){if(a.jqx.dataFormat){l=a.jqx.dataFormat.tryparsedate(c,this.gridlocalization)}if(l=="Invalid Date"||l==null){l=""}}}}if(h.displayfield!=h.datafield){l={text:l,value:l}}break;case"datetimeinput":if(this.editcell.editor.jqxDateTimeInput){this.editcell.editor.jqxDateTimeInput({isEditing:false});l=this.editcell.editor.jqxDateTimeInput("getDate");if(l==null){return null}l=new Date(l.toString());if(h.displayfield!=h.datafield){l={text:l,value:l}}}break;case"dropdownlist":if(this.editcell.editor.jqxDropDownList){var g=this.editcell.editor.jqxDropDownList("selectedIndex");var n=this.editcell.editor.jqxDropDownList("listBox").getVisibleItem(g);if(h.displayfield!=h.datafield){if(n){l={text:n.text,value:n.value}}else{l=""}}else{if(n){l=n.text}else{l=""}}if(l==null){l=""}}break;case"combobox":if(this.editcell.editor.jqxComboBox){l=this.editcell.editor.jqxComboBox("val");if(h.displayfield!=h.datafield){var n=this.editcell.editor.jqxComboBox("getSelectedItem");if(n!=null){l={text:l,value:n.value}}}if(l==null){l=""}}break;case"numberinput":if(this.editcell.editor.jqxNumberInput){if(this.touchdevice){this.editcell.editor.jqxNumberInput("_doTouchHandling")}var j=this.editcell.editor.jqxNumberInput("getDecimal");l=new Number(j);l=parseFloat(l);if(isNaN(l)){l=0}if(h.displayfield!=h.datafield){l={text:l,value:l}}}break}if(h.geteditorvalue){l=h.geteditorvalue(this.editcell.row,this.editcell.value,this.editcell.editor)}}return l},hidevalidationpopups:function(){if(this.popups){a.each(this.popups,function(){this.validation.remove();this.validationrow.remove()});this.popups=new Array()}if(this.validationpopup){this.validationpopuparrow.hide();this.validationpopup.hide()}},showvalidationpopup:function(n,d,o){if(o==undefined){var o=this.gridlocalization.validationstring}var m=a("<div style='z-index: 99999; top: 0px; left: 0px; position: absolute;'></div>");var l=a("<div style='width: 20px; height: 20px; z-index: 999999; top: 0px; left: 0px; position: absolute;'></div>");m.html(o);l.addClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));m.addClass(this.toThemeProperty("jqx-grid-validation"));m.addClass(this.toThemeProperty("jqx-rc-all"));m.prependTo(this.table);l.prependTo(this.table);var f=this.hScrollInstance;var h=f.value;var e=parseInt(h);var j=this.getcolumn(d).uielement;var i=a(this.hittestinfo[n].visualrow);m.css("top",parseInt(i.position().top)+30+"px");var b=parseInt(m.css("top"));l.css("top",b-12);l.removeClass();l.addClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));if(b>=this._gettableheight()){l.removeClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));l.addClass(this.toThemeProperty("jqx-grid-validation-arrow-down"));b=parseInt(i.position().top)-this.rowsheight-5;m.css("top",b+"px");l.css("top",b+m.outerHeight()-9)}var k=-e+parseInt(a(j).position().left);l.css("left",e+k+30);var c=m.width();if(c+k>this.host.width()-20){var g=c+k-this.host.width()+40;k-=g}m.css("left",e+k);m.show();l.show();if(!this.popups){this.popups=new Array()}this.popups[this.popups.length]={validation:m,validationrow:l}},_showvalidationpopup:function(n,d,o){var i=this.editcell.editor;if(!i){return}if(!this.validationpopup){var l=a("<div style='z-index: 99999; top: 0px; left: 0px; position: absolute;'></div>");var k=a("<div style='width: 20px; height: 20px; z-index: 999999; top: 0px; left: 0px; position: absolute;'></div>");l.html(o);k.addClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));l.addClass(this.toThemeProperty("jqx-grid-validation"));l.addClass(this.toThemeProperty("jqx-rc-all"));l.prependTo(this.table);k.prependTo(this.table);this.validationpopup=l;this.validationpopuparrow=k}else{this.validationpopup.html(o)}var f=this.hScrollInstance;var h=f.value;var e=parseInt(h);this.validationpopup.css("top",parseInt(a(this.editcell.element).parent().position().top)+(this.rowsheight+5)+"px");var b=parseInt(this.validationpopup.css("top"));this.validationpopuparrow.css("top",b-12);this.validationpopuparrow.removeClass();this.validationpopuparrow.addClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));var m=this._gettableheight();if(b>=m){this.validationpopuparrow.removeClass(this.toThemeProperty("jqx-grid-validation-arrow-up"));this.validationpopuparrow.addClass(this.toThemeProperty("jqx-grid-validation-arrow-down"));b=parseInt(a(this.editcell.element).parent().position().top)-this.rowsheight-5;this.validationpopup.css("top",b+"px");this.validationpopuparrow.css("top",b+this.validationpopup.outerHeight()-9)}var j=-e+parseInt(a(this.editcell.element).position().left);this.validationpopuparrow.css("left",e+j+30);var c=this.validationpopup.width();if(c+j>this.host.width()-20){var g=c+j-this.host.width()+40;j-=g}this.validationpopup.css("left",e+j);this.validationpopup.show();this.validationpopuparrow.show()}})})(jQuery);