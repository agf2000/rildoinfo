YUI.add("cache-offline",function(e,t){function n(){n.superclass.constructor.apply(this,arguments)}var r=null,i=e.JSON;try{r=e.config.win.localStorage}catch(s){}e.mix(n,{NAME:"cacheOffline",ATTRS:{sandbox:{value:"default",writeOnce:"initOnly"},expires:{value:864e5},max:{value:null,readOnly:!0},uniqueKeys:{value:!0,readOnly:!0,setter:function(){return!0}}},flushAll:function(){var e=r,t;if(e)if(e.clear)e.clear();else for(t in e)e.hasOwnProperty(t)&&(e.removeItem(t),delete e[t])}}),e.extend(n,e.Cache,r?{_setMax:function(e){return null},_getSize:function(){var e=0,t=0,n=r.length;for(;t<n;++t)r.key(t).indexOf(this.get("sandbox"))===0&&e++;return e},_getEntries:function(){var e=[],t=0,n=r.length,s=this.get("sandbox");for(;t<n;++t)r.key(t).indexOf(s)===0&&(e[t]=i.parse(r.key(t).substring(s.length)));return e},_defAddFn:function(e){var t=e.entry,n=t.request,s=t.cached,o=t.expires;t.cached=s.getTime(),t.expires=o?o.getTime():o;try{r.setItem(this.get("sandbox")+i.stringify({request:n}),i.stringify(t))}catch(u){this.fire("error",{error:u})}},_defFlushFn:function(e){var t,n=r.length-1;for(;n>-1;--n)t=r.key(n),t.indexOf(this.get("sandbox"))===0&&r.removeItem(t)},retrieve:function(e){this.fire("request",{request:e});var t,n,s;try{s=this.get("sandbox")+i.stringify({request:e});try{t=i.parse(r.getItem(s))}catch(o){}}catch(u){}if(t){t.cached=new Date(t.cached),n=t.expires,n=n?new Date(n):null,t.expires=n;if(this._isMatch(e,t))return this.fire("retrieve",{entry:t}),t}return null}}:{_setMax:function(e){return null}}),e.CacheOffline=n},"patched-dev-3.x",{requires:["cache-base","json"]});
