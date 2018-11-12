YUI.add("aui-audio",function(e,t){var n=e.Object,r=e.Lang,i=e.UA,s=e.config.doc,t="audio",o=e.getClassName,u=o(t),a=o(t,"node"),f=e.config.base+"aui-audio/assets/player.swf",l="fixedAttributes",c="flashVars",h="mp3",p="oggUrl",d="src",v="swfUrl",m="url",g=/\.([^\.]+)$/,y=e.Component.create({NAME:t,ATTRS:{url:{value:"",validator:r.isString},oggUrl:{value:"",validator:r.isString},type:{value:h,validator:r.isString},swfWidth:{value:"100%",validator:r.isString},swfHeight:{value:"30",validator:r.isString},swfUrl:{value:f,validator:r.isString},fixedAttributes:{value:{},validator:r.isObject},flashVars:{value:{},validator:r.isObject},render:{value:!0,validator:r.isBoolean}},BIND_UI_ATTRS:[m,p,v,l,c],SYNC_UI_ATTRS:[m,p],prototype:{renderUI:function(){var t=this;t._renderAudioTask=e.debounce(t._renderAudio,1,t),t._renderSwfTask=e.debounce(t._renderSwf,1,t),t._renderAudio(!t.get(p))},bindUI:function(){var e=this;e.publish("audioReady",{fireOnce:!0})},_createSource:function(t){var n=new e.Node(s.createElement("source"));return n.attr("type",t),n},_renderSwf:function(){var t=this,s=t.get(v);if(s){var o=t.get(c);t._setMedia(o);var u=e.QueryString.stringify(o);t._swfId?t._audio.removeChild(e.one("#"+t._swfId)):t._swfId=e.guid();var a='type="application/x-shockwave-flash" data="'+s+'"',f="";i.ie&&(a='classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"',f='<param name="movie" value="'+s+'"/>');var h=t.get(l),p=[];for(var d in h)n.owns(h,d)&&p.push('<param name="',d,'" value="',h[d],'" />');var m="";u&&(m='<param name="flashVars" value="'+u+'" />');var g=t.get("swfHeight"),b=t.get("swfWidth"),w=r.sub(y.TPL_FLASH,{applicationType:a,id:t._swfId,fixedAttributes:p.join(""),flashVars:m,height:g,movie:f,width:b});t._audio.append(w)}},_renderAudio:function(t){var n=this,s=y.TPL_AUDIO;i.gecko&&t&&(s=y.TPL_AUDIO_FALLBACK);var o=r.sub(s,[e.guid()]),u=e.Node.create(o);return n.get("contentBox").append(u),n._audio=u,u},_setMedia:function(e){var t=this;if(!n.owns(e,h)&&!n.owns(e,"mp4")&&!n.owns(e,"flv")){var r=t.get(m),i=t.get("type");if(!i){var s=g.exec(r);s&&(i=s[1])}e[i]=r}},_uiSetFixedAttributes:function(e){var t=this;t._renderSwfTask()},_uiSetFlashVars:function(e){var t=this;t._renderSwfTask()},_uiSetOggUrl:function(e){var t=this;if(i.gecko||i.opera){var n=t._audio,r=t._usingAudio();if(!e&&r||e&&!r)n.remove(!0),n=t._renderAudio(!e);if(!e)t._renderSwfTask();else{var s=t._sourceOgg;s||(s=t._createSource("audio/ogg"),n.append(s),t._sourceOgg=s),s.attr(d,e)}}},_uiSetSwfUrl:function(e){var t=this;t._renderSwfTask()},_uiSetUrl:function(e){var t=this,n=t.get(p),r=t._audio,s=t._sourceMp3;if(i.gecko&&!t._usingAudio())s!=null&&(s.remove(!0),t._sourceMp3=null);else if(r||!n)s||(s=t._createSource("audio/mp3"),r.append(s),t._sourceMp3=s),s.attr(d,e);t._renderSwfTask()},_usingAudio:function(){var e=this;return e._audio.get("nodeName").toLowerCase()=="audio"}}});y.TPL_AUDIO='<audio id="{0}" controls class="'+a+'"></audio>',y.TPL_AUDIO_FALLBACK='<div class="'+a+'"></div>',y.TPL_FLASH='<object id="{id}" {applicationType} height="{height}" width="{width}">{movie}{fixedAttributes}{flashVars}</object>',e.Audio=y},"2.0.0pr7",{requires:["aui-node","aui-component","querystring-stringify-simple"],skinnable:!0});
