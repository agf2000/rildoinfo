<%@ control language="vb" autoeventwireup="false" codebehind="Agenda.ascx.vb" inherits="RIW.Modules.Store_Manager.Views.Agenda" %>

<div id="agenda" class="container-fluid Normal">
    <div class="span3">
        <div id="kcal1"></div>
        <div id="kcal2"></div>
    </div>
    <div class="span9">
        <div id="scheduler">
        </div>
    </div>
    <div class="clearfix"></div>
</div>
<script id="editor" type="text/x-kendo-template">
   <h3>Edit meeting</h3>
   <p>
       <label>Title: <input name="title" /></label>
   </p>
   <p>
       <label>Start: <input data-role="datetimepicker" name="start" /></label>
   </p>
   <p>
       <label>Start: <input data-role="datetimepicker" name="end" /></label>
   </p>
</script>
<script id="template" type="text/x-kendo-template">

        <strong>event start:</strong> #=kendo.format('{0:d}',start)#<br />
        <strong>event end:</strong> #=kendo.format('{0:d}',end)#<br />
        <strong>event description:</strong> #=description#<br />

</script>