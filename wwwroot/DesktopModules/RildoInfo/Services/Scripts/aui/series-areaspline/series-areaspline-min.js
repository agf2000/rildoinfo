YUI.add("series-areaspline",function(e,t){e.AreaSplineSeries=e.Base.create("areaSplineSeries",e.AreaSeries,[e.CurveUtil],{drawSeries:function(){this.drawAreaSpline()}},{ATTRS:{type:{value:"areaSpline"}}})},"patched-dev-3.x",{requires:["series-area","series-curve-util"]});