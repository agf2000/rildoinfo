YUI.add("aui-parse-content",function(e,t){var n=e.Lang,r=n.isString,i=e.config.doc,s="append",o="documentElement",u="firstChild",a="head",f="host",l="innerHTML",c="<div>_</div>",h="ParseContent",p="queue",d="script",v=";",m="src",g={"":1,"text/javascript":1},y=e.Component.create({NAME:h,NS:h,ATTRS:{queue:{value:null}},EXTENDS:e.Plugin.Base,prototype:{initializer:function(){var t=this;y.superclass.initializer.apply(this,arguments),t.set(p,new e.AsyncQueue),t._bindAOP()},globalEval:function(t){var r=e.getDoc(),s=r.one(a)||r.get(o),u=i.createElement(d);u.type="text/javascript",t&&(u.text=n.trim(t)),s.appendChild(u).remove()},parseContent:function(e){var t=this,n=t._clean(e);return t._dispatch(n),n},_addInlineScript:function(e){var t=this;t.get(p).add({args:e,context:t,fn:t.globalEval,timeout:0})},_bindAOP:function(){var t=this,n=function(n){var r=Array.prototype.slice.call(arguments),i=t.parseContent(n);return r.splice(0,1,i.fragment),new e.Do.AlterArgs(null,r)};this.doBefore("insert",n),this.doBefore("replaceChild",n);var r=function(n){var r=t.parseContent(n);return new e.Do.AlterArgs(null,[r.fragment])};this.doBefore("replace",r),this.doBefore("setContent",r)},_clean:function(t){var n={},i=e.Node.create("<div></div>");return r(t)?(t=c+t,e.DOM.addHTML(i,t,s)):(i.append(c),i.append(t)),n.js=i.all(d).filter(function(e){return g[e.getAttribute("type").toLowerCase()]}),n.js.each(function(e,t){e.remove()}),i.get(u).remove(),n.fragment=i.get("childNodes").toFrag(),n},_dispatch:function(t){var n=this,r=n.get(p),i=[];t.js.each(function(t,s){var o=t.get(m);if(o)i.length&&(n._addInlineScript(i.join(v)),i.length=0),r.add({autoContinue:!1,fn:function(){e.Get.script(o,{onEnd:function(e){e.purge(),r.run()}})},timeout:0});else{var u=t._node;i.push(u.text||u.textContent||u.innerHTML||"")}}),i.length&&n._addInlineScript(i.join(v)),r.run()}}});e.namespace("Plugin").ParseContent=y},"2.0.0pr7",{requires:["async-queue","plugin","io-base","aui-component","aui-node-base"]});
