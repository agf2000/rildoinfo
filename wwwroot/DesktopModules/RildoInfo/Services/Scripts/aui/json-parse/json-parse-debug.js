YUI.add('json-parse', function (Y, NAME) {

var _JSON = Y.config.global.JSON;

Y.namespace('JSON').parse = function (obj, reviver, space) {
    return _JSON.parse((typeof obj === 'string' ? obj : obj + ''), reviver, space);
};


}, 'patched-dev-3.x', {"requires": ["yui-base"]});
