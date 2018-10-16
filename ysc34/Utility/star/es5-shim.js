/*!
 * https://github.com/es-shims/es5-shim
 * @license es5-shim Copyright 2009-2015 by contributors, MIT License
 * see https://github.com/es-shims/es5-shim/blob/master/LICENSE
 */

// vim: ts=4 sts=4 sw=4 expandtab

// Add semicolon to prevent IIFE from being passed as argument to concatenated code.
;

// UMD (Universal Module Definition)
// see https://github.com/umdjs/umd/blob/master/templates/returnExports.js
(function (root, factory) {
    'use strict';

    /* global define, exports, module */
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(factory);
    } else if (typeof exports === 'object') {
        // Node. Does not work with strict CommonJS, but
        // only CommonJS-like enviroments that support module.exports,
        // like Node.
        module.exports = factory();
    } else {
        // Browser globals (root is window)
        root.returnExports = factory();
    }
}(this, function () {
    /**
     * Brings an environment as close to ECMAScript 5 compliance
     * as is possible with the facilities of erstwhile engines.
     *
     * Annotated ES5: http://es5.github.com/ (specific links below)
     * ES5 Spec: http://www.ecma-international.org/publications/files/ECMA-ST/Ecma-262.pdf
     * Required reading: http://javascriptweblog.wordpress.com/2011/12/05/extending-javascript-natives/
     */

    // Shortcut to an often accessed properties, in order to avoid multiple
    // dereference that costs universally. This also holds a reference to known-good
    // functions.
    var $Array = Array;
    var ArrayPrototype = $Array.prototype;
    var $Object = Object;
    var ObjectPrototype = $Object.prototype;
    var $Function = Function;
    var FunctionPrototype = $Function.prototype;
    var $String = String;
    var StringPrototype = $String.prototype;
    var $Number = Number;
    var NumberPrototype = $Number.prototype;
    var array_slice = ArrayPrototype.slice;
    var array_splice = ArrayPrototype.splice;
    var array_push = ArrayPrototype.push;
    var array_unshift = ArrayPrototype.unshift;
    var array_concat = ArrayPrototype.concat;
    var array_join = ArrayPrototype.join;
    var call = FunctionPrototype.call;
    var apply = FunctionPrototype.apply;
    var max = Math.max;
    var min = Math.min;

    // Having a toString local variable name breaks in Opera so use to_string.
    var to_string = ObjectPrototype.toString;

    /* global Symbol */
    /* eslint-disable one-var-declaration-per-line, no-redeclare, max-statements-per-line */
    var hasToStringTag = typeof Symbol === 'function' && typeof Symbol.toStringTag === 'symbol';
    var isCallable; /* inlined from https://npmjs.com/is-callable */ var fnToStr = Function.prototype.toString, constructorRegex = /^\s*class /, isES6ClassFn = function isES6ClassFn(value) { try { var fnStr = fnToStr.call(value); var singleStripped = fnStr.replace(/\/\/.*\n/g, ''); var multiStripped = singleStripped.replace(/\/\*[.\s\S]*\*\//g, ''); var spaceStripped = multiStripped.replace(/\n/mg, ' ').replace(/ {2}/g, ' '); return constructorRegex.test(spaceStripped); } catch (e) { return false; /* not a function */ } }, tryFunctionObject = function tryFunctionObject(value) { try { if (isES6ClassFn(value)) { return false; } fnToStr.call(value); return true; } catch (e) { return false; } }, fnClass = '[object Function]', genClass = '[object GeneratorFunction]', isCallable = function isCallable(value) { if (!value) { return false; } if (typeof value !== 'function' && typeof value !== 'object') { return false; } if (hasToStringTag) { return tryFunctionObject(value); } if (isES6ClassFn(value)) { return false; } var strClass = to_string.call(value); return strClass === fnClass || strClass === genClass; };

    var isRegex; /* inlined from https://npmjs.com/is-regex */ var regexExec = RegExp.prototype.exec, tryRegexExec = function tryRegexExec(value) { try { regexExec.call(value); return true; } catch (e) { return false; } }, regexClass = '[object RegExp]'; isRegex = function isRegex(value) { if (typeof value !== 'object') { return false; } return hasToStringTag ? tryRegexExec(value) : to_string.call(value) === regexClass; };
    var isString; /* inlined from https://npmjs.com/is-string */ var strValue = String.prototype.valueOf, tryStringObject = function tryStringObject(value) { try { strValue.call(value); return true; } catch (e) { return false; } }, stringClass = '[object String]'; isString = function isString(value) { if (typeof value === 'string') { return true; } if (typeof value !== 'object') { return false; } return hasToStringTag ? tryStringObject(value) : to_string.call(value) === stringClass; };
    /* eslint-enable one-var-declaration-per-line, no-redeclare, max-statements-per-line */

    /* inlined from http://npmjs.com/define-properties */
    var supportsDescriptors = $Object.defineProperty && (function () {
        try {
            var obj = {};
            $Object.defineProperty(obj, 'x', { enumerable: false, value: obj });
            for (var _ in obj) { return false; } // jscs:ignore disallowUnusedVariables
            return obj.x === obj;
        } catch (e) { /* this is ES3 */
            return false;
        }
    }());
    var defineProperties = (function (has) {
        // Define configurable, writable, and non-enumerable props
        // if they don't exist.
        var defineProperty;
        if (supportsDescriptors) {
            defineProperty = function (object, name, method, forceAssign) {
                if (!forceAssign && (name in object)) { return; }
                $Object.defineProperty(object, name, {
                    configurable: true,
                    enumerable: false,
                    writable: true,
                    value: method
                });
            };
        } else {
            defineProperty = function (object, name, method, forceAssign) {
                if (!forceAssign && (name in object)) { return; }
                object[name] = method;
            };
        }
        return function defineProperties(object, map, forceAssign) {
            for (var name in map) {
                if (has.call(map, name)) {
                    defineProperty(object, name, map[name], forceAssign);
                }
            }
        };
    }(ObjectPrototype.hasOwnProperty));

    //
    // Function
    // ========
    //

    // ES-5 15.3.4.5
    // http://es5.github.com/#x15.3.4.5

    var Empty = function Empty() {};

    defineProperties(FunctionPrototype, {
        bind: function bind(that) { // .length is 1
            // 1. Let Target be the this value.
            var target = this;
            // 2. If IsCallable(Target) is false, throw a TypeError exception.
            if (!isCallable(target)) {
                throw new TypeError('Function.prototype.bind called on incompatible ' + target);
            }
            // 3. Let A be a new (possibly empty) internal list of all of the
            //   argument values provided after thisArg (arg1, arg2 etc), in order.
            // XXX slicedArgs will stand in for "A" if used
            var args = array_slice.call(arguments, 1); // for normal call
            // 4. Let F be a new native ECMAScript object.
            // 11. Set the [[Prototype]] internal property of F to the standard
            //   built-in Function prototype object as specified in 15.3.3.1.
            // 12. Set the [[Call]] internal property of F as described in
            //   15.3.4.5.1.
            // 13. Set the [[Construct]] internal property of F as described in
            //   15.3.4.5.2.
            // 14. Set the [[HasInstance]] internal property of F as described in
            //   15.3.4.5.3.
            var bound;
            var binder = function () {

                if (this instanceof bound) {
                    // 15.3.4.5.2 [[Construct]]
                    // When the [[Construct]] internal method of a function object,
                    // F that was created using the bind function is called with a
                    // list of arguments ExtraArgs, the following steps are taken:
                    // 1. Let target be the value of F's [[TargetFunction]]
                    //   internal property.
                    // 2. If target has no [[Construct]] internal method, a
                    //   TypeError exception is thrown.
                    // 3. Let boundArgs be the value of F's [[BoundArgs]] internal
                    //   property.
                    // 4. Let args be a new list containing the same values as the
                    //   list boundArgs in the same order followed by the same
                    //   values as the list ExtraArgs in the same order.
                    // 5. Return the result of calling the [[Construct]] internal
                    //   method of target providing args as the arguments.

                    var result = apply.call(
                        target,
                        this,
                        array_concat.call(args, array_slice.call(arguments))
                    );
                    if ($Object(result) === result) {
                        return result;
                    }
                    return this;

                } else {
                    // 15.3.4.5.1 [[Call]]
                    // When the [[Call]] internal method of a function object, F,
                    // which was created using the bind function is called with a
                    // this value and a list of arguments ExtraArgs, the following
                    // steps are taken:
                    // 1. Let boundArgs be the value of F's [[BoundArgs]] internal
                    //   property.
                    // 2. Let boundThis be the value of F's [[BoundThis]] internal
                    //   property.
                    // 3. Let target be the value of F's [[TargetFunction]] internal
                    //   property.
                    // 4. Let args be a new list containing the same values as the
                    //   list boundArgs in the same order followed by the same
                    //   values as the list ExtraArgs in the same order.
                    // 5. Return the result of calling the [[Call]] internal method
                    //   of target providing boundThis as the this value and
                    //   providing args as the arguments.

                    // equiv: target.call(this, ...boundArgs, ...args)
                    return apply.call(
                        target,
                        that,
                        array_concat.call(args, array_slice.call(arguments))
                    );

                }

            };

            // 15. If the [[Class]] internal property of Target is "Function", then
            //     a. Let L be the length property of Target minus the length of A.
            //     b. Set the length own property of F to either 0 or L, whichever is
            //       larger.
            // 16. Else set the length own property of F to 0.

            var boundLength = max(0, target.length - args.length);

            // 17. Set the attributes of the length own property of F to the values
            //   specified in 15.3.5.1.
            var boundArgs = [];
            for (var i = 0; i < boundLength; i++) {
                array_push.call(boundArgs, '$' + i);
            }

            // XXX Build a dynamic function with desired amount of arguments is the only
            // way to set the length property of a function.
            // In environments where Content Security Policies enabled (Chrome extensions,
            // for ex.) all use of eval or Function costructor throws an exception.
            // However in all of these environments Function.prototype.bind exists
            // and so this code will never be executed.
            bound = $Function('binder', 'return function (' + array_join.call(boundArgs, ',') + '){ return binder.apply(this, arguments); }')(binder);

            if (target.prototype) {
                Empty.prototype = target.prototype;
                bound.prototype = new Empty();
                // Clean up dangling references.
                Empty.prototype = null;
            }

            // TODO
            // 18. Set the [[Extensible]] internal property of F to true.

            // TODO
            // 19. Let thrower be the [[ThrowTypeError]] function Object (13.2.3).
            // 20. Call the [[DefineOwnProperty]] internal method of F with
            //   arguments "caller", PropertyDescriptor {[[Get]]: thrower, [[Set]]:
            //   thrower, [[Enumerable]]: false, [[Configurable]]: false}, and
            //   false.
            // 21. Call the [[DefineOwnProperty]] internal method of F with
            //   arguments "arguments", PropertyDescriptor {[[Get]]: thrower,
            //   [[Set]]: thrower, [[Enumerable]]: false, [[Configurable]]: false},
            //   and false.

            // TODO
            // NOTE Function objects created using Function.prototype.bind do not
            // have a prototype property or the [[Code]], [[FormalParameters]], and
            // [[Scope]] internal properties.
            // XXX can't delete prototype in pure-js.

            // 22. Return F.
            return bound;
        }
    });

    // _Please note: Shortcuts are defined after `Function.prototype.bind` as we
    // use it in defining shortcuts.
    var owns = call.bind(ObjectPrototype.hasOwnProperty);
    var toStr = call.bind(ObjectPrototype.toString);
    var arraySlice = call.bind(array_slice);
    var arraySliceApply = apply.bind(array_slice);
    var strSlice = call.bind(StringPrototype.slice);
    var strSplit = call.bind(StringPrototype.split);
    var strIndexOf = call.bind(StringPrototype.indexOf);
    var pushCall = call.bind(array_push);
    var isEnum = call.bind(ObjectPrototype.propertyIsEnumerable);
    var arraySort = call.bind(ArrayPrototype.sort);

    //
    // Array
    // =====
    //

    var isArray = $Array.isArray || function isArray(obj) {
        return toStr(obj) === '[object Array]';
    };

    // ES5 15.4.4.12
    // http://es5.github.com/#x15.4.4.13
    // Return len+argCount.
    // [bugfix, ielt8]
    // IE < 8 bug: [].unshift(0) === undefined but should be "1"
    var hasUnshiftReturnValueBug = [].unshift(0) !== 1;
    defineProperties(ArrayPrototype, {
        unshift: function () {
            array_unshift.apply(this, arguments);
            return this.length;
        }
    }, hasUnshiftReturnValueBug);

    // ES5 15.4.3.2
    // http://es5.github.com/#x15.4.3.2
    // https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/Array/isArray
    defineProperties($Array, { isArray: isArray });

    // The IsCallable() check in the Array functions
    // has been replaced with a strict check on the
    // internal class of the object to trap cases where
    // the provided function was actually a regular
    // expression literal, which in V8 and
    // JavaScriptCore is a typeof "function".  Only in
    // V8 are regular expression literals permitted as
    // reduce parameters, so it is desirable in the
    // general case for the shim to match the more
    // strict and common behavior of rejecting regular
    // expressions.

    // ES5 15.4.4.18
    // http://es5.github.com/#x15.4.4.18
    // https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/array/forEach

    // Check failure of by-index access of string characters (IE < 9)
    // and failure of `0 in boxedString` (Rhino)
    var boxedString = $Object('a');
    var splitString = boxedString[0] !== 'a' || !(0 in boxedString);

    var properlyBoxesContext = function properlyBoxed(method) {
        // Check node 0.6.21 bug where third parameter is not boxed
        var properlyBoxesNonStrict = true;
        var properlyBoxesStrict = true;
        var threwException = false;
        if (method) {
            try {
                method.call('foo', function (_, __, context) {
                    if (typeof context !== 'object') {
                        properlyBoxesNonStrict = false;
                    }
                });

                method.call([1], function () {
                    'use strict';

                    properlyBoxesStrict = typeof this === 'string';
                }, 'x');
            } catch (e) {
                threwException = true;
            }
        }
        return !!method && !threwException && properlyBoxesNonStrict && properlyBoxesStrict;
    };

    defineProperties(ArrayPrototype, {
        forEach: function forEach(callbackfn/*, thisArg*/) {
            var object = ES.ToObject(this);
            var self = splitString && isString(this) ? strSplit(this, '') : object;
            var i = -1;
            var length = ES.ToUint32(self.length);
            var T;
            if (arguments.length > 1) {
                T = arguments[1];
            }

            // If no callback function or if callback is not a callable function
            if (!isCallable(callbackfn)) {
                throw new TypeError('Array.prototype.forEach callback must be a function');
            }

            while (++i < length) {
                if (i in self) {
                    // Invoke the callback function with call, passing arguments:
                    // context, property value, property key, thisArg object
                    if (typeof T === 'undefined') {
                        callbackfn(self[i], i, object);
                    } else {
                        callbackfn.call(T, self[i], i, object);
                    }
                }
            }
        }
    }, !properlyBoxesContext(ArrayPrototype.forEach));




 

 


    //
    // Object
    // ======
    //

    // ES5 15.2.3.14
    // http://es5.github.com/#x15.2.3.14

    // http://whattheheadsaid.com/2010/10/a-safer-object-keys-compatibility-implementation
    var hasDontEnumBug = !({ 'toString': null }).propertyIsEnumerable('toString');
    var hasProtoEnumBug = function () {}.propertyIsEnumerable('prototype');
    var hasStringEnumBug = !owns('x', '0');
    var equalsConstructorPrototype = function (o) {
        var ctor = o.constructor;
        return ctor && ctor.prototype === o;
    };
    var blacklistedKeys = {
        $window: true,
        $console: true,
        $parent: true,
        $self: true,
        $frame: true,
        $frames: true,
        $frameElement: true,
        $webkitIndexedDB: true,
        $webkitStorageInfo: true,
        $external: true
    };
    var hasAutomationEqualityBug = (function () {
        /* globals window */
        if (typeof window === 'undefined') { return false; }
        for (var k in window) {
            try {
                if (!blacklistedKeys['$' + k] && owns(window, k) && window[k] !== null && typeof window[k] === 'object') {
                    equalsConstructorPrototype(window[k]);
                }
            } catch (e) {
                return true;
            }
        }
        return false;
    }());
    var equalsConstructorPrototypeIfNotBuggy = function (object) {
        if (typeof window === 'undefined' || !hasAutomationEqualityBug) { return equalsConstructorPrototype(object); }
        try {
            return equalsConstructorPrototype(object);
        } catch (e) {
            return false;
        }
    };
    var dontEnums = [
        'toString',
        'toLocaleString',
        'valueOf',
        'hasOwnProperty',
        'isPrototypeOf',
        'propertyIsEnumerable',
        'constructor'
    ];
    var dontEnumsLength = dontEnums.length;

    // taken directly from https://github.com/ljharb/is-arguments/blob/master/index.js
    // can be replaced with require('is-arguments') if we ever use a build process instead
    var isStandardArguments = function isArguments(value) {
        return toStr(value) === '[object Arguments]';
    };
    var isLegacyArguments = function isArguments(value) {
        return value !== null &&
            typeof value === 'object' &&
            typeof value.length === 'number' &&
            value.length >= 0 &&
            !isArray(value) &&
            isCallable(value.callee);
    };
    var isArguments = isStandardArguments(arguments) ? isStandardArguments : isLegacyArguments;

    defineProperties($Object, {
        keys: function keys(object) {
            var isFn = isCallable(object);
            var isArgs = isArguments(object);
            var isObject = object !== null && typeof object === 'object';
            var isStr = isObject && isString(object);

            if (!isObject && !isFn && !isArgs) {
                throw new TypeError('Object.keys called on a non-object');
            }

            var theKeys = [];
            var skipProto = hasProtoEnumBug && isFn;
            if ((isStr && hasStringEnumBug) || isArgs) {
                for (var i = 0; i < object.length; ++i) {
                    pushCall(theKeys, $String(i));
                }
            }

            if (!isArgs) {
                for (var name in object) {
                    if (!(skipProto && name === 'prototype') && owns(object, name)) {
                        pushCall(theKeys, $String(name));
                    }
                }
            }

            if (hasDontEnumBug) {
                var skipConstructor = equalsConstructorPrototypeIfNotBuggy(object);
                for (var j = 0; j < dontEnumsLength; j++) {
                    var dontEnum = dontEnums[j];
                    if (!(skipConstructor && dontEnum === 'constructor') && owns(object, dontEnum)) {
                        pushCall(theKeys, dontEnum);
                    }
                }
            }
            return theKeys;
        }
    });

    var keysWorksWithArguments = $Object.keys && (function () {
        // Safari 5.0 bug
        return $Object.keys(arguments).length === 2;
    }(1, 2));
    var keysHasArgumentsLengthBug = $Object.keys && (function () {
        var argKeys = $Object.keys(arguments);
        return arguments.length !== 1 || argKeys.length !== 1 || argKeys[0] !== 1;
    }(1));
    var originalKeys = $Object.keys;
    defineProperties($Object, {
        keys: function keys(object) {
            if (isArguments(object)) {
                return originalKeys(arraySlice(object));
            } else {
                return originalKeys(object);
            }
        }
    }, !keysWorksWithArguments || keysHasArgumentsLengthBug);
}));
