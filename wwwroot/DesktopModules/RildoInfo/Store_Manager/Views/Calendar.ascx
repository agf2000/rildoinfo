<%@ control language="vb" autoeventwireup="false" codebehind="Calendar.ascx.vb" inherits="RIW.Modules.Store_Manager.Views.Calendar" %>
<div id="scheduler"></div>
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