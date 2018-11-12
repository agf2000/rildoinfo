YUI.add("aui-diagram-builder-impl",function(e,t){var n=e.Lang,r=n.isArray,i=n.isBoolean,s=n.isObject,o=n.isString,u=e.WidgetStdMod,a=e.Array,f="activeElement",l="attributeName",c="availableField",h="availableFields",p="backspace",d="boolean",v="boundary",m="boundingBox",g="builder",y="canvas",b="click",w="condition",E="connector",S="connectors",x="content",T="controls",N="controlsToolbar",C="createDocumentFragment",k="data",L="data-nodeId",A="delete",O="deleteConnectorsMessage",M="deleteNodesMessage",_="description",D="diagram",P="diagram-builder",H="diagramNode",B="diagram-node-manager",j="diagram-node",F="dragNode",I="dragging",q="editing",R="end",U="esc",z="field",W="fields",X="fieldsDragConfig",V="fork",$="graphic",J="height",K="highlightBoundaryStroke",Q="highlightDropZones",G="highlighted",Y="id",Z="join",et="keydown",tt="label",nt="lock",rt="mouseenter",it="mouseleave",t="name",st="node",ot="p1",ut="p2",at="parentNode",ft="radius",lt="rendered",ct="required",ht="selected",pt="shape",dt="shapeBoundary",vt="shapeInvite",mt="showSuggestConnector",gt="source",yt="start",bt="state",wt="stroke",Et="suggest",St="suggestConnectorOverlay",xt="target",Tt="task",Nt="transition",Ct="transitions",kt="type",Lt="value",At="visible",Ot="width",Mt="xy",_t="zIndex",Dt="-",Pt=".",Ht="",Bt="_",jt=e.getClassName,Ft=jt(D,g,T),It=jt(D,g,z),qt=jt(D,st),Rt=jt(D,st,x),Ut=jt(D,st,q),zt=jt(D,st,tt),Wt=jt(D,st,ht),Xt=jt(D,st,pt,v),Vt=jt(D,st,Et,E),$t=function(e,t){var n=r(e)?e:e.get(m).getXY();return[n[0]+t[0],n[1]+t[1]]},Jt=function(e,t){var n=t[0]-e[0],r=t[1]-e[1];return Math.sqrt(n*n+r*r)},Kt=function(e,t){var n=e.hotPoints,r=t.hotPoints,i=e.get(m).getXY(),s=t.get(m).getXY(),o,u,a,f,l=Infinity,c=[[0,0],[0,0]];for(a=0,o=n.length;a<o;a++){var h=n[a],p=$t(i,h);for(f=0,u=r.length;f<u;f++){var d=r[f],v=$t(s,d),g=Jt(v,p);g<l&&(c[0]=h,c[1]=d,l=g)}}return c},Qt=function(e,t){var n=r(t)?t:t.getXY(),i=r(e)?e:e.getXY();return a.map(i,function(e,t){return Math.max(0,e-n[t])})},Gt=function(t){return e.instanceOf(t,e.Connector)},Yt=function(t){return e.instanceOf(t,e.Map)},Zt=function(t){return e.instanceOf(t,e.DiagramBuilderBase)},en=function(t){return e.instanceOf(t,e.DiagramNode)},tn=e.Component.create({NAME:P,ATTRS:{connector:{setter:"_setConnector",value:null},fieldsDragConfig:{value:null,setter:"_setFieldsDragConfig",validator:s},graphic:{valueFn:function(){return new e.Graphic},validator:s},highlightDropZones:{validator:i,value:!0},strings:{value:{addNode:"Add node",cancel:"Cancel",close:"Close",deleteConnectorsMessage:"Are you sure you want to delete the selected connector(s)?",deleteNodesMessage:"Are you sure you want to delete the selected node(s)?",propertyName:"Property Name",save:"Save",settings:"Settings",value:"Value"}},showSuggestConnector:{validator:i,value:!0},suggestConnectorOverlay:{value:null,setter:"_setSuggestConnectorOverlay"}},EXTENDS:e.DiagramBuilderBase,FIELDS_TAB:0,SETTINGS_TAB:1,prototype:{editingConnector:null,editingNode:null,publishedSource:null,publishedTarget:null,selectedConnector:null,selectedNode:null,initializer:function(){var t=this;t.after({render:t.syncConnectionsUI}),t.on({cancel:t._onCancel,"drag:drag":t._onDrag,"drag:end":t._onDragEnd,"drop:hit":t._onDropHit,save:t._onSave}),e.DiagramNodeManager.on({publishedSource:function(e){t.publishedTarget=null,t.publishedSource=e.publishedSource}}),t.handlerKeyDown=e.getDoc().on(et,e.bind(t._afterKeyEvent,t)),t.dropContainer.delegate(b,e.bind(t._onNodeClick,t),Pt+qt),t.dropContainer.delegate(rt,e.bind(t._onNodeMouseEnter,t),Pt+qt),t.dropContainer.delegate(it,e.bind(t._onNodeMouseLeave,t),Pt+qt)},renderUI:function(){var t=this;e.DiagramBuilder.superclass.renderUI.apply(this,arguments),t._renderGraphic()},syncUI:function(){var t=this;e.DiagramBuilder.superclass.syncUI.apply(this,arguments),t._setupFieldsDrag(),t.connector=t.get(E)},syncConnectionsUI:function(){var e=this;e.get(W).each(function(e){e.syncConnectionsUI()})},clearFields:function(){var e=this,t=[];e.get(W).each(function(e){t.push(e)}),a.each(t,function(e){e.destroy()}),t=e.editingConnector=e.editingNode=e.selectedNode=null},closeEditProperties:function(){var t=this,n=t.editingNode,r=t.tabView;r.selectChild(e.DiagramBuilder.FIELDS_TAB),r.disableTab(e.DiagramBuilder.SETTINGS_TAB),n&&n.get(m).removeClass(Ut),t.editingConnector=t.editingNode=null},connect:function(n,r,i){var s=this;return o(n)&&(n=e.DiagramNode.getNodeByName(n)),o(r)&&(r=e.DiagramNode.getNodeByName(r)),n&&r&&n.connect(r.get(t),i),s},connectAll:function(e){var t=this;return a.each(e,function(e){e.hasOwnProperty(gt)&&e.hasOwnProperty(xt)&&t.connect(e.source,e.target,e.connector)}),t},createField:function(e){var t=this;return en(e)||(e.builder=t,e.bubbleTargets=t,e=new(t.getFieldClass(e.type||st))(e)),e},deleteSelectedConnectors:function(){var t=this,n=t.getStrings(),r=t.getSelectedConnectors();r.length&&confirm(n[O])&&(a.each(r,function(t){var n=t.get(Nt);e.DiagramNode.getNodeByName(n.source).disconnect(n)}),t.stopEditing())},deleteSelectedNode:function(){var e=this,t=e.getStrings(),n=e.selectedNode;n&&!n.get(ct)&&confirm(t[M])&&(n.close(),e.editingNode=e.selectedNode=null,e.stopEditing())},destructor:function(e){var t=this;t.get(St).destroy()},eachConnector:function(e){var t=this;t.get(W).each(function(n){var r=n.get(Ct);a.each(r.values(),function(r){e.call(t,n.getConnector(r),r,n)})})},editConnector:function(t){var n=this;if(t){var r=n.tabView;n.closeEditProperties(),r.enableTab(e.DiagramBuilder.SETTINGS_TAB),r.selectChild(e.DiagramBuilder.SETTINGS_TAB),n.propertyList.set(k,t.getProperties()),n.editingConnector=n.selectedConnector=t}},editNode:function(t){var n=this;if(t){var r=n.tabView;n.closeEditProperties(),r.enableTab(e.DiagramBuilder.SETTINGS_TAB),r.selectChild(e.DiagramBuilder.SETTINGS_TAB),n.propertyList.set(k,t.getProperties()),t.get(m).addClass(Ut),n.editingNode=n.selectedNode=t}},getFieldClass:function(t){var n=this,r=e.DiagramBuilder.types[t];return r?r:(e.log("The field type: ["+t+"] couldn't be found."
),null)},getNodesByTransitionProperty:function(e,t){var n=this,r=[],i;return n.get(W).each(function(n){i=n.get(Ct),a.each(i.values(),function(i){if(i[e]===t)return r.push(n),!1})}),r},getSelectedConnectors:function(){var e=this,t=[];return e.eachConnector(function(e){e.get(ht)&&t.push(e)}),t},getSourceNodes:function(e){var n=this;return n.getNodesByTransitionProperty(xt,e.get(t))},hideSuggestConnetorOverlay:function(e,t){var n=this;n.connector.hide(),n.get(St).hide();try{n.fieldsDrag.dd.set(nt,!1)}catch(r){}},isAbleToConnect:function(){var e=this;return!!e.publishedSource&&!!e.publishedTarget},isFieldsDrag:function(e){var t=this;return e===t.fieldsDrag.dd},plotField:function(e){var t=this;e.get(lt)||e.render(t.dropContainer)},select:function(e){var t=this;t.unselectNodes(),t.selectedNode=e.set(ht,!0).focus()},showSuggestConnetorOverlay:function(e){var t=this,n=t.get(St),r=n.get(m);n.get(m).addClass(Vt),n.set(Mt,e||t.connector.get(ut)).show();try{t.fieldsDrag.dd.set(nt,!0)}catch(i){}},stopEditing:function(){var e=this;e.unselectConnectors(),e.unselectNodes(),e.closeEditProperties()},toJSON:function(){var e=this,t={nodes:[]};return e.get(W).each(function(e){var n={transitions:[]},r=e.get(Ct);a.each(e.SERIALIZABLE_ATTRS,function(t){n[t]=e.get(t)}),a.each(r.values(),function(t){var r=e.getConnector(t);t.connector=r.toJSON(),n.transitions.push(t)}),t.nodes.push(n)}),t},unselectConnectors:function(){var e=this;a.each(e.getSelectedConnectors(),function(e){e.set(ht,!1)})},unselectNodes:function(){var e=this,t=e.selectedNode;t&&t.set(ht,!1),e.selectedNode=null},_afterKeyEvent:function(t){var n=this;if(t.hasModifier()||e.getDoc().get(f).test(":input,td"))return;t.isKey(U)?n._onEscKey(t):(t.isKey(p)||t.isKey(A))&&n._onDeleteKey(t)},_onCancel:function(e){var t=this;t.closeEditProperties()},_onDeleteKey:function(t){var n=this;en(e.Widget.getByNode(t.target))&&(n.deleteSelectedConnectors(),n.deleteSelectedNode(),t.halt())},_onDrag:function(t){var n=this,r=t.target;if(n.isFieldsDrag(r)){var i=e.Widget.getByNode(r.get(F));i.alignTransitions(),a.each(n.getSourceNodes(i),function(e){e.alignTransitions()})}},_onDragEnd:function(t){var n=this,r=t.target,i=e.Widget.getByNode(r.get(F));i&&n.isFieldsDrag(r)&&i.set(Mt,i.getLeftTop())},_onDropHit:function(e){var t=this,n=e.drag;if(t.isAvailableFieldsDrag(n)){var r=n.get(st).getData(c),i=t.addField({xy:Qt(n.lastXY,t.dropContainer),type:r.get(kt)});t.select(i)}},_onEscKey:function(e){var t=this;t.hideSuggestConnetorOverlay(),t.stopEditing(),e.halt()},_onCanvasClick:function(e){var t=this;t.stopEditing(),t.hideSuggestConnetorOverlay()},_onNodeClick:function(t){var n=this,r=e.Widget.getByNode(t.currentTarget);n.select(r),n._onNodeEdit(t),t.stopPropagation()},_onNodeEdit:function(t){var n=this;if(!t.target.ancestor(Pt+Rt,!0))return;var r=e.Widget.getByNode(t.currentTarget);r&&n.editNode(r)},_onNodeMouseEnter:function(t){var n=this,r=e.Widget.getByNode(t.currentTarget);r.set(G,!0)},_onNodeMouseLeave:function(t){var n=this,r=n.publishedSource,i=e.Widget.getByNode(t.currentTarget);(!r||!r.boundaryDragDelegate.dd.get(I))&&i.set(G,!1)},_onSave:function(e){var t=this,n=t.editingNode,r=t.editingConnector,i=t.propertyList.get(k);n?i.each(function(e){n.set(e.get(l),e.get(Lt))}):r&&i.each(function(e){r.set(e.get(l),e.get(Lt))})},_onSuggestConnectorNodeClick:function(e){var t=this,n=e.currentTarget.getData(c),r=t.connector,i=t.addField({type:n.get(kt),xy:r.toCoordinate(r.get(ut))});t.hideSuggestConnetorOverlay(),t.publishedSource.connectNode(i)},_renderGraphic:function(){var t=this,n=t.get($),r=t.get(y);n.render(r),e.one(r).on(b,e.bind(t._onCanvasClick,t))},_setConnector:function(t){var n=this;if(!Gt(t)){var r=n.get(y).getXY();t=new e.Connector(e.merge({builder:n,graphic:n.get($),lazyDraw:!0,p1:r,p2:r,shapeHover:null,showName:!1},t))}return t},_setFieldsDragConfig:function(t){var n=this,r=n.dropContainer;return e.merge({bubbleTargets:n,container:r,dragConfig:{plugins:[{cfg:{constrain:r},fn:e.Plugin.DDConstrained},{cfg:{scrollDelay:150},fn:e.Plugin.DDWinScroll}]},nodes:Pt+qt},t||{})},_setSuggestConnectorOverlay:function(t){var n=this;if(!t){var r=e.getDoc().invoke(C),i,s;a.each(n.get(h),function(e){var t=e.get(st);r.appendChild(t.clone().setData(c,t.getData(c)))}),t=new e.Overlay({bodyContent:r,render:!0,visible:!1,width:280,zIndex:1e4}),i=t.get(m),s=t.get("contentBox"),s.addClass("popover-content"),i.addClass("popover"),i.delegate(b,e.bind(n._onSuggestConnectorNodeClick,n),Pt+It)}return t},_setupFieldsDrag:function(){var t=this;t.fieldsDrag=new e.DD.Delegate(t.get(X))}}});e.DiagramBuilder=tn,e.DiagramBuilder.types={};var nn=e.Component.create({NAME:B,EXTENDS:e.Base});e.DiagramNodeManager=new nn;var rn=e.Component.create({NAME:j,UI_ATTRS:[G,t,ct,ht],ATTRS:{builder:{validator:Zt},connectors:{valueFn:"_connectorsValueFn",writeOnce:!0},controlsToolbar:{validator:s,valueFn:"_controlsToolbarValueFn"},description:{value:Ht,validator:o},graphic:{writeOnce:!0,validator:s},height:{value:60},highlighted:{validator:i,value:!1},name:{valueFn:function(){var t=this;return t.get(kt)+ ++e.Env._uidx},validator:o},required:{value:!1,validator:i},selected:{value:!1,validator:i},shapeBoundary:{validator:s,valueFn:"_valueShapeBoundary"},highlightBoundaryStroke:{validator:s,value:{weight:7,color:"#484B4C",opacity:.25}},shapeInvite:{validator:s,value:{radius:12,type:"circle",stroke:{weight:6,color:"#ff6600",opacity:.8},fill:{color:"#ffd700",opacity:.8}}},strings:{value:{closeMessage:"Close",connectMessage:"Connect",description:"Description",editMessage:"Edit",name:"Name",type:"Type"}},tabIndex:{value:1},transitions:{value:null,writeOnce:!0,setter:"_setTransitions"},type:{value:st,validator:o},width:{value:60},zIndex:{value:100}},EXTENDS:e.Overlay,CIRCLE_POINTS:[[35,20],[28,33],[14,34],[5,22],[10,9],[24,6],[34,16],[31,30],[18,35],[6,26],[7,12],[20,5],[33,12],[34,26],[22,35],[9,30],[6,16],[16,6],[30,9],[35,22],[26,34],[12,33],[5,20],[12,7],[26,6],[35,18],[30,31],[16,34],[6,24],[9,10],[22,5],[34,14],[33,28
],[20,35],[7,28],[6,14],[18,5],[31,10],[34,24],[24,34],[10,31],[5,18],[14,6],[28,8],[35,20],[28,33],[14,34],[5,22],[10,8],[25,6],[34,16],[31,30],[18,35],[6,26],[8,12],[20,5],[33,12],[33,27],[22,35],[8,30],[6,15],[16,6],[30,9],[35,23],[26,34],[12,32],[5,20],[12,7],[27,7],[35,18],[29,32],[15,34]],DIAMOND_POINTS:[[30,5],[35,10],[40,15],[45,20],[50,25],[55,30],[50,35],[45,40],[40,45],[35,50],[30,55],[25,50],[20,45],[15,40],[10,35],[5,30],[10,25],[15,20],[20,15],[25,10]],SQUARE_POINTS:[[5,5],[10,5],[15,5],[20,5],[25,5],[30,5],[35,5],[40,5],[50,5],[55,5],[60,5],[65,5],[65,10],[65,15],[65,20],[65,25],[65,30],[65,35],[65,40],[65,45],[65,50],[65,55],[65,60],[65,65],[60,65],[55,65],[50,65],[45,65],[40,65],[35,65],[30,65],[25,65],[20,65],[15,65],[10,65],[5,65],[5,60],[5,55],[5,50],[5,45],[5,40],[5,35],[5,30],[5,25],[5,20],[5,15],[5,10]],getNodeByName:function(t){return e.Widget.getByNode("[data-nodeId="+e.DiagramNode.buildNodeId(t)+"]")},buildNodeId:function(e){return H+Bt+z+Bt+e.replace(/[^a-z0-9.:_\-]/ig,"_")},prototype:{LABEL_TEMPLATE:'<div class="'+zt+'">{label}</div>',boundary:null,hotPoints:[[0,0]],CONTROLS_TEMPLATE:'<div class="'+Ft+'"></div>',SERIALIZABLE_ATTRS:[_,t,ct,kt,Ot,J,_t,Mt],initializer:function(){var t=this;t.after({"map:remove":e.bind(t._afterMapRemove,t),render:t._afterRender}),t.on({nameChange:t._onNameChange}),t.publish({connectDrop:{defaultFn:t.connectDrop},connectEnd:{defaultFn:t.connectEnd},connectMove:{defaultFn:t.connectMove},connectOutTarget:{defaultFn:t.connectOutTarget},connectOverTarget:{defaultFn:t.connectOverTarget},connectStart:{defaultFn:t.connectStart},boundaryMouseEnter:{},boundaryMouseLeave:{}}),t.get(m).addClass(qt+Dt+t.get(kt))},destructor:function(){var e=this;e.eachConnector(function(e,t,n){n.removeTransition(e.get(Nt))}),e.invite.destroy(),e.get($).destroy(),e.get(g).removeField(e)},addTransition:function(t){var n=this,r=n.get(Ct);return t=n.prepareTransition(t),r.has(t.uid)||(t.uid=e.guid(),r.put(t.uid,t)),t},alignTransition:function(t){var n=this,r=e.DiagramNode.getNodeByName(t.target);if(r){var i=Kt(n,r);t=e.merge(t,{sourceXY:i[0],targetXY:i[1]}),n.getConnector(t).setAttrs({p1:$t(n,t.sourceXY),p2:$t(r,t.targetXY)})}},alignTransitions:function(){var t=this,n=t.get(Ct);a.each(n.values(),e.bind(t.alignTransition,t))},close:function(){var e=this;return e.destroy()},connect:function(t,n){var r=this;t=r.addTransition(t);var i=null,s=e.DiagramNode.getNodeByName(t.target);if(s&&!r.isTransitionConnected(t)){var o=r.get(g),u=Kt(r,s);e.mix(t,{sourceXY:u[0],targetXY:u[1]}),i=new e.Connector(e.merge({builder:o,graphic:o.get($),transition:t},n)),r.get(S).put(t.uid,i)}return r.alignTransition(t),i},connectDrop:function(e){var t=this;t.connectNode(e.publishedTarget)},connectEnd:function(e){var t=this,n=e.target,r=t.get(g),i=r.publishedSource;!r.isAbleToConnect()&&r.get(mt)&&r.connector.get(At)?r.showSuggestConnetorOverlay():(r.connector.hide(),i.invite.set(At,!1)),r.get(Q)&&r.get(W).each(function(e){e.set(G,!1)})},connectMove:function(e){var t=this,n=t.get(g),r=e.mouseXY;n.connector.set(ut,r);if(n.publishedTarget){var i=t.invite,s=i.get(ft)||0;i.get(At)||i.set(At,!0),i.setXY([r[0]-s,r[1]-s])}},connectNode:function(e){var n=this,r=n.boundaryDragDelegate.dd;n.connect(n.prepareTransition({sourceXY:Qt(r.startXY,n.get(m)),target:e.get(t),targetXY:Qt(r.mouseXY,e.get(m))}))},connectOutTarget:function(e){var t=this,n=t.get(g);n.publishedTarget=null,n.publishedSource.invite.set(At,!1)},connectOverTarget:function(e){var t=this,n=t.get(g);n.publishedSource!==t&&(n.publishedTarget=t)},connectStart:function(t){var n=this,r=n.get(g),i=r.get(y);r.connector.show().set(ot,t.startXY),r.get(Q)&&r.get(W).each(function(e){e.set(G,!0)}),e.DiagramNodeManager.fire("publishedSource",{publishedSource:n})},disconnect:function(e){var t=this;t.isTransitionConnected(e)&&t.removeTransition(e)},eachConnector:function(e){var n=this,r=[],i=[].concat(n.get(S).values()),s=i.length;return a.each(n.get(g).getSourceNodes(n),function(e){var s=e.get(S);a.each(s.values(),function(s){n.get(t)===s.get(Nt).target&&(r.push(e),i.push(s))})}),a.each(i,function(t,i){e.call(n,t,i,i<s?n:r[i-s])}),i=r=null,i},getConnector:function(e){var t=this;return t.get(S).getValue(e.uid)},getContainer:function(){var e=this;return e.get(g).dropContainer||e.get(m).get(at)},getLeftTop:function(){var e=this;return Qt(e.get(m),e.getContainer())},getProperties:function(){var e=this,t=e.getPropertyModel();return a.each(t,function(t){var r=e.get(t.attributeName),i=n.type(r);i===d&&(r=String(r)),t.value=r}),t},getPropertyModel:function(){var n=this,r=n.getStrings();return[{attributeName:_,editor:new e.TextAreaCellEditor,name:r[_]},{attributeName:t,editor:new e.TextCellEditor({validator:{rules:{value:{required:!0}}}}),name:r[t]},{attributeName:kt,editor:!1,name:r[kt]}]},isBoundaryDrag:function(e){var t=this;return e===t.boundaryDragDelegate.dd},isTransitionConnected:function(e){var t=this;return t.get(S).has(e.uid)},prepareTransition:function(n){var r=this,i={source:r.get(t),target:null,uid:e.guid()};return o(n)?i.target=n:s(n)&&(i=e.merge(i,n)),i},removeTransition:function(e){var t=this;return t.get(Ct).remove(e.uid)},renderShapeBoundary:function(){var e=this,t=e.boundary=e.get($).addShape(e.get(dt));return t},renderShapeInvite:function(){var e=this,t=e.invite=e.get(g).get($).addShape(e.get(vt));return t.set(At,!1),t},syncConnectionsUI:function(){var e=this,t=e.get(Ct);a.each(t.values(),function(t){e.connect(t,t.connector)})},_afterConnectorRemove:function(e){var t=this;e.value.destroy()},_afterRender:function(e){var t=this;t.setStdModContent(u.BODY,Ht,u.AFTER),t._renderGraphic(),t._renderControls(),t._renderLabel(),t._uiSetHighlighted(t.get(G))},_afterTransitionsRemove:function(e){var t=this;t.get(S).remove(e.value.uid)},_bindBoundaryEvents:function(){var t=this;t.boundary.detachAll().on({mouseenter:e.bind(t._onBoundaryMouseEnter,t),mouseleave:e.bind(t._onBoundaryMouseLeave,t)})},_connectorsValueFn:function(t){var n=this;return new e.Map({after
:{remove:e.bind(n._afterConnectorRemove,n)}})},_controlsToolbarValueFn:function(t){var n=this,r=n.get(Y);return{children:[{icon:"icon-remove",on:{click:e.bind(n._handleCloseEvent,n)}}]}},_handleCloseEvent:function(e){var t=this;t.get(g).deleteSelectedNode()},_handleConnectStart:function(e){var t=this;t.fire("connectStart",{startXY:e})},_handleConnectMove:function(e){var t=this,n=t.get(g);t.fire("connectMove",{mouseXY:e,publishedSource:n.publishedSource})},_handleConnectEnd:function(){var e=this,t=e.get(g),n=t.publishedSource,r=t.publishedTarget;n&&r&&e.fire("connectDrop",{publishedSource:n,publishedTarget:r}),e.fire("connectEnd",{publishedSource:n})},_handleConnectOutTarget:function(){var e=this,t=e.get(g),n=t.publishedSource;n&&e.fire("connectOutTarget",{publishedSource:n})},_handleConnectOverTarget:function(){var e=this,t=e.get(g),n=t.publishedSource;n&&e.fire("connectOverTarget",{publishedSource:n})},_onBoundaryDrag:function(e){var t=this,n=t.boundaryDragDelegate.dd;t._handleConnectMove(n.con._checkRegion(n.mouseXY))},_onBoundaryDragEnd:function(e){var t=this;t._handleConnectEnd(),e.target.get(F).show()},_onBoundaryDragStart:function(e){var t=this;t._handleConnectStart(t.boundaryDragDelegate.dd.startXY),e.target.get(F).hide()},_onBoundaryMouseEnter:function(e){var t=this;t.fire("boundaryMouseEnter",{domEvent:e}),t._handleConnectOverTarget()},_onBoundaryMouseLeave:function(e){var t=this;t.fire("boundaryMouseLeave",{domEvent:e}),t._handleConnectOutTarget()},_onNameChange:function(e){var t=this;t.eachConnector(function(n,r,i){var s=n.get(Nt);s[t===i?gt:xt]=e.newVal,n.set(Nt,s)})},_renderControls:function(){var t=this,n=t.get(m);t.controlsNode=e.Node.create(t.CONTROLS_TEMPLATE).appendTo(n)},_renderControlsToolbar:function(t){var n=this;n.controlsToolbar=(new e.Toolbar(n.get(N))).render(n.controlsNode),n._uiSetRequired(n.get(ct))},_renderGraphic:function(){var t=this;t.set($,new e.Graphic({height:t.get(J),render:t.bodyNode,width:t.get(Ot)})),t.renderShapeInvite(),t.renderShapeBoundary().addClass(Xt),t._bindBoundaryEvents(),t._setupBoundaryDrag()},_renderLabel:function(){var t=this;t.labelNode=e.Node.create(n.sub(t.LABEL_TEMPLATE,{label:t.get("name")})),t.get("contentBox").placeAfter(t.labelNode)},_setTransitions:function(t){var n=this;if(!Yt(t)){var r=new e.Map({after:{remove:e.bind(n._afterTransitionsRemove,n)}});e.Array.each(t,function(t){var i=e.guid();t=s(t)?e.mix(t,{uid:i}):{uid:i,target:t},r.put(i,n.prepareTransition(t))}),t=r}return t},_setupBoundaryDrag:function(){var t=this,n=t.get(g);t.boundaryDragDelegate=new e.DD.Delegate({bubbleTargets:t,container:t.bodyNode,nodes:Pt+Xt,dragConfig:{useShim:!1,plugins:[{cfg:{constrain:n?n.get(y):null},fn:e.Plugin.DDConstrained},{cfg:{scrollDelay:150},fn:e.Plugin.DDWinScroll},{cfg:{borderStyle:"0px",moveOnEnd:!1,resizeFrame:!1},fn:e.Plugin.DDProxy}]},on:{"drag:drag":e.bind(t._onBoundaryDrag,t),"drag:end":e.bind(t._onBoundaryDragEnd,t),"drag:start":e.bind(t._onBoundaryDragStart,t)}}),e.Do.after(t._bindBoundaryEvents,t.boundaryDragDelegate.dd,"_unprep",t)},_uiSetHighlighted:function(e){var t=this;if(t.get(lt)){var n=e?t.get(K):t.get(dt+Pt+wt);n&&t.boundary.set(wt,n)}},_uiSetName:function(t){var n=this,r=n.get(m);r.setAttribute(L,e.DiagramNode.buildNodeId(t)),n.get("rendered")&&n.labelNode.setContent(t)},_uiSetRequired:function(e){var t=this,n=t.controlsToolbar;n},_uiSetSelected:function(e){var t=this;t.get(m).toggleClass(Wt,e),e&&!t.controlsToolbar&&t._renderControlsToolbar()},_uiSetXY:function(e){var t=this,n=t.getContainer().getXY();this._posNode.setXY([e[0]+n[0],e[1]+n[1]])},_valueShapeBoundary:function(){var e=this;return{height:41,type:"rect",stroke:{weight:7,color:"transparent",opacity:0},width:41}}}});e.DiagramNode=rn,e.DiagramBuilder.types[st]=e.DiagramNode,e.DiagramNodeState=e.Component.create({NAME:j,ATTRS:{height:{value:40},type:{value:bt},width:{value:40}},EXTENDS:e.DiagramNode,prototype:{hotPoints:e.DiagramNode.CIRCLE_POINTS,renderShapeBoundary:function(){var e=this,t=e.boundary=e.get($).addShape(e.get(dt));return t.translate(5,5),t},_valueShapeBoundary:function(){var e=this;return{radius:15,type:"circle",stroke:{weight:7,color:"transparent",opacity:0}}}}}),e.DiagramBuilder.types[bt]=e.DiagramNodeState,e.DiagramNodeCondition=e.Component.create({NAME:j,ATTRS:{height:{value:60},type:{value:w},width:{value:60}},EXTENDS:e.DiagramNodeState,prototype:{hotPoints:e.DiagramNode.DIAMOND_POINTS,renderShapeBoundary:function(){var e=this,t=e.boundary=e.get($).addShape(e.get(dt));return t.translate(10,10),t.rotate(45),t},_valueShapeBoundary:e.DiagramNode.prototype._valueShapeBoundary}}),e.DiagramBuilder.types[w]=e.DiagramNodeCondition,e.DiagramNodeStart=e.Component.create({NAME:j,ATTRS:{type:{value:yt}},EXTENDS:e.DiagramNodeState}),e.DiagramBuilder.types[yt]=e.DiagramNodeStart,e.DiagramNodeEnd=e.Component.create({NAME:j,ATTRS:{type:{value:R}},EXTENDS:e.DiagramNodeState}),e.DiagramBuilder.types[R]=e.DiagramNodeEnd,e.DiagramNodeJoin=e.Component.create({NAME:j,ATTRS:{height:{value:60},type:{value:Z},width:{value:60}},EXTENDS:e.DiagramNodeState,prototype:{hotPoints:e.DiagramNode.DIAMOND_POINTS,renderShapeBoundary:e.DiagramNodeCondition.prototype.renderShapeBoundary,_valueShapeBoundary:e.DiagramNode.prototype._valueShapeBoundary}}),e.DiagramBuilder.types[Z]=e.DiagramNodeJoin,e.DiagramNodeFork=e.Component.create({NAME:j,ATTRS:{height:{value:60},type:{value:V},width:{value:60}},EXTENDS:e.DiagramNodeState,prototype:{hotPoints:e.DiagramNode.DIAMOND_POINTS,renderShapeBoundary:e.DiagramNodeCondition.prototype.renderShapeBoundary,_valueShapeBoundary:e.DiagramNode.prototype._valueShapeBoundary}}),e.DiagramBuilder.types[V]=e.DiagramNodeFork,e.DiagramNodeTask=e.Component.create({NAME:j,ATTRS:{height:{value:70},type:{value:Tt},width:{value:70}},EXTENDS:e.DiagramNodeState,prototype:{hotPoints:e.DiagramNode.SQUARE_POINTS,renderShapeBoundary:function(){var e=this,t=e.boundary=e.get($).addShape(e.get(dt));return t.translate(8,8),t},_valueShapeBoundary:function(){var e=
this;return{height:55,type:"rect",stroke:{weight:7,color:"transparent",opacity:0},width:55}}}}),e.DiagramBuilder.types[Tt]=e.DiagramNodeTask},"2.0.0pr7",{requires:["overlay","aui-map","aui-diagram-builder-base","aui-diagram-builder-connector"],skinnable:!0});
